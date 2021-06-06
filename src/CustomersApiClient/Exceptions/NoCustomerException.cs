using System;

namespace CustomersApiClient.Exceptions
{
    public class NoCustomerException : Exception
    {
        public string RequestedEmail { get; private set; }

        public NoCustomerException(string email)
        {
            RequestedEmail = email; 
        }
    }
}
