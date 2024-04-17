using Raven.Client.Documents;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.HowTo
{
    public class Evict
    {
        private interface IFoo
        {
            #region evict_1
            void Evict<T>(T entity);
            #endregion
        }

        public Evict()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region evict_2
                    Employee employee1 = new Employee
                    {
                        FirstName = "John",
                        LastName = "Doe"
                    };

                    Employee employee2 = new Employee
                    {
                        FirstName = "Joe",
                        LastName = "Shmoe"
                    };

                    session.Store(employee1);
                    session.Store(employee2);

                    session.Advanced.Evict(employee1);

                    session.SaveChanges(); // only 'Joe Shmoe' will be saved
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region evict_3
                    Employee employee = session.Load<Employee>("employees/1-A"); // loading from server
                    employee = session.Load<Employee>("employees/1-A"); // no server call
                    session.Advanced.Evict(employee);
                    employee = session.Load<Employee>("employees/1-A"); // loading from server
                    #endregion
                }
            }
        }
    }
}
