using HotelReservationSystem.Models.Domain;
using HotelReservationSystem.Models.DTOs;
using System.ServiceModel;

namespace HotelReservationSystem.Services.Contracts
{
    [ServiceContract]
    public interface IOrderService
    {
        [OperationContract]
        IEnumerable<Order> GetOrders();

        [OperationContract]
        Order? GetOrder(int id);

        [OperationContract]
        int CreateNewOrder(NewOrderDto newOrder);

        [OperationContract]
        void UpdateOrder(int id, Order order);

        [OperationContract]
        void DeleteOrder(int id);
    }
}