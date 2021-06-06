using CustomersApiClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using CustomersApiClient.Exceptions;

namespace MmtCustomerOrders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomersHttpClient _customersHttpClient;

        public CustomerController(ILogger<CustomerController> logger, ICustomersHttpClient customersHttpClient)
        {
            _logger = logger;
            _customersHttpClient = customersHttpClient;
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

                return Ok(customer);
            } 
            catch (NoCustomerException)
            {
                _logger.LogWarning($"Request for a customer that does not exist: {email}");

                return NotFound(); 
            }
        }
    }
}

