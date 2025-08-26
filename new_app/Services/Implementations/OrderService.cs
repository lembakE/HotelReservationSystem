using HotelReservationSystem.Data;
using HotelReservationSystem.Models.Domain;
using HotelReservationSystem.Models.DTOs;
using HotelReservationSystem.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelReservationSystem.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            return await _context.Orders
                .Include(c => c.Customer)
                .Include(c => c.Hotel)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderAsync(int id)
        {
            return await _context.Orders
                .Include(c => c.Customer)
                .Include(c => c.Hotel)
                .SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<int> CreateNewOrderAsync(NewOrderDto newOrder)
        {
            var customer = await _context.Customers.SingleAsync(c => c.Id == newOrder.CustomerId);
            var hotel = await _context.Hotels.SingleAsync(c => c.Id == newOrder.HotelId);

            var numOfDays = Convert.ToInt32((newOrder.EndDate - newOrder.StartDate).TotalDays);
            var fullPrice = Math.Round((hotel.PricePerNight * numOfDays), 2);

            var order = new Order
            {
                Customer = customer,
                Hotel = hotel,
                DateOrdered = DateTime.Now,
                StartDate = newOrder.StartDate,
                EndDate = newOrder.EndDate,
                NumberOfDays = numOfDays,
                FullPrice = fullPrice
            };

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            return order.Id;
        }

        public async Task UpdateOrderAsync(int id, Order order)
        {
            var orderInDb = await _context.Orders.SingleOrDefaultAsync(c => c.Id == id);

            if (orderInDb == null)
                throw new KeyNotFoundException($"Order with ID {id} not found");

            orderInDb.Customer = order.Customer;
            orderInDb.Hotel = order.Hotel;
            orderInDb.DateOrdered = order.DateOrdered;
            orderInDb.StartDate = order.StartDate;
            orderInDb.EndDate = order.EndDate;
            orderInDb.FullPrice = order.FullPrice;
            orderInDb.NumberOfDays = order.NumberOfDays;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int id)
        {
            var order = await _context.Orders.SingleOrDefaultAsync(c => c.Id == id);

            if (order == null)
                throw new KeyNotFoundException($"Order with ID {id} not found");

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }

        // Provide the synchronous implementations as well to satisfy the interface
        public IEnumerable<Order> GetOrders()
        {
            return GetOrdersAsync().GetAwaiter().GetResult();
        }

        public Order? GetOrder(int id)
        {
            return GetOrderAsync(id).GetAwaiter().GetResult();
        }

        public int CreateNewOrder(NewOrderDto newOrder)
        {
            return CreateNewOrderAsync(newOrder).GetAwaiter().GetResult();
        }

        public void UpdateOrder(int id, Order order)
        {
            UpdateOrderAsync(id, order).GetAwaiter().GetResult();
        }

        public void DeleteOrder(int id)
        {
            DeleteOrderAsync(id).GetAwaiter().GetResult();
        }
    }
}