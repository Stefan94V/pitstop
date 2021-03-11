using Pitstop.Infrastructure.Messaging;
using System;

namespace Pitstop.InvoiceService.Events
{
    public class MaintenanceJobPlanned : Event
    {
        public string JobId { get; private set; }
        public (string Id, string Name, string TelephoneNumber) CustomerInfo { get; private set; }
        public (string LicenseNumber, string Brand, string Type) VehicleInfo { get; private set; }
        public string Description { get; private set; }

        public MaintenanceJobPlanned(string jobId, (string Id, string Name, string TelephoneNumber) customerInfo,
            (string LicenseNumber, string Brand, string Type) vehicleInfo, string description) 
        {
            JobId = jobId;
            CustomerInfo = customerInfo;
            VehicleInfo = vehicleInfo;
            Description = description;
        }
    }
}
