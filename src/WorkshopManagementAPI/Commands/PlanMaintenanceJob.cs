using Pitstop.Infrastructure.Messaging;
using System;
using System.Collections.Generic;
using System.Text;
using Pitstop.WorkshopManagementAPI.Domain.ValueObjects;

namespace Pitstop.WorkshopManagementAPI.Commands
{
    public class PlanMaintenanceJob : Command
    {
        public readonly Guid JobId;
        public readonly DateTime StartTime;
        public readonly VehicleInfo VehicleInfo;
        public readonly CustomerInfo CustomerInfo;
        public readonly DateTime EndTime;
        public readonly string Description;

        public PlanMaintenanceJob(Guid messageId, Guid jobId, DateTime startTime, DateTime endTime,
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