using IdentityAPI.Models;
using Identity.Service.Extensions;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
namespace Identity.Service.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> users;

        public UserService(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("IdentityDb"));
            var database = client.GetDatabase("IdentityDb");
            users = database.GetCollection<User>("Users");
        }

        public List<User> GetUsers() => users.Find(user => true).ToList();

        public User GetUser(string id) => users.Find(user => user.Id == id).FirstOrDefault();

        public User Create(User user)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                string hash = Hash.GetHash(sha256Hash, user.Password);
                user.Password = hash;
            }

            users.InsertOne(user);
            return user;
        }
    }
}
