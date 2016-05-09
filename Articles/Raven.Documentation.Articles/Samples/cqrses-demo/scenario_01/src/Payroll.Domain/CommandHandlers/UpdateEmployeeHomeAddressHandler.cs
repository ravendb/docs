using Payroll.Domain.Commands;
using Payroll.Domain.Events;
using Payroll.Domain.Repositories;
using Payroll.Infrastructure;

namespace Payroll.Domain.CommandHandlers
{
    public class UpdateEmployeeHomeAddressHandler 
        : IMessageHandler<UpdateEmployeeHomeAddressCommand>
    {
        private readonly IBus _bus;
        private readonly IEmployeeRepository _repository;

        public UpdateEmployeeHomeAddressHandler(
            IBus bus, IEmployeeRepository repository)
        {
            _bus = bus;
            _repository = repository;
        }

        public void Handle(UpdateEmployeeHomeAddressCommand message)
        {
            _repository.UpdateHomeAddress(message.Id, message.HomeAddress);
            _bus.RaiseEvent(new EmployeeHomeAddressUpdatedEvent(
                message.Id,
                message.HomeAddress
                ));
        }
    }
}
