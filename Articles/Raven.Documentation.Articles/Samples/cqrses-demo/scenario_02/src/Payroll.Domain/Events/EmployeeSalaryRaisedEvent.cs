using System;
using Infrastructure.EventSourcing;

namespace Payroll.Domain.Events
{
    #region article_cqrs_6
    public class EmployeeSalaryRaisedEvent : VersionedEvent<Guid>
    {
        public decimal Amount { get; }

        public EmployeeSalaryRaisedEvent(decimal amout)
        {
            Amount = amout;
        }
    }
    #endregion
}
