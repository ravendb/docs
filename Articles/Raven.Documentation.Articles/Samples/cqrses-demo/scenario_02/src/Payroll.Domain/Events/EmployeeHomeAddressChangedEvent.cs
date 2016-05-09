using System;
using Infrastructure.EventSourcing;
using Payroll.Domain.Model;

namespace Payroll.Domain.Events
{
    public class EmployeeHomeAddressChangedEvent : VersionedEvent<Guid>
    {
        public Address NewAddress { get; }

        public EmployeeHomeAddressChangedEvent(Address newAddress)
        {
            NewAddress = newAddress;
        }
    }
}