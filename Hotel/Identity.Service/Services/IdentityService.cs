
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

        public IdentityService(IDatabaseSettings settings, IConfiguration config)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<User>(settings.CollectionName);
            _key = config.GetSection("SecretKey").Value;
        }

        public LoggedUser Authenticate(LoginUser loginUser)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                loginUser.Password = Hash.GetHash(sha256Hash, loginUser.Password);
            }

            var user = _users.Find(u => u.Email == loginUser.Email && u.Password == loginUser.Password).FirstOrDefault();

            if (user == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(_key);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                }),

                Expires = DateTime.Now,

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new LoggedUser() { Token = tokenHandler.WriteToken(token), Email = user.Email, Role = user.Role };
        }
    }
}
