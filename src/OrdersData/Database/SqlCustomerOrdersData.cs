using System;

namespace OrdersData
{
    public class SqlCustomerOrdersData
    {
        public int OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryExpected { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
