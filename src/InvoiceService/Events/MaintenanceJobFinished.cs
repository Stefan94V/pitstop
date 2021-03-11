using Pitstop.Infrastructure.Messaging;
using System;

namespace Pitstop.InvoiceService.Events
{
    public class MaintenanceJobFinished : Event
    {
        public string JobId { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }

        public MaintenanceJobFinished(string jobId, DateTime startTime, DateTime endTime) 
        {
            JobId = jobId;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
