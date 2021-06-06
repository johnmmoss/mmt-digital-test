using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace OrdersData
{
    public class SqlCustomerOrderDatabase
    {
        private string _connectionString;

        public SqlCustomerOrderDatabase(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<SqlCustomerOrdersData>> GetOrdersForCustomerAsync(string customerId)
        {
            using IDbConnection connection = new SqlConnection(_connectionString);

            var sql = $@"SELECT 
                                o.ORDERID AS OrderNumber,
                                ORDERDATE,
                                DELIVERYEXPECTED,
                                p.PRODUCTNAME,
                                oi.QUANTITY,
                                oi.PRICE
                            FROM Orders o
                                JOIN OrderItems oi ON o.ORDERID = oi.ORDERID
                                JOIN Products p ON oi.PRODUCTID = p.PRODUCTID
                            WHERE 
                                o.CUSTOMERID = '{customerId}'";

            return await connection.QueryAsync<SqlCustomerOrdersData>(sql);
        }
    }
}
