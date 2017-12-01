using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Orders;namespace Raven.Documentation.Samples.ClientApi.Session.Querying
{
    public class HowToGetQueryStatistics
    {
        private interface IFoo<TResult>
        {
            #region stats_1
            IRavenQueryable<TResult> Statistics(out QueryStatistics stats);
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
                        .Where(x => x.FirstName == "Robert")
                        .Statistics(out QueryStatistics stats)
                        .ToList();

                    int totalResults = stats.TotalResults;
                    long durationInMilliseconds = stats.DurationInMs;
                    #endregion
                }
            }
        }
    }
}
