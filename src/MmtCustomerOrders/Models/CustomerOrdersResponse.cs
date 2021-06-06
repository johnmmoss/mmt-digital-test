using Newtonsoft.Json;
using System.Collections.Generic;

namespace MmtCustomerOrders
{

    public class CustomerOrdersResponse
    {
        [JsonProperty("customer")]
        public Customer Customer { get; set; }

        [JsonProperty("order")]
        public List<Order> Order { get; set; }

        public CustomerOrdersResponse()
        {
            Customer = new Customer();
            Order = new List<Order>();
        }
    }
}
