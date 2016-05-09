using Payroll.Domain.Model;

namespace Payroll.Domain.Events
{
    public class FailedToRegisterEmployeeEvent : EmployeeEvent
    {
        public FailedToRegisterEmployeeEvent(EmployeeId employeeId) : base(employeeId)
        {}
    }
}
