using AutoMapper;
using HotelReservationSystem.Data;
using HotelReservationSystem.Models.Domain;
using HotelReservationSystem.Models.DTOs;
using HotelReservationSystem.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationSystem.Services.Implementations
{
    public class HotelService : IHotelService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public HotelService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<HotelDto> GetHotels()
        {
            var hotels = _context.Hotels
                .Include(c => c.Country)
                .ToList();
            
            return _mapper.Map<IEnumerable<HotelDto>>(hotels);
        }

        public HotelDto GetHotel(int id)
        {
            var hotel = _context.Hotels
                .Include(c => c.Country)
                .SingleOrDefault(c => c.Id == id);

            if (hotel == null)
                throw new Exception($"Hotel with ID {id} not found");

            return _mapper.Map<HotelDto>(hotel);
        }

        public int CreateHotel(HotelDto hotelDto)
        {
            var hotel = _mapper.Map<Hotel>(hotelDto);

            _context.Hotels.Add(hotel);
            _context.SaveChanges();

            return hotel.Id;
        }

        public void UpdateHotel(int id, HotelDto hotelDto)
        {
            var hotelInDb = _context.Hotels.SingleOrDefault(c => c.Id == id);

            if (hotelInDb == null)
                throw new Exception($"Hotel with ID {id} not found");

            _mapper.Map(hotelDto, hotelInDb);
            _context.SaveChanges();
        }

        public void DeleteHotel(int id)
        {
            var hotel = _context.Hotels.SingleOrDefault(c => c.Id == id);

            if (hotel == null)
                throw new Exception($"Hotel with ID {id} not found");

            _context.Hotels.Remove(hotel);
            _context.SaveChanges();
        }
    }
}