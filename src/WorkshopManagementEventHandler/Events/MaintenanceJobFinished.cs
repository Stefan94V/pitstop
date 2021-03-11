using Pitstop.Infrastructure.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pitstop.WorkshopManagementEventHandler.Events
{
    public class MaintenanceJobFinished : Event
    {
        public Guid JobId { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public string Notes { get; private set; }

        public MaintenanceJobFinished(Guid jobId, DateTime startTime, DateTime endTime, string notes) 
        {
            JobId = jobId;
            StartTime = startTime;
            EndTime = endTime;
            Notes = notes;
        }
    }
}
