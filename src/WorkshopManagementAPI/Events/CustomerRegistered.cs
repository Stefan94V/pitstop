using System;
using Pitstop.Infrastructure.Messaging;

namespace Pitstop.WorkshopManagementAPI.Events
{
    public class CustomerRegistered : Event
    {
        public string CustomerId { get; private set; }
        public string Name { get; private set; }
        public string TelephoneNumber { get; private set; }

        public CustomerRegistered(string customerId, string name, string telephoneNumber)
        {
            CustomerId = customerId;
            Name = name;
            TelephoneNumber = telephoneNumber;
        }
    }
}