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
        public class Orders_ByShipToCountry : AbstractIndexCreationTask<Order, Orders_ByShipToCountry.IndexEntry>
        {
            public class IndexEntry
            {
                public string Country { get; set; }
                public int CountryCount { get; set; }
            }

            public Orders_ByShipToCountry()
            {
                // The Map phase indexes the country listed in each order document
                // CountryCount is assigned with 1, which will be aggregated in the Reduce phase
                Map = orders => from order in orders
                    select new IndexEntry
                    {
                        Country = order.ShipTo.Country,
                        CountryCount = 1
                    };

                // The Reduce phase will group the country results and aggregate the CountryCount
                Reduce = results => from result in results
                    group result by result.Country
                    into g
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
                    // Get a sorted list without duplicates:
                    // =====================================
                    
                    List<string> countries = session
                        .Query<Order>()
                        .OrderBy(x => x.ShipTo.Country)
                        .Select(x => x.ShipTo.Country)
                         // Call 'Distinct' to remove duplicates from results
                         // Items wil be compared based on field 'Country' that is specified in the above 'Select' 
                        .Distinct()
                        .ToList();
                    
                    // Running this on the Northwind sample data
                    // will result in a sorted list of 21 countries w/o duplicates.
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region distinct_1_2
                    // Get a sorted list without duplicates:
                    // =====================================
                    
                    IList<string> countries = session
                        .Advanced
                        .DocumentQuery<Order>()
                        .OrderBy(x => x.ShipTo.Country)
                        .SelectFields<string>("ShipTo.Country")
                         // Call 'Distinct' to remove duplicates from results
                         // Items wil be compared based on field 'Country' that is specified in the above 'SelectFields' 
                        .Distinct()
                        .ToList();
                    
                    // Running this on the Northwind sample data
                    // will result in a sorted list of 21 countries w/o duplicates.
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region distinct_2_1
                    // Count the number of unique countries:
                    // =====================================
                    
                    var numberOfCountries = session
                        .Query<Order>()
                        .Select(x => x.ShipTo.Country)
                        .Distinct()
                        .Count();
                    
                    // Running this on the Northwind sample data,
                    // will result in 21, which is the number of unique countries.
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region distinct_2_2
                    // Count the number of unique countries:
                    // =====================================
                    
                    var numberOfCountries = session
                        .Advanced
                        .DocumentQuery<Order>()
                        .SelectFields<string>("ShipTo.Country")
                        .Distinct()
                        .Count();
                    
                    // Running this on the Northwind sample data,
                    // will result in 21, which is the number of unique countries.
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region distinct_3_1
                    // Query the map-reduce index defined above
                    var queryResult = session
                        .Query<Orders_ByShipToCountry.IndexEntry, Orders_ByShipToCountry>()
                        .ToList();
                    
                    // The resulting list contains all index-entry items where each entry represents a country. 
                    // The size of the list corresponds to the number of unique countries.
                    var numberOfUniqueCountries = queryResult.Count;
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region distinct_3_2
                    // Query the map-reduce index defined above
                    var queryResult = session.Advanced
                        .DocumentQuery<Orders_ByShipToCountry.IndexEntry, Orders_ByShipToCountry>()
                        .ToList();
                    
                    // The resulting list contains all index-entry items where each entry represents a country. 
                    // The size of the list corresponds to the number of unique countries.
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
                    // Get a sorted list without duplicates:
                    // =====================================
                    
                    List<string> countries = await asyncSession
                        .Query<Order>()
                        .OrderBy(x => x.ShipTo.Country)
                        .Select(x => x.ShipTo.Country)
                         // Call 'Distinct' to remove duplicates from results
                         // Items wil be compared based on field 'Country' that is specified in the above 'Select' 
                        .Distinct()
                        .ToListAsync();
                    
                    // Running this on the Northwind sample data
                    // will result in a sorted list of 21 countries w/o duplicates.
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region distinct_2_1_async
                    // Count the number of unique countries:
                    // =====================================
                    
                    var numberOfCountries = await asyncSession
                        .Query<Order>()
                        .Select(x => x.ShipTo.Country)
                        .Distinct()
                        .CountAsync();
                    
                    // Running this on the Northwind sample data,
                    // will result in 21, which is the number of unique countries.
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region distinct_3_1_async
                    // Query the map-reduce index defined above
                    var queryResult = await asyncSession
                        .Query<Orders_ByShipToCountry.IndexEntry, Orders_ByShipToCountry>()
                        .ToListAsync();

                    // The resulting list contains all index-entry items where each entry represents a country. 
                    // The size of the list corresponds to the number of unique countries.
                    var numberOfUniqueCountries = queryResult.Count;
                    #endregion
                }
            }
        }
    }
}
