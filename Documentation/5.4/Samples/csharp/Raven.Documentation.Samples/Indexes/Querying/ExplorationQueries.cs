using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Linq;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.Indexes.Querying.ExplorationQueries
{
    public class ExplorationQueries
    {
        public ExplorationQueries()
        {
            using var store = new DocumentStore();

            // filter in a collection query
            using (var session = store.OpenSession())
            {
                #region exploration-queries_1.1
                var result = session.Query<Employee>()
                    .Filter(f => f.Address.Country == "USA", limit: 500)
                    .SingleOrDefault();
                #endregion

                #region exploration-queries_1.2
                result = session.Advanced.DocumentQuery<Employee>()
                    .Filter(p => p.Equals(a => a.Address.Country, "USA"), limit: 500)
                    .SingleOrDefault();
                #endregion

                #region exploration-queries_1.3
                result = session.Advanced.RawQuery<Employee>
                    ("from Employees as e " +
                       "filter e.Address.Country = 'USA' " +
                       "filter_limit 500").SingleOrDefault();
                #endregion
                
            }

            // filter in an index query
            using (var session = store.OpenSession())
            {
                #region exploration-queries_2.1
                var emp = session.Query<Employee>()
                    .Where(w => w.Title == "Sales Representative")
                    .Filter(f => f.Address.Country == "USA", limit: 500)
                    .SingleOrDefault();
                #endregion

                #region exploration-queries_2.2
                emp = session.Advanced.DocumentQuery<Employee>()
                      .WhereEquals(w => w.Title, "Sales Representative")
                      .Filter(p => p.Equals(a => a.Address.Country, "USA"), limit: 500)
                      .SingleOrDefault();
                #endregion

                #region exploration-queries_2.3
                emp = session.Advanced.RawQuery<Employee>
                    ("from Employees as e" +
                     "where e.Title = $title" +
                     "filter e.Address.Country = $country" +
                     "filter_limit $limit")
                    .AddParameter("title", "Sales Representative")
                    .AddParameter("country", "USA")
                    .AddParameter("limit", 500)
                    .SingleOrDefault();
                #endregion
            }

            // filter and projection
            using (var session = store.OpenSession())
            {
                #region exploration-queries_3.1
                var emp1 = session
                    .Query<Employee>()
                    .Filter(f => f.Address.Country == "USA", limit: 500)
                    .Select(x => new
                     {
                        FullName = x.FirstName + " " + x.LastName
                     })
                    .ToList();
                #endregion

                #region exploration-queries_3.2
                var fullName = new string[]{
                    "FirstName",
                    "LastName"
                };

                var emp2 = session.Advanced.DocumentQuery<Employee>()
                      .Filter(p => p.Equals(a => a.Address.Country, "USA"), limit: 500)
                      .SelectFields<Employee>(fullName)
                      .ToList();
                #endregion

                #region exploration-queries_3.3
                var emp3 = session.Advanced.RawQuery<Employee>
                    ("from Employees as e" +
                        "filter startsWith(e.FirstName, 'A')" +
                        "select { FullName: e.FirstName + ' ' + e.LastName }");
                #endregion
            }
        }
    }
}
