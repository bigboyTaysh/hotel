
using Identity.Service.Extensions;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Identity.Service.DAL;
using Identity.Service.Models;

namespace Identity.Service.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<User>(settings.CollectionName);
        }

        public List<User> GetUsers() => _users.Find(user => true).ToList();

        public User GetUser(string id) => _users.Find(user => user.Id == id).FirstOrDefault();

        public User Create(User user)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                string hash = Hash.GetHash(sha256Hash, user.Password);
                user.Password = hash;
            }

            _users.InsertOne(user);
            return user;
        }
    }
}
