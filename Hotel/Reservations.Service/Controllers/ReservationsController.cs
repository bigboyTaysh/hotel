﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reservations.Service.Models;
using Reservations.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reservations.Service.Controllers 
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly ReservationService service;

        public ReservationsController(ReservationService _reservationService)
        {
            service = _reservationService;
        }

        // GET: api/<ReservationsController>
        [HttpGet]
        public IEnumerable<Reservation> Get()
        {
            return service.GetReservations();
        }

        // GET api/<ReservationsController>/5
        [HttpGet("{id}")]
        public ActionResult Get(string id)
        {
            var reservation = service.GetReservation(id);

            if (reservation == null)
            {
                return NotFound();
            }

            return Ok(reservation);
        }

        // GET api/<ReservationsController>/5
        [HttpPost]
        [Route("emptyRooms")]
        public IEnumerable<Room> GetEmptyRooms(EmptyRoomsRequest request)
        {
            return service.GetEmptyRooms(request.Rooms, request.StartDate, request.EndDate);
        }



        [HttpGet("customerReservations/{id}")]
        //[Route("customerReservations")]
        public IEnumerable<Reservation> GetCustomerReservations(string id)
        {
            var reservations = service.GetCustomerReservations(id);
            return reservations;
        }

        [HttpPost("reservationByName")]
        public IEnumerable<Reservation> GetReservationByName(ReservationFilter reservation)
        {
            var reservations = service.GetReservationByName(reservation);
            return reservations;
        }


        // POST api/<ReservationsController>
        [HttpPost]
        public ActionResult Post(Reservation reservation)
        {
            service.Create(reservation);

            return Ok(reservation);
        }

        // PUT api/<ReservationsController>/5
        [HttpPut("{id}")]
        public ActionResult Put(string id, Reservation reservation)
        {
            var result = service.Update(id, reservation);

            if (result.MatchedCount == 0)
            {
                return NotFound();
            }

            return Ok(reservation);
        }

        // DELETE api/<ReservationsController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            if (service.Remove(id).DeletedCount == 0)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
