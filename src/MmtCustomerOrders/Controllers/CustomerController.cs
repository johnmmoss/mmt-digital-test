using CustomersApiClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using CustomersApiClient.Exceptions;
using OrdersData;
using System.Collections.Generic;
using System.Linq;
using MmtCustomerOrders.Models;
using System;
using Microsoft.AspNetCore.Http;

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

        [HttpPost]
        public async Task<IActionResult> Post(CustomerOrdersRequest customerOrdersRequest)
        {
            if (customerOrdersRequest == null)
            {
                _logger.LogWarning("Get Customer called with invalid request");

                NotFound();
            }

            try
            {
                var email = customerOrdersRequest.User;
                var customerId = customerOrdersRequest.CustomerID;

                // TODO Validation for email and customerID what if they are not matching???
                
                _logger.LogInformation($"Getting customer data for user: {email} with customerId: {customerId}");

                // TODO GetCustomerAsync throws a NoCustomerException, but what does GetOrdersForCustomerAsync do???

                var customerTask = _customersHttpClient.GetCustomerAsync(email);
                var sqlCustomerOrdersDataTask = _customerOrderRepository.GetOrdersForCustomerAsync(customerId);

                await Task.WhenAll(customerTask, sqlCustomerOrdersDataTask);

                var customer = customerTask.Result;
                var sqlCustomerOrdersData = sqlCustomerOrdersDataTask.Result;

                // Todo Move to automapper/mapper class
                var response = new CustomerOrdersResponse();
                response.Customer.FirstName = customer.FirstName;
                response.Customer.LastName = customer.LastName;

                // Todo would really like some unit test around this :(
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
                _logger.LogWarning($"Getting customer data for user: {customerOrdersRequest.User} with customerId: {customerOrdersRequest.CustomerID}");

                return NotFound(); 
            } 
            catch (Exception ex)
            {
                _logger.LogError("An unexpected error occured", ex);

               return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}

