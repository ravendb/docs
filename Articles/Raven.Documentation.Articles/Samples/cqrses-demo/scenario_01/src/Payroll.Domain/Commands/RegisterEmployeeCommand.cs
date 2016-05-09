using Payroll.Domain.Model;

namespace Payroll.Domain.Commands
{
    public sealed class RegisterEmployeeCommand
        : EmployeeCommand
    {
        public FullName Name { get; }
        public decimal InitialSalary { get; }

        public RegisterEmployeeCommand(
            EmployeeId id,
            FullName name,
            decimal initialSalary
            ) : base(id)
        {
            Name = name;
            InitialSalary = initialSalary;
        }
    }
}
