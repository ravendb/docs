using System.Collections.Generic;

namespace Infrastructure.EventSourcing
{
    public interface IEventSourced<out TId> 
    {
        int Version { get; }
        IEnumerable<IVersionedEvent<TId>> PendingEvents { get; }
    }
}