namespace Infrastructure.EventSourcing
{
    public interface IVersionedEvent<out TSourceId> : IEvent<TSourceId>
    {
        int Version { get; }
    }
}