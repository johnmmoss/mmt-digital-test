using CustomersApiClient;
using CustomersApiClient.Exceptions;
using CustomersApiClient.Models;
using Flurl.Http.Testing;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace CustomerApiClient.UnitTests
{
    public class CustomerApiClientTests
    {
        private readonly string BaseUrl = "http://some/url";
        private readonly string ApiKey = "asdf12345@#![";

        [Test]
        public async Task Given_an_existing_customer_email_is_provided_then_that_customers_details_are_returned()
        {
            // Arrange
            using var httpTest = new HttpTest();
            var customerEmail = "me@my.com";
            var customer = new Customer { CustomerId = "1", FirstName = "John", LastName = "Smith", Email = customerEmail };
            httpTest.RespondWithJson(customer);

            var customerHttpClient = new CustomersHttpClient(MockSettings().Object);

            // Act
            var customerResponse = await customerHttpClient.GetCustomerAsync(customerEmail);

            // Assert
            Assert.AreEqual(customer.CustomerId, customerResponse.CustomerId);
            httpTest.ShouldHaveCalled($"{BaseUrl}/GetUserDetails").WithQueryParam("email", customerEmail);
        }

        [Test]
        public void Given_an_email_is_provided_but_that_customer_does_not_exist_then_a_NoCustomerException_is_thrown()
        {
            // Arrange
            using var httpTest = new HttpTest();
            httpTest.RespondWith("error", 404);
            var customerHttpClient = new CustomersHttpClient(MockSettings().Object);

            // Act, Assert
            Assert.ThrowsAsync<NoCustomerException>(async () => await customerHttpClient.GetCustomerAsync("me@my.com"));
        }

        [TestCase(null)]
        [TestCase("")]
        public async Task Given_an_empty_string_is_provided_returns_null(string email)
        {
            // Arrange
            using var httpTest = new HttpTest();
            httpTest.RespondWith("error", 404);
            var customerHttpClient = new CustomersHttpClient(MockSettings().Object);

            // Act
            var customerResponse = await customerHttpClient.GetCustomerAsync(email);

            // Assert
            Assert.IsNull(customerResponse);
        }

        private Mock<IOptions<CustomerApiSettings>> MockSettings()
        {
            var settings = new Mock<IOptions<CustomerApiSettings>>();
            settings.Setup(x => x.Value).Returns(new CustomerApiSettings() { ApiKey = ApiKey, ApiUrl = BaseUrl});
            return settings;
        }
    }
}