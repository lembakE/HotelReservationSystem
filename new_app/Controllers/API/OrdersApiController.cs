using HotelReservationSystem.Data;
using HotelReservationSystem.Models.Domain;
using HotelReservationSystem.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationSystem.Controllers.API
{
    [Route("api/orders")]
    [ApiController]
    [Authorize(Roles = RoleName.CanManageHotels)]
    public class OrdersApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrdersApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _context.Orders
                .Include(c => c.Customer)
                .Include(c => c.Hotel)
                .ToListAsync();
            
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _context.Orders
                .Include(c => c.Customer)
                .Include(c => c.Hotel)
                .SingleOrDefaultAsync(c => c.Id == id);

            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewOrder(NewOrderDto newOrder)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customer = await _context.Customers.SingleOrDefaultAsync(c => c.Id == newOrder.CustomerId);
            if (customer == null)
                return BadRequest("Invalid Customer ID");

            var hotel = await _context.Hotels.SingleOrDefaultAsync(c => c.Id == newOrder.HotelId);
            if (hotel == null)
                return BadRequest("Invalid Hotel ID");

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
            await _context.SaveChangesAsync();

            return Ok(new { OrderId = order.Id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, Order orderUpdate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var orderInDb = await _context.Orders.SingleOrDefaultAsync(c => c.Id == id);

            if (orderInDb == null)
                return NotFound();

            // Update only allowed properties
            orderInDb.StartDate = orderUpdate.StartDate;
            orderInDb.EndDate = orderUpdate.EndDate;
            orderInDb.NumberOfDays = orderUpdate.NumberOfDays;
            orderInDb.FullPrice = orderUpdate.FullPrice;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.SingleOrDefaultAsync(c => c.Id == id);

            if (order == null)
                return NotFound();

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}