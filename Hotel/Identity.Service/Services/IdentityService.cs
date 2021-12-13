
using Identity.Service.Extensions;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Identity.Service.DAL;
using Identity.Service.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Identity.Service.Services
{
    public class IdentityService
    {
        private readonly IMongoCollection<User> _users;
        private readonly string _key;
        private readonly UserService _userService;

        public IdentityService(
            IDatabaseSettings settings,
            IConfiguration config,
            UserService userService)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<User>(settings.CollectionName);
            _key = config.GetSection("SecretKey").Value;
            _userService = userService;
        }

        public Token Authenticate(LoginUser loginUser)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                loginUser.Password = Hash.GetHash(sha256Hash, loginUser.Password);
            }

            var user = _users.Find(u => u.Login == loginUser.Login && u.Password == loginUser.Password).FirstOrDefault();

            if (user == null)
                return null;

            var refreshToken = GetToken(user, DateTime.UtcNow.AddDays(4));
            var accessToken = GetToken(user, DateTime.UtcNow.AddMinutes(1));

            user.RefreshToken = refreshToken;
            user.Password = null;
            _userService.Update(user);

            return new Token() { RefreshToken = refreshToken, AccessToken = accessToken };

        }

        private string GetToken(User user, DateTime expires)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(_key);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Login),
                    new Claim(ClaimTypes.Role, user.Role)
                }),

                Expires = expires,

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
