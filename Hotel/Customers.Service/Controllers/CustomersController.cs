﻿using Customers.Service.Models;
using Identity.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Customers.Service.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly CustomerService service;

        public CustomersController(CustomerService _customerService)
        {
            service = _customerService;
        }

        // GET: api/<CustomersController>
        [HttpGet]
        public IEnumerable<Customer> Get()
        {
            return service.GetCustomers();
        }

        // GET api/<CustomersController>/5
        [HttpGet("{id}")]
        public ActionResult Get(string id)
        {
            var customer = service.GetCustomer(id);

            if( customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        [HttpPost("customerByName")]
        //[Route("customerReservations")]
        public IEnumerable<Customer> GetCustomerByName(CustomerFilter customer)
        {
            var customers = service.GetCustomerByName(customer);
            return customers;
        }


        // POST api/<CustomersController>
        [HttpPost]
        public ActionResult Post(Customer customer)
        {
            service.Create(customer);

            return Ok(customer);
        }

        // PUT api/<CustomersController>/5
        [HttpPut]
        public ActionResult Put(Customer customer)
        {
            var result = service.Update(customer);

            if(result.MatchedCount == 0)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        // DELETE api/<CustomersController>/5
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
