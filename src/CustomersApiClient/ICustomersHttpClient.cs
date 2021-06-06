using CustomersApiClient.Models;
using System.Threading.Tasks;

namespace CustomersApiClient
{
    public interface ICustomersHttpClient
    {
        Task<Customer> GetCustomerAsync(string email); 
    }
}
