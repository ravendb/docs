using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying
{
    public class HowToGetQueryStatistics
    {
        private interface IFoo<TResult>
        {
            #region stats_1
            IRavenQueryable<T> Statistics(out QueryStatistics stats);
            #endregion
        }

        public HowToGetQueryStatistics()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region stats_2
                    List<Employee> employees = session.Query<Employee>()
                        .Where(x => x.FirstName == "John")
                        .Statistics(out QueryStatistics stats) // Get query statistics
                        .ToList();

                    int totalResults = stats.TotalResults; // Get matching results count
                    long durationInMilliseconds = stats.DurationInMs; // Get query duration
                    #endregion
                }
            }
        }
    }

    public class foo
    {
        #region QueryStatisticsDefinition
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
