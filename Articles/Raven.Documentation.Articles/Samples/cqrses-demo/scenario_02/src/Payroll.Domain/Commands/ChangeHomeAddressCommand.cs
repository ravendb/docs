using System;
using Payroll.Domain.Model;

namespace Payroll.Domain.Commands
{
    public class ChangeHomeAddressCommand
    {
        public Guid EmployeeId { get; }
        public Address NewAddress { get; }

        public ChangeHomeAddressCommand(Guid employeeId, Address newAddress)
        {
            EmployeeId = employeeId;
            NewAddress = newAddress;
        }
    }
}
