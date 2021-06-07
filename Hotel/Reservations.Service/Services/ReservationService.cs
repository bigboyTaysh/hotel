using MongoDB.Driver;
using Reservations.Service.DAL;
using Reservations.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reservations.Service.Services
{
    public class ReservationService
    {
        private readonly IMongoCollection<Reservation> _reservations;

        public ReservationService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _reservations = database.GetCollection<Reservation>(settings.CollectionName);
        }

        public List<Reservation> GetReservations() => _reservations.Find(reservation => true).ToList();

        public Reservation GetReservation(string id) => _reservations.Find(reservation => reservation.Id == id).FirstOrDefault();

        public Reservation Create(Reservation reservation)
        {
            _reservations.InsertOne(reservation);
            return reservation;
        }

        public ReplaceOneResult Update(string id, Reservation reservationIn)
        {
            return _reservations.ReplaceOne(reservation => reservation.Id == id, reservationIn);
        }

        public DeleteResult Remove(string id)
        {
            return _reservations.DeleteOne(reservation => reservation.Id == id);
        }
        public List<Room> GetEmptyRooms(List<Room> rooms, DateTime startDate, DateTime endDate)
        {
            
            var list = _reservations.Find(r => 
                (r.StartDate > startDate && r.StartDate < endDate) ||
                (r.EndDate > startDate && r.EndDate < endDate) || 
                (r.StartDate < startDate && r.EndDate > endDate)).ToList().SelectMany(r => r.Rooms).Distinct().ToList();

            var newlist = rooms.Where(r => !list.Any(l => l.Id == r.Id)).ToList();
            return newlist;
        }

        public List<Reservation> GetCustomerReservations(string id)
        {
            var list = _reservations.Find(reservation => reservation.CustomerId == id).ToList();
            return list;
        }

    }
}
