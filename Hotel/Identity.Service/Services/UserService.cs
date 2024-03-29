﻿
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
        
        public User GetUserByName(string login) => _users.Find(user => user.Login == login).FirstOrDefault();
        
        public User GetUserByToken(string token) => _users.Find(user => user.RefreshToken == token).FirstOrDefault();

        public User Create(User user)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                user.Password = Hash.GetHash(sha256Hash, user.Password);
            }

            _users.InsertOne(user);
            return user;
        }

        public ReplaceOneResult Update(User userIn)
        {
            if (string.IsNullOrEmpty(userIn.Password))
            {
                userIn.Password = _users.Find(user => user.Id == userIn.Id).FirstOrDefault().Password;
            } 
            else
            {
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    userIn.Password = Hash.GetHash(sha256Hash, userIn.Password);
                }
            }

            return _users. ReplaceOne(user => user.Id == userIn.Id, userIn);
        }

        public DeleteResult Remove(string id)
        {
            return _users.DeleteOne(user => user.Id == id);
        }
    }
}
