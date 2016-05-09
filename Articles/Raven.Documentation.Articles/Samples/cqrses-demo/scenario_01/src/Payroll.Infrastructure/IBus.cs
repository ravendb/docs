namespace Payroll.Infrastructure
{
    public interface IBus
    {
        void SendCommand<T>(T theCommand) where T : Command;
        void RaiseEvent<T>(T theEvent) where T : Event;
        void RegisterHandler<T>();
    }
}
