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
                // Append a heartrate of 70 at the first-minute timestamp 
                session.TimeSeriesFor("users/john", "Heartrate")
                    .Append(baseline.AddMinutes(1), 70d, "watches/fitbit");

                session.SaveChanges();
            }
            #endregion

            // retrieve a single value
            #region timeseries_region_TimeSeriesFor_with_document_load
            using (var session = store.OpenSession())
            {
                // Use the session to load a document
                var user = session.Load<User>("users/john");

                // Pass the document object returned from session.Load as a param
                // Retrieve a single value from the time series
                var val = session.TimeSeriesFor(user, "Heartrate")
                    .Get(DateTime.MinValue, DateTime.MaxValue);
            }
            #endregion

            // retrieve a single value - use the document object
            #region timeseries_region_TimeSeriesFor-Get-Single-Value-Using-Document-Object
            using (var session = store.OpenSession())
            {
                // Use the session to load a document
                var user = session.Load<User>("users/john");

                // Pass the document object returned from session.Load as a param
                // Retrieve a single value from the time series
                var val = session.TimeSeriesFor(user, "Heartrate")
                    .Get(DateTime.MinValue, DateTime.MaxValue)
                    .Single();
            }
            #endregion

            #region timeseries_region_TimeSeriesFor-Get-Single-Value-Using-Document-ID
            // retrieve all time points of a time-series named "Heartratea" 
            // by passing TimeSeriesFor.Get an explict document ID
            using (var session = store.OpenSession())
            {
                var val = session.TimeSeriesFor("users/john", "Heartrate")
                    .Get(DateTime.MinValue, DateTime.MaxValue);
            }
            #endregion

            #region timeseries_region_Pass-TimeSeriesFor-Get-Query-Results
            // Query for a document with the Name property "John" 
            // and get its Heartrate time-series values
            using (var session = store.OpenSession())
            {
                var baseline = DateTime.Today;

                var query = session.Query<User>()
                    .Where(u => u.Name == "John");

                var result = query.ToList();

                var val = session.TimeSeriesFor(result[0], "Heartrate")
                    .Get(DateTime.MinValue, DateTime.MaxValue);

                session.SaveChanges();
            }
            #endregion


            // retrieve time series names
            using (var session = store.OpenSession())
            {
                #region timeseries_region_Retrieve-TimeSeries-Names
                var user = session.Load<User>("users/john");
                var tsNames = session.Advanced.GetTimeSeriesFor(user);
                #endregion
            }

            #region timeseries_region_TimeSeriesFor-Append-TimeSeries-Range
            var baseline = DateTime.Today;

            // Append 10 heartrate values
            using (var session = store.OpenSession())
            {
                session.Store(new { Name = "John" }, "users/john");

                var tsf = session.TimeSeriesFor("users/john", "Heartrate");

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
                //remove a single time point
                session.TimeSeriesFor("users/john", "Heartrate")
                    .Remove(baseline.AddMinutes(4));

                session.SaveChanges();
            }
            #endregion

            #region timeseries_region_TimeSeriesFor-Remove-Time-Points-Range
            var baseline = DateTime.Today;

            // Append 10 heartrate values
            using (var session = store.OpenSession())
            {
                session.Store(new { Name = "John" }, "users/john");

                var tsf = session.TimeSeriesFor("users/john", "Heartrate");

                for (int i = 0; i < 10; i++)
                {
                    tsf.Append(baseline.AddSeconds(i), new[] { 67d }, "watches/fitbit");
                }

                session.SaveChanges();
            }

            // remove a range of 4 values from the time series
            using (var session = store.OpenSession())
            {
                session.TimeSeriesFor("users/john", "Heartrate")
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
                    // Use the session to create a document
                    session.Store(new { Name = "John" }, "users/john");

                    IEnumerable<double> values = new List<double>
                    {
                        65d,
                        52d,
                        72d
                    };

                    // Create an instance of TimeSeriesFor
                    // Pass an explicit document ID to the TimeSeriesFor constructor 
                    // Append the three IEnumerable heartrates at the first-minute timestamp 
                    session.TimeSeriesFor("users/john", "Heartrate")
                        .Append(baseline.AddMinutes(1), values, "watches/fitbit");

                    session.SaveChanges();
                }
            }
            #endregion


            #region timeseries_region_Load-Document-And-Include-TimeSeries
            // Load a document and Include a specified range of a time-series
            using (var session = store.OpenSession())
            {
                var baseline = DateTime.Today;

                var user = session.Load<User>("users/1-A", includeBuilder =>
                    includeBuilder.IncludeTimeSeries("Heartrate",
                    baseline.AddMinutes(200), baseline.AddMinutes(299)));

                var val = session.TimeSeriesFor("users/1-A", "Heartrate")
                    .Get(baseline.AddMinutes(200), baseline.AddMinutes(299));
            }
            #endregion

            #region timeseries_region_Query-Document-And-Include-TimeSeries
            // Query for a document and include a whole time-series
            using (var session = store.OpenSession())
            {
                var baseline = DateTime.Today;

                var query = session.Query<User>()
                    .Where(u => u.Name == "John")
                    .Include(includeBuilder => includeBuilder.IncludeTimeSeries(
                        "Heartrate", DateTime.MinValue, DateTime.MaxValue));

                var result = query.ToList();

                var val = session.TimeSeriesFor(result[0], "Heartrate")
                    .Get(DateTime.MinValue, DateTime.MaxValue);
            }
            #endregion

            #region timeseries_region_Raw-Query-Document-And-Include-TimeSeries
            // Include a Time Series in a Raw Query
            using (var session = store.OpenSession())
            {
                var baseline = DateTime.Today;

                var start = baseline;
                var end = baseline.AddHours(1);

                var query = session.Advanced.RawQuery<User>("from Users include timeseries('Heartrate', $start, $end)")
                    .AddParameter("start", start)
                    .AddParameter("end", end);

                var result = query.ToList();

                var val = session.TimeSeriesFor(result[0], "Heartrate")
                    .Get(start, end);
            }
            #endregion

            #region TS_region-Session_Patch-Append-Single-TS-Entry
            // Patch a document a single time-series entry
            using (var session = store.OpenSession())
            {
                var baseline = DateTime.Today;

                session.Advanced.Defer(new PatchCommandData("users/1-A", null,
                    new PatchRequest
                    {
                        Script = @"timeseries(this, args.timeseries).
                                        append( 
                                                args.timestamp, 
                                                args.values, 
                                                args.tag
                                               );",
                        Values =
                        {
                                { "timeseries", "Heartrate" },
                                { "timestamp", baseline.AddMinutes(1) },
                                { "values", 59d },
                                { "tag", "watches/fitbit" }
                        }
                    }, null));
                session.SaveChanges();
            }
            #endregion

            #region TS_region-Session_Patch-Append-100-TS-Entries
            var baseline = DateTime.Today;

            // Create an array of values to patch
            var toAppend = Enumerable.Range(0, 100)
                .Select(i => new Tuple<DateTime, double>
                            (baseline.AddSeconds(i), 59d))
                .ToArray();

            session.Advanced.Defer(new PatchCommandData("users/1-A", null,
                new PatchRequest
                {
                    Script = @"
                                var i = 0;
                                for(i = 0; i < args.toAppend.length; i++)
                                {
                                    timeseries(id(this), args.timeseries)
                                    .append (
                                      new Date(args.toAppend[i].Item1), 
                                      args.toAppend[i].Item2, 
                                      args.tag);
                                }",

                    Values =
                    {
                                { "timeseries", "Heartrate" },
                                { "toAppend", toAppend },
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
                    Script = @"timeseries(this, args.timeseries).remove(args.from, args.to);",
                    Values =
                    {
                                { "timeseries", "Heartrate" },
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
                    Name = "Heartrate",
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
                    Name = "Heartrate",

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
                var SingleTimesSeriesDetails = store.Operations.Send(
                    new GetTimeSeriesOperation(documentId, "RoutineHeartrate", DateTime.MinValue, DateTime.MaxValue));
                #endregion

                #region timeseries_region_Get-Multiple-Time-Series
                // Get chosen values of two time-series
                var MultipletimesSeriesDetails = store.Operations.Send(
                    new GetTimeSeriesOperation(documentId, new List<TimeSeriesRange>
                    {
                        new TimeSeriesRange
                        {
                            Name = "RoutineHeartrate",
                            From = baseline.AddMinutes(1),
                            To = baseline.AddMinutes(10)
                        },

                        new TimeSeriesRange
                        {
                            Name = "BicycleHeartrate",
                            From = baseline.AddMinutes(1),
                            To = baseline.AddMinutes(10)
                        }
                    }));
                #endregion

                #region timeseries_region_Use-BulkInsert-To-Append-2-Entries
                // Use BulkInsert to append 2 time-series entries
                using (var bulkInsert = store.BulkInsert())
                {
                    using (var timeSeriesBulkInsert = bulkInsert.TimeSeriesFor(documentID, "Heartrate"))
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
                    using (var bulkInsert = store.BulkInsert())
                    {
                        using (var timeSeriesBulkInsert = bulkInsert.TimeSeriesFor(documentId, "Heartrate"))
                        {
                            timeSeriesBulkInsert.Append(baseline.AddMinutes(minute), new double[] { minute }, "watches/fitbit");
                        }
                    }
                }
                #endregion

                #region TS_region-Operation_Patch-Append-Single-TS-Entry
                store.Operations.Send(new PatchOperation("users/1-A", null,
                    new PatchRequest
                    {
                        Script = "timeseries(this, args.timeseries).append(args.timestamp, args.values, args.tag);",
                        Values =
                        {
                            { "timeseries", "Heartrate" },
                            { "timestamp", baseline.AddMinutes(1) },
                            { "values", 59d },
                            { "tag", "watches/fitbit" }
                        }
                    }));
                #endregion

                #region TS_region-Operation_Patch-Append-100-TS-Entries
                // Create an array of values to patch
                var toAppend = Enumerable.Range(0, 100)
                 .Select(i => new Tuple<DateTime, double>
                                (baseline.AddSeconds(i), 59d))
                    .ToArray();

                store.Operations.Send(new PatchOperation("users/1-A", null,
                    new PatchRequest
                    {
                        Script = "var i = 0; " +
                        "for (i = 0; i < args.toAppend.length; i++) " +
                        "{timeseries(id(this), " +
                        "args.timeseries).append(" +
                        "new Date(" +
                        "args.toAppend[i]." +
                        "Item1), " +
                        "args.toAppend[i].Item2, args.tag);" +
                        "}",
                        Values =
                        {
                                { "timeseries", "Heartrate" },
                                { "toAppend", toAppend },
                                { "tag", "watches/fitbit" }
                        }
                    }));
                #endregion

                #region TS_region-Operation_Patch-Remove-50-TS-Entries
                store.Operations.Send(new PatchOperation("users/1-A", null,
                    new PatchRequest
                    {
                        Script = "timeseries(this, args.timeseries).remove(args.from, args.to);",
                        Values =
                        {
                                { "timeseries", "Heartrate" },
                                { "from", baseline.AddSeconds(0) },
                                { "to", baseline.AddSeconds(49) }
                        }
                    }));
                #endregion

                #region TS_region-PatchByQueryOperation-Append-To-Multiple-Docs
                // Append time-series to all users
                var appendOperation = new PatchByQueryOperation(new IndexQuery
                {
                    Query = @"from Users as u update
                                {
                                    timeseries(u, $name).append($time, $values, $tag)
                                }",
                    QueryParameters = new Parameters
                            {
                                { "name", "Heartrate" },
                                { "time", baseline.AddMinutes(1) },
                                { "values", new[]{59d} },
                                { "tag", "watches/fitbit" }
                            }
                });
                store.Operations.Send(appendOperation);
                #endregion

                #region TS_region-PatchByQueryOperation-Remove-From-Multiple-Docs
                // Remove time-series from all users
                var removeOperation = new PatchByQueryOperation(new IndexQuery
                {
                    Query = @"from Users as u
                                update
                                {
                                    timeseries(u, $name).remove($from, $to)
                                }",
                    QueryParameters = new Parameters
                            {
                                { "name", "Heartrate" },
                                { "from", DateTime.MinValue },
                                { "to", DateTime.MaxValue }
                            }
                });
                store.Operations.Send(removeOperation);
                #endregion

                #region TS_region-PatchByQueryOperation-Get
                // Get ranges of time-series entries from all users 
                var getOperation = new PatchByQueryOperation(new IndexQuery
                {
                    Query = @"from Users as u
                                update
                                {
                                    timeseries(u, $name).get($from, $to)
                                }",
                    QueryParameters = new Parameters
                            {
                                { "name", "Heartrate" },
                                { "from", DateTime.MinValue },
                                { "to", DateTime.MaxValue }
                            }
                });
                store.Operations.Send(appendOperation);
                #endregion






            }
        }






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
        void Append(DateTime timestamp, double value, string tag = null);
        #endregion

        #region TimeSeriesFor-Append-definition-inum
        void Append(DateTime timestamp, IEnumerable<double> values, string tag = null);
        #endregion

        #region TimeSeriesFor-Remove-definition-single-timepoint
        void Remove(DateTime at);
        #endregion

        #region TimeSeriesFor-Remove-definition-range-of-timepoints
        void Remove(DateTime from, DateTime to);
        #endregion

        #region TimeSeriesFor-Get-definition
        IEnumerable<TimeSeriesEntry> Get(DateTime from, DateTime to, int start = 0, int pageSize = int.MaxValue);
        #endregion

        #region IncludeTimeSeries-definition
        TBuilder IncludeTimeSeries(string name, DateTime from, DateTime to);
        #endregion

        #region TimeSeriesOperation-class
        public class TimeSeriesOperation
        {
            public List<AppendOperation> Appends;
            public List<RemoveOperation> Removals;
            public string Name;
            #endregion
        }

        #region TimeSeriesRangeResult-class
        public class TimeSeriesRangeResult
        {
            public DateTime From, To;
            public TimeSeriesEntry[] Entries;
            public long? TotalResults;
            internal string Hash;
        }
        #endregion



    }


}
