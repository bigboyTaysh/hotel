using HotelApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HotelApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly HttpClient _client;
        private readonly string _roomsServiceUrl;


        public RoomsController(IConfiguration config)
        {
            _roomsServiceUrl = config.GetSection("Rooms.Service").GetSection("Connection").Value;
            _client = new HttpClient();
        }


        // GET: api/<RoomsController>
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            Request.Headers.TryGetValue("Authorization", out var token);
            if (string.IsNullOrEmpty(token))
                return Unauthorized();
            _client.DefaultRequestHeaders.Add("Authorization", token.FirstOrDefault());

            HttpResponseMessage response = await _client.GetAsync(_roomsServiceUrl);

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
        public async Task<ActionResult> Get(string id)
        {
            Request.Headers.TryGetValue("Authorization", out var token);
            if (string.IsNullOrEmpty(token))
                return Unauthorized();
            _client.DefaultRequestHeaders.Add("Authorization", token.FirstOrDefault());

            HttpResponseMessage response = await _client.GetAsync(_roomsServiceUrl + id);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Content.ReadAsStringAsync().Result);
            }
            else
            {
                return StatusCode((int)response.StatusCode);
            }
        }

        // POST api/<RoomsController>
        [HttpPost]
        public async Task<ActionResult> Post(Room room)
        {
            Request.Headers.TryGetValue("Authorization", out var token);

            if (string.IsNullOrEmpty(token))
                return Unauthorized("Empty token");
            _client.DefaultRequestHeaders.Add("Authorization", token.FirstOrDefault());

            string json = JsonConvert.SerializeObject(room);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync(_roomsServiceUrl, httpContent);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Content.ReadAsStringAsync().Result);
            }
            else
            {
                return StatusCode((int)response.StatusCode);
            }
        }

        // PUT api/<RoomsController>/5
        [HttpPut]
        public async Task<ActionResult> Put(Room room)
        {
            Request.Headers.TryGetValue("Authorization", out var token);
            if (string.IsNullOrEmpty(token))
                return Unauthorized("Empty token");
            _client.DefaultRequestHeaders.Add("Authorization", token.FirstOrDefault());

            string json = JsonConvert.SerializeObject(room);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PutAsync(_roomsServiceUrl, httpContent);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Content.ReadAsStringAsync().Result);
            }
            else
            {
                return StatusCode((int)response.StatusCode);
            }
        }

        // DELETE api/<RoomsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            Request.Headers.TryGetValue("Authorization", out var token);
            if (string.IsNullOrEmpty(token))
                return Unauthorized("Empty token");
            _client.DefaultRequestHeaders.Add("Authorization", token.FirstOrDefault());

            HttpResponseMessage response = await _client.DeleteAsync(_roomsServiceUrl + id);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Content.ReadAsStringAsync().Result);
            }
            else
            {
                return StatusCode((int)response.StatusCode);
            }
        }
    }
}
