using System;

namespace Infrastructure.EventSourcing
{
    public class VersionedEvent<TSourceId> : IVersionedEvent<TSourceId>
    {
        public TSourceId SourceId { get; internal set; }
        public DateTime When { get; private set; }
        public int Version { get; internal set; }

        public VersionedEvent()
        {
            When = DateTime.Now;
        }
    }
}