﻿using Pitstop.Infrastructure.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pitstop.NotificationService.Events
{
    public class CustomerRegistered : Event
    {
        public string CustomerId { get; private set; }
        public string Name { get; private set; }
        public string TelephoneNumber { get; private set; }
        public string EmailAddress { get; private set; }

        public CustomerRegistered(string customerId, string name, string telephoneNumber, string emailAddress)
        {
            CustomerId = customerId;
            Name = name;
            TelephoneNumber = telephoneNumber;
            EmailAddress = emailAddress;
        }
    }
}