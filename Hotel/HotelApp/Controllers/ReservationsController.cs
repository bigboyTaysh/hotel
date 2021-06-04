using HotelApp.Models;
using HotelApp.Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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
    public class ReservationsController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _client;
        private readonly string _reservationsServiceUrl;


        public ReservationsController(IConfiguration config)
        {
            _config = config;
            _reservationsServiceUrl = _config.GetSection("Reservations.Service").GetSection("Connection").Value;
            _client = new HttpClient();
        }


        // GET: api/<ReservationsController>
        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            HttpResponseMessage response = await _client.GetAsync(_reservationsServiceUrl);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Content.ReadAsStringAsync().Result);
            }
            else
            {
                return StatusCode((int)response.StatusCode);
            }
        }

        // GET api/<ReservationsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(string id)
        {
            HttpResponseMessage response = await _client.GetAsync(_reservationsServiceUrl + id);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Content.ReadAsStringAsync().Result);
            }
            else
            {
                return StatusCode((int)response.StatusCode);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetE(string id)
        {
            HttpResponseMessage response = await _client.GetAsync(_reservationsServiceUrl + id);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Content.ReadAsStringAsync().Result);
            }
            else
            {
                return StatusCode((int)response.StatusCode);
            }
        }

        [HttpPost]
        [Route("emptyRooms")]
        public async Task<ActionResult> GetEmptyRooms(EmptyRoomsRequest request)
        {
            StringContent httpContent = new StringContent(JsonConvert.SerializeObject(request), System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync(_reservationsServiceUrl, httpContent);

            return Ok(response.Content.ReadAsStringAsync().Result);
        }

        // POST api/<ReservationsController>
        [HttpPost]
        public async Task<ActionResult> Post(Reservation reservation)
        {
            string json = JsonConvert.SerializeObject(reservation);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _client.PostAsync(_reservationsServiceUrl, httpContent);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Content.ReadAsStringAsync().Result);
            }
            else
            {
                return StatusCode((int)response.StatusCode);
            }
        }

        // PUT api/<ReservationsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, Reservation reservation)
        {
            string json = JsonConvert.SerializeObject(reservation);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _client.PutAsync(_reservationsServiceUrl + id, httpContent);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Content.ReadAsStringAsync().Result);
            }
            else
            {
                return StatusCode((int)response.StatusCode);
            }
        }

        // DELETE api/<ReservationsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            HttpResponseMessage response = await _client.DeleteAsync(_reservationsServiceUrl + id);

            return StatusCode((int)response.StatusCode);
        }
    }
}
