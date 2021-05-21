using HotelApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Rooms.Service.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HotelApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _client;
        private readonly string _roomsServiceUrl;


        public RoomsController(IConfiguration config)
        {
            _config = config;
            _roomsServiceUrl = _config.GetSection("Rooms.Service").GetSection("Connection").Value;
            _client = new HttpClient();
        }


        // GET: api/<RoomsController>
        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            HttpResponseMessage response = await _client.GetAsync(_roomsServiceUrl + "api/rooms");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Content.ReadAsStringAsync().Result);
            }
            else
            {
                return StatusCode((int)response.StatusCode);
            }
        }

        // GET api/<RoomsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<RoomsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
            /*
            ApplicationUser applicationUser = new ApplicationUser();

            string json = JsonConvert.SerializeObject(applicationUser);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _client.PostAsync(_roomsServiceUrl + "api/rooms", httpContent);
            */
        }

        // PUT api/<RoomsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<RoomsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
