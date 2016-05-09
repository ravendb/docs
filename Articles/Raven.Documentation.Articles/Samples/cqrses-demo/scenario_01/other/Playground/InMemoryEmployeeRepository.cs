using System.Collections.Generic;

using Payroll.Domain;
using Payroll.Domain.Model;
using Payroll.Domain.Repositories;

namespace Playground             
{
    class InMemoryEmployeeRepository : IEmployeeRepository
    {
        readonly IDictionary<EmployeeId, Employee> _data = 
            new Dictionary<EmployeeId, Employee>();

        public bool IsRegistered(EmployeeId id)
        {
            return (_data.ContainsKey(id));
        }

        public Employee Load(EmployeeId id)
        {
            return _data[id];
        }

        public void CreateEmployee(EmployeeId id, FullName name, decimal initialSalary)
        {
            _data.Add(id, new Employee(id, name, Address.NotInformed, initialSalary));
        }

        public void RaiseSalary(EmployeeId id, decimal amount)
        {
            var employee = _data[id];
            _data[id] = new Employee(
                employee.Id,
                employee.Name,
                employee.HomeAddress,
                employee.Salary + amount
                );
         }

        public void UpdateHomeAddress(EmployeeId id, Address homeAddress)
        {
            var employee = _data[id];
            _data[id] = new Employee(
                employee.Id,
                employee.Name,
                homeAddress,
                employee.Salary
                );
        }
    }
}