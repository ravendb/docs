using System;
using System.Linq;
using Xunit.Abstractions;
using Raven.Client.Documents;

namespace SlowTests.Client.TimeSeries.Session
{
    public class TimeSeriesSessionTests
    {
        public TimeSeriesSessionTests(ITestOutputHelper output)
        {
        }

        public void TimeSeriesSessionTests()
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
        }

        public void ReebStoreOperationsTests()
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
