using System;
using System.Linq;
using System.Collections.Generic;

namespace Payroll.Infrastructure.InMemoryBus
{
    public class NaiveInMemoryBus : IBus
    {
        private readonly IDependencyInjector _injector;

        public NaiveInMemoryBus(IDependencyInjector injector)
        {
            _injector = injector;
        }

        readonly IList<Type> _handlers = new List<Type>();

        public void SendCommand<T>(T theCommand) where T : Command
        {
            Send<T>(theCommand);
        }

        public void RaiseEvent<T>(T theEvent) where T : Event
        {
            Send<T>(theEvent);
        }

        private void Send<T>(T message) where T : Message
        {
            var constructedType = typeof(IMessageHandler<>)
                .MakeGenericType(typeof(T));

            var handlersToNotify = _handlers
                .Where(h => constructedType.IsAssignableFrom(h));
            
            foreach (var handler in handlersToNotify)
            {
                var instance = _injector.Get(handler);
                var method = constructedType.GetMethod("Handle", new[] {typeof(T)});
                method.Invoke(instance, new object[] { message });
            }
        }

        public void RegisterHandler<T>()
        {
            _handlers.Add(typeof(T));
        }
    }
}
