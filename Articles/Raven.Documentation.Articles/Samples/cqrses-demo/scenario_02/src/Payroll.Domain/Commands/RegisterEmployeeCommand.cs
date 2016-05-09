using System;
using Payroll.Domain.Model;

namespace Payroll.Domain.Commands
{
    public class RegisterEmployeeCommand
    {
        public Guid Id { get; }
        public FullName Name { get; }
        public decimal InitialSalary { get; }

        public RegisterEmployeeCommand(Guid id, FullName name, decimal initialSalary)
        {
            Id = id;
            Name = name;
            InitialSalary = initialSalary;
        }
    }
}
