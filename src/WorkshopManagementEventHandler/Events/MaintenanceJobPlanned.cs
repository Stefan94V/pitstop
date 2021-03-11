using Pitstop.Infrastructure.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pitstop.WorkshopManagementEventHandler.Events
{
    public class MaintenanceJobPlanned : Event
    {
        public Guid JobId { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public (string Id, string Name, string TelephoneNumber) CustomerInfo { get; private set; }
        public (string LicenseNumber, string Brand, string Type) VehicleInfo { get; private set; }
        public string Description { get; private set; }

        public MaintenanceJobPlanned(Guid jobId, DateTime startTime, DateTime endTime,
            (string Id, string Name, string TelephoneNumber) customerInfo,
            (string LicenseNumber, string Brand, string Type) vehicleInfo,
            string description)
        {
            JobId = jobId;
            StartTime = startTime;
            EndTime = endTime;
            CustomerInfo = customerInfo;
            VehicleInfo = vehicleInfo;
            Description = description;
        }
    }
}