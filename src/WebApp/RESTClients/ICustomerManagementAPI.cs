using Pitstop.Models;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Commands;

namespace WebApp.RESTClients
{
    public interface ICustomerManagementAPI
    {
        Task<List<Customer>> GetCustomers();
        
        Task<Customer> GetCustomerById(string customerId);
        
        Task RegisterCustomer(RegisterCustomer command);
    }
}
