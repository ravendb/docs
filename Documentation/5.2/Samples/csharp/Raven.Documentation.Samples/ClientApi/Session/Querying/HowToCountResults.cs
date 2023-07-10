using System;
using System.Collections.Generic;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Orders;
using static NodaTime.TimeZones.ZoneEqualityComparer;
using Xunit;
using System.Threading.Tasks;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying
{
    public class CountQueryResults
    {
        public async Task CanUseCountAsync(Options options)
        {
            using (var store = new DocumentStore())
            {
                using (var s = store.OpenAsyncSession())
                {
                    await s.StoreAsync(new User
                    {
                        Name = "John"
                    });

                    await s.SaveChangesAsync();
                }

                using (var s = store.OpenAsyncSession())
                {
                    QueryStatistics stats;

                    #region CountAsync
                    // Use CountAsync in an Async session
                    System.Int32 count = await
                        s.Query<User>()
                            .Statistics(out stats)
                            .Search(u => u.Name, "John")
                            .CountAsync();
                    #endregion
                }
            }
        }

        public void CanUseLongCount(Options options)
        {
            using (var store = new DocumentStore())
            {
                using (var s = store.OpenSession())
                {
                    s.Store(new User
                    {
                        Name = "John"
                    });

                    s.SaveChanges();
                }

                using (var s = store.OpenSession())
                {
                    QueryStatistics stats;

                    #region LongCount
                    // Use LongCount in a synchronous session
                    System.Int64 longCount = 
                        s.Query<User>()
                            .Statistics(out stats)
                            .Search(u => u.Name, "John")
                            .LongCount();
                    #endregion
                }
            }
        }

        public async Task CanUseLongCountAsync(Options options)
        {
            using (var store = new DocumentStore())
            {
                using (var s = store.OpenAsyncSession())
                {
                    await s.StoreAsync(new User
                    {
                        Name = "John"
                    });

                    await s.SaveChangesAsync();
                }

                using (var s = store.OpenAsyncSession())
                {
                    QueryStatistics stats;

                    #region LongCountAsync
                    // Use LongCountAsync in an Async session
                    System.Int64 longCount = await
                        s.Query<User>()
                            .Statistics(out stats)
                            .Search(u => u.Name, "John")
                            .LongCountAsync();
                    #endregion
                }
            }
        }


    }
}
