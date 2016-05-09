using System;
using System.Collections.Generic;

namespace Infrastructure.EventSourcing
{
    public abstract class EventSourced<TId> : 
        IEventSourced<TId>
    {
        private readonly Dictionary<Type, Action<IVersionedEvent<TId>>> _handlers = 
            new Dictionary<Type, Action<IVersionedEvent<TId>>>();

        private readonly IList<IVersionedEvent<TId>> _pendingEvents;

        protected EventSourced(TId id)
        {
            Id = id;
            _pendingEvents = new List<IVersionedEvent<TId>>();
        }

        public TId Id { get; }
        public int Version { get; protected set; }

        
        public IEnumerable<IVersionedEvent<TId>> PendingEvents
            => _pendingEvents;

        protected void Handles<TEvent>(Action<TEvent> handler)
            where TEvent : IEvent<TId>
        {
            _handlers.Add(typeof(TEvent), @event => handler((TEvent)@event));
        }

        protected void LoadFrom(IEnumerable<IVersionedEvent<TId>> pastEvents)
        {
            foreach (var e in pastEvents)
            {
                _handlers[e.GetType()].Invoke(e);
                Version = e.Version;
            }
        }

        protected void Update(VersionedEvent<TId> e)
        {
            e.SourceId = Id;
            e.Version = Version + 1;

            _handlers[e.GetType()].Invoke(e);

            Version = e.Version;
            _pendingEvents.Add(e);
        }
    }
}
