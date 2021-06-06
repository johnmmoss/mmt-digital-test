using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrdersData
{
    public class CustomerOrderRepository : ICustomerOrderRepository
    {
        private readonly SqlCustomerOrderDatabase _sqlCustomerOrderDatabase; 

        public CustomerOrderRepository(IOptions<SqlConnectionSettings> sqlConnectionSettings)
        {
            if (sqlConnectionSettings?.Value == null || sqlConnectionSettings.Value.SqlConnectionString == "NOTSET")
            {
                throw new ArgumentNullException("The Orders Database connection string is not set");
            }    

            _sqlCustomerOrderDatabase = new SqlCustomerOrderDatabase(sqlConnectionSettings.Value.SqlConnectionString);
        }

        public async Task<IEnumerable<SqlCustomerOrdersData>> GetOrdersForCustomerAsync(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException();
            }

            return await _sqlCustomerOrderDatabase.GetOrdersForCustomerAsync(customerId);
        }
    }
}
