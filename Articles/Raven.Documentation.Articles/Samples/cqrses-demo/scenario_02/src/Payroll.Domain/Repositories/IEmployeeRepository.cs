using System;
using Payroll.Domain.Model;

namespace Payroll.Domain.Repositories
{
    public interface IEmployeeRepository
    {
        void Save(Employee employee);
        Employee Load(Guid id);
    }
}