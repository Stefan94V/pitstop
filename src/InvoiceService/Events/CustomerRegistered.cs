using Pitstop.Infrastructure.Messaging;
using System;

namespace Pitstop.InvoiceService.Events
{
    public class CustomerRegistered : Event
    {
        public string CustomerId { get; private set; }
        public string Name { get; private set; }
        public string Address { get; private set; }
        public string PostalCode { get; private set; }
        public string City { get; private set; }


        public CustomerRegistered(string customerId, string name, string address, string postalCode,
            string city)
        {
            CustomerId = customerId;
            Name = name;
            Address = address;
            PostalCode = postalCode;
            City = city;
        }
    }
}