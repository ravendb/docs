using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.Indexes.Querying
{
    public class Distinct
    {
        #region distinct_3_1
        // Define static index
        public class Employees_ByCountry : AbstractIndexCreationTask<Employee, Employees_ByCountry.IndexEntry>
        {
            public class IndexEntry
            {
                public string Country { get; set; }
                public int CountryCount { get; set; }
            }

            public Employees_ByCountry()
            {
                // The Map phase indexes the country listed in each employee document
                // CountryCount is assigned with 1, which will be aggregated in the Reduce phase
                Map = employees => from employee in employees
                                   select new IndexEntry
                                   {
                                       Country = employee.Address.Country,
                                       CountryCount = 1
                                   };

                // The Reduce phase will group the country results and aggregate the CountryCount
                Reduce = results => from result in results
                                    group result by result.Country into g
                                    select new IndexEntry
                                    {
                                        Country = g.Key,
                                        CountryCount = g.Sum(x => x.CountryCount)
                                    };
            }
        }
        #endregion

        public Distinct()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region distinct_1_1
                    // returns sorted list of countries w/o duplicates
                    IList<string> countries = session
                        .Query<Order>()
                        .OrderBy(x => x.ShipTo.Country)
                        .Select(x => x.ShipTo.Country)
                        .Distinct()
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region distinct_1_2
                    // returns sorted list of countries w/o duplicates
                    IList<string> countries = session
                        .Advanced
                        .DocumentQuery<Order>()
                        .OrderBy(x => x.ShipTo.Country)
                        .SelectFields<string>("ShipTo.Country")
                        .Distinct()
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region distinct_2_1
                    // results will contain the number of unique countries
                    var numberOfCountries = session
                        .Query<Order>()
                        .Select(x => x.ShipTo.Country)
                        .Distinct()
                        .Count();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region distinct_2_2
                    // results will contain the number of unique countries
                    var numberOfCountries = session
                        .Advanced
                        .DocumentQuery<Order>()
                        .SelectFields<string>("ShipTo.Country")
                        .Distinct()
                        .Count();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region distinct_3_2
                    // Query the map-reduce index defined above
                    var queryResult = session.Query<Employees_ByCountry.IndexEntry, Employees_ByCountry>()
                          .FirstOrDefault(x => x.Country == "UK");

                    var numberOfEmployeesFromCountry = queryResult?.CountryCount ?? 0;
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region distinct_3_3
                    // Query the map-reduce index defined above
                    var queryResult = session.Advanced.DocumentQuery<Employees_ByCountry.IndexEntry, Employees_ByCountry>()
                        .WhereEquals("Country", "UK").FirstOrDefault();

                    var numberOfEmployeesFromCountry = queryResult?.CountryCount ?? 0;
                    #endregion
                }
            }
        }
    }
}
