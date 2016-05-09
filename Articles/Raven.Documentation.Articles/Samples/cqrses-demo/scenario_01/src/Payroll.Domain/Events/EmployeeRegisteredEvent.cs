using Payroll.Domain.Model;
using Payroll.Infrastructure;

namespace Payroll.Domain.Events
{
    public sealed class EmployeeRegisteredEvent : EmployeeEvent
    {
        public FullName Name { get; private set; }
        public decimal InitialSalary { get; private set; }

        public EmployeeRegisteredEvent(
            EmployeeId employeeId,
            FullName name, 
            decimal initialSalary
            ) : base(employeeId)
        {
            Name = name;
            InitialSalary = initialSalary;
        }
    }
}
