using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using static NodaTime.TimeZones.ZoneEqualityComparer;
using System.Threading.Tasks;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying
{
    public class CountQueryResults
    {
        public void CanUseCount(Options options)
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

                    #region Count
                    // Use Count in a synchronous session
                    var query =
                        s.Query<User>()
                            .Statistics(out stats)
                            .Search(u => u.Name, "John");
                    System.Int32 count = System.Linq.Enumerable.Count(query);
                    #endregion
                }
            }
        }

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
