using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Customers.Service.Models
{
    public class Address
    {
        [BsonElement("Street")]
        public string Street { get; set; }
        [BsonElement("ZipCode")]
        public string ZipCode { get; set; }
        [BsonElement("City")]
        public string City { get; set; }
        [BsonElement("Country")]
        public string Country { get; set; }
    }
}
