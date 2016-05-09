using Infrastructure.Messaging;
using Payroll.Domain.Commands;
using Payroll.Domain.Model;
using Payroll.Domain.Repositories;

namespace Payroll.Domain.CommandHandlers
{
    public class EmployeeCommandsHandler 
        :
        IMessageHandler<RegisterEmployeeCommand>,
        IMessageHandler<RaiseSalaryCommand>,
        IMessageHandler<ChangeHomeAddressCommand>

    {
        private readonly IEmployeeRepository _repository;

        public EmployeeCommandsHandler(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public void Handle(RegisterEmployeeCommand command)
        {
            var newEmployee = new Employee(command.Id, command.Name, command.InitialSalary);
            _repository.Save(newEmployee);
        }

        public void Handle(RaiseSalaryCommand command)
        {
            var employee = _repository.Load(command.EmployeeId);
            employee.RaiseSalary(command.Amount);
            _repository.Save(employee);
        }

        public void Handle(ChangeHomeAddressCommand command)
        {
            var employee = _repository.Load(command.EmployeeId);
            employee.ChangeHomeAddress(command.NewAddress);
            _repository.Save(employee);
        }
    }
}
