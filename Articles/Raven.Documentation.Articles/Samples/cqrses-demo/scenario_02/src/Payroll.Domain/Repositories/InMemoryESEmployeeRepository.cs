using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Infrastructure.EventSourcing;
using Infrastructure.Messaging;
using Payroll.Domain.Model;

namespace Payroll.Domain.Repositories
{
    public class InMemoryESEmployeeRepository : IEmployeeRepository
    {
        public IBus Bus { get; }
        private readonly IList<KeyValuePair<Guid, IVersionedEvent<Guid>>>
            _events = new List<KeyValuePair<Guid, IVersionedEvent<Guid>>>();

        public InMemoryESEmployeeRepository(IBus bus)
        {
            Bus = bus;
        }

        public void Save(Employee employee)
        {
            foreach (var evt in employee.PendingEvents)
            {
                _events.Add(new KeyValuePair<Guid, IVersionedEvent<Guid>>(
                    employee.Id,
                    evt
                    ));

                Bus.RaiseEvent(evt);
            }
        }

        public Employee Load(Guid employeeId)
        {
            var history = from evt in _events
                where evt.Key == employeeId
                select evt.Value;

            return new Employee(employeeId, history);
        }
    }
}
