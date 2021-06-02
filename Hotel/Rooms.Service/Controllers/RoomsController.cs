using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rooms.Service.Models;
using Rooms.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rooms.Service.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly RoomService service;

        public RoomsController(RoomService _roomService)
        {
            service = _roomService;
        }

        // GET: api/<RoomsController>
        [HttpGet]
        public IEnumerable<Room> Get()
        {
            return service.GetRooms();
        }

        // GET api/<RoomsController>/5
        [HttpGet("{id}")]
        public ActionResult Get(string id)
        {
            var room = service.GetRoom(id);

            if (room == null)
            {
                return NotFound();
            }

            return Ok(room);
        }

        // POST api/<RoomsController>
        [HttpPost]
        public ActionResult Post(Room room)
        {
            service.Create(room);

            return Ok(room);
        }

        // PUT api/<RoomsController>/5
        [HttpPut("{id}")]
        public ActionResult Put(string id, Room room)
        {
            var result = service.Update(id, room);

            if (result.MatchedCount == 0)
            {
                return NotFound();
            }

            return Ok(room);
        }

        // DELETE api/<RoomsController>/5
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
