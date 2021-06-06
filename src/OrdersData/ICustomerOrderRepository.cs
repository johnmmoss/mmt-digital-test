using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrdersData
{
    public interface ICustomerOrderRepository
    {
        Task<IEnumerable<SqlCustomerOrdersData>> GetOrdersForCustomerAsync(string customerId);
    }
}
