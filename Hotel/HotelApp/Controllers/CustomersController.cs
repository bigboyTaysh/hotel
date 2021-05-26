using HotelApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace HotelApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _client;
        private readonly string _roomsServiceUrl;


        public CustomersController(IConfiguration config)
        {
            _config = config;
            _roomsServiceUrl = _config.GetSection("Customers.Service").GetSection("Connection").Value;
            _client = new HttpClient();
        }


        // GET: api/<CustomersController>
        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
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

        // GET api/<CustomersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(string id)
        {
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

        // POST api/<CustomersController>
        [HttpPost]
        public async Task<ActionResult> Post(Customer customer)
        {
            string json = JsonConvert.SerializeObject(customer);
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

        // PUT api/<CustomersController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, Customer customer)
        {
            string json = JsonConvert.SerializeObject(customer);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _client.PutAsync(_roomsServiceUrl + id, httpContent);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Content.ReadAsStringAsync().Result);
            }
            else
            {
                return StatusCode((int)response.StatusCode);
            }
        }

        // DELETE api/<CustomersController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            HttpResponseMessage response = await _client.DeleteAsync(_roomsServiceUrl + id);

            return StatusCode((int)response.StatusCode);
        }
    }
}
