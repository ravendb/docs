using Payroll.Domain.Model;

namespace Payroll.Domain.Commands
{
    public sealed class RaiseEmployeeSalaryCommand 
        : EmployeeCommand
    {
        public decimal Amount { get; }

        public RaiseEmployeeSalaryCommand(
            EmployeeId id, 
            decimal amount
            ) : base(id)
        {
            Amount = amount;
        }
    }
}
