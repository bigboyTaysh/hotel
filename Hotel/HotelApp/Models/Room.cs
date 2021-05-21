using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelApp.Models
{
    public class Room
    {
        public string Id { get; set; }

        public int Number { get; set; }

        public int NumberOfSeats { get; set; }

        public double Price { get; set; }

        public string Description { get; set; }

    }
}
