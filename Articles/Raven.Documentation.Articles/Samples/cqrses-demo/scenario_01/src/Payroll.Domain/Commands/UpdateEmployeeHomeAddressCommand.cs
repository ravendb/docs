using Payroll.Domain.Model;

namespace Payroll.Domain.Commands
{
    public sealed class UpdateEmployeeHomeAddressCommand :
        EmployeeCommand
    {
        public Address HomeAddress { get; }

        public UpdateEmployeeHomeAddressCommand(
            EmployeeId id,
            Address address) : base(id)
        {
            HomeAddress = address;
        }
    }
}
