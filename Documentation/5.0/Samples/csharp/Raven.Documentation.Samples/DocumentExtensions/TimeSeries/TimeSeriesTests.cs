using System;
using System.Linq;
using FastTests;
using Raven.Client.Documents;
using Raven.Tests.Core.Utils.Entities;
using Xunit;
using Xunit.Abstractions;
using System.Collections.Generic;
using Raven.Server.Utils;
using MongoDB.Driver;
using Raven.Client.Documents.Operations.TimeSeries;
using Raven.Client.Documents.Commands.Batches;
using PatchRequest = Raven.Client.Documents.Operations.PatchRequest;
using Raven.Client.Documents.Operations;
using Xunit.Abstractions;
using Raven.Client.Documents.Session;
using Raven.Client.Documents.Session.TimeSeries;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.BulkInsert;
using static Raven.Client.Documents.BulkInsert.BulkInsertOperation;
using Raven.Client.Documents.Indexes.TimeSeries;
using Raven.Client.Documents.Queries.TimeSeries;
using Raven.Client.Documents.Queries;

namespace SlowTests.Client.TimeSeries.Session
{
    public class TimeSeriesSessionTests
    {
        public TimeSeriesSessionTests(ITestOutputHelper output)
        {
        }

        public void SessionTests()
        {
            var store = new DocumentStore
            {
                Urls = new[] { "http://localhost:8080" },
                Database = "products"
            };
            store.Initialize();

            var baseline = DateTime.Today;

            // create time series
            #region timeseries_region_TimeSeriesFor_without_document_load
            // Open a session
            using (var session = store.OpenSession())
            {
                // Use the session to create a document
                session.Store(new { Name = "John" }, "users/john");

                // Create an instance of TimeSeriesFor
                // Pass an explicit document ID to the TimeSeriesFor constructor 
                // Append a HeartRate of 70 at the first-minute timestamp 
                session.TimeSeriesFor("users/john", "HeartRate")
                    .Append(baseline.AddMinutes(1), 70d, "watches/fitbit");

                session.SaveChanges();
            }
            #endregion

            // retrieve a single value
            #region timeseries_region_TimeSeriesFor_with_document_load
            using (var session = store.OpenSession())
            {
                // Use the session to load a document
                User user = session.Load<User>("users/john");

                // Pass the document object returned from session.Load as a param
                // Retrieve a single value from the time series
                IEnumerable<TimeSeriesEntry> val = session.TimeSeriesFor(user, "HeartRate")
                    .Get(DateTime.MinValue, DateTime.MaxValue);
            }
            #endregion

            // retrieve a single value - use the document object
            #region timeseries_region_TimeSeriesFor-Get-Single-Value-Using-Document-Object
            using (var session = store.OpenSession())
            {
                // Use the session to load a document
                User user = session.Load<User>("users/john");

                // Pass the document object returned from session.Load as a param
                // Retrieve a single value from the time series
                IEnumerable<TimeSeriesEntry> val = session.TimeSeriesFor(user, "HeartRate")
                    .Get(DateTime.MinValue, DateTime.MaxValue);
            }
            #endregion

            #region timeseries_region_TimeSeriesFor-Get-Single-Value-Using-Document-ID
            // retrieve all entries of a time-series named "HeartRate" 
            // by passing TimeSeriesFor.Get an explict document ID
            using (var session = store.OpenSession())
            {
                IEnumerable<TimeSeriesEntry> val = session.TimeSeriesFor("users/john", "HeartRate")
                    .Get(DateTime.MinValue, DateTime.MaxValue);
            }
            #endregion

            #region timeseries_region_Pass-TimeSeriesFor-Get-Query-Results
            // Query for a document with the Name property "John" 
            // and get its HeartRate time-series values
            using (var session = store.OpenSession())
            {
                baseline = DateTime.Today;

                IRavenQueryable<User> query = session.Query<User>()
                    .Where(u => u.Name == "John");

                var result = query.ToList();

                IEnumerable<TimeSeriesEntry> val = session.TimeSeriesFor(result[0], "HeartRate")
                    .Get(DateTime.MinValue, DateTime.MaxValue);

                session.SaveChanges();
            }
            #endregion


            // retrieve time series names
            using (var session = store.OpenSession())
            {
                #region timeseries_region_Retrieve-TimeSeries-Names
                User user = session.Load<User>("users/john");
                List<string> tsNames = session.Advanced.GetTimeSeriesFor(user);
                #endregion
            }

            #region timeseries_region_TimeSeriesFor-Append-TimeSeries-Range
            var baseline = DateTime.Today;

            // Append 10 HeartRate values
            using (var session = store.OpenSession())
            {
                session.Store(new { Name = "John" }, "users/john");

                ISessionDocumentTimeSeries tsf = session.TimeSeriesFor("users/john", "HeartRate");

                for (int i = 0; i < 10; i++)
                {
                    tsf.Append(baseline.AddSeconds(i), new[] { 67d }, "watches/fitbit");
                }

                session.SaveChanges();
            }
            #endregion

            #region timeseries_region_Remove-TimeSeriesFor-Single-Time-Point
            var baseline = DateTime.Today;
            using (var session = store.OpenSession())
            {
                //remove a single entry
                session.TimeSeriesFor("users/john", "HeartRate")
                    .Remove(baseline.AddMinutes(4));

                session.SaveChanges();
            }
            #endregion

            #region timeseries_region_TimeSeriesFor-Remove-Time-Points-Range
            var baseline = DateTime.Today;

            // Append 10 HeartRate values
            using (var session = store.OpenSession())
            {
                session.Store(new { Name = "John" }, "users/john");

                var tsf = session.TimeSeriesFor("users/john", "HeartRate");

                for (int i = 0; i < 10; i++)
                {
                    tsf.Append(baseline.AddSeconds(i), new[] { 67d }, "watches/fitbit");
                }

                session.SaveChanges();
            }

            // remove a range of 4 values from the time series
            using (var session = store.OpenSession())
            {
                session.TimeSeriesFor("users/john", "HeartRate")
                    .Remove(baseline.AddSeconds(4), baseline.AddSeconds(7));

                session.SaveChanges();
            }
            #endregion

            #region timeseries_region_Append-With-IEnumerable
            using (var store = GetDocumentStore())
            {
                var baseline = DateTime.Today;

                // Open a session
                using (var session = store.OpenSession())
                {
                    // Create a document
                    session.Store(new { Name = "John" }, "users/john");

                    // Pass TimeSeriesFor an explicit document ID
                    // Append values at the first-minute timestamp 
                    session.TimeSeriesFor("users/john", "HeartRate")
                    .Append(baseline.AddMinutes(1),
                            new[] { 65d, 52d, 72d },
                            "watches/fitbit");

                    session.SaveChanges();
                }
            }
            #endregion


            #region timeseries_region_Load-Document-And-Include-TimeSeries
            // Load a document and Include a specified range of a time-series
            using (var session = store.OpenSession())
            {
                var baseline = DateTime.Today;

                User user = session.Load<User>("users/1-A", includeBuilder =>
                    includeBuilder.IncludeTimeSeries("HeartRate",
                    baseline.AddMinutes(200), baseline.AddMinutes(299)));

                IEnumerable<TimeSeriesEntry> val = session.TimeSeriesFor("users/1-A", "HeartRate")
                    .Get(baseline.AddMinutes(200), baseline.AddMinutes(299));
            }
            #endregion

            #region timeseries_region_Query-Document-And-Include-TimeSeries
            // Query for a document and include a whole time-series
            using (var session = store.OpenSession())
            {
                baseline = DateTime.Today;

                IRavenQueryable<User> query = session.Query<User>()
                    .Where(u => u.Name == "John")
                    .Include(includeBuilder => includeBuilder.IncludeTimeSeries(
                        "HeartRate", DateTime.MinValue, DateTime.MaxValue));

                var result = query.ToList();

                IEnumerable<TimeSeriesEntry> val = session.TimeSeriesFor(result[0], "HeartRate")
                    .Get(DateTime.MinValue, DateTime.MaxValue);
            }
            #endregion

            #region timeseries_region_Raw-Query-Document-And-Include-TimeSeries
            // Include a Time Series in a Raw Query
            using (var session = store.OpenSession())
            {
                baseline = DateTime.Today;

                var start = baseline;
                var end = baseline.AddHours(1);

                IRawDocumentQuery<User> query = session.Advanced.RawQuery<User>
                    ("from Users include timeseries('HeartRate', $start, $end)")
                        .AddParameter("start", start)
                        .AddParameter("end", end);

                var result = query.ToList();

                IEnumerable<TimeSeriesEntry> val = session.TimeSeriesFor(result[0], "HeartRate")
                    .Get(start, end);
            }
            #endregion

            #region TS_region-Session_Patch-Append-100-TS-Entries
            var baseline = DateTime.Today;

            // Create arrays of timestamps and random values to patch
            List<double> values = new List<double>();
            List<DateTime> timeStamps = new List<DateTime>();

            for (var cnt = 0; cnt < 100; cnt++)
            {
                values.Add(68 + Math.Round(19 * new Random().NextDouble()));
                timeStamps.Add(baseline.AddSeconds(cnt));
            }

            session.Advanced.Defer(new PatchCommandData("users/1-A", null,
                new PatchRequest
                {
                    Script = @"
                                var i = 0;
                                for(i = 0; i < $values.length; i++)
                                {
                                    timeseries(id(this), $timeseries)
                                    .append (
                                       new Date($timeStamps[i]), 
                                       $values[i], 
                                       $tag);
                                }",

                    Values =
                    {
                                { "timeseries", "HeartRate" },
                                { "timeStamps", timeStamps },
                                { "values", values },
                                { "tag", "watches/fitbit" }
                    }
                }, null));

            session.SaveChanges();
            #endregion

            #region TS_region-Session_Patch-Remove-50-TS-Entries
            // Remove time-series entries
            session.Advanced.Defer(new PatchCommandData("users/1-A", null,
                new PatchRequest
                {
                    Script = @"timeseries(this, $timeseries)
                             .remove(
                                $from, 
                                $to
                              );",
                    Values =
                    {
                                { "timeseries", "HeartRate" },
                                { "from", baseline.AddSeconds(0) },
                                { "to", baseline.AddSeconds(49) }
                    }
                }, null));
            session.SaveChanges();
            #endregion

        }

        public void StoreOperationsTests()
        {
            #region timeseries_region_Append-Using-TimeSeriesBatchOperation
            const string documentId = "users/john";

            using (var store = GetDocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    session.Store(new User(), documentId);
                    session.SaveChanges();
                }

                var baseline = DateTime.Today;

                var timeSeriesOp = new TimeSeriesOperation
                {
                    Name = "HeartRate",
                    Appends = new List<TimeSeriesOperation.AppendOperation>()
                    {
                        new TimeSeriesOperation.AppendOperation
                        {
                            Tag = "watches/fitbit",
                            Timestamp = baseline.AddMinutes(1),
                            Values = new[]
                            {
                                65d
                            }
                        },

                        new TimeSeriesOperation.AppendOperation
                        {
                            Tag = "watches/fitbit",
                            Timestamp = baseline.AddMinutes(2),
                            Values = new[]
                            {
                                68d
                            }
                        }
                    }
                };

                var timeSeriesBatch = new TimeSeriesBatchOperation(documentId, timeSeriesOp);

                store.Operations.Send(timeSeriesBatch);

                #endregion

                #region timeseries_region_Remove-Range-Using-TimeSeriesBatchOperation
                // remove a range of 5 minutes from time-series start
                timeSeriesOp = new TimeSeriesOperation
                {
                    Name = "HeartRate",

                    Removals = new List<TimeSeriesOperation.RemoveOperation>()
                    {
                        new TimeSeriesOperation.RemoveOperation
                        {
                            From = baseline.AddMinutes(20),
                            To = baseline.AddMinutes(50)
                        },

                        new TimeSeriesOperation.RemoveOperation
                        {
                            From = baseline.AddMinutes(70),
                            To = baseline.AddMinutes(100)
                        }
                    }
                };

                timeSeriesBatch = new TimeSeriesBatchOperation(documentId, timeSeriesOp);

                store.Operations.Send(timeSeriesBatch);
                #endregion

                #region timeseries_region_Get-Single-Time-Series
                // Get all values of a single time-series
                TimeSeriesRangeResult singleTimeSeriesRange = store.Operations.Send(
                    new GetTimeSeriesOperation(documentId, "HeartRate", DateTime.MinValue, DateTime.MaxValue));
                #endregion

                #region timeseries_region_Get-Multiple-Time-Series
                // Get value ranges from two time-series using GetMultipleTimeSeriesOperation
                TimeSeriesDetails multipleTimesSeriesDetails = store.Operations.Send(
                        new GetMultipleTimeSeriesOperation(documentId, new List<TimeSeriesRange>
                            {
                                new TimeSeriesRange
                                {
                                    Name = "ExcersizeHeartRate",
                                    From = baseTime.AddHours(1),
                                    To = baseTime.AddHours(10)
                                },

                                new TimeSeriesRange
                                {
                                    Name = "RestHeartRate",
                                    From = baseTime.AddHours(11),
                                    To = baseTime.AddHours(20)
                                }
                            }));
                #endregion

                #region timeseries_region_Use-BulkInsert-To-Append-2-Entries
                // Use BulkInsert to append 2 time-series entries
                using (BulkInsertOperation bulkInsert = store.BulkInsert())
                {
                    using (TimeSeriesBulkInsert timeSeriesBulkInsert = bulkInsert.TimeSeriesFor(documentID, "HeartRate"))
                    {
                        timeSeriesBulkInsert.Append(baseline.AddMinutes(2), 61d, "watches/fitbit");
                        timeSeriesBulkInsert.Append(baseline.AddMinutes(3), 62d, "watches/apple-watch");
                    }
                }
                #endregion

                #region timeseries_region_Use-BulkInsert-To-Append-100-Entries
                // Use BulkInsert to append 100 time-series entries
                for (int minute = 0; minute < 100; minute++)
                {
                    using (BulkInsertOperation bulkInsert = store.BulkInsert())
                    {
                        using (TimeSeriesBulkInsert timeSeriesBulkInsert = bulkInsert.TimeSeriesFor(documentId, "HeartRate"))
                        {
                            timeSeriesBulkInsert.Append(baseline.AddMinutes(minute), new double[] { minute }, "watches/fitbit");
                        }
                    }
                }
                #endregion

                #region BulkInsert-overload-2-Append-100-Multi-Value-Entries
                IEnumerable<double> values = new List<double>
                        {59d, 63d, 71d, 69d, 64, 65d };

                // Use BulkInsert to append 100 multi-values time-series entries
                for (int minute = 0; minute < 100; minute++)
                {
                    using (BulkInsertOperation bulkInsert = store.BulkInsert())
                    {
                        using (TimeSeriesBulkInsert timeSeriesBulkInsert = bulkInsert.TimeSeriesFor(documentId, "HeartRate"))
                        {
                            timeSeriesBulkInsert.Append(baseline.AddMinutes(minute), values, "watches/fitbit");
                        }
                    }
                }
                #endregion




                #region TS_region-Operation_Patch-Append-Single-TS-Entry
                store.Operations.Send(new PatchOperation("users/1-A", null,
                    new PatchRequest
                    {
                        Script = "timeseries(this, $timeseries).append($timestamp, $values, $tag);",
                        Values =
                        {
                            { "timeseries", "HeartRate" },
                            { "timestamp", baseline.AddMinutes(1) },
                            { "values", 59d },
                            { "tag", "watches/fitbit" }
                        }
                    }));
                #endregion

                #region TS_region-Operation_Patch-Append-100-TS-Entries
                var baseline = DateTime.Today;

                // Create arrays of timestamps and random values to patch
                List<double> values = new List<double>();
                List<DateTime> timeStamps = new List<DateTime>();

                for (var cnt = 0; cnt < 100; cnt++)
                {
                    values.Add(68 + Math.Round(19 * new Random().NextDouble()));
                    timeStamps.Add(baseline.AddSeconds(cnt));
                }

                store.Operations.Send(new PatchOperation("users/1-A", null,
                    new PatchRequest
                    {
                        Script = @"var i = 0; 
                            for (i = 0; i < $values.length; i++) 
                            {timeseries(id(this), $timeseries)
                            .append (
                                      new Date($timeStamps[i]), 
                                      $values[i], 
                                      $tag);
                            }",
                        Values =
                        {
                                { "timeseries", "HeartRate" },
                                { "timeStamps", timeStamps},
                                { "values", values },
                                { "tag", "watches/fitbit" }
                        }

                    }));
                #endregion

                #region TS_region-Operation_Patch-Remove-50-TS-Entries
                store.Operations.Send(new PatchOperation("users/1-A", null,
                    new PatchRequest
                    {
                        Script = "timeseries(this, $timeseries).remove($from, $to);",
                        Values =
                        {
                                { "timeseries", "HeartRate" },
                                { "from", baseline.AddSeconds(0) },
                                { "to", baseline.AddSeconds(49) }
                        }
                    }));
                #endregion

                #region TS_region-PatchByQueryOperation-Append-To-Multiple-Docs
                // Append time-series to all users
                PatchByQueryOperation appendOperation = new PatchByQueryOperation(new IndexQuery
                {
                    Query = @"from Users as u update
                                {
                                    timeseries(u, $name).append($time, $values, $tag)
                                }",
                    QueryParameters = new Parameters
                            {
                                { "name", "HeartRate" },
                                { "time", baseline.AddMinutes(1) },
                                { "values", new[]{59d} },
                                { "tag", "watches/fitbit" }
                            }
                });
                store.Operations.Send(appendOperation);
                #endregion

                #region TS_region-PatchByQueryOperation-Remove-From-Multiple-Docs
                // Remove time-series from all users
                PatchByQueryOperation removeOperation = new PatchByQueryOperation(new IndexQuery
                {
                    Query = @"from Users as u
                                update
                                {
                                    timeseries(u, $name).remove($from, $to)
                                }",
                    QueryParameters = new Parameters
                            {
                                { "name", "HeartRate" },
                                { "from", DateTime.MinValue },
                                { "to", DateTime.MaxValue }
                            }
                });
                store.Operations.Send(removeOperation);
                #endregion

                #region TS_region-PatchByQueryOperation-Get
                // Get ranges of time-series entries from all users 
                PatchByQueryOperation getOperation = new PatchByQueryOperation(new IndexQuery
                {
                    Query = @"from Users as u
                                update
                                {
                                    timeseries(u, $name).get($from, $to)
                                }",
                    QueryParameters = new Parameters
                            {
                                { "name", "HeartRate" },
                                { "from", DateTime.MinValue },
                                { "to", DateTime.MaxValue }
                            }
                });
                Operation getOp = store.Operations.Send(getOperation);
                #endregion

                #region ts_region_Raw-Query-Non-Aggregated-Declare-Syntax
                // May 17 2020, 00:00:00
                var baseline = new DateTime(2020, 5, 17, 00, 00, 00);

                // Raw query with no aggregation - Declare syntax
                IRawDocumentQuery<TimeSeriesRawResult> nonAggregatedRawQuery =
                    session.Advanced.RawQuery<TimeSeriesRawResult>(@"
                            declare timeseries getHeartRate(user) 
                            {
                                from user.HeartRate 
                                    between $start and $end
                                    offset '02:00'
                            }
                            from Users as u where Age < 30
                            select getHeartRate(u)
                            ")
                    .AddParameter("start", baseline)
                    .AddParameter("end", baseline.AddHours(24));

                var nonAggregatedRawQueryResult = nonAggregatedRawQuery.ToList();
                #endregion

                #region ts_region_Raw-Query-Non-Aggregated-Select-Syntax
                // May 17 2020, 00:00:00
                var baseline = new DateTime(2020, 5, 17, 00, 00, 00);

                // Raw query with no aggregation - Select syntax
                IRawDocumentQuery<TimeSeriesRawResult> nonAggregatedRawQuery =
                    session.Advanced.RawQuery<TimeSeriesRawResult>(@"
                            from Users as u where Age < 30                            
                            select timeseries (
                                from HeartRate 
                                    between $start and $end
                                    offset '02:00'
                            )")
                    .AddParameter("start", baseline)
                    .AddParameter("end", baseline.AddHours(24));

                var nonAggregatedRawQueryResult = nonAggregatedRawQuery.ToList();
                #endregion

                #region ts_region_Raw-Query-Aggregated
                // May 17 2020, 00:00:00
                var baseline = new DateTime(2020, 5, 17, 00, 00, 00);

                // Raw Query with aggregation
                IRawDocumentQuery<TimeSeriesAggregationResult> aggregatedRawQuery =
                    session.Advanced.RawQuery<TimeSeriesAggregationResult>(@"
                            from Users as u
                            select timeseries(
                                from HeartRate 
                                    between $start and $end
                                group by '1 days'
                                select min(), max())
                            ")
                    .AddParameter("start", baseline)
                    .AddParameter("end", baseline.AddDays(7));

                var aggregatedRawQueryResult = aggregatedRawQuery.ToList();
                #endregion

                #region ts_region_LINQ-1-Select-Timeseries
                using (var session = store.OpenSession())
                {
                    IRavenQueryable<TimeSeriesRawResult> query = (IRavenQueryable<TimeSeriesRawResult>)session
                        .Query<User>()
                            .Where(u => u.Age < 30)
                                .Select(q => RavenQuery.TimeSeries(q, "HeartRate")
                                .ToList());

                    var result = query.ToList();
                }
                #endregion

                #region ts_region_LINQ-2-RQL-Equivalent
                using (var session = store.OpenSession())
                {
                    IRawDocumentQuery<TimeSeriesRawResult> nonAggregatedRawQuery =
                        session.Advanced.RawQuery<TimeSeriesRawResult>(@"
                            from Users as u where Age < 30                            
                            select timeseries (
                                from HeartRate
                            )");

                    var nonAggregatedRawQueryResult = nonAggregatedRawQuery.ToList();
                }
                #endregion

                // Query - LINQ format with Range selection
                using (var session = store.OpenSession())
                {
                    var baseline = new DateTime(2020, 5, 17, 00, 00, 00);

                    #region ts_region_LINQ-3-Range-Selection
                    IRavenQueryable<TimeSeriesRawResult> query =
                        (IRavenQueryable<TimeSeriesRawResult>)session.Query<User>()
                            .Where(u => u.Age < 30)
                            .Select(q => RavenQuery.TimeSeries(q, "HeartRate", baseline, baseline.AddDays(3))
                            .ToList());
                    #endregion

                    var result = query.ToList();
                }

                using (var session = store.OpenSession())
                {
                    var baseline = new DateTime(2020, 5, 17, 00, 00, 00);

                    #region ts_region_LINQ-4-Where
                    IRavenQueryable<TimeSeriesRawResult> query =
                    (IRavenQueryable<TimeSeriesRawResult>)session.Query<User>()

                            // Choose user profiles of users under the age of 30
                            .Where(u => u.Age < 30)

                            .Select(q => RavenQuery.TimeSeries(q, "HeartRate", baseline, baseline.AddDays(3))

                            // Filter time-series entries by their tag.  
                            .Where(ts => ts.== "watches/fitbit")
                    #endregion

                            .ToList());

                    var result = query.ToList();
                }

                using (var session = store.OpenSession())
                {
                    // May 17 2020, 00:00:00
                    var baseline = new DateTime(2020, 5, 17, 00, 00, 00);

                    #region ts_region_Filter-By-load-Tag-Raw-RQL
                    IRawDocumentQuery<TimeSeriesRawResult> nonAggregatedRawQuery =
                        session.Advanced.RawQuery<TimeSeriesRawResult>(@"
                            from Companies as c where c.Address.Country = 'USA'
                            select timeseries(
                                from StockPrice
                                   load Tag as emp
                                   where emp.Title == 'Sales Representative'
                            )");

                    var result = nonAggregatedRawQuery.ToList();
                    #endregion
                }
            }

            // Query - LINQ format - LoadTag to find a stock broker
            using (var session = store.OpenSession())
            {
                var baseline = new DateTime(2020, 5, 17, 00, 00, 00);

                #region ts_region_Filter-By-LoadTag-LINQ
                IRavenQueryable<TimeSeriesRawResult> query =
                    (IRavenQueryable<TimeSeriesRawResult>)session.Query<Orders.Company>()

                        .Where(c => c.Address.Country == "USA")
                        .Select(q => RavenQuery.TimeSeries(q, "StockPrice")
                        
                        .LoadTag<Employee>()
                        .Where((ts, src) => src.Title == "Sales Representative")
                        
                        .ToList());

                var result = query.ToList();
                #endregion
            }


            // Query - LINQ format - Aggregation 
            using (var session = store.OpenSession())
                {
                    var baseline = DateTime.Today;

                    #region ts_region_LINQ-6-Aggregation
                    IRavenQueryable<TimeSeriesAggregationResult> query = session.Query<User>()
                        .Where(u => u.Age > 72)
                        .Select(q => RavenQuery.TimeSeries(q, "HeartRate", baseline, baseline.AddDays(10))
                            .Where(ts => ts.Tag == "watches/fitbit")
                            .GroupBy(g => g.Days(1))
                            .Select(g => new
                            {
                                Avg = g.Average(),
                                Cnt = g.Count()
                            })
                            .ToList());
                    #endregion


                // Query - LINQ format - StockPrice
                using (var session = store.OpenSession())
                {
                    var baseline = new DateTime(2020, 5, 17);

                    #region ts_region_LINQ-Aggregation-and-Projection-StockPrice
                    IRavenQueryable<TimeSeriesAggregationResult> query = session.Query<Orders.Company>()
                        .Where(c => c.Address.Country == "USA")
                        .Select(q => RavenQuery.TimeSeries(q, "StockPrice")
                            .Where(ts => ts.Values[4] > 500000)
                            .GroupBy(g => g.Days(7))
                            .Select(g => new
                            {
                                Min = g.Min(),
                                Max = g.Max()
                            })
                            .ToList());

                    var result = query.ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    var baseline = new DateTime(2020, 5, 17);

                    var start = baseline;
                    var end = baseline.AddHours(1);

                    #region ts_region_Raw-RQL-Select-Syntax-Aggregation-and-Projection-StockPrice
                    IRawDocumentQuery<TimeSeriesAggregationResult> aggregatedRawQuery =
                        session.Advanced.RawQuery<TimeSeriesAggregationResult>(@"
                            from Companies as c
                                where c.Address.Country = 'USA'
                                select timeseries ( 
                                    from StockPrice 
                                    where Values[4] > 500000
                                        group by '7 day'
                                        select max(), min()
                                )
                            ");

                    var aggregatedRawQueryResult = aggregatedRawQuery.ToList();
                    #endregion
                }

                // Raw Query - StockPrice
                using (var session = store.OpenSession())
                {
                    var baseline = new DateTime(2020, 5, 17);

                    var start = baseline;
                    var end = baseline.AddHours(1);

                    #region ts_region_Raw-RQL-Declare-Syntax-Aggregation-and-Projection-StockPrice
                    IRawDocumentQuery<TimeSeriesAggregationResult> aggregatedRawQuery =
                        session.Advanced.RawQuery<TimeSeriesAggregationResult>(@"
                            declare timeseries SP(c) {
                                from c.StockPrice
                                where Values[4] > 500000
                                group by '7 day'
                                select max(), min()
                            }
                            from Companies as c
                            where c.Address.Country = 'USA'
                            select c.Name, SP(c)"
                            );

                    var aggregatedRawQueryResult = aggregatedRawQuery.ToList();
                    #endregion
                }


                var result = query.ToList();
                }

                // index queries
                #region ts_region_Index-TS-Queries-1-session-Query
                // Query time-series index using session.Query
                using (var session = store.OpenSession())
                {
                    List<SimpleIndex.Result> results = session.Query<SimpleIndex.Result, SimpleIndex>()
                        .ToList();
                }
                #endregion

                #region ts_region_Index-TS-Queries-2-session-Query-with-Linq
                // Enhance the query using LINQ expressions
                var chosenDate = new DateTime(2020, 5, 20);
                using (var session = store.OpenSession())
                {
                    List<SimpleIndex.Result> results = session.Query<SimpleIndex.Result, SimpleIndex>()
                        .Where(w => w.Date < chosenDate)
                        .OrderBy(o => o.HeartBeat)
                        .ToList();
                }
                #endregion

                #region ts_region_Index-TS-Queries-3-DocumentQuery
                // Query time-series index using DocumentQuery
                using (var session = store.OpenSession())
                {
                    List<SimpleIndex.Result> results = session.Advanced.DocumentQuery<SimpleIndex.Result, SimpleIndex>()
                        .ToList();
                }
                #endregion

                #region ts_region_Index-TS-Queries-4-DocumentQuery-with-Linq
                // Query time-series index using DocumentQuery with Linq-like expressions
                using (var session = store.OpenSession())
                {
                    List<SimpleIndex.Result> results = session.Advanced.DocumentQuery<SimpleIndex.Result, SimpleIndex>()
                        .WhereEquals("Tag", "watches/fitbit")
                        .ToList();
                }
                #endregion

                #region ts_region_Index-TS-Queries-5-session-Query-Async
                // Time-series async index query using session.Query
                using (var session = store.OpenAsyncSession())
                {
                    List<SimpleIndex.Result> results = await session.Query<SimpleIndex.Result, SimpleIndex>()
                        .ToListAsync();
                }
                #endregion


            }
        }

        #region ts_region_Index-TS-Queries-6-Index-Definition-And-Results-Class
        public class SimpleIndex : AbstractTimeSeriesIndexCreationTask<Employee>
        {

            public class Result
            {
                public double HeartBeat { get; set; }
                public DateTime Date { get; set; }
                public string User { get; set; }
                public string Tag { get; set; }
            }

            public SimpleIndex()
            {
                AddMap(
                    "HeartRate",
                    timeSeries => from ts in timeSeries
                                  from entry in ts.Entries
                                  select new
                                  {
                                      HeartBeat = entry.Values[0],
                                      entry.Timestamp.Date,
                                      User = ts.DocumentId,
                                      Tag = entry.Tag
                                  });
            }
        }
        #endregion



        private IDisposable GetDocumentStore()
        {
            throw new NotImplementedException();
        }
    }

    internal class User
    {
    }

    private interface IFoo
    {
        #region TimeSeriesFor-Append-definition-double
        // Append an entry with a single value (double)
        void Append(DateTime timestamp, double value, string tag = null);
        #endregion

        #region TimeSeriesFor-Append-definition-inum
        // Append an entry with multiple values (IEnumerable)
        void Append(DateTime timestamp, IEnumerable<double> values, string tag = null);
        #endregion

        #region TimeSeriesFor-Remove-definition-single-timepoint
        // Remove a single time-series entry
        void Remove(DateTime at);
        #endregion

        #region TimeSeriesFor-Remove-definition-range-of-timepoints
        // Remove a range of time-series entries
        void Remove(DateTime from, DateTime to);
        #endregion

        #region TimeSeriesFor-Get-definition
        IEnumerable<TimeSeriesEntry> Get(DateTime from, DateTime to, int start = 0, int pageSize = int.MaxValue);
        #endregion

        #region IncludeTimeSeries-definition
        TBuilder IncludeTimeSeries(string name, DateTime from, DateTime to);
        #endregion

        #region GetTimeSeriesFor-definition
        List<string> GetTimeSeriesFor<T>(T instance);
        #endregion

        #region Load-definition
        T Load<T>(string id, Action<IIncludeBuilder<T>> includes);
        #endregion

        #region Include-definition
        IRavenQueryable<TResult> Include<TResult>(this IQueryable<TResult> source, Action<IQueryIncludeBuilder<TResult>> includes)
        #endregion

        #region RawQuery-definition
        IRawDocumentQuery<T> RawQuery<T>(string query);
        #endregion

        #region Query-definition
        IRavenQueryable<T> Query<T>(string indexName = null, string collectionName = null, bool isMapReduce = false);
        #endregion

        #region PatchCommandData-definition
        public PatchCommandData(string id, string changeVector, PatchRequest patch, PatchRequest patchIfMissing)
        #endregion

        #region PatchRequest-definition
        public class PatchRequest
        {
            // Patching script
            public string Script { get; set; }
            // Values that can be used by the patching script
            public Dictionary<string, object> Values { get; set; }
            //...
        }
        #endregion

        #region TimeSeriesBatchOperation-definition
        public TimeSeriesBatchOperation(string documentId, TimeSeriesOperation operation)
        #endregion

        #region TimeSeriesOperation-class
        public class TimeSeriesOperation
        {
            // A list of Append actions
            public List<AppendOperation> Appends;

            // A list of Remove actions
            public List<RemoveOperation> Removals;

            public string Name;
            //...
        }
        #endregion

        #region AppendOperation-class
        public class AppendOperation
        {
            public DateTime Timestamp;
            public double[] Values;
            public string Tag;
            //...
        }
        #endregion

        #region RemoveOperation-class
        public class RemoveOperation
        {
            public DateTime From, To;
            //...
        }
        #endregion

        #region TimeSeriesRangeResult-class
        public class TimeSeriesRangeResult
        {
            public DateTime From, To;
            public TimeSeriesEntry[] Entries;
            public long? TotalResults;
            
            //..
        }
        #endregion

        #region GetTimeSeriesOperation-Definition
        public GetTimeSeriesOperation(string docId, string timeseries, DateTime? @from = null, DateTime? to = null, int start = 0, int pageSize = int.MaxValue)
        #endregion

        #region TimeSeriesDetails-class
        public class TimeSeriesDetails
        {
            public string Id { get; set; }
            public Dictionary<string, List<TimeSeriesRangeResult>> Values { get; set; }
        }
        #endregion

        #region GetMultipleTimeSeriesOperation-Definition
        public GetMultipleTimeSeriesOperation(string docId, IEnumerable<TimeSeriesRange> ranges, int start = 0, int pageSize = int.MaxValue)
        #endregion

        #region TimeSeriesRange-class
        public class TimeSeriesRange
        {
            public string Name;
            public DateTime From, To;
        }
        #endregion

        #region BulkInsert-Append-Single-Value-Definition
        public void Append(DateTime timestamp, double value, string tag = null)
        #endregion

        #region BulkInsert-Append-Multiple-Values-Definition
        public void Append(DateTime timestamp, IEnumerable<double> values, string tag = null)
        #endregion

        #region PatchOperation-Definition
        public PatchOperation(string id, string changeVector, PatchRequest patch, PatchRequest patchIfMissing = null, bool skipPatchIfChangeVectorMismatch = false)
        #endregion

        #region PatchByQueryOperation-Definition
        public PatchByQueryOperation(IndexQuery queryToUpdate, QueryOperationOptions options = null)
        #endregion

        #region Store-Operations-send-Definition
        public Operation Send(IOperation<OperationIdResult> operation, SessionInfo sessionInfo = null)
        #endregion

        #region RavenQuery-TimeSeries-Definition-With-Range
        public static ITimeSeriesQueryable TimeSeries(object documentInstance, string name, DateTime from, DateTime to)
        #endregion

        #region RavenQuery-TimeSeries-Definition-Without-Range
        public static ITimeSeriesQueryable TimeSeries(object documentInstance, string name)
        #endregion

    }


}
