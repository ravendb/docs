using System;
using Infrastructure.EventSourcing;
using Payroll.Domain.Model;

namespace Payroll.Domain.Events
{
    public class EmployeeRegisteredEvent : VersionedEvent<Guid>
    {
        public FullName Name { get; }
        public decimal InitialSalary { get; }

        public EmployeeRegisteredEvent(FullName name, decimal initialSalary)
        {
            Name = name;
            InitialSalary = initialSalary;
        }
    }
}