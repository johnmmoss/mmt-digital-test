﻿namespace CustomersApiClient.Models
{
    public class Customer
    {
        public string CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public string HouseNumber { get; set; }
        public string Street { get; set; }
        public string Postcode { get; set; }
        public string Town { get; set; }

        public string Address()
        {
            return $"{HouseNumber}, {Street}, {Town}, {Postcode}";
        }
    }
}
