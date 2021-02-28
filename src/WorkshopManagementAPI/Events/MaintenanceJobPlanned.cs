using Pitstop.Infrastructure.Messaging;
using System;
using System.Collections.Generic;
using System.Text;
using Pitstop.WorkshopManagementAPI.Domain.ValueObjects;

namespace Pitstop.WorkshopManagementAPI.Events
{
    public class MaintenanceJobPlanned : Event
    {
        public readonly Guid JobId;
        public readonly DateTime StartTime;
        public readonly DateTime EndTime;
        public readonly CustomerInfo CustomerInfo;
        public readonly VehicleInfo VehicleInfo;
        public readonly string Description;

        public MaintenanceJobPlanned(Guid messageId, Guid jobId, DateTime startTime, DateTime endTime,
            CustomerInfo customerInfo,
            VehicleInfo vehicleInfo,
            string description) : base(messageId)
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
