using System.ComponentModel.DataAnnotations;

namespace HotelReservationSystem.Models.DTOs
{
    public class HotelDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public int CountryId { get; set; }
        
        public string? CountryName { get; set; }
        
        public string? City { get; set; }
        
        public int Stars { get; set; }
        
        public double PricePerNight { get; set; }
        
        public bool IsAllInclusive { get; set; }
    }
}