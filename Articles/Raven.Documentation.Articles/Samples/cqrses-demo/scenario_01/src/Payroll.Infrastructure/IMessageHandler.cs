namespace Payroll.Infrastructure
{
    public interface IMessageHandler<in T>
        where T : Message
    {
        void Handle(T message);
    }
}
