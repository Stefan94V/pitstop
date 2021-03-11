using System;
using System.Collections.Generic;
using System.Text;

namespace Pitstop.Infrastructure.Messaging
{
    public class Event 
    {
        public Event()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        public Guid Id { get; private set; }

        public DateTime CreationDate { get; private set; }
    }
}
