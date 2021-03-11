using Pitstop.Infrastructure.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pitstop.WorkshopManagementAPI.Events
{
    public class WorkshopPlanningCreated : Event
    {
        public DateTime Date { get; private set; }

        public WorkshopPlanningCreated(DateTime date) 
        {
            Date = date;
        }
    }
}
