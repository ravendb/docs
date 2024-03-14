using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying
{
    public class HowToGetQueryStatistics
    {
        public async Task GetQueryStatistics()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region stats_1
                    List<Employee> employees = session
                        .Query<Employee>()
                        .Where(x => x.FirstName == "Anne")
                         // Get query stats:
                         // * Call 'Statistics'
                         // * Pass an out 'QueryStatistics' param for getting the stats
                        .Statistics(out QueryStatistics stats)
                        .ToList();

                    int numberOfResults = stats.TotalResults; // Get results count
                    long queryDuration = stats.DurationInMs;  // Get query duration
                    string indexNameUsed = stats.IndexName;   // Get index name used in query
                    // ...
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region stats_2
                    List<Employee> employees = await asyncSession
                        .Query<Employee>()
                        .Where(x => x.FirstName == "Anne")
                         // Get query stats:
                         // * Call 'Statistics'
                         // * Pass an out 'QueryStatistics' param for getting the stats
                        .Statistics(out QueryStatistics stats)
                        .ToListAsync();

                    int numberOfResults = stats.TotalResults; // Get results count
                    long queryDuration = stats.DurationInMs;  // Get query duration
                    string indexNameUsed = stats.IndexName;   // Get index name used in query
                    // ...
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region stats_3
                    List<Employee> employees = session.Advanced
                        .DocumentQuery<Employee>()
                        .WhereEquals(x => x.FirstName, "Anne")
                         // Get query stats:
                         // * Call 'Statistics'
                         // * Pass an out 'QueryStatistics' param for getting the stats
                        .Statistics(out QueryStatistics stats)
                        .ToList();

                    int numberOfResults = stats.TotalResults; // Get results count
                    long queryDuration = stats.DurationInMs;  // Get query duration
                    string indexNameUsed = stats.IndexName;   // Get index name used in query
                    // ...
                    #endregion
                }
            }
        }

        private interface IFoo<TResult>
        {
            #region syntax_1
            IRavenQueryable<T> Statistics(out QueryStatistics stats);
            #endregion
        }
    }

    public class Foo
    {
        #region syntax_2
        public class QueryStatistics
        {
            public bool IsStale { get; set; }
            public long DurationInMs { get; set; }
            public int TotalResults { get; set; }
            public long LongTotalResults { get; set; }
            public int SkippedResults { get; set; }
            public DateTime Timestamp { get; set; }
            public string IndexName { get; set; }
            public DateTime IndexTimestamp { get; set; }
            public DateTime LastQueryTime { get; set; }
            public long? ResultEtag { get; set; }
            public string NodeTag { get; set; }
        }
        #endregion
    }
}
