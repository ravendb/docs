using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Queries;
using Raven.Client.Documents.Queries.Timings;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Orders;
using Sparrow.Json;
using Sparrow.Logging;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying
{
    public class HowToCustomize
    {
        public async Task HowToCustomizeExamples()
        {
            using (var store = new DocumentStore())
            {
                var _logger = LoggingSource.Instance.GetLogger("", "");
                
                using (var session = store.OpenSession())
                {
                    #region customize_1_1
                    List<Employee> results = session
                        .Query<Employee>()
                         // Call 'Customize' with 'BeforeQueryExecuted'
                        .Customize(x => x.BeforeQueryExecuted(query =>
                        {
                            // Can modify query parameters
                            query.SkipDuplicateChecking = true;
                            // Can apply any needed action, e.g. write to log
                            _logger.Info($"Query to be executed is: {query.Query}");
                        }))
                        .Where(x => x.FirstName == "Robert")
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region customize_1_2
                    List<Employee> results = await asyncSession
                        .Query<Employee>()
                         // Call 'Customize' with 'BeforeQueryExecuted'
                        .Customize(x => x.BeforeQueryExecuted(query =>
                        {
                            // Can modify query parameters
                            query.SkipDuplicateChecking = true;
                            // Can apply any needed action, e.g. write to log
                            _logger.Info($"Query to be executed is: {query.Query}");
                        }))
                        .Where(x => x.FirstName == "Robert")
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region customize_1_3
                    List<Employee> results = session.Advanced
                        .DocumentQuery<Employee>()
                        .WhereEquals(x => x.FirstName, "Robert")
                         // Call 'BeforeQueryExecuted'
                        .BeforeQueryExecuted(query =>
                        {
                            // Can modify query parameters
                            query.SkipDuplicateChecking = true;
                            // Can apply any needed action, e.g. write to log
                            _logger.Info($"Query to be executed is: {query.Query}");
                        })
                        .ToList();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region customize_1_4
                    List<Employee> results = session.Advanced
                        .RawQuery<Employee>("from 'Employees' where FirstName == 'Robert'")
                         // Call 'BeforeQueryExecuted'
                        .BeforeQueryExecuted(query =>
                        {
                            // Can modify query parameters
                            query.SkipDuplicateChecking = true;
                            // Can apply any needed action, e.g. write to log
                            _logger.Info($"Query to be executed is: {query.Query}");
                        })
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region customize_2_1
                    List<Employee> results = session
                        .Query<Employee>()
                         // Call 'Customize' with 'AfterQueryExecuted'
                        .Customize(x => x.AfterQueryExecuted(rawResult =>
                        {
                            // Can access the raw query result
                            var queryDuration = rawResult.DurationInMs;
                            // Can apply any needed action, e.g. write to log
                            _logger.Info($"{rawResult.LastQueryTime}");
                        }))
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region customize_2_2
                    List<Employee> results = await asyncSession
                        .Query<Employee>()
                         // Call 'Customize' with 'AfterQueryExecuted'
                        .Customize(x => x.AfterQueryExecuted(rawResult =>
                        {
                            // Can access the raw query result
                            var queryDuration = rawResult.DurationInMs;
                            // Can apply any needed action, e.g. write to log
                            _logger.Info($"{rawResult.LastQueryTime}");
                        }))
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region customize_2_3
                    List<Employee> results = session.Advanced
                        .DocumentQuery<Employee>()
                         // Call 'AfterQueryExecuted'
                        .AfterQueryExecuted(rawResult =>
                        {
                            // Can access the raw query result
                            var queryDuration = rawResult.DurationInMs;
                            // Can apply any needed action, e.g. write to log
                            _logger.Info($"{rawResult.LastQueryTime}");
                        })
                        .ToList();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region customize_2_4
                    List<Employee> results = session.Advanced
                        .RawQuery<Employee>("from 'Employees'")
                         // Call 'AfterQueryExecuted'
                        .AfterQueryExecuted(rawResult =>
                        {
                            // Can access the raw query result
                            var queryDuration = rawResult.DurationInMs;
                            // Can apply any needed action, e.g. write to log
                            _logger.Info($"{rawResult.LastQueryTime}");
                        })
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region customize_3_1
                    long totalStreamedResultsSize = 0;
                    
                    // Define the query
                    var query = session
                        .Query<Employee>()
                         // Call 'Customize' with 'AfterStreamExecuted'
                        .Customize(x => x.AfterStreamExecuted(streamResult =>
                            // Can access the stream result
                            totalStreamedResultsSize += streamResult.Size));
                        
                    // Call 'Stream' to execute the query
                    var streamResults = session.Advanced.Stream(query);
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region customize_3_2
                    long totalStreamedResultsSize = 0;
                    
                    // Define the query
                    var query = asyncSession
                        .Query<Employee>()
                         // Call 'Customize' with 'AfterStreamExecuted'
                        .Customize(x => x.AfterStreamExecuted(streamResult =>
                            // Can access the stream result
                            totalStreamedResultsSize += streamResult.Size));
                        
                    // Call 'Stream' to execute the query
                    var streamResults = await asyncSession.Advanced.StreamAsync(query);
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region customize_3_3
                    long totalStreamedResultsSize = 0;
                    
                    // Define the document query
                    var query = session.Advanced
                        .DocumentQuery<Employee>()
                         // Call 'AfterStreamExecuted'
                        .AfterStreamExecuted(streamResult =>
                            // Can access the stream result
                            totalStreamedResultsSize += streamResult.Size);
                        
                    // Call 'Stream' to execute the document query
                    var streamResults = session.Advanced.Stream(query);
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region customize_3_4
                    long totalStreamedResultsSize = 0;
                    
                    // Define the raw query
                    var query = session.Advanced
                        .RawQuery<Employee>("from 'Employees'")
                         // Call 'AfterStreamExecuted'
                        .AfterStreamExecuted(streamResult =>
                            // Can access the stream result
                            totalStreamedResultsSize += streamResult.Size);

                    // Call 'Stream' to execute the document query
                    var streamResults = session.Advanced.Stream(query);
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region customize_4_1
                    List<Employee> results = session
                        .Query<Employee>()
                         // Call 'Customize' with 'NoCaching'
                        .Customize(x => x.NoCaching())
                        .Where(x => x.FirstName == "Robert")
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region customize_4_2
                    List<Employee> results = await asyncSession
                        .Query<Employee>()
                         // Call 'Customize' with 'NoCaching'
                        .Customize(x => x.NoCaching())
                        .Where(x => x.FirstName == "Robert")
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region customize_4_3
                    List<Employee> results = session.Advanced
                        .DocumentQuery<Employee>()
                        .WhereEquals(x => x.FirstName, "Robert")
                         // Call 'NoCaching'
                        .NoCaching()
                        .ToList();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region customize_4_4
                    List<Employee> results = session.Advanced
                        .RawQuery<Employee>("from 'Employees' where FirstName == 'Robert'")
                         // Call 'NoCaching'
                        .NoCaching()
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region customize_5_1
                    List<Employee> results = session
                        .Query<Employee>()
                         // Call 'Customize' with 'NoTracking'
                        .Customize(x => x.NoTracking())
                        .Where(x => x.FirstName == "Robert")
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region customize_5_2
                    List<Employee> results = await asyncSession
                        .Query<Employee>()
                         // Call 'Customize' with 'NoTracking'
                        .Customize(x => x.NoTracking())
                        .Where(x => x.FirstName == "Robert")
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region customize_5_3
                    List<Employee> results = session.Advanced
                        .DocumentQuery<Employee>()
                        .WhereEquals(x => x.FirstName, "Robert")
                         // Call 'NoTracking'
                        .NoTracking()
                        .ToList();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region customize_5_4
                    List<Employee> results = session.Advanced
                        .RawQuery<Employee>("from 'Employees' where FirstName == 'Robert'")
                         // Call 'NoTracking'
                        .NoTracking()
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region customize_6_1
                    List<string> results = session
                        .Query<Employees_ByFullName.IndexEntry, Employees_ByFullName>()
                         // Call 'Customize'
                         // Pass the requested projection behavior to the 'Projection' method
                        .Customize(x => x.Projection(ProjectionBehavior.FromDocumentOrThrow))
                         // Select the fields that will be returned by the projection
                        .Select(x => x.FullName)
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region customize_6_2
                    List<string> results = await asyncSession
                        .Query<Employees_ByFullName.IndexEntry, Employees_ByFullName>()
                         // Call 'Customize'
                         // Pass the requested projection behavior to the 'Projection' method
                        .Customize(x => x.Projection(ProjectionBehavior.FromDocumentOrThrow))
                         // Select the fields that will be returned by the projection
                        .Select(x => x.FullName)
                        .ToListAsync();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region customize_6_3
                    List<Employee> results = session.Advanced
                        .DocumentQuery<Employee>("Employees/ByFullName")
                         // Pass the requested projection behavior to the 'SelectFields' method
                         // and specify the field that will be returned by the projection
                        .SelectFields<Employee>(ProjectionBehavior.FromDocumentOrThrow, "FullName")
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region customize_6_4
                    List<Employee> results = session.Advanced
                         // Define an RQL query that returns a projection
                        .RawQuery<Employee>(@"from index 'Employees/ByFullName' select FullName")
                         // Pass the requested projection behavior to the 'Projection' method
                        .Projection(ProjectionBehavior.FromDocumentOrThrow)
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region customize_7_1
                    List<Employee> results = session
                        .Query<Employee>()
                        // Call 'Customize' with 'RandomOrdering'
                        .Customize(x => x.RandomOrdering())
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region customize_7_2
                    List<Employee> results = await asyncSession
                        .Query<Employee>()
                         // Call 'Customize' with 'RandomOrdering'
                        .Customize(x => x.RandomOrdering())
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region customize_7_3
                    List<Employee> results = session.Advanced
                        .DocumentQuery<Employee>()
                         // Call 'RandomOrdering'
                        .RandomOrdering()
                        .ToList();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region customize_7_4
                    List<Employee> results = session.Advanced
                         // Define an RQL query that orders the results randomly
                        .RawQuery<Employee>("from 'Employees' order by random()")
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region customize_8_1
                    List<Employee> results = session
                        .Query<Employee>()
                         // Call 'Customize' with 'WaitForNonStaleResults'
                        .Customize(x => x.WaitForNonStaleResults(TimeSpan.FromSeconds(10)))
                        .Where(x => x.FirstName == "Robert")
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region customize_8_2
                    List<Employee> results = await asyncSession
                        .Query<Employee>()
                         // Call 'Customize' with 'WaitForNonStaleResults'
                        .Customize(x => x.WaitForNonStaleResults(TimeSpan.FromSeconds(10)))
                        .Where(x => x.FirstName == "Robert")
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region customize_8_3
                    List<Employee> results = session.Advanced
                        .DocumentQuery<Employee>()
                        .WhereEquals(x => x.FirstName, "Robert")
                         // Call 'WaitForNonStaleResults'
                        .WaitForNonStaleResults(TimeSpan.FromSeconds(10))
                        .ToList();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region customize_8_4
                    List<Employee> results = session.Advanced
                        .RawQuery<Employee>("from 'Employees' where FirstName == 'Robert'")
                         // Call 'WaitForNonStaleResults'
                        .WaitForNonStaleResults(TimeSpan.FromSeconds(10))
                        .ToList();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region customize_9_1
                    List<Employee> results = session
                        .Query<Employee>()
                         // Call 'Customize' with 'Timings'
                         // Provide an out param for the timings results
                        .Customize(x => x.Timings(out QueryTimings timings))
                        .Where(x => x.FirstName == "Robert")
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region customize_9_2
                    List<Employee> results = await asyncSession
                        .Query<Employee>()
                         // Call 'Customize' with 'Timings'
                         // Provide an out param for the timings results
                        .Customize(x => x.Timings(out QueryTimings timings))
                        .Where(x => x.FirstName == "Robert")
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region customize_9_3
                    List<Employee> results = session.Advanced
                        .DocumentQuery<Employee>()
                        .WhereEquals(x => x.FirstName, "Robert")
                         // Call 'Timings'.
                         // Provide an out param for the timings results
                        .Timings(out QueryTimings timings)
                        .ToList();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region customize_9_4
                    List<Employee> results = session.Advanced
                        .RawQuery<Employee>("from 'Employees' where FirstName == 'Robert'")
                         // Call 'Timings'.
                         // Provide an out param for the timings results
                        .Timings(out QueryTimings timings)
                        .ToList();
                    #endregion
                }
            }
        }
        
        private interface IFoo
        {
            #region customize_1_5
            IDocumentQueryCustomization BeforeQueryExecuted(Action<IndexQuery> action);
            #endregion

            #region customize_2_5
            IDocumentQueryCustomization AfterQueryExecuted(Action<QueryResult> action);
            #endregion

            #region customize_3_5
            IDocumentQueryCustomization AfterStreamExecuted(Action<BlittableJsonReaderObject> action);
            #endregion

            #region customize_4_5
            IDocumentQueryCustomization NoCaching();
            #endregion

            #region customize_5_5
            IDocumentQueryCustomization NoTracking();
            #endregion

            #region customize_6_5
            IDocumentQueryCustomization Projection(ProjectionBehavior projectionBehavior);

            public enum ProjectionBehavior {
                Default,
                FromIndex,
                FromIndexOrThrow,
                FromDocument,
                FromDocumentOrThrow
            }
            #endregion

            #region customize_7_5
            IDocumentQueryCustomization RandomOrdering();
            IDocumentQueryCustomization RandomOrdering(string seed);
            #endregion

            #region customize_8_5
            IDocumentQueryCustomization WaitForNonStaleResults(TimeSpan? waitTimeout);
            #endregion
            
            #region customize_9_5
            IDocumentQueryCustomization Timings(out QueryTimings timings);
            #endregion
        }
    }

    #region the_index
    public class Employees_ByFullName : AbstractIndexCreationTask<Employee, Employees_ByFullName.IndexEntry>
    {
        // The IndexEntry class defines the index-fields.
        public class IndexEntry
        {
            public string FullName { get; set; }
        }
            
        public Employees_ByFullName()
        {
            // The 'Map' function defines the content of the index-fields
            Map = employees => from employee in employees
                select new IndexEntry
                {
                    FullName = $"{employee.FirstName} {employee.LastName}"
                };
            
            // Store field 'FullName' in the index
            Store(x => x.FullName, FieldStorage.Yes);
        }
    }
    #endregion
}
