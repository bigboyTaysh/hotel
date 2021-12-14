
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

            var refreshToken = GetToken(user, DateTime.UtcNow.AddDays(4), true);
            var accessToken = GetToken(user, DateTime.UtcNow.AddSeconds(10), false);

            user.RefreshToken = refreshToken;
            user.Password = null;
            _userService.Update(user);

            return new Token() { RefreshToken = refreshToken, AccessToken = accessToken };
        }

        public void Logout(string token)
        {
            var user = _userService.GetUserByToken(token);
            if (user == null)
                return;

            user.RefreshToken = null;
            user.Password = null;
            _userService.Update(user);
        }

        public Token GetNewToken(string token)
        {
            var replacedToken = token.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.ReadJwtToken(replacedToken);
            var claim = "unique_name";

            var cos = DateTime.Now;
            var cos2 = securityToken.ValidTo;

            if (securityToken.ValidTo < DateTime.Now ||
                !securityToken.Claims.Any(c => c.Type == claim))
            {
                return null;
            }

            var user = _userService.GetUserByToken(replacedToken);

            if (user == null ||
                user?.Login == securityToken.Claims.FirstOrDefault(c => c.Type == claim).Value)
            {
                return null;
            }    

            var refreshToken = GetToken(user, DateTime.UtcNow.AddDays(4), true);
            var accessToken = GetToken(user, DateTime.UtcNow.AddSeconds(10), false);

            user.RefreshToken = refreshToken;
            user.Password = null;
            _userService.Update(user);

            return new Token() { RefreshToken = refreshToken, AccessToken = accessToken };
        }

        private string GetToken(User user, DateTime expires, bool isRefreshToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(_key);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Login),
                    isRefreshToken ? null : new Claim(ClaimTypes.Role, user.Role)
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
