using Pitstop.Infrastructure.Messaging;
using System;

namespace Pitstop.NotificationService.Events
{
    public class MaintenanceJobFinished : Event
    {
        public string JobId { get; private set; }

        public MaintenanceJobFinished(string jobId) 
        {
            JobId = jobId;
        }
    }
}
