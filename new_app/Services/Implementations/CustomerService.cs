using HotelReservationSystem.Data;
using HotelReservationSystem.Models.Domain;
using HotelReservationSystem.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HotelReservationSystem.Services.Implementations
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _context;

        public CustomerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Customer> GetCustomers()
        {
            return _context.Customers.ToList();
        }

        public Customer? GetCustomer(int id)
        {
            return _context.Customers.SingleOrDefault(c => c.Id == id);
        }

        public int CreateCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();

            return customer.Id;
        }

        public void UpdateCustomer(int id, Customer customer)
        {
            var customerInDb = _context.Customers.SingleOrDefault(c => c.Id == id);

            if (customerInDb == null)
                throw new Exception($"Customer with ID {id} not found");

            customerInDb.Name = customer.Name;
            customerInDb.Birthdate = customer.Birthdate;

            _context.SaveChanges();
        }

        public void DeleteCustomer(int id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Id == id);

            if (customer == null)
                throw new Exception($"Customer with ID {id} not found");

            _context.Customers.Remove(customer);
            _context.SaveChanges();
        }
    }
}