using Customers.Service.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
namespace Identity.Service.Services
{
    public class CustomerService
    {
        private readonly IMongoCollection<Customer> _customers;

        public CustomerService(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("IdentityDb"));
            var database = client.GetDatabase("IdentityDb");
            _customers = database.GetCollection<Customer>("Customers");
        }

        public List<Customer> GetCustomers() => _customers.Find(user => true).ToList();

        public Customer GetCustomer(string id) => _customers.Find(user => user.Id == id).FirstOrDefault();

        public Customer Create(Customer customer)
        {
            _customers.InsertOne(customer);
            return customer;
        }

        public void Update(string id, Customer customerIn) =>
            _customers.ReplaceOne(customer => customer.Id == id, customerIn);

        public void Remove(Customer customerIn) =>
            _customers.DeleteOne(customer => customer.Id == customerIn.Id);

        public void Remove(string id) =>
            _customers.DeleteOne(customer => customer.Id == id);
    }
}
