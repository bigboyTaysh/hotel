using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelApp.Models
{
    public class Customer
    {
        public string Id { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public DateTime Birthdate { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public Address Address { get; set; }
    }
}
