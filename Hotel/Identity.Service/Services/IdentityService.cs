
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

namespace Identity.Service.Services
{
    public class IdentityService
    {
        private readonly IMongoCollection<User> _users;

        public IdentityService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<User>(settings.CollectionName);
        }

        public string Login(LoginUser user)
        {

            return user.Email;
        }
    }
}
