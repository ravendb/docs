using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Messaging
{
    public interface IMessageHandler<in T>
    {
        void Handle(T message);
    }
}
