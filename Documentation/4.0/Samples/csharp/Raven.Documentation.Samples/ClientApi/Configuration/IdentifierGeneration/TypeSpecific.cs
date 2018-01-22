using System;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Conventions;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Configuration.IdentifierGeneration
{
    public class TypeSpecific
    {
        public TypeSpecific()
        {
            var store = new DocumentStore();

            #region employees_custom_async_convention

            store.Conventions.RegisterAsyncIdConvention<Employee>(
                (dbname, employee) =>
                    Task.FromResult(string.Format("employees/{0}/{1}", employee.LastName, employee.FirstName)));

            #endregion

            #region employees_custom_convention_example

            using (var session = store.OpenSession())
            {
                session.Store(new Employee
                {
                    FirstName = "James",
                    LastName = "Bond"
                });

                session.SaveChanges();
            }

            #endregion

            #region employees_custom_convention_inheritance

            using (var session = store.OpenSession())
            {
                session.Store(new Employee // employees/Smith/Adam
                {
                    FirstName = "Adam",
                    LastName = "Smith"
                });

                session.Store(new EmployeeManager // employees/Jones/David
                {
                    FirstName = "David",
                    LastName = "Jones"
                });

                session.SaveChanges();
            }

            #endregion


            #region custom_convention_inheritance_2

            store.Conventions.RegisterAsyncIdConvention<Employee>(
                (dbname, employee) =>
                    Task.FromResult(string.Format("employees/{0}/{1}", employee.LastName, employee.FirstName)));

            store.Conventions.RegisterAsyncIdConvention<EmployeeManager>(
                (dbname, employee) =>
                    Task.FromResult(string.Format("managers/{0}/{1}", employee.LastName, employee.FirstName)));

            using (var session = store.OpenSession())
            {
                session.Store(new Employee // employees/Smith/Adam
                {
                    FirstName = "Adam",
                    LastName = "Smith"
                });

                session.Store(new EmployeeManager // managers/Jones/David
                {
                    FirstName = "David",
                    LastName = "Jones"
                });

                session.SaveChanges();
            }

            #endregion
        }

        public interface IFoo
        {
            #region register_id_convention_method_async

            DocumentConventions RegisterAsyncIdConvention<TEntity>(Func<string, TEntity, Task<string>> func);

            #endregion

            #region register_id_load

            DocumentConventions RegisterIdLoadConvention<TEntity>(Func<ValueType, string> func);

            #endregion
        }

        public class EmployeeManager : Employee
        {
        }

        #region class_with_interger_id

        public class EntityWithIntegerId
        {
            public int Id { get; set; }
            /*
            ...
            */
        }

        #endregion
    }
}
