using Payroll.Domain.Model;

namespace Payroll.Domain.Repositories
{
    public interface IEmployeeRepository
    {
        bool IsRegistered(EmployeeId id);
        Employee Load(EmployeeId id);

        void CreateEmployee(
            EmployeeId id,
            FullName name,
            decimal initialSalary);

        void RaiseSalary(EmployeeId id, decimal amount);
        void UpdateHomeAddress(EmployeeId id, Address homeAddress);
    }
}
