using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Linq;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.Indexes.Querying.ResultsFilter
{
    public class ResultsFilter
    {

        public ResultsFilter()
        {
            using var store = new DocumentStore();

            using (var session = store.OpenSession())
            {
                #region exploration-queries_1.1
                var result = session.Query<Employee>()
                    .Filter(f => f.Address.Country == "USA")
                    .SingleOrDefault();
                #endregion

                #region exploration-queries_1.2
                result = session.Advanced.DocumentQuery<Employee>()
                    .Filter(p => p.Equals(a => a.Address.Country, "USA"))
                    .SingleOrDefault();
                #endregion

                #region exploration-queries_1.3
                result = session.Advanced.RawQuery<Employee>
                    ("from Employees filter Name = 'Jane'").SingleOrDefault();
                #endregion
            }

            using (var session = store.OpenSession())
            {
                #region exploration-queries_2.1
                var emp = session.Query<Employee>()
                    .Where(w => w.ReportsTo == "Central Office")
                    .Filter(f => f.Address.Country == "USA")
                    .SingleOrDefault();
                ;
                #endregion

                #region exploration-queries_2.2
                emp = session.Advanced.DocumentQuery<Employee>()
                      .WhereEquals(w => w.ReportsTo, "Central Office")
                      .Filter(p => p.Equals(a => a.Address.Country, "USA"))
                      .SingleOrDefault();
                #endregion

                #region exploration-queries_2.3
                emp = session.Advanced.RawQuery<Employee>
                    ("from Employees as e where e.ReportsTo = $office " +
                     "filter e.Address.Country = $location")
                    .AddParameter("office", "Central Office")
                    .AddParameter("location", "USA")
                    .SingleOrDefault();
                #endregion
            }
        }
    }
}
