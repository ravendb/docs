using System;

namespace Payroll.Infrastructure
{
    public abstract class Message
    {
        public string MessageType { get; private set; }

        protected Message()
        {
            MessageType = GetType().Name;
        }
    }
}
