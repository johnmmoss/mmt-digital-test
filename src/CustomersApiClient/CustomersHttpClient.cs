using CustomersApiClient.Exceptions;
using CustomersApiClient.Models;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomersApiClient
{
    public class CustomersHttpClient : ICustomersHttpClient
    {
        private string _baseUrl;
        private string _apiKey;

        public CustomersHttpClient(IOptions<CustomerApiSettings> customerApiSettings)
        {
            _baseUrl = customerApiSettings.Value.ApiUrl;
            _apiKey = customerApiSettings.Value.ApiKey;
        }

        public async Task<Customer> GetCustomerAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return default;
            }

            var url = Url.Combine(_baseUrl, "GetUserDetails")
                .SetQueryParams(new
                {
                    email = email,
                    code = _apiKey
                });

            try
            {
                return await url.GetJsonAsync<Customer>();
            }
            catch (FlurlHttpException ex)
            {
                if (ex.StatusCode == 404)
                {
                    throw new NoCustomerException(email);
                }

                throw new Exception(ex.Message);
            }
        }
    }
}
