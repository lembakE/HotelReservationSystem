using HotelReservationSystem.Data;
using HotelReservationSystem.Models.Domain;
using HotelReservationSystem.Models.DTOs;
using HotelReservationSystem.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationSystem.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Order> GetOrders()
        {
            return _context.Orders
                .Include(c => c.Customer)
                .Include(c => c.Hotel)
                .ToList();
        }

        public Order? GetOrder(int id)
        {
            return _context.Orders
                .Include(c => c.Customer)
                .Include(c => c.Hotel)
                .SingleOrDefault(c => c.Id == id);
        }

        public int CreateNewOrder(NewOrderDto newOrder)
        {
            var customer = _context.Customers.Single(c => c.Id == newOrder.CustomerId);
            var hotel = _context.Hotels.Single(c => c.Id == newOrder.HotelId);

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

            _context.Orders.Add(order);
            _context.SaveChanges();

            return order.Id;
        }

        public void UpdateOrder(int id, Order order)
        {
            var orderInDb = _context.Orders.SingleOrDefault(c => c.Id == id);

            if (orderInDb == null)
                throw new Exception($"Order with ID {id} not found");

            orderInDb.Customer = order.Customer;
            orderInDb.Hotel = order.Hotel;
            orderInDb.DateOrdered = order.DateOrdered;
            orderInDb.StartDate = order.StartDate;
            orderInDb.EndDate = order.EndDate;
            orderInDb.FullPrice = order.FullPrice;
            orderInDb.NumberOfDays = order.NumberOfDays;

            _context.SaveChanges();
        }

        public void DeleteOrder(int id)
        {
            var order = _context.Orders.SingleOrDefault(c => c.Id == id);

            if (order == null)
                throw new Exception($"Order with ID {id} not found");

            _context.Orders.Remove(order);
            _context.SaveChanges();
        }
    }
}