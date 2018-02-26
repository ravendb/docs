using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.Indexes.Querying
{
    public class Distinct
    {
        public Distinct()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region distinct_1_0
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
                    #region distinct_1_1
                    // returns number of countries
                    var numberOfCountries = session
                        .Query<Order>()
                        .Select(x => x.ShipTo.Country)
                        .Distinct()
                        .Count();
                    #endregion
                }
            }
        }
    }
}
