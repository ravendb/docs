using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.Indexes.Querying
{
    public class Distinct
    {
        #region index
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
                    select new IndexEntry {Country = employee.Address.Country, CountryCount = 1};

                // The Reduce phase will group the country results and aggregate the CountryCount
                Reduce = results => from result in results
                    group result by result.Country
                    into g
                    select new IndexEntry {Country = g.Key, CountryCount = g.Sum(x => x.CountryCount)};
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
                    // Results will contain a sorted list of countries w/o duplicates
                    List<string> countries = session
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
                    // Results will contain a sorted list of countries w/o duplicates
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
                    // Results will contain the number of unique countries
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
                    // Results will contain the number of unique countries
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
                    #region distinct_3_1
                    // Query the map-reduce index defined above
                    var queryResult = session
                        .Query<Employees_ByCountry.IndexEntry, Employees_ByCountry>()
                        .ToList();
                    
                    // The number of resulting items in the query result represents the number of unique countries.
                    var numberOfUniqueCountries = queryResult.Count;
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region distinct_3_2
                    // Query the map-reduce index defined above
                    var queryResult = session.Advanced
                        .DocumentQuery<Employees_ByCountry.IndexEntry, Employees_ByCountry>()
                        .ToList();
                    
                    // The number of resulting items in the query result represents the number of unique countries.
                    var numberOfUniqueCountries = queryResult.Count;
                    #endregion
                }
            }
        }

        public async Task DistinctAsync()
        {
            using (var store = new DocumentStore())
            {
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region distinct_1_1_async
                    // Results will contain a sorted list of countries w/o duplicates
                    List<string> countries = await asyncSession
                        .Query<Order>()
                        .OrderBy(x => x.ShipTo.Country)
                        .Select(x => x.ShipTo.Country)
                        .Distinct()
                        .ToListAsync();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region distinct_2_1_async
                    // Results will contain the number of unique countries
                    var numberOfCountries = await asyncSession
                        .Query<Order>()
                        .Select(x => x.ShipTo.Country)
                        .Distinct()
                        .CountAsync();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region distinct_3_1_async
                    // Query the map-reduce index defined above
                    var queryResult = await asyncSession
                        .Query<Employees_ByCountry.IndexEntry, Employees_ByCountry>()
                        .ToListAsync();

                    // The number of resulting items in the query result represents the number of unique countries.
                    var numberOfUniqueCountries = queryResult.Count;
                    #endregion
                }
            }
        }
    }
}
