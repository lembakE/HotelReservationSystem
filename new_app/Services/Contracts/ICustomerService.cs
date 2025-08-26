using HotelReservationSystem.Models.Domain;
using System.ServiceModel;

namespace HotelReservationSystem.Services.Contracts
{
    [ServiceContract]
    public interface ICustomerService
    {
        [OperationContract]
        IEnumerable<Customer> GetCustomers();

        [OperationContract]
        Customer? GetCustomer(int id);

        [OperationContract]
        int CreateCustomer(Customer customer);

        [OperationContract]
        void UpdateCustomer(int id, Customer customer);

        [OperationContract]
        void DeleteCustomer(int id);
    }
}