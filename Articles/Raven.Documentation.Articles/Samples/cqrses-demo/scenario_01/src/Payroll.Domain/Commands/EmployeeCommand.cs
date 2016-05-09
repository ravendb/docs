using Payroll.Domain.Model;
using Payroll.Infrastructure;

namespace Payroll.Domain.Commands
{
    public abstract class EmployeeCommand : Command
    {
        public EmployeeId Id { get; private set; }

        protected EmployeeCommand(EmployeeId id)
        {
            Id = id;
        }
    }
}
