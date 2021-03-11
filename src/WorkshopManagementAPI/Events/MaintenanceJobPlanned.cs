using Pitstop.Infrastructure.Messaging;
using System;
using System.Collections.Generic;
using System.Text;
using Pitstop.WorkshopManagementAPI.Domain.ValueObjects;

namespace Pitstop.WorkshopManagementAPI.Events
{
    public class MaintenanceJobPlanned : Event
    {
        public Guid JobId { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public CustomerInfo CustomerInfo { get; private set; }
        public VehicleInfo VehicleInfo { get; private set; }
        public string Description { get; private set; }

        public MaintenanceJobPlanned(Guid jobId, DateTime startTime, DateTime endTime,
            CustomerInfo customerInfo,
            VehicleInfo vehicleInfo,
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
