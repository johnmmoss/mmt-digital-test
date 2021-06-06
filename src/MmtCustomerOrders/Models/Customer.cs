using Newtonsoft.Json;

namespace MmtCustomerOrders
{
    public class Customer
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }
    }


}
