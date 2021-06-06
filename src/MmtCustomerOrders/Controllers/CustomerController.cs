using CustomersApiClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using CustomersApiClient.Exceptions;
using OrdersData;
using System.Collections.Generic;
using System.Linq;

namespace MmtCustomerOrders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomersHttpClient _customersHttpClient;
        private readonly ICustomerOrderRepository _customerOrderRepository;

        public CustomerController(ILogger<CustomerController> logger, ICustomersHttpClient customersHttpClient, ICustomerOrderRepository customerOrderRepository)
        {
            _logger = logger;
            _customersHttpClient = customersHttpClient;
            _customerOrderRepository = customerOrderRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                _logger.LogWarning("Get Customer called with no email");

                NotFound();
            }

            try
            {
                _logger.LogInformation($"Getting customer data for email: {email}");

                var customer = await _customersHttpClient.GetCustomerAsync(email);
                var sqlCustomerOrdersData = await _customerOrderRepository.GetOrdersForCustomerAsync(customer.CustomerId);

                // Todo Move to automapper/mapper class
                var response = new CustomerOrdersResponse();
                response.Customer.FirstName = customer.FirstName;
                response.Customer.LastName = customer.LastName;

                var customerOrders = sqlCustomerOrdersData.GroupBy(order => order.OrderNumber)
                    .Select(orderGrouping => new Order() 
                    { 
                        OrderNumber = orderGrouping.Key.ToString(), 
                        OrderDate = orderGrouping.First().OrderDate.ToString("dd-MMM-yyyy"),
                        DeliveryExpected = orderGrouping.First().DeliveryExpected.ToString("dd-MMM-yyyy"), 
                        DeliveryAddress = customer.Address(),
                        OrderItems = orderGrouping.Select(orderItem => new OrderItem 
                            { 
                                Product = orderItem.ProductName,
                                Quantity = orderItem.Quantity,
                                PriceEach = orderItem.Price
                            } ).ToList() 
                    });

                response.Order = customerOrders.ToList();

                return Ok(response);
            } 
            catch (NoCustomerException)
            {
                _logger.LogWarning($"Request for a customer that does not exist: {email}");

                return NotFound(); 
            }
        }
    }
}

