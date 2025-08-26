using AutoMapper;
using HotelReservationSystem.Models.Domain;
using HotelReservationSystem.Models.DTOs;

namespace HotelReservationSystem.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain to DTO
            CreateMap<Hotel, HotelDto>()
                .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Country != null ? src.Country.Name : string.Empty));

            CreateMap<Country, CountryDto>();

            // DTO to Domain
            CreateMap<HotelDto, Hotel>()
                .ForMember(dest => dest.Country, opt => opt.Ignore());
                
            CreateMap<CountryDto, Country>();
        }
    }
}