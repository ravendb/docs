using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Queries;
using Raven.Client.Documents.Session;using Raven.Documentation.Samples.Orders;namespace Raven.Documentation.Samples.ClientApi.Session.Querying
{
    public class HowToCustomize
    {
        private interface IFoo
        {
            #region customize_1_0
            IDocumentQueryCustomization BeforeQueryExecuted(Action<IndexQuery> action);
            #endregion

            #region customize_2_0
            IDocumentQueryCustomization NoCaching();
            #endregion

            #region customize_3_0
            IDocumentQueryCustomization NoTracking();
            #endregion

            #region customize_4_0
            IDocumentQueryCustomization RandomOrdering();

            IDocumentQueryCustomization RandomOrdering(string seed);
            #endregion

            #region customize_6_0
            IDocumentQueryCustomization ShowTimings();
            #endregion

            #region customize_8_0
            IDocumentQueryCustomization WaitForNonStaleResults();

            IDocumentQueryCustomization WaitForNonStaleResults(TimeSpan waitTimeout);
            #endregion

            #region customize_9_0
            IDocumentQueryCustomization WaitForNonStaleResultsAsOf(long cutOffEtag);

            IDocumentQueryCustomization WaitForNonStaleResultsAsOf(
                long cutOffEtag,
                TimeSpan waitTimeout);
            #endregion
        }

        public HowToCustomize()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region customize_1_1
                    // set 'PageSize' to 10
                    List<Employee> results = session.Query<Employee>()
                        .Customize(x => x.BeforeQueryExecuted(query => query.PageSize = 10))
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region customize_2_1
                    List<Employee> results = session.Query<Employee>()
                        .Customize(x => x.NoCaching())
                        .Where(x => x.FirstName == "Robert")
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region customize_3_1
                    List<Employee> results = session.Query<Employee>()
                        .Customize(x => x.NoTracking())
                        .Where(x => x.FirstName == "Robert")
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region customize_4_1
                    // results will be ordered randomly each time
                    List<Employee> results = session.Query<Employee>()
                        .Customize(x => x.RandomOrdering())
                        .Where(x => x.FirstName == "Robert")
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region customize_6_1
                    QueryStatistics stats;
                    List<Employee> results = session.Query<Employee>()
                        .Customize(x => x.ShowTimings())
                        .Statistics(out stats)
                        .Where(x => x.FirstName == "Robert")
                        .ToList();

                    Dictionary<string, double> timings = stats.TimingsInMs;
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region customize_8_1
                    List<Employee> results = session.Query<Employee>()
                        .Customize(x => x.WaitForNonStaleResults())
                        .Where(x => x.FirstName == "Robert")
                        .ToList();
                    #endregion
                }
            }
        }
    }
}
