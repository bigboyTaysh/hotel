using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Identity.Service.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("Login")]
        public string Login { get; set; }
        [BsonElement("Password")]
        public string Password { get; set; }
        [BsonElement("Role")]
        public string Role { get; set; }
        [BsonElement("RefreshToken")]
        public string RefreshToken { get; set; }
    }
}