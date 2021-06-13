using Customers.Service.DAL;
using Customers.Service.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
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

        public List<Customer> GetCustomerByName(CustomerFilter customerFilter)
        {

            if (customerFilter.Id != "" && customerFilter.Id.Length == 24)
            {
                return _customers.Find(customer =>
                customer.Id == customerFilter.Id &&
                (customer.Firstname.Contains(customerFilter.Name) || customer.Lastname.Contains(customerFilter.Name)) &&
                customer.Email.Contains(customerFilter.Email) &&
                customer.Phone.Contains(customerFilter.Phone)).ToList();
            }
            else
            {
                return _customers.Find(customer =>
                (customer.Firstname.Contains(customerFilter.Name) || customer.Lastname.Contains(customerFilter.Name)) &&
                customer.Email.Contains(customerFilter.Email) &&
                customer.Phone.Contains(customerFilter.Phone)).ToList();
            }
            
            //var list = _customers.Find(new BsonDocument { { "_id", new BsonDocument { { "$regex", customerFilter.Id } } } }).ToList();
        }

    }
}
