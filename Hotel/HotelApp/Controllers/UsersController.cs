﻿using HotelApp.Models;
using Microsoft.AspNetCore.Http;
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
    public class UsersController : ControllerBase
    {

        private readonly HttpClient _client;
        private readonly string _usersServiceUrl;


        public UsersController(IConfiguration config)
        {
            _usersServiceUrl = config.GetSection("Identity.Service").GetSection("UsersConnection").Value;
            _client = new HttpClient();
        }

        // GET: api/<UsersController>
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            Request.Headers.TryGetValue("Authorization", out var token);

            if (StringValues.IsNullOrEmpty(token))
                return Unauthorized();

            _client.DefaultRequestHeaders.Add("Authorization", token.FirstOrDefault());

            HttpResponseMessage response = await _client.GetAsync(_usersServiceUrl);
 
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Content.ReadAsStringAsync().Result);
            }
            else
            {
                return StatusCode((int)response.StatusCode);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(string id)
        {
            Request.Headers.TryGetValue("Authorization", out var token);

            if (StringValues.IsNullOrEmpty(token))
                return Unauthorized();

            _client.DefaultRequestHeaders.Add("Authorization", token.FirstOrDefault());

            HttpResponseMessage response = await _client.GetAsync(_usersServiceUrl + id);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Content.ReadAsStringAsync().Result);
            }
            else
            {
                return StatusCode((int)response.StatusCode);
            }
        }

        // POST api/<UsersController>
        [HttpPost]
        public async Task<ActionResult> Post(InputUser inputModel)
        {
            Request.Headers.TryGetValue("Authorization", out var token);

            if (StringValues.IsNullOrEmpty(token))
                return Unauthorized("Empty token");

            _client.DefaultRequestHeaders.Add("Authorization", token.FirstOrDefault());

            StringContent httpContent = new StringContent(JsonConvert.SerializeObject(inputModel), System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync(_usersServiceUrl, httpContent);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Content.ReadAsStringAsync().Result);
            }
            else
            {
                return StatusCode((int)response.StatusCode);
            }
        }

        // PUT api/<UsersController>/5
        [HttpPut]
        public async Task<ActionResult> Put(InputUser user)
        {
            Request.Headers.TryGetValue("Authorization", out var token);

            if (StringValues.IsNullOrEmpty(token))
                return Unauthorized("Empty token");

            _client.DefaultRequestHeaders.Add("Authorization", token.FirstOrDefault());

            StringContent httpContent = new StringContent(JsonConvert.SerializeObject(user), System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PutAsync(_usersServiceUrl, httpContent);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Content.ReadAsStringAsync().Result);
            }
            else
            {
                return StatusCode((int)response.StatusCode);
            }
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            Request.Headers.TryGetValue("Authorization", out var token);

            if (StringValues.IsNullOrEmpty(token))
                return Unauthorized("Empty token");

            _client.DefaultRequestHeaders.Add("Authorization", token.FirstOrDefault());

            HttpResponseMessage response = await _client.DeleteAsync(_usersServiceUrl + id);

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
