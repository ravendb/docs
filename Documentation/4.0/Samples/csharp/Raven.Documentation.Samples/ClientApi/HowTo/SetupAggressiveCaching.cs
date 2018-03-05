using System;
using System.Collections.Generic;
using System.Linq;

using Raven.Client.Documents;
using Raven.Documentation.Samples.Orders;
using Sparrow;

namespace Raven.Documentation.Samples.ClientApi.HowTo
{
    public class SetupAggressiveCaching
    {
        public SetupAggressiveCaching()
        {
            using (var documentStore = new DocumentStore
            {
                Urls = new[] { "http://localhost:8080" },
                Database = "NorthWind"
            })
            {
                documentStore.Initialize();
                #region aggressive_cache_global

                documentStore.AggressivelyCacheFor(TimeSpan.FromMinutes(5));

                documentStore.AggressivelyCache(); // Defines the cache duration for 1 day

                #endregion
                using (var session = documentStore.OpenSession())
                {
                    

                    #region aggressive_cache_load
                    using (session.Advanced.DocumentStore.AggressivelyCacheFor(TimeSpan.FromMinutes(5)))
                    {
                        Order user = session.Load<Order>("orders/1");
                    }
                    #endregion

                    #region disable_aggressive_cache

                    using (session.Advanced.DocumentStore.DisableAggressiveCaching())
                    {
                        Order user = session.Load<Order>("orders/1");
                    }

                    #endregion
                }

                using (var session = documentStore.OpenSession())
                {
                    #region aggressive_cache_query
                    using (session.Advanced.DocumentStore.AggressivelyCacheFor(TimeSpan.FromMinutes(5)))
                    {
                        List<Order> users = session.Query<Order>().ToList();
                    }
                    #endregion

                    #region aggressive_cache_for_one_day_1
                    using (session.Advanced.DocumentStore.AggressivelyCache())
                    { }
                    #endregion

                    #region aggressive_cache_for_one_day_2
                    using (session.Advanced.DocumentStore.AggressivelyCacheFor(TimeSpan.FromDays(1)))
                    { }
                    #endregion
                }

            }
        }
    }
}
