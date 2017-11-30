using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying.DQ
{
    public class HowToUseNotOperator
    {
        public HowToUseNotOperator()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region not_1
                    // load all entities from 'Employees' collection
                    // where FirstName NOT equals 'Robert'
                    List<Employee> employees = session
                        .Advanced
                        .DocumentQuery<Employee>()
                        .Not
                        .WhereEquals(x => x.FirstName, "Robert")
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region not_2
                    // load all entities from 'Employees' collection
                    // where FirstName NOT equals 'Robert'
                    // and LastName NOT equals 'King'
                    List<Employee> employees = session
                        .Advanced
                        .DocumentQuery<Employee>()
                        .Not
                        .OpenSubclause()
                        .WhereEquals(x => x.FirstName, "Robert")
                        .AndAlso()
                        .WhereEquals(x => x.LastName, "King")
                        .CloseSubclause()
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region not_3
                    // load all entities from 'Employees' collection
                    // where FirstName NOT equals 'Robert'
                    // and LastName NOT equals 'King'
                    // identical to 'Example II' but 'WhereNotEquals' is used
                    List<Employee> employees = session
                        .Advanced
                        .DocumentQuery<Employee>()
                        .WhereNotEquals(x => x.FirstName, "Robert")
                        .AndAlso()
                        .WhereNotEquals(x => x.LastName, "King")
                        .ToList();
                    #endregion
                }
            }
        }
    }
}
