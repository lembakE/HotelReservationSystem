using AutoMapper;
using HotelReservationSystem.Data;
using HotelReservationSystem.Models.Domain;
using HotelReservationSystem.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationSystem.Controllers.API
{
    [Route("api/hotels")]
    [ApiController]
    public class HotelsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public HotelsApiController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetHotels()
        {
            var hotels = await _context.Hotels
                .Include(c => c.Country)
                .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<HotelDto>>(hotels));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetHotel(int id)
        {
            var hotel = await _context.Hotels
                .Include(c => c.Country)
                .SingleOrDefaultAsync(c => c.Id == id);

            if (hotel == null)
                return NotFound();

            return Ok(_mapper.Map<HotelDto>(hotel));
        }

        [HttpPost]
        [Authorize(Roles = RoleName.CanManageHotels)]
        public async Task<IActionResult> CreateHotel(HotelDto hotelDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var hotel = _mapper.Map<Hotel>(hotelDto);

            _context.Hotels.Add(hotel);
            await _context.SaveChangesAsync();

            hotelDto.Id = hotel.Id;

            return CreatedAtAction(nameof(GetHotel), new { id = hotel.Id }, hotelDto);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = RoleName.CanManageHotels)]
        public async Task<IActionResult> UpdateHotel(int id, HotelDto hotelDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var hotelInDb = await _context.Hotels.SingleOrDefaultAsync(c => c.Id == id);

            if (hotelInDb == null)
                return NotFound();

            _mapper.Map(hotelDto, hotelInDb);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = RoleName.CanManageHotels)]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            var hotel = await _context.Hotels.SingleOrDefaultAsync(c => c.Id == id);

            if (hotel == null)
                return NotFound();

            _context.Hotels.Remove(hotel);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}