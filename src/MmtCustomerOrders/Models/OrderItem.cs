using Newtonsoft.Json;

namespace MmtCustomerOrders
{
    public class OrderItem
    {
        [JsonProperty("product")]
        public string Product { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("priceEach")]
        public decimal PriceEach { get; set; }
    }
}
