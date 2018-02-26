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
        public class Order_Countries : AbstractIndexCreationTask<Order, Order_Countries.Result>
        {
            public class Result
            {
                public string Country { get; set; }
            }

            public Order_Countries()
            {
                Map = orders => from o in orders
                                select new Result
                                {
                                    Country = o.ShipTo.Country
                                };

                Reduce = results => from r in results
                                    group r by r.Country into g
                                    select new Result
                                    {
                                        Country = g.Key
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
                    var numberOfCountries = session
                        .Query<Order_Countries.Result, Order_Countries>()
                        .Count();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region distinct_3_3
                    var numberOfCountries = session
                        .Advanced
                        .DocumentQuery<Order_Countries.Result, Order_Countries>()
                        .Count();
                    #endregion
                }
            }
        }
    }
}
