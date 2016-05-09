using System;
using System.Collections.Generic;
using System.Linq;
using Payroll.Domain;
using Payroll.Domain.Events;
using Payroll.Domain.Model;
using Payroll.Domain.Repositories;
using Payroll.Infrastructure;

namespace Playground
{
    class InMemoryEmployeeEventSourceRepository 
        : IEmployeeRepository,
        IMessageHandler<EmployeeRegisteredEvent>,
        IMessageHandler<EmployeeSalaryRaisedEvent>,
        IMessageHandler<EmployeeHomeAddressUpdatedEvent>
    {
        
        readonly IList<EmployeeEvent> _events = new List<EmployeeEvent>();

        public bool IsRegistered(EmployeeId id)
        {
            return _events.Any(e => e.EmployeeId.Equals(id));
        }

        public Employee Load(EmployeeId id)
        {
            Console.WriteLine("");
            Console.WriteLine($"Replaying events of employee {id}");

            var employeeEvents = _events.Where(e => e.EmployeeId.Equals(id));
            Employee result = null;

            foreach (var e in employeeEvents)
            {
                if (e is EmployeeRegisteredEvent)
                {
                    var l = e as EmployeeRegisteredEvent;
                    result = new Employee(l.EmployeeId, l.Name, Address.NotInformed, l.InitialSalary);
                    Console.WriteLine($"{l.MessageType} - {l.Name} (${l.InitialSalary})");
                }
                else if (e is EmployeeSalaryRaisedEvent)
                {
                    var l = e as EmployeeSalaryRaisedEvent;
                    var newSalary = result.Salary + l.Amount;
                    Console.WriteLine($"{l.MessageType} - ${l.Amount} (from ${result.Salary} to ${newSalary})");
                    result = new Employee(result.Id, result.Name, result.HomeAddress, newSalary);
                }
                else if (e is EmployeeHomeAddressUpdatedEvent)
                {
                    var l = e as EmployeeHomeAddressUpdatedEvent;
                    Console.WriteLine($"{l.MessageType} - from {result.HomeAddress} to {l.NewHomeAddress}");
                    result = new Employee(result.Id, result.Name, l.NewHomeAddress, result.Salary);
                }
            }

            Console.WriteLine("Done!");
            Console.WriteLine("");

            return result;
        }

        #region Unused repository methods
        public void CreateEmployee(EmployeeId id, FullName name, decimal initialSalary)
        {}

        public void RaiseSalary(EmployeeId id, decimal amount)
        {}

        public void UpdateHomeAddress(EmployeeId id, Address homeAddress)
        {}
        #endregion

        public void Handle(EmployeeRegisteredEvent e)
        {
            _events.Add(e);
        }

        public void Handle(EmployeeSalaryRaisedEvent e)
        {
            _events.Add(e);
        }

        public void Handle(EmployeeHomeAddressUpdatedEvent e)
        {
            _events.Add(e);
        }
    }
}
