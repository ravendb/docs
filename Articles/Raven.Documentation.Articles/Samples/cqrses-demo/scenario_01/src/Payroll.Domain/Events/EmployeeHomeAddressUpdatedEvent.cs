using Payroll.Domain.Model;

namespace Payroll.Domain.Events
{
    public sealed class EmployeeHomeAddressUpdatedEvent
        : EmployeeEvent
    {
        public Address NewHomeAddress { get; }

        public EmployeeHomeAddressUpdatedEvent(
            EmployeeId employeeId, 
            Address address
            ) : base(employeeId)
        {
            NewHomeAddress = address;
        }
    }
}
