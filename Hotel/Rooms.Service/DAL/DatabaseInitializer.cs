using Rooms.Service.Models;
using Rooms.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rooms.Service.DAL
{
    public static class DatabaseInitializer
    {
        public static void Seed(RoomService roomService) 
        {
            if (!roomService.GetRooms().Any())
            {
                var rooms = new List<Room>()
                {
                    new Room()
                    {
                        Number = 1,
                        NumberOfSeats = 1,
                        Price = 150.0,
                        Description = "- 1 single bed\n" +
                        "- free WiFi\n" +
                        "- TV",
                    },
                    new Room()
                    {
                        Number = 2,
                        NumberOfSeats = 2,
                        Price = 300.0,
                        Description = "- 1 extra-large double bed\n" +
                        "- free WiFi\n" +
                        "- TV\n" +
                        "- fridge",
                    },
                    new Room()
                    {
                        Number = 3,
                        NumberOfSeats = 3,
                        Price = 450.0,
                        Description = "- 1 single bed\n" +
                        "- 1 extra-large double bed\n" +
                        "- free WiFi\n" +
                        "- TV\n" +
                        "- fridge",
                    },
                };

                rooms.ForEach(room => roomService.Create(room));
            }   
        }
    }
}
