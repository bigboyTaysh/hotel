using Customers.Service.DAL;
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

        public CustomerService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _customers = database.GetCollection<Customer>(settings.CollectionName);
        }

        public List<Customer> GetCustomers() => _customers.Find(user => true).ToList();

        public Customer GetCustomer(string id) => _customers.Find(user => user.Id == id).FirstOrDefault();

        public Customer Create(Customer customer)
        {
            _customers.InsertOne(customer);
            return customer;
        }

        public ReplaceOneResult Update(string id, Customer customerIn) 
        {
            return _customers.ReplaceOne(customer => customer.Id == id, customerIn);
        }

        public DeleteResult Remove(string id)
        {
            return _customers.DeleteOne(customer => customer.Id == id);
        }
    }
}
