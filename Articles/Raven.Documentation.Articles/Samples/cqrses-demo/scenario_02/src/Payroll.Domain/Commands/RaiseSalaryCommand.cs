using System;

namespace Payroll.Domain.Commands
{
    #region article_cqrs_5
    public class RaiseSalaryCommand
    {
        public Guid EmployeeId { get; }
        public decimal Amount { get; }

        public RaiseSalaryCommand(Guid employeeId, decimal amount)
        {
            EmployeeId = employeeId;
            Amount = amount;
        }
    }
    #endregion
}
