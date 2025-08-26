aproLambinator result: 

using System.Collections.Generic;
using HotelRetelingSivation.HotelManag: IHcontrat:  ssing
using HotelReservation.Models.Models.Domain;
using HotelReselation.Models.Models.Models;
using HOTEL.Models.ToDTOs.emsk;
using System.Collections.Generic:
using System;
using System.ServiceModel;
using System.ServiceModel;
using System.Threading.Tasks;

namespace HAssemtReservation.Services.Contracts
{
using System.ServiceM
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
        
        // Async methods (not exposed as SOAP operations)
        Task<IEnumerable<Order>> GetOrdersAsync();
        
        Task<Order?> GetOrderAsync(int id);
        
        Task<int> CreateNewOrderAsync(NewOrderDto newOrder);
        
        Task UpdateOrderAsync(int id, Order order);
        
        Task DeleteOrderAsync(int id);
    }
}