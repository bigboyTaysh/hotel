using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelApp.Models
{
    public class Reservation
    {
        public string Id { get; set; }

        public string CustomerId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public double Price { get; set; }

        /* póki co zostawiam zakomentowane
        [BsonElement("Rooms")]
        public virtual List<Room> Rooms { get; set; }
        */
    }
}
