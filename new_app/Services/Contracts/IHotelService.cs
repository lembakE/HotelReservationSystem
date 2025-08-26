using HotelReservationSystem.Models.DTOs;
using System.ServiceModel;

namespace HotelReservationSystem.Services.Contracts
{
    [ServiceContract]
    public interface IHotelService
    {
        [OperationContract]
        IEnumerable<HotelDto> GetHotels();

        [OperationContract]
        HotelDto GetHotel(int id);

        [OperationContract]
        int CreateHotel(HotelDto hotelDto);

        [OperationContract]
        void UpdateHotel(int id, HotelDto hotelDto);

        [OperationContract]
        void DeleteHotel(int id);
    }
}