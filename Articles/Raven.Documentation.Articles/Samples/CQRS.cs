using System;

using Raven.Client;
using Raven.Imports.Newtonsoft.Json;

namespace Raven.Documentation.Articles.articles.Samples
{
    public interface IEmployeeRepository
    {
        void CreateEmployee(EmployeeId id, FullName name, decimal initialSalary);
        bool IsRegistered(EmployeeId id);
        Employee Load(EmployeeId id);
        void RaiseSalary(EmployeeId id, decimal amount);
        void UpdateHomeAddress(EmployeeId id, Address homeAddress);
    }

    public abstract class Address
    {
        public static object NotInformed { get; set; }
    }

    public class Employee
    {
        public Employee(EmployeeId id, FullName name, object notInformed, decimal initialSalary)
        {
            throw new NotImplementedException();
        }
    }

    public class FullName
    {
    }

    public class EmployeeId
    {
    }

    #region cqrs_1_0
    public class EmployeeRepository : IEmployeeRepository, IDisposable
    {
        private readonly IDocumentStore _store;
        private JsonSerializer _serializer;

        public EmployeeRepository() { /* implementation omitted */ }

        public bool IsRegistered(EmployeeId id)
        {
            return _store.DatabaseCommands.Head($"employees/{id}") != null;
        }

        public Employee Load(EmployeeId id)
        {
            Employee result;
            using (var session = _store.OpenSession())
            { result = session.Load<Employee>($"employees/{id}"); }
            return result;
        }

        public void CreateEmployee(EmployeeId id,
            FullName name,
            decimal initialSalary)
        {
            using (var session = _store.OpenSession())
            {
                var employee = new Employee(id, name,
                    Address.NotInformed,
                    initialSalary);
                session.Store(employee);
                session.SaveChanges();
            }
        }

        public void RaiseSalary(EmployeeId id, decimal amount) { /* implementation omitted */ }

        public void UpdateHomeAddress(EmployeeId id, Address homeAddress) { /* implementation omitted */ }

        public void Dispose() { _store.Dispose(); }
    }
    #endregion
}