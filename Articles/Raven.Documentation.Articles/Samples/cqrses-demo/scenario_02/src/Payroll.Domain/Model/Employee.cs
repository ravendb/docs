using System;
using System.Collections.Generic;
using Infrastructure.EventSourcing;
using Payroll.Domain.Events;

namespace Payroll.Domain.Model
{
    public sealed class Employee : EventSourced<Guid>
    {
        public FullName Name { get; private set; }
        public decimal Salary { get; private set; }
        public Address HomeAddress { get; set; }

        private Employee(Guid id) : base(id)
        {
            Handles<EmployeeRegisteredEvent>(OnEmployeeRegistered);
            Handles<EmployeeSalaryRaisedEvent>(OnEmployeeSalaryRaised);
            Handles<EmployeeHomeAddressChangedEvent>(OnEmployeeHomeAddressChanged);    
        }

        public Employee(Guid id, FullName name, decimal initialSalary) 
            : this(id)
        {
            Throw.IfArgumentIsNull(name, nameof(name));
            Throw.IfArgumentIsNegative(initialSalary, nameof(initialSalary));

            Update(new EmployeeRegisteredEvent(name, initialSalary));
        }

        public Employee(Guid id, IEnumerable<IVersionedEvent<Guid>> history)
            : this(id)
        {
            LoadFrom(history);
        }

        public void RaiseSalary(decimal amount)
        {
            Throw.IfArgumentIsNegative(amount, nameof(amount));
            Update(new EmployeeSalaryRaisedEvent(amount));
        }

        public void ChangeHomeAddress(Address address)
        {
            Throw.IfArgumentIsNull(address, nameof(address));
            Update(new EmployeeHomeAddressChangedEvent(address));
        }

        private void OnEmployeeRegistered(EmployeeRegisteredEvent @event)
        {
            Name = @event.Name;
            Salary = @event.InitialSalary;
        }

        private void OnEmployeeSalaryRaised(EmployeeSalaryRaisedEvent @event)
        {
            Salary += @event.Amount;
        }

        private void OnEmployeeHomeAddressChanged(EmployeeHomeAddressChangedEvent @event)
        {
            HomeAddress = @event.NewAddress;
        }
    }
}
