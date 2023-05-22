using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Queries;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Orders;
using Sparrow.Json;
using Sparrow.Json.Parsing;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying
{
    public class HowToCustomize
    {
        private interface IFoo
        {
            #region customize_1_0
            IDocumentQueryCustomization BeforeQueryExecuted(Action<IndexQuery> action);
            #endregion

            #region customize_1_0_0
            IDocumentQueryCustomization AfterQueryExecuted(Action<QueryResult> action);
            #endregion

            #region customize_1_0_1
            IDocumentQueryCustomization AfterStreamExecuted(Action<BlittableJsonReaderObject> action);
            #endregion

            #region customize_2_0
            IDocumentQueryCustomization NoCaching();
            #endregion

            #region customize_3_0
            IDocumentQueryCustomization NoTracking();
            #endregion

            #region projectionbehavior
            IDocumentQueryCustomization Projection(ProjectionBehavior projectionBehavior);

            public enum ProjectionBehavior {
                Default,
                FromIndex,
                FromIndexOrThrow,
                FromDocument,
                FromDocumentOrThrow
            }
            #endregion

            #region customize_4_0
            IDocumentQueryCustomization RandomOrdering();

            IDocumentQueryCustomization RandomOrdering(string seed);
            #endregion

            #region customize_8_0
            IDocumentQueryCustomization WaitForNonStaleResults(TimeSpan? waitTimeout);
            #endregion
        }

        public HowToCustomize()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region customize_1_1
                    // set DisableCaching to true
                    List<Employee> results = session.Query<Employee>()
                        .Customize(x => x.BeforeQueryExecuted(query => query.DisableCaching = true))
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region customize_1_1_0
                    TimeSpan queryDuration;

                    List<Employee> results = session.Query<Employee>()
                        .Customize(x => x.AfterQueryExecuted(
                            result => queryDuration = TimeSpan.FromMilliseconds(result.DurationInMs)))
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region customize_1_1_1

                    long totalStreamedResultsSize = 0;

                    List<Employee> results = session.Query<Employee>()
                        .Customize(x => x.AfterStreamExecuted(
                            result => totalStreamedResultsSize += result.Size))
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
                    #region projectionbehavior_query
                    List<Employee> results = session.Query<Employee>()
                        .Customize(x => x.Projection(ProjectionBehavior.FromDocument))
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region projectionbehavior_rawquery
                    List<Employee> results = session.Advanced.RawQuery<Employee>(
                        @"from Employees")
                        .Projection(ProjectionBehavior.FromDocument)
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region projectionbehavior_docquery
                    List<Employee> results = session.Advanced.DocumentQuery<Employee>()
                        .SelectFields<Employee>(ProjectionBehavior.FromDocument)
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
