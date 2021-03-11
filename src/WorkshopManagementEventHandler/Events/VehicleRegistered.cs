using Pitstop.Infrastructure.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pitstop.WorkshopManagementEventHandler.Events
{
    public class VehicleRegistered : Event
    {
        public string LicenseNumber  { get; private set; }
        public string Brand  { get; private set; }
        public string Type  { get; private set; }
        public string OwnerId  { get; private set; }
        public VehicleRegistered(string licenseNumber, string brand, string type, string ownerId) 
        {
            LicenseNumber = licenseNumber;
            Brand = brand;
            Type = type;
            OwnerId = ownerId;
        }
    }
}
