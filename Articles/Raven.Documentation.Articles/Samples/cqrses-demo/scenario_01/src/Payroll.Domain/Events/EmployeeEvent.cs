using Payroll.Domain.Model;
using Payroll.Infrastructure;

namespace Payroll.Domain.Events
{
    public abstract class EmployeeEvent : Event
    {
        public EmployeeId EmployeeId { get; private set; }

        protected EmployeeEvent(EmployeeId employeeId)
        {
            EmployeeId = employeeId;
        }
    }
}
