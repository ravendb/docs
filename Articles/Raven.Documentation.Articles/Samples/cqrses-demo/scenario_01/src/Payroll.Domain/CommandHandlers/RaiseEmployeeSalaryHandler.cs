using Payroll.Domain.Commands;
using Payroll.Domain.Events;
using Payroll.Domain.Repositories;
using Payroll.Infrastructure;

namespace Payroll.Domain.CommandHandlers
{
    public class RaiseEmployeeSalaryHandler 
        : IMessageHandler<RaiseEmployeeSalaryCommand>
    {
        private readonly IBus _bus;
        private readonly IEmployeeRepository _repository;

        public RaiseEmployeeSalaryHandler(IBus bus, IEmployeeRepository repository)
        {
            _bus = bus;
            _repository = repository;
        }

        public void Handle(RaiseEmployeeSalaryCommand message)
        {
            _repository.RaiseSalary(message.Id, message.Amount);
            _bus.RaiseEvent(new EmployeeSalaryRaisedEvent(message.Id, message.Amount));
        }
    }
}
