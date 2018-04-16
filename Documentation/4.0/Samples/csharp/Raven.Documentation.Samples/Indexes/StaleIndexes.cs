using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.Indexes
{
    public class StaleIndexes
    {
        public void SimpleStaleChecks()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region stale1
                    QueryStatistics stats;
                    List<Product> results = session.Query<Product>()
                        .Statistics(out stats)
                        .Where(x => x.PricePerUnit > 10)
                        .ToList();

                    if (stats.IsStale)
                    {
                        // Results are known to be stale
                    }
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region stale2
                    List<Product> results = session.Query<Product>()
                        .Customize(x => x.WaitForNonStaleResults(TimeSpan.FromSeconds(5)))
                        .Where(x => x.PricePerUnit > 10)
                        .ToList();
                    #endregion
                }

                #region stale3
                store.OnBeforeQuery += (sender, beforeQueryExecutedArgs) =>
                {
                    beforeQueryExecutedArgs.QueryCustomization.WaitForNonStaleResults();
                };
                #endregion

                using (var session = store.OpenSession())
                {
                    #region stale4
                    session.Advanced.WaitForIndexesAfterSaveChanges();
                    #endregion

                    #region stale5
                    session.Advanced.WaitForIndexesAfterSaveChanges(
                        timeout: TimeSpan.FromSeconds(5),
                        throwOnTimeout: false,
                        indexes: new[] { "Products/ByName" });
                    #endregion
                }

            }
        }
    }
}
