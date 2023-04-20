using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.Indexes
{
    public class IndexingBasics
    {
        public IndexingBasics()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region indexes_1
                    IQueryable<Employee> employees =
                        from employee in session.Query<Employee>("Employees/ByFirstName")
                        where employee.FirstName == "Robert"
                        select employee;
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region indexes_2
                    List<Employee> employees = session
                        .Query<Employee>("Employees/ByFirstName")
                        .Where(x => x.FirstName == "Robert")
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region indexes_3
                    List<Employee> employees = session
                        .Advanced
                        .DocumentQuery<Employee>("Employees/ByFirstName")
                        .WhereEquals(x => x.FirstName, "Robert")
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region indexes_4
                    List<Employee> employees = session
                        .Advanced
                        .RawQuery<Employee>("from index 'Employees/ByFirstName' where FirstName = 'Robert'")
                        .ToList();
                    #endregion
                }
            }
        }
    }
}
