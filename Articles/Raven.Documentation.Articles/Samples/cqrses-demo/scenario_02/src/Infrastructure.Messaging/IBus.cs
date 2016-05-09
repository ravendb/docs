namespace Infrastructure.Messaging
{
    public interface IBus
    {
        void SendCommand(object theCommand);
        void RaiseEvent(object theEvent);
        void RegisterHandler<T>();
    }
}
