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
using System.Threading.Tasks;

namespace HotelApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _client;
        private readonly string _customersServiceUrl;


        public CustomersController(IConfiguration config)
        {
            _config = config;
            _customersServiceUrl = _config.GetSection("Customers.Service").GetSection("Connection").Value;
            _client = new HttpClient();
        }


        // GET: api/<CustomersController>
        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            Request.Headers.TryGetValue("Authorization", out var token);
            if (StringValues.IsNullOrEmpty(token))
                return Unauthorized();
            _client.DefaultRequestHeaders.Add("Authorization", token.FirstOrDefault());

            HttpResponseMessage response = await _client.GetAsync(_customersServiceUrl);

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
            Request.Headers.TryGetValue("Authorization", out var token);
            if (StringValues.IsNullOrEmpty(token))
                return Unauthorized();
            _client.DefaultRequestHeaders.Add("Authorization", token.FirstOrDefault());

            HttpResponseMessage response = await _client.GetAsync(_customersServiceUrl + id);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Content.ReadAsStringAsync().Result);
            }
            else
            {
                return StatusCode((int)response.StatusCode);
            }
        }


        [HttpPost("customerByName")]
        //[Route("customerReservations")]
        public async Task<ActionResult> GetCustomerByName(CustomerFilter customer)
        {
            Request.Headers.TryGetValue("Authorization", out var token);
            if (StringValues.IsNullOrEmpty(token))
                return Unauthorized();
            _client.DefaultRequestHeaders.Add("Authorization", token.FirstOrDefault());

            string json = JsonConvert.SerializeObject(customer);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _client.PostAsync(_customersServiceUrl + "customerByName", httpContent);

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
            Request.Headers.TryGetValue("Authorization", out var token);
            if (StringValues.IsNullOrEmpty(token))
                return Unauthorized("Empty token");
            _client.DefaultRequestHeaders.Add("Authorization", token.FirstOrDefault());

            string json = JsonConvert.SerializeObject(customer);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _client.PostAsync(_customersServiceUrl, httpContent);

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
        [HttpPut]
        public async Task<ActionResult> Put(Customer customer)
        {
            string json = JsonConvert.SerializeObject(customer);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _client.PutAsync(_customersServiceUrl, httpContent);

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
            HttpResponseMessage response = await _client.DeleteAsync(_customersServiceUrl + id);

            return StatusCode((int)response.StatusCode);
        }
    }
}
