using Pitstop.CustomerManagementAPI.Commands;
using Pitstop.Infrastructure.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pitstop.CustomerManagementAPI.Events
{
    public class CustomerRegistered : Event
    {
        public string CustomerId { get; private set; }
        public string Name { get; private set; }
        public string Address { get; private set; }
        public string PostalCode { get; private set; }
        public string City { get; private set; }
        public string TelephoneNumber { get; private set; }
        public string EmailAddress { get; private set; }

        public CustomerRegistered(string customerId, string name, string address, string postalCode, string city,
            string telephoneNumber, string emailAddress)
        {
            CustomerId = customerId;
            Name = name;
            Address = address;
            PostalCode = postalCode;
            City = city;
            TelephoneNumber = telephoneNumber;
            EmailAddress = emailAddress;
        }
    }
}