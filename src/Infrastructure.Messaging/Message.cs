using System;
using System.Collections.Generic;
using System.Text;

namespace Pitstop.Infrastructure.Messaging
{
    public class Message
    {
        public Guid MessageId { get; private set; }
        public string MessageType { get; private set; }

        public Message() : this(Guid.NewGuid())
        {
        }

        public Message(Guid messageId)
        {
            MessageId = messageId;
            MessageType = this.GetType().Name;
        }

        public Message(string messageType) : this(Guid.NewGuid())
        {
            MessageType = messageType;
        }

        public Message(Guid messageId, string messageType)
        {
            MessageId = messageId;
            MessageType = messageType;
        }
    }
}
