using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Customers.Service.Models
{
    public class Customer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("Firstname")]
        public string Firstname { get; set; }
        [BsonElement("Lastname")]
        public string Lastname { get; set; }
        [BsonElement("Birthdate")]
        public DateTime Birthdate { get; set; }
        [BsonElement("Phone")]
        public string Phone { get; set; }
        [BsonElement("Email")]
        public string Email { get; set; }
        [BsonElement("Address")]
        public Address Address { get; set; }
    }
}
