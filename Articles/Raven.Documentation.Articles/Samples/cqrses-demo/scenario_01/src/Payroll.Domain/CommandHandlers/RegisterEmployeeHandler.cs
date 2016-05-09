using Payroll.Domain.Commands;
using Payroll.Domain.Events;
using Payroll.Domain.Repositories;
using Payroll.Infrastructure;

namespace Payroll.Domain.CommandHandlers
{
    public class RegisterEmployeeHandler 
        : IMessageHandler<RegisterEmployeeCommand>
    {
        private readonly IBus _bus;
        private readonly IEmployeeRepository _repository;

        public RegisterEmployeeHandler(IBus bus, IEmployeeRepository repository)
        {
            _bus = bus;
            _repository = repository;
        }

        public void Handle(RegisterEmployeeCommand message)
        {
            if (!_repository.IsRegistered(message.Id))
            {
                _repository.CreateEmployee(message.Id, message.Name, message.InitialSalary);

                _bus.RaiseEvent(
                    new EmployeeRegisteredEvent(
                        message.Id,
                        message.Name,
                        message.InitialSalary
                        )
                    );
            }
            else
            {
                 _bus.RaiseEvent(new FailedToRegisterEmployeeEvent(message.Id));
            }
        }
    }
}
