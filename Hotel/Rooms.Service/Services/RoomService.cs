using MongoDB.Driver;
using Rooms.Service.DAL;
using Rooms.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Threading.Tasks;

namespace Rooms.Service.Services
{
    public class RoomService
    {
        private readonly IMongoCollection<Room> _rooms;

        public RoomService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _rooms = database.GetCollection<Room>(settings.CollectionName);
        }

        public List<Room> GetRooms() => _rooms.Find(room => true).ToList();

        public Room GetRoom(string id) => _rooms.Find(room => room.Id == id).FirstOrDefault();

        public Room Create(Room room)
        {
            _rooms.InsertOne(room);
            return room;
        }

        public ReplaceOneResult Update(Room roomIn)
        {
            return _rooms.ReplaceOne(room => room.Id == roomIn.Id, roomIn);
        }

        public DeleteResult Remove(string id)
        {
            return _rooms.DeleteOne(room => room.Id == id);
        }
    }
}
