using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Rooms.Service.Models
{
    public class Room
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Number")]
        public int Number { get; set; }

        [BsonElement("Password")]
        public int NumberOfSeats { get; set; }

        [BsonElement("Price")]
        public double Price { get; set; }

        [BsonElement("Description")]
        public string Description { get; set; }

    }
}
