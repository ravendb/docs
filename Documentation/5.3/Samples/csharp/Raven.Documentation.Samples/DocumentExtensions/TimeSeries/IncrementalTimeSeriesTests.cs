using System;
using System.Linq;
using Raven.Client.Documents;
using Xunit;
using Xunit.Abstractions;
using System.Collections.Generic;
using Raven.Client.Documents.Operations.TimeSeries;
using Raven.Client.Documents.Commands.Batches;
using PatchRequest = Raven.Client.Documents.Operations.PatchRequest;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Queries;
using Raven.Client;
using System.Threading.Tasks;
using Raven.Client.Documents.Session.TimeSeries;
using Raven.Client.Documents.Session;
using Raven.Client.Documents.Linq;
using static Raven.Client.Documents.BulkInsert.BulkInsertOperation;
using Raven.Client.Documents.BulkInsert;
using Raven.Client.Documents.Queries.TimeSeries;
using Raven.Client.Documents.Indexes.TimeSeries;
using Raven.Client.Documents.Operations.Indexes;
using Raven.Client.ServerWide.Operations;
using Raven.Client.ServerWide;
using Raven.Client.Documents.Session.Loaders;

namespace Documentation.Samples.DocumentExtensions.TimeSeries
{
    public class SampleTimeSeriesMethods
    {
        private SampleTimeSeriesMethods(ITestOutputHelper output)
        {
        }

        public void CreateIncrementalTimeSeries()
        {
            using (var store = new DocumentStore())
            {
                var baseline = DateTime.Today;

                using (var session = store.OpenSession())
                {
                    session.Store(new User { Name = "webstore" }, "companies/webstore");
                    var ts = session.IncrementalTimeSeriesFor("companies/webstore", "INC:Downloads");
                    ts.Increment(baseline, new double[] { 1, 0 });
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {
                    #region incremental_create-incremental-time-series
                    var ts = session.IncrementalTimeSeriesFor("companies/webstore", "INC:Downloads");
                    ts.Increment(baseline.AddMinutes(1), new double[] { 10, -10, 0, 0 });
                    session.SaveChanges();
                    #endregion
                }

                // Get time series HeartRates' time points data
                using (var session = store.OpenSession())
                {

                    #region incremental_get-time-series
                    // Get all time series entries
                    TimeSeriesEntry[] val = session.IncrementalTimeSeriesFor("companies/webstore", "INC:Downloads")
                    .Get(DateTime.MinValue, DateTime.MaxValue);
                    #endregion

                }

                #region incremental_delete-single-entry
                // Delete a single entry
                using (var session = store.OpenSession())
                {
                    session.IncrementalTimeSeriesFor<Downloads>("companies/webstore")
                        .Delete(baseline.AddMinutes(1));

                    session.SaveChanges();
                }
                #endregion

                #region incremental_delete-entries-range
                // Delete a range of entries from the time series
                using (var session = store.OpenSession())
                {
                    session.IncrementalTimeSeriesFor("companies/webstore", "INC:Downloads")
                        .Delete(baseline.AddDays(0), baseline.AddDays(9));

                    session.SaveChanges();
                }
                #endregion

                #region incremental_GetTimeSeriesOperation
                var pageSize = 100;
                var values = store.Operations
                                  .Send(new GetTimeSeriesOperation("users/ayende",
                                   "INC:Downloads", start: 0, pageSize: pageSize / 2, 
                                   returnFullResults: true));

                // we get 50 unique entries but we read 100 entries from the segment
                // so next call we should start from position 101: numberOfUniqueEntries + skippedResults 
                var nextStart = values.Entries.Length + values.SkippedResults;

                values = store.Operations
                                  .Send(new GetTimeSeriesOperation("users/ayende",
                                   "INC:Downloads", start: (int)nextStart,
                                   pageSize: pageSize / 2, returnFullResults: true));
                #endregion

            }
        }
        
        
        public DocumentStore getDocumentStore()
        {
            DocumentStore store = new DocumentStore
            {
                Urls = new[] { "http://localhost:8080" },
                Database = "TestDatabase"
            };
            store.Initialize();

            var parameters = new DeleteDatabasesOperation.Parameters
            {
                DatabaseNames = new[] { "TestDatabase" },
                HardDelete = true,
            };
            store.Maintenance.Server.Send(new DeleteDatabasesOperation(parameters));
            store.Maintenance.Server.Send(new CreateDatabaseOperation(new DatabaseRecord("TestDatabase")));

            return store;
        }

        [Fact]
        public void CanCreateSimpleTimeSeries()
        {
            using (var store = getDocumentStore())
            {
                var baseline = DateTime.Today;

                // Open a session
                using (var session = store.OpenSession())
                {
                    // Use the session to create a document
                    session.Store(new User { Name = "John" }, "users/john");

                    // Create an instance of TimeSeriesFor
                    // Pass an explicit document ID to the TimeSeriesFor constructor 
                    // Append a HeartRate of 70 at the first-minute timestamp 
                    session.TimeSeriesFor("users/john", "HeartRates")
                        .Append(baseline.AddMinutes(1), 70d, "watches/fitbit");

                    session.SaveChanges();
                }

                // Get time series names
                using (var session = store.OpenSession())
                {
                    #region timeseries_region_Retrieve-TimeSeries-Names
                    User user = session.Load<User>("users/john");
                    List<string> tsNames = session.Advanced.GetTimeSeriesFor(user);
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    // Use the session to load a document
                    User user = session.Load<User>("users/john");

                    // Pass the document object returned from session.Load as a param
                    // Retrieve a single value from the "HeartRates" time series 
                    IEnumerable<TimeSeriesEntry> val = session.TimeSeriesFor(user, "HeartRates")
                        .Get(DateTime.MinValue, DateTime.MaxValue);
                }

                #region timeseries_region_Delete-TimeSeriesFor-Single-Time-Point
                // Delete a single entry
                using (var session = store.OpenSession())
                {
                    session.TimeSeriesFor<HeartRate>("users/john")
                        .Delete(baseline.AddMinutes(1));

                    session.SaveChanges();
                }
                #endregion

            }
        }

        [Fact]
        public async Task StronglyTypes()
        {
            using (var store = getDocumentStore())
            {

                store.TimeSeries.Register<User, HeartRate>();
                store.TimeSeries.Register<User, StockPrice>();
                #region timeseries_region_Named-Values-Register
                store.TimeSeries.Register<User, RoutePoint>();
                #endregion

                var baseline = DateTime.Today;

                // Append entries
                using (var session = store.OpenSession())
                {
                    session.Store(new User { Name = "John" }, "users/john");

                    #region timeseries_region_Append-Named-Values-1
                    // Append coordinates
                    session.TimeSeriesFor<RoutePoint>("users/john")
                        .Append(baseline.AddHours(1), new RoutePoint
                        {
                            Latitude = 40.712776,
                            Longitude = -74.005974
                        }, "devices/Navigator");
                    #endregion

                    session.TimeSeriesFor<RoutePoint>("users/john")
                        .Append(baseline.AddHours(2), new RoutePoint
                        {
                            Latitude = 40.712781,
                            Longitude = -74.005979
                        }, "devices/Navigator");

                    session.TimeSeriesFor<RoutePoint>("users/john")
                        .Append(baseline.AddHours(3), new RoutePoint
                        {
                            Latitude = 40.712789,
                            Longitude = -74.005987
                        }, "devices/Navigator");

                    session.TimeSeriesFor<RoutePoint>("users/john")
                        .Append(baseline.AddHours(4), new RoutePoint
                        {
                            Latitude = 40.712792,
                            Longitude = -74.006002
                        }, "devices/Navigator");

                    session.SaveChanges();
                }

                // Get entries
                using (var session = store.OpenSession())
                {
                    // Use the session to load a document
                    User user = session.Load<User>("users/john");

                    // Pass the document object returned from session.Load as a param
                    // Retrieve a single value from the "HeartRates" time series 
                    TimeSeriesEntry<RoutePoint>[] results =
                        session.TimeSeriesFor<RoutePoint>("users/john")
                        .Get();
                }


                //append entries
                using (var session = store.OpenSession())
                {
                    session.Store(new User { Name = "John" }, "users/john");

                    // Append a HeartRate entry
                    session.TimeSeriesFor("users/john", "HeartRates")
                        .Append(baseline.AddMinutes(1), 70d, "watches/fitbit");

                    session.SaveChanges();
                }

                // append entries using a registered time series type
                using (var session = store.OpenSession())
                {
                    session.Store(new User { Name = "John" }, "users/john");

                    //store.TimeSeries.Register<User, HeartRate>();

                    session.TimeSeriesFor<HeartRate>("users/john")
                        .Append(DateTime.Now, new HeartRate
                        {
                            HeartRateMeasure = 80
                        }, "watches/anotherFirm");

                    session.SaveChanges();
                }

                // append multi-value entries by name
                #region timeseries_region_Append-Named-Values-2
                using (var session = store.OpenSession())
                {
                    session.Store(new User { Name = "John" }, "users/john");

                    session.TimeSeriesFor<StockPrice>("users/john")
                    .Append(baseline.AddDays(1), new StockPrice
                    {
                        Open = 52,
                        Close = 54,
                        High = 63.5,
                        Low = 51.4,
                        Volume = 9824,
                    }, "companies/kitchenAppliances");

                    session.TimeSeriesFor<StockPrice>("users/john")
                    .Append(baseline.AddDays(2), new StockPrice
                    {
                        Open = 54,
                        Close = 55,
                        High = 61.5,
                        Low = 49.4,
                        Volume = 8400,
                    }, "companies/kitchenAppliances");

                    session.TimeSeriesFor<StockPrice>("users/john")
                    .Append(baseline.AddDays(3), new StockPrice
                    {
                        Open = 55,
                        Close = 57,
                        High = 65.5,
                        Low = 50,
                        Volume = 9020,
                    }, "companies/kitchenAppliances");

                    session.SaveChanges();
                }
                #endregion

                #region timeseries_region_Append-Unnamed-Values-2
                using (var session = store.OpenSession())
                {
                    session.Store(new User { Name = "John" }, "users/john");

                    session.TimeSeriesFor("users/john", "StockPrices")
                    .Append(baseline.AddDays(1),
                        new[] { 52, 54, 63.5, 51.4, 9824 }, "companies/kitchenAppliances");

                    session.TimeSeriesFor("users/john", "StockPrices")
                    .Append(baseline.AddDays(2),
                        new[] { 54, 55, 61.5, 49.4, 8400 }, "companies/kitchenAppliances");

                    session.TimeSeriesFor("users/john", "StockPrices")
                    .Append(baseline.AddDays(3),
                        new[] { 55, 57, 65.5, 50, 9020 }, "companies/kitchenAppliances");

                    session.SaveChanges();
                }
                #endregion

                // append multi-value entries using a registered time series type
                using (var session = store.OpenSession())
                {
                    session.Store(new Company 
                    { 
                        Name = "kitchenAppliances",
                        Address = new Address { City = "New York" } 
                    },
                    "companies/kitchenAppliances");

                    session.TimeSeriesFor<StockPrice>("companies/kitchenAppliances")
                    .Append(baseline.AddDays(1), new StockPrice
                    {
                        Open = 52,
                        Close = 54,
                        High = 63.5,
                        Low = 51.4,
                        Volume = 9824,
                    }, "companies/kitchenAppliances");

                    session.TimeSeriesFor<StockPrice>("companies/kitchenAppliances")
                    .Append(baseline.AddDays(2), new StockPrice
                    {
                        Open = 54,
                        Close = 55,
                        High = 61.5,
                        Low = 49.4,
                        Volume = 8400,
                    }, "companies/kitchenAppliances");

                    session.TimeSeriesFor<StockPrice>("companies/kitchenAppliances")
                    .Append(baseline.AddDays(3), new StockPrice
                    {
                        Open = 55,
                        Close = 57,
                        High = 65.5,
                        Low = 50,
                        Volume = 9020,
                    }, "companies/kitchenAppliances");

                    session.SaveChanges();
                }


                double day1Volume;
                double day2Volume;
                double day3Volume;

                #region timeseries_region_Named-Values-Query
                // Named Values Query
                using (var session = store.OpenSession())
                {
                    IRavenQueryable<TimeSeriesRawResult<StockPrice>> query =
                        session.Query<Company>()
                        .Where(c => c.Address.City == "New York")
                        .Select(q => RavenQuery.TimeSeries<StockPrice>(q, "StockPrices", baseline, baseline.AddDays(3))
                            .Where(ts => ts.Tag == "companies/kitchenAppliances")
                            .ToList());

                    var result = query.ToList()[0];

                    day1Volume = result.Results[0].Value.Volume;
                    day2Volume = result.Results[1].Value.Volume;
                    day3Volume = result.Results[2].Value.Volume;
                }
                #endregion

                #region timeseries_region_Unnamed-Values-Query
                // Same query, only unnamed
                using (var session = store.OpenSession())
                {
                    IRavenQueryable<TimeSeriesRawResult> query =
                        session.Query<Company>()
                        .Where(c => c.Address.City == "New York")
                        .Select(q => RavenQuery.TimeSeries(q, "StockPrices", baseline, baseline.AddDays(3))
                            .Where(ts => ts.Tag == "companies/kitchenAppliances")
                            .ToList());

                    var result = query.ToList()[0];

                    day1Volume = result.Results[0].Values[4];
                    day2Volume = result.Results[1].Values[4];
                    day3Volume = result.Results[2].Values[4];
                }
                #endregion

                // get entries
                using (var session = store.OpenSession())
                {
                    // Use the session to load a document
                    User user = session.Load<User>("users/john");

                    // Pass the document object returned from session.Load as a param
                    // Retrieve a single value from the "HeartRates" time series 
                    TimeSeriesEntry[] val = session.TimeSeriesFor(user, "HeartRates")
                        .Get(DateTime.MinValue, DateTime.MaxValue);
                }


                #region timeseries_region_Get-NO-Named-Values
                // Use Get without a named type
                // Is the stock's closing-price rising?
                bool goingUp = false;

                using (var session = store.OpenSession())
                {
                    TimeSeriesEntry[] val = session.TimeSeriesFor("users/john", "StockPrices")
                        .Get();

                    var closePriceDay1 = val[0].Values[1];
                    var closePriceDay2 = val[1].Values[1];
                    var closePriceDay3 = val[2].Values[1];

                    if ((closePriceDay2 > closePriceDay1)
                        &&
                        (closePriceDay3 > closePriceDay2))
                        goingUp = true;
                }
                #endregion

                #region timeseries_region_Get-Named-Values
                goingUp = false;

                // Use Get with a Named type
                using (var session = store.OpenSession())
                {
                    TimeSeriesEntry<StockPrice>[] val = session.TimeSeriesFor<StockPrice>("users/john")
                        .Get();

                    var closePriceDay1 = val[0].Value.Close;
                    var closePriceDay2 = val[1].Value.Close;
                    var closePriceDay3 = val[2].Value.Close;

                    if ((closePriceDay2 > closePriceDay1)
                        &&
                        (closePriceDay3 > closePriceDay2))
                        goingUp = true;
                }
                #endregion

                // Use GetAsync
                using (var session = store.OpenAsyncSession())
                {
                    // get entries by GetAsync, using the strongly typed StockPrice class
                    var results = await session.TimeSeriesFor<StockPrice>("users/john")
                        .GetAsync();

                    var ClosePriceDay1 = results[0].Value.Close;
                    var ClosePriceDay2 = results[1].Value.Close;
                    var ClosePriceDay3 = results[2].Value.Close;

                    if ((ClosePriceDay2 > ClosePriceDay1)
                        &&
                        (ClosePriceDay3 > ClosePriceDay2))
                        goingUp = true;
                }

                // remove entries
                /*using (var session = store.OpenSession())
                {
                    session.TimeSeriesFor("users/john", "HeartRates")
                        .Delete(baseline.AddMinutes(1));

                    session.SaveChanges();
                }*/

                // remove entries using a registered time series type
                using (var session = store.OpenSession())
                {
                    session.TimeSeriesFor("users/john", "HeartRates")
                        .Delete(baseline.AddMinutes(1));

                    session.TimeSeriesFor<StockPrice>("users/john").Delete(baseline.AddDays(1), baseline.AddDays(2));

                    session.SaveChanges();
                }
            }
        }


        //query
        [Fact]
        public void CanAppendAndGetUsingQuery()
        {
            using (var store = getDocumentStore())
            {
                // Create a document
                using (var session = store.OpenSession())
                {
                    var user = new User
                    {
                        Name = "John"
                    };
                    session.Store(user);
                    session.SaveChanges();
                }

                // Query for a document with the Name property "John" and append it a time point
                using (var session = store.OpenSession())
                {
                    var baseline = DateTime.Today;

                    IRavenQueryable<User> query = session.Query<User>()
                        .Where(u => u.Name == "John");

                    var result = query.ToList();

                    session.TimeSeriesFor(result[0], "HeartRates")
                        .Append(baseline.AddMinutes(1), 72d, "watches/fitbit");

                    session.SaveChanges();
                }

                #region timeseries_region_Pass-TimeSeriesFor-Get-Query-Results
                // Query for a document with the Name property "John" 
                // and get its HeartRates time-series values
                using (var session = store.OpenSession())
                {
                    var baseline = DateTime.Today;

                    IRavenQueryable<User> query = session.Query<User>()
                        .Where(u => u.Name == "John");

                    var result = query.ToList();

                    TimeSeriesEntry[] val = session.TimeSeriesFor(result[0], "HeartRates")
                        .Get(DateTime.MinValue, DateTime.MaxValue);

                    session.SaveChanges();
                }
                #endregion
            }
        }

        //include
        [Fact]
        public void CanIncludeTimeSeriesData()
        {
            using (var store = getDocumentStore())
            {
                // Create a document
                using (var session = store.OpenSession())
                {
                    var user = new User
                    {
                        Name = "John"
                    };
                    session.Store(user);
                    session.SaveChanges();
                }

                // Query for a document with the Name property "John" and append it a time point
                using (var session = store.OpenSession())
                {
                    var baseline = DateTime.Today;

                    IRavenQueryable<User> query = session.Query<User>()
                        .Where(u => u.Name == "John");

                    var result = query.ToList();

                    for (var cnt = 0; cnt < 10; cnt++)
                    {
                        session.TimeSeriesFor(result[0], "HeartRates")
                            .Append(baseline.AddMinutes(cnt), 72d, "watches/fitbit");
                    }

                    session.SaveChanges();
                }

                #region timeseries_region_Load-Document-And-Include-TimeSeries
                // Load a document and Include a specified range of a time-series
                using (var session = store.OpenSession())
                {
                    var baseline = DateTime.Today;

                    User user = session.Load<User>("users/1-A", includeBuilder =>
                        includeBuilder.IncludeTimeSeries("HeartRates",
                        baseline.AddMinutes(3), baseline.AddMinutes(8)));

                    IEnumerable<TimeSeriesEntry> val = session.TimeSeriesFor("users/1-A", "HeartRates")
                        .Get(baseline.AddMinutes(3), baseline.AddMinutes(8));
                }
                #endregion

                #region timeseries_region_Query-Document-And-Include-TimeSeries
                // Query for a document and include a whole time-series
                using (var session = store.OpenSession())
                {
                    var baseline = DateTime.Today;

                    IRavenQueryable<User> query = session.Query<User>()
                        .Where(u => u.Name == "John")
                        .Include(includeBuilder => includeBuilder.IncludeTimeSeries(
                            "HeartRates", DateTime.MinValue, DateTime.MaxValue));

                    var result = query.ToList();

                    IEnumerable<TimeSeriesEntry> val = session.TimeSeriesFor(result[0], "HeartRates")
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

                    IRawDocumentQuery<User> query = session.Advanced.RawQuery<User>
                              ("from Users include timeseries('HeartRates', $start, $end)")
                        .AddParameter("start", start)
                        .AddParameter("end", end);

                    var result = query.ToList();

                    IEnumerable<TimeSeriesEntry> val = session.TimeSeriesFor(result[0], "HeartRates")
                        .Get(start, end);
                }
                #endregion

            }
        }


        [Fact]
        public void AppendWithIEnumerable()
        {
            using (var store = getDocumentStore())
            {
                var baseline = DateTime.Today;

                // Open a session
                using (var session = store.OpenSession())
                {
                    // Use the session to create a document
                    session.Store(new User { Name = "John" }, "users/john");

                    session.TimeSeriesFor("users/john", "HeartRates")
                    .Append(baseline.AddMinutes(1),
                            new[] { 65d, 52d, 72d },
                            "watches/fitbit");

                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {
                    // Use the session to load a document
                    User user = session.Load<User>("users/john");

                    // Pass the document object returned from session.Load as a param
                    // Retrieve a single value from the "HeartRates" time series 
                    TimeSeriesEntry[] val = session.TimeSeriesFor(user, "HeartRates")
                        .Get(DateTime.MinValue, DateTime.MaxValue);

                }

                // Get time series HeartRates' time points data
                using (var session = store.OpenSession())
                {

                    #region timeseries_region_Get-All-Entries-Using-Document-ID
                    // Get all time series entries
                    TimeSeriesEntry[] val = session.TimeSeriesFor("users/john", "HeartRates")
                        .Get(DateTime.MinValue, DateTime.MaxValue);
                    #endregion

                }
            }
        }

        [Fact]
        public void RemoveRange()
        {
            using (var store = getDocumentStore())
            {
                #region timeseries_region_TimeSeriesFor-Append-TimeSeries-Range
                var baseline = DateTime.Today;

                // Append 10 HeartRate values
                using (var session = store.OpenSession())
                {
                    session.Store(new User { Name = "John" }, "users/john");

                    ISessionDocumentTimeSeries tsf = session.TimeSeriesFor("users/john", "HeartRates");

                    for (int i = 0; i < 10; i++)
                    {
                        tsf.Append(baseline.AddSeconds(i), new[] { 67d }, "watches/fitbit");
                    }

                    session.SaveChanges();
                }
                #endregion

                #region timeseries_region_TimeSeriesFor-Delete-Time-Points-Range
                // Delete a range of entries from the time series
                using (var session = store.OpenSession())
                {
                    session.TimeSeriesFor("users/john", "HeartRates")
                        .Delete(baseline.AddSeconds(0), baseline.AddSeconds(9));

                    session.SaveChanges();
                }
                #endregion
            }
        }

        // Use GetTimeSeriesOperation and GetMultipleTimeSeriesOperation
        [Fact]
        public async Task UseGetTimeSeriesOperation()
        {
            using (var store = getDocumentStore())
            {
                // Create a document
                using (var session = store.OpenSession())
                {
                    var employee1 = new Employee
                    {
                        FirstName = "John"
                    };
                    session.Store(employee1);

                    var employee2 = new Employee
                    {
                        FirstName = "Mia"
                    };
                    session.Store(employee2);

                    var employee3 = new Employee
                    {
                        FirstName = "Emil"
                    };
                    session.Store(employee3);

                    session.SaveChanges();
                }

                // get employees Id list
                List<string> employeesIdList;
                using (var session = store.OpenSession())
                {
                    employeesIdList = session
                        .Query<Employee>()
                        .Select(e => e.Id)
                        .ToList();
                }

                // Append each employee a week (168 hours) of random exercise HeartRate values 
                // and a week (168 hours) of random rest HeartRate values 
                var baseTime = new DateTime(2020, 5, 17);
                Random randomValues = new Random();
                using (var session = store.OpenSession())
                {
                    for (var emp = 0; emp < employeesIdList.Count; emp++)
                    {
                        for (var tse = 0; tse < 168; tse++)
                        {
                            session.TimeSeriesFor(employeesIdList[emp], "ExerciseHeartRate")
                            .Append(baseTime.AddHours(tse),
                                    (68 + Math.Round(19 * randomValues.NextDouble())),
                                    "watches/fitbit");

                            session.TimeSeriesFor(employeesIdList[emp], "RestHeartRate")
                            .Append(baseTime.AddHours(tse),
                                    (52 + Math.Round(19 * randomValues.NextDouble())),
                                    "watches/fitbit");
                        }
                    }
                    session.SaveChanges();
                }

                const string documentId = "employees/1-A";

                #region timeseries_region_Get-Single-Time-Series
                // Get all values of a single time-series
                TimeSeriesRangeResult singleTimeSeriesDetails = store.Operations.Send(
                    new GetTimeSeriesOperation(documentId, "HeartRates", DateTime.MinValue, DateTime.MaxValue));
                #endregion

                #region timeseries_region_Get-Multiple-Time-Series
                // Get value ranges from two time-series using GetMultipleTimeSeriesOperation
                TimeSeriesDetails multipleTimesSeriesDetails = store.Operations.Send(
                        new GetMultipleTimeSeriesOperation(documentId, new List<TimeSeriesRange>
                            {
                                new TimeSeriesRange
                                {
                                    Name = "ExerciseHeartRate",
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
            }
        }


        [Fact]
        public void UseTimeSeriesBatchOperation()
        {
            const string documentId = "users/john";

            using (var store = getDocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    session.Store(new User(), documentId);
                    session.SaveChanges();
                }

                #region timeseries_region_Append-Using-TimeSeriesBatchOperation
                var baseline = DateTime.Today;

                var timeSeriesExerciseHeartRate = new TimeSeriesOperation
                {
                    Name = "RoutineHeartRate"
                };

                timeSeriesExerciseHeartRate.Append(new TimeSeriesOperation.AppendOperation { Tag = "watches/fitbit", Timestamp = baseline.AddMinutes(1), Values = new[] { 79d } });
                timeSeriesExerciseHeartRate.Append(new TimeSeriesOperation.AppendOperation { Tag = "watches/fitbit", Timestamp = baseline.AddMinutes(2), Values = new[] { 82d } });
                timeSeriesExerciseHeartRate.Append(new TimeSeriesOperation.AppendOperation { Tag = "watches/fitbit", Timestamp = baseline.AddMinutes(3), Values = new[] { 80d } });
                timeSeriesExerciseHeartRate.Append(new TimeSeriesOperation.AppendOperation { Tag = "watches/fitbit", Timestamp = baseline.AddMinutes(4), Values = new[] { 78d } });

                var timeSeriesBatch = new TimeSeriesBatchOperation(documentId, timeSeriesExerciseHeartRate);

                store.Operations.Send(timeSeriesBatch);
                #endregion


                #region timeseries_region_Delete-Range-Using-TimeSeriesBatchOperation
                var removeEntries = new TimeSeriesOperation
                {
                    Name = "RoutineHeartRate"
                };

                removeEntries.Delete(new TimeSeriesOperation.DeleteOperation { From = baseline.AddMinutes(2), To = baseline.AddMinutes(3) });

                var removeEntriesBatch = new TimeSeriesBatchOperation(documentId, removeEntries);

                store.Operations.Send(removeEntriesBatch);
                #endregion
            }
        }


        // bulk insert
        // Use BulkInsert.TimeSeriesBulkInsert.Append with doubles
        [Fact]
        public void AppendUsingBulkInsert()
        {
            using (var store = getDocumentStore())
            {
                // Create a document
                using (var session = store.OpenSession())
                {
                    var user = new User
                    {
                        Name = "John"
                    };
                    session.Store(user);
                    session.SaveChanges();
                }

                // Query for a document with the Name property "John" and append it a time point
                using (var session = store.OpenSession())
                {
                    var baseline = DateTime.Today;

                    IRavenQueryable<User> query = session.Query<User>()
                        .Where(u => u.Name == "John");

                    var result = query.ToList();
                    string documentId = result[0].Id;

                    #region timeseries_region_Use-BulkInsert-To-Append-2-Entries
                    // Use BulkInsert to append 2 time-series entries
                    using (BulkInsertOperation bulkInsert = store.BulkInsert())
                    {
                        using (TimeSeriesBulkInsert timeSeriesBulkInsert = bulkInsert.TimeSeriesFor(documentId, "HeartRates"))
                        {
                            timeSeriesBulkInsert.Append(baseline.AddMinutes(2), 61d, "watches/fitbit");
                            timeSeriesBulkInsert.Append(baseline.AddMinutes(3), 62d, "watches/apple-watch");
                        }
                    }
                    #endregion

                    #region timeseries_region_Use-BulkInsert-To-Append-100-Entries
                    // Use BulkInsert to append 100 time-series entries
                    using (BulkInsertOperation bulkInsert = store.BulkInsert())
                    {
                        using (TimeSeriesBulkInsert timeSeriesBulkInsert = bulkInsert.TimeSeriesFor(documentId, "HeartRates"))
                        {
                            for (int minute = 0; minute < 100; minute++)
                            {
                                timeSeriesBulkInsert.Append(baseline.AddMinutes(minute), new double[] { 80d }, "watches/fitbit");
                            }
                        }
                    }
                    #endregion
                }
            }
        }

        // bulk insert
        // Use BulkInsert.TimeSeriesBulkInsert.Append with IEnumerable
        [Fact]
        public void AppendUsingBulkInsertIEnumerable()
        {
            using (var store = getDocumentStore())
            {
                // Create a document
                using (var session = store.OpenSession())
                {
                    var user = new User
                    {
                        Name = "John"
                    };
                    session.Store(user);
                    session.SaveChanges();
                }

                // Query for a document with the Name property "John" and append it a time point
                using (var session = store.OpenSession())
                {
                    var baseline = DateTime.Today;

                    IRavenQueryable<User> query = session.Query<User>()
                        .Where(u => u.Name == "John");

                    var result = query.ToList();
                    string documentId = result[0].Id;

                    #region BulkInsert-overload-2-Two-HeartRate-Sets
                    // Use BulkInsert to append 2 sets of time series entries
                    using (BulkInsertOperation bulkInsert = store.BulkInsert())
                    {

                        ICollection<double> ExerciseHeartRate = new List<double>
                        { 89d, 82d, 85d };

                        ICollection<double> RestingHeartRate = new List<double>
                        {59d, 63d, 61d, 64d, 64d, 65d };

                        using (TimeSeriesBulkInsert timeSeriesBulkInsert = bulkInsert.TimeSeriesFor(documentId, "HeartRates"))
                        {
                            timeSeriesBulkInsert.Append(baseline.AddMinutes(2), ExerciseHeartRate, "watches/fitbit");
                            timeSeriesBulkInsert.Append(baseline.AddMinutes(3), RestingHeartRate, "watches/apple-watch");
                        }
                    }
                    #endregion

                    ICollection<double> values = new List<double>
                        {59d, 63d, 71d, 69d, 64, 65d };

                    // Use BulkInsert to append 100 multi-values time-series entries
                    using (BulkInsertOperation bulkInsert = store.BulkInsert())
                    {
                        using (TimeSeriesBulkInsert timeSeriesBulkInsert = bulkInsert.TimeSeriesFor(documentId, "HeartRates"))
                        {
                            for (int minute = 0; minute < 100; minute++)
                            {
                                timeSeriesBulkInsert.Append(baseline.AddMinutes(minute), values, "watches/fitbit");
                            }
                        }
                    }

                }
            }
        }





        // patching
        [Fact]
        public void PatchTimeSeries()
        {
            using (var store = getDocumentStore())
            {
                // Create a document
                using (var session = store.OpenSession())
                {
                    var user = new User
                    {
                        Name = "John"
                    };
                    session.Store(user);
                    session.SaveChanges();
                }

                // Patch a time-series to a document whose Name property is "John"
                using (var session = store.OpenSession())
                {
                    var baseline = DateTime.Today;

                    IRavenQueryable<User> query = session.Query<User>()
                        .Where(u => u.Name == "John");
                    var result = query.ToList();
                    string documentId = result[0].Id;

                    double[] values = { 59d };
                    const string tag = "watches/fitbit";
                    const string timeseries = "HeartRates";

                    session.Advanced.Defer(new PatchCommandData(documentId, null,
                        new PatchRequest
                        {
                            Script = @"timeseries(this, $timeseries)
                                     .append( 
                                        $timestamp, 
                                        $values, 
                                        $tag
                                      );", // 'tag' should appear last
                            Values =
                            {
                                { "timeseries", timeseries },
                                { "timestamp", baseline.AddMinutes(1) },
                                { "values", values },
                                { "tag", tag }
                            }
                        }, null));
                    session.SaveChanges();

                }
            }
        }

        // patching a document a single time-series entry
        // using session.Advanced.Defer
        [Fact]
        public void PatchSingleEntryUsingSessionDefer()
        {
            using (var store = getDocumentStore())
            {
                // Create a document
                using (var session = store.OpenSession())
                {
                    var user = new User
                    {
                        Name = "John"
                    };
                    session.Store(user);
                    session.SaveChanges();
                }

                // Patch a document a single time-series entry
                using (var session = store.OpenSession())
                {
                    var baseline = DateTime.Today;

                    session.Advanced.Defer(new PatchCommandData("users/1-A", null,
                        new PatchRequest
                        {
                            Script = @"timeseries(this, $timeseries)
                                     .append( 
                                         $timestamp, 
                                         $values, 
                                         $tag
                                       );",
                            Values =
                            {
                                { "timeseries", "HeartRates" },
                                { "timestamp", baseline.AddMinutes(1) },
                                { "values", 59d },
                                { "tag", "watches/fitbit" }
                            }
                        }, null));
                    session.SaveChanges();

                }
            }
        }


        // patching a document a single time-series entry
        // using PatchOperation
        [Fact]
        public void PatchSingleEntryUsingPatchOperation()
        {
            using (var store = getDocumentStore())
            {
                // Create a document
                using (var session = store.OpenSession())
                {
                    var user = new User
                    {
                        Name = "John"
                    };
                    session.Store(user);
                    session.SaveChanges();
                }

                #region TS_region-Operation_Patch-Append-Single-TS-Entry
                var baseline = DateTime.Today;

                store.Operations.Send(new PatchOperation("users/1-A", null,
                    new PatchRequest
                    {
                        Script = "timeseries(this, $timeseries).append($timestamp, $values, $tag);",
                        Values =
                        {
                            { "timeseries", "HeartRates" },
                            { "timestamp", baseline.AddMinutes(1) },
                            { "values", 59d },
                            { "tag", "watches/fitbit" }
                        }
                    }));
                #endregion
            }
        }


        // Patching: Append and Remove multiple time-series entries
        // Using session.Advanced.Defer
        [Fact]
        public void PatcAndhDeleteMultipleEntriesSession()
        {
            using (var store = getDocumentStore())
            {
                // Create a document
                using (var session = store.OpenSession())
                {
                    var user = new User
                    {
                        Name = "John"
                    };
                    session.Store(user);
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {
                    #region TS_region-Session_Patch-Append-100-Random-TS-Entries
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
                                { "timeseries", "HeartRates" },
                                { "timeStamps", timeStamps},
                                { "values", values },
                                { "tag", "watches/fitbit" }
                            }
                        }, null));

                    session.SaveChanges();
                    #endregion

                    #region TS_region-Session_Patch-Delete-50-TS-Entries
                    // Delete time-series entries
                    session.Advanced.Defer(new PatchCommandData("users/1-A", null,
                        new PatchRequest
                        {
                            Script = @"timeseries(this, $timeseries)
                                     .delete(
                                        $from, 
                                        $to
                                      );",
                            Values =
                            {
                                { "timeseries", "HeartRates" },
                                { "from", baseline.AddSeconds(0) },
                                { "to", baseline.AddSeconds(49) }
                            }
                        }, null));
                    session.SaveChanges();
                    #endregion

                }
            }
        }



        // Patching:multiple time-series entries Using session.Advanced.Defer
        [Fact]
        public void PatcMultipleEntriesSession()
        {
            using (var store = getDocumentStore())
            {
                // Create a document
                using (var session = store.OpenSession())
                {
                    var user = new User
                    {
                        Name = "John"
                    };
                    session.Store(user);
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {
                    var baseline = DateTime.Today;

                    // Create arrays of timestamps and random values to patch
                    List<double> values = new List<double>();
                    List<DateTime> timeStamps = new List<DateTime>();

                    for (var cnt = 0; cnt < 100; cnt++)
                    {
                        values.Add(68 + Math.Round(19 * new Random().NextDouble()));
                        timeStamps.Add(baseline.AddSeconds(cnt));
                    }

                    #region TS_region-Session_Patch-Append-TS-Entries
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
                                { "timeseries", "HeartRates" },
                                { "timeStamps", timeStamps},
                                { "values", values },
                                { "tag", "watches/fitbit" }
                            }
                        }, null));

                    session.SaveChanges();
                    #endregion

                }
            }
        }



        // Patching: Append and Remove multiple time-series entries
        // Using PatchOperation
        [Fact]
        public void PatcAndhDeleteMultipleEntriesOperation()
        {
            using (var store = getDocumentStore())
            {
                // Create a document
                using (var session = store.OpenSession())
                {
                    var user = new User
                    {
                        Name = "John"
                    };
                    session.Store(user);
                    session.SaveChanges();
                }

                // Patch a document 100 time-series entries
                using (var session = store.OpenSession())
                {
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
                                { "timeseries", "HeartRates" },
                                { "timeStamps", timeStamps},
                                { "values", values },
                                { "tag", "watches/fitbit" }
                            }

                        }));
                    #endregion

                    #region TS_region-Operation_Patch-Delete-50-TS-Entries
                    store.Operations.Send(new PatchOperation("users/1-A", null,
                        new PatchRequest
                        {
                            Script = "timeseries(this, $timeseries).delete($from, $to);",
                            Values =
                            {
                                { "timeseries", "HeartRates" },
                                { "from", baseline.AddSeconds(0) },
                                { "to", baseline.AddSeconds(49) }
                            }
                        }));
                    #endregion

                }
            }
        }



        //Query Time-Series Using Raw RQL
        [Fact]
        public void QueryTimeSeriesUsingRawRQL()
        {
            using (var store = getDocumentStore())
            {
                // Create a document
                using (var session = store.OpenSession())
                {
                    var user = new User
                    {
                        Name = "John"
                    };
                    session.Store(user);
                    session.SaveChanges();
                }

                // Query for a document with the Name property "John" and append it a time point
                using (var session = store.OpenSession())
                {
                    var baseline = DateTime.Today;

                    IRavenQueryable<User> query = session.Query<User>()
                        .Where(u => u.Name == "John");

                    var result = query.ToList();

                    for (var cnt = 0; cnt < 120; cnt++)
                    {
                        session.TimeSeriesFor(result[0], "HeartRates")
                            .Append(baseline.AddDays(cnt), 72d, "watches/fitbit");
                    }

                    session.SaveChanges();

                }

                // Query - LINQ format - Aggregation 
                using (var session = store.OpenSession())
                {
                    var baseline = DateTime.Today;

                    #region ts_region_LINQ-6-Aggregation
                    IRavenQueryable<TimeSeriesAggregationResult> query = session.Query<User>()
                        .Where(u => u.Age > 72)
                        .Select(q => RavenQuery.TimeSeries(q, "HeartRates", baseline, baseline.AddDays(10))
                            .Where(ts => ts.Tag == "watches/fitbit")
                            .GroupBy(g => g.Days(1))
                            .Select(g => new
                            {
                                Avg = g.Average(),
                                Cnt = g.Count()
                            })
                            .ToList());

                    var result = query.ToList();
                    #endregion
                }

                // Raw Query
                using (var session = store.OpenSession())
                {
                    var baseline = DateTime.Today;

                    var start = baseline;
                    var end = baseline.AddHours(1);

                    IRawDocumentQuery<User> query = session.Advanced.RawQuery<User>
                              ("from Users include timeseries('HeartRates', $start, $end)")
                        .AddParameter("start", start)
                        .AddParameter("end", end);

                    // Raw Query with aggregation
                    IRawDocumentQuery<TimeSeriesAggregationResult> aggregatedRawQuery =
                        session.Advanced.RawQuery<TimeSeriesAggregationResult>(@"
                            from Users as u where Age < 30
                            select timeseries(
                                from HeartRates between 
                                    '2020-05-27T00:00:00.0000000Z' 
                                        and '2020-06-23T00:00:00.0000000Z'
                                group by '7 days'
                                select min(), max())
                            ");

                    var aggregatedRawQueryResult = aggregatedRawQuery.ToList();
                }

            }
        }


        //Raw RQL and LINQ aggregation and projection queries
        [Fact]
        public void AggregationQueries()
        {
            using (var store = getDocumentStore())
            {
                // Create user documents and time-series
                using (var session = store.OpenSession())
                {
                    var employee1 = new User
                    {
                        Name = "John",
                        Age = 22
                    };
                    session.Store(employee1);

                    var employee2 = new User
                    {
                        Name = "Mia",
                        Age = 26
                    };
                    session.Store(employee2);

                    var employee3 = new User
                    {
                        Name = "Emil",
                        Age = 29
                    };
                    session.Store(employee3);

                    session.SaveChanges();
                }

                // get employees Id list
                List<string> UsersIdList;
                using (var session = store.OpenSession())
                {
                    UsersIdList = session
                        .Query<User>()
                        .Select(e => e.Id)
                        .ToList();
                }

                // Append each employee a week (168 hours) of random HeartRate values 
                var baseTime = new DateTime(2020, 5, 17);
                Random randomValues = new Random();
                using (var session = store.OpenSession())
                {
                    for (var emp = 0; emp < UsersIdList.Count; emp++)
                    {
                        for (var tse = 0; tse < 168; tse++)
                        {
                            session.TimeSeriesFor(UsersIdList[emp], "HeartRates")
                            .Append(baseTime.AddHours(tse),
                                    (68 + Math.Round(19 * randomValues.NextDouble())),
                                    "watches/fitbit");
                        }
                    }
                    session.SaveChanges();
                }

                // Query - LINQ format - HeartRate
                using (var session = store.OpenSession())
                {
                    var baseline = new DateTime(2020, 5, 17);

                    IRavenQueryable<TimeSeriesAggregationResult> query = session.Query<User>()
                        .Where(u => u.Age < 30)
                        .Select(q => RavenQuery.TimeSeries(q, "HeartRates", baseline, baseline.AddDays(7))
                            .Where(ts => ts.Tag == "watches/fitbit")
                            .GroupBy(g => g.Days(1))
                            .Select(g => new
                            {
                                Min = g.Min(),
                                Max = g.Max()
                            })
                            .ToList());

                    var result = query.ToList();
                }

                // Query - LINQ format - StockPrice
                using (var session = store.OpenSession())
                {
                    var baseline = new DateTime(2020, 5, 17);

                    #region ts_region_LINQ-Aggregation-and-Projections-StockPrice
                    IRavenQueryable<TimeSeriesAggregationResult> query = session.Query<Company>()
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

                // Raw Query - HeartRates using "where Tag in"
                using (var session = store.OpenSession())
                {
                    var baseline = new DateTime(2020, 5, 17);

                    var start = baseline;
                    var end = baseline.AddHours(1);

                    // Raw Query with aggregation
                    IRawDocumentQuery<TimeSeriesAggregationResult> aggregatedRawQuery =
                        session.Advanced.RawQuery<TimeSeriesAggregationResult>(@"
                            from Users as u where Age < 30
                            select timeseries(
                                from HeartRates between 
                                    '2020-05-17T00:00:00.0000000Z' 
                                    and '2020-05-23T00:00:00.0000000Z'
                                    where Tag in ('watches/Letsfit', 'watches/Willful', 'watches/Lintelek')
                                group by '1 days'
                                select min(), max()
                            )
                            ");

                    var aggregatedRawQueryResult = aggregatedRawQuery.ToList();
                }


                // Raw Query - HeartRates using "where Tag =="
                using (var session = store.OpenSession())
                {
                    var baseline = new DateTime(2020, 5, 17);

                    var start = baseline;
                    var end = baseline.AddHours(1);

                    // Raw Query with aggregation
                    IRawDocumentQuery<TimeSeriesAggregationResult> aggregatedRawQuery =
                        session.Advanced.RawQuery<TimeSeriesAggregationResult>(@"
                            from Users as u where Age < 30
                            select timeseries(
                                from HeartRates between 
                                    '2020-05-17T00:00:00.0000000Z' 
                                    and '2020-05-23T00:00:00.0000000Z'
                                    where Tag == 'watches/fitbit'
                                group by '1 days'
                                select min(), max()
                            )
                            ");

                    var aggregatedRawQueryResult = aggregatedRawQuery.ToList();

                }


                // Raw Query - StockPrice - Select Syntax
                using (var session = store.OpenSession())
                {
                    var baseline = new DateTime(2020, 5, 17);

                    var start = baseline;
                    var end = baseline.AddHours(1);

                    // Select Syntax
                    #region ts_region_Raw-RQL-Select-Syntax-Aggregation-and-Projections-StockPrice
                    IRawDocumentQuery<TimeSeriesAggregationResult> aggregatedRawQuery =
                        session.Advanced.RawQuery<TimeSeriesAggregationResult>(@"
                            from Companies as c
                                where c.Address.Country = 'USA'
                                select timeseries ( 
                                    from StockPrices 
                                    where Values[4] > 500000
                                        group by '7 day'
                                        select max(), min()
                                )
                            ");

                    var aggregatedRawQueryResult = aggregatedRawQuery.ToList();
                    #endregion
                }

                // LINQ query group by tag
                using (var session = store.OpenSession())
                {
                    #region LINQ_GroupBy_Tag
                    var query = session.Query<User>()
                        .Select(u => RavenQuery.TimeSeries(u, "HeartRates")
                            .GroupBy(g => g
                                    .Hours(1)
                                    .ByTag()
                                   )
                            .Select(g => new
                            {
                                Max = g.Max(),
                                Min = g.Min()
                            }));
                    #endregion
                }

                // Raw Query - StockPrice
                using (var session = store.OpenSession())
                {
                    var baseline = new DateTime(2020, 5, 17);

                    var start = baseline;
                    var end = baseline.AddHours(1);

                    // Select Syntax
                    #region ts_region_Raw-RQL-Declare-Syntax-Aggregation-and-Projections-StockPrice
                    IRawDocumentQuery<TimeSeriesAggregationResult> aggregatedRawQuery =
                        session.Advanced.RawQuery<TimeSeriesAggregationResult>(@"
                            declare timeseries SP(c) {
                                from c.StockPrices
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
            }
        }

        //Query Time-Series Using Raw RQL
        [Fact]
        public void QueryRawRQLNoAggregation()
        {
            using (var store = getDocumentStore())
            {
                // Create a document
                using (var session = store.OpenSession())
                {
                    var user = new User
                    {
                        Name = "John",
                        Age = 27
                    };
                    session.Store(user);
                    session.SaveChanges();
                }

                // Query for a document with the Name property "John" and append it a time point
                using (var session = store.OpenSession())
                {
                    // May 17 2020, 18:00:00
                    var baseline = new DateTime(2020, 5, 17, 00, 00, 00);

                    IRavenQueryable<User> query = session.Query<User>()
                        .Where(u => u.Name == "John");

                    var result = query.ToList();

                    // Two weeks of hourly HeartRate values
                    for (var cnt = 0; cnt < 336; cnt++)
                    {
                        session.TimeSeriesFor(result[0], "HeartRates")
                            .Append(baseline.AddHours(cnt), 72d, "watches/fitbit");
                    }

                    session.SaveChanges();

                }

                // Raw Query
                using (var session = store.OpenSession())
                {
                    #region ts_region_Raw-Query-Non-Aggregated-Declare-Syntax
                    // May 17 2020, 00:00:00
                    var baseline = new DateTime(2020, 5, 17, 00, 00, 00);

                    // Raw query with no aggregation - Declare syntax
                    IRawDocumentQuery<TimeSeriesRawResult> nonAggregatedRawQuery =
                        session.Advanced.RawQuery<TimeSeriesRawResult>(@"
                            declare timeseries getHeartRates(user) 
                            {
                                from user.HeartRates 
                                    between $start and $end
                                    offset '02:00'
                            }
                            from Users as u where Age < 30
                            select getHeartRates(u)
                            ")
                        .AddParameter("start", baseline)
                        .AddParameter("end", baseline.AddHours(24));

                    var nonAggregatedRawQueryResult = nonAggregatedRawQuery.ToList();
                    #endregion

                }

                using (var session = store.OpenSession())
                {
                    #region ts_region_Raw-Query-Non-Aggregated-Select-Syntax
                    // May 17 2020, 00:00:00
                    var baseline = new DateTime(2020, 5, 17, 00, 00, 00);

                    // Raw query with no aggregation - Select syntax
                    IRawDocumentQuery<TimeSeriesRawResult> nonAggregatedRawQuery =
                        session.Advanced.RawQuery<TimeSeriesRawResult>(@"
                            from Users as u where Age < 30                            
                            select timeseries (
                                from HeartRates 
                                    between $start and $end
                                    offset '02:00'
                            )")
                        .AddParameter("start", baseline)
                        .AddParameter("end", baseline.AddHours(24));

                    var nonAggregatedRawQueryResult = nonAggregatedRawQuery.ToList();
                    #endregion

                }

                using (var session = store.OpenSession())
                {
                    #region ts_region_Raw-Query-Aggregated
                    // May 17 2020, 00:00:00
                    var baseline = new DateTime(2020, 5, 17, 00, 00, 00);

                    // Raw Query with aggregation
                    IRawDocumentQuery<TimeSeriesAggregationResult> aggregatedRawQuery =
                        session.Advanced.RawQuery<TimeSeriesAggregationResult>(@"
                            from Users as u
                            select timeseries(
                                from HeartRates 
                                    between $start and $end
                                group by '1 days'
                                select min(), max())
                            ")
                        .AddParameter("start", baseline)
                        .AddParameter("end", baseline.AddDays(7));

                    var aggregatedRawQueryResult = aggregatedRawQuery.ToList();
                    #endregion

                }


            }
        }

        // simple RQL query and its LINQ equivalent
        [Fact]
        public void RawRqlAndLinqqueries()
        {
            using (var store = getDocumentStore())
            {
                // Create a document
                using (var session = store.OpenSession())
                {
                    var user = new User
                    {
                        Name = "John",
                        Age = 28
                    };
                    session.Store(user);
                    session.SaveChanges();
                }

                // Query for a document with the Name property "John" and append it a time point
                using (var session = store.OpenSession())
                {
                    // May 17 2020, 18:00:00
                    var baseline = new DateTime(2020, 5, 17, 00, 00, 00);

                    IRavenQueryable<User> query = session.Query<User>()
                        .Where(u => u.Name == "John");

                    var result = query.ToList();

                    // Two weeks of hourly HeartRate values
                    for (var cnt = 0; cnt < 336; cnt++)
                    {
                        session.TimeSeriesFor(result[0], "HeartRates")
                            .Append(baseline.AddHours(cnt), 72d, "watches/fitbit");
                    }

                    session.SaveChanges();

                }

                using (var session = store.OpenSession())
                {
                    #region ts_region_LINQ-2-RQL-Equivalent
                    // May 17 2020, 00:00:00
                    var baseline = new DateTime(2020, 5, 17, 00, 00, 00);

                    // Raw query with no aggregation - Select syntax
                    IRawDocumentQuery<TimeSeriesRawResult> nonAggregatedRawQuery =
                        session.Advanced.RawQuery<TimeSeriesRawResult>(@"
                            from Users as u where Age < 30                            
                            select timeseries (
                                from HeartRates
                            )");

                    var nonAggregatedRawQueryResult = nonAggregatedRawQuery.ToList();
                    #endregion
                }

                #region ts_region_LINQ-1-Select-Timeseries
                // Query - LINQ format
                using (var session = store.OpenSession())
                {
                    var baseline = new DateTime(2020, 5, 17, 00, 00, 00);

                    IRavenQueryable<TimeSeriesRawResult> query =
                        (IRavenQueryable<TimeSeriesRawResult>)session.Query<User>()
                            .Where(u => u.Age < 30)
                            .Select(q => RavenQuery.TimeSeries(q, "HeartRates")
                            .Where(ts => ts.Tag == "watches/fitbit")
                            .ToList());

                    var result = query.ToList();
                }
                #endregion


                // Query - LINQ format with Range selection 1
                using (var session = store.OpenSession())
                {
                    var baseline = new DateTime(2020, 5, 17, 00, 00, 00);

                    #region ts_region_LINQ-3-Range-Selection
                    IRavenQueryable<TimeSeriesRawResult> query =
                        (IRavenQueryable<TimeSeriesRawResult>)session.Query<User>()
                            .Where(u => u.Age < 30)
                            .Select(q => RavenQuery.TimeSeries(q, "HeartRates", baseline, baseline.AddDays(3))
                            .ToList());

                    var result = query.ToList();
                    #endregion
                }

                // Query - LINQ format with Range selection 2
                using (var session = store.OpenSession())
                {
                    var baseline = new DateTime(2020, 5, 17, 00, 00, 00);

                    #region ts_region_LINQ-4-Where
                    IRavenQueryable<TimeSeriesRawResult> query =
                        (IRavenQueryable<TimeSeriesRawResult>)session.Query<User>()

                            // Choose user profiles of users under the age of 30
                            .Where(u => u.Age < 30)

                            .Select(q => RavenQuery.TimeSeries(q, "HeartRates", baseline, baseline.AddDays(3))

                            // Filter entries by tag.  
                            .Where(ts => ts.Tag == "watches/fitbit")

                            .ToList());

                    var result = query.ToList();
                    #endregion
                }


                // Query - LINQ format - LoadByTag to find employee address
                using (var session = store.OpenSession())
                {
                    var baseline = new DateTime(2020, 5, 17, 00, 00, 00);

                    IRavenQueryable<TimeSeriesRawResult> query =
                        (IRavenQueryable<TimeSeriesRawResult>)session.Query<Company>()

                            // Choose profiles of US companies
                            .Where(c => c.Address.Country == "USA")

                            .Select(q => RavenQuery.TimeSeries(q, "StockPrices")

                            .LoadByTag<Employee>()
                            .Where((ts, src) => src.Address.Country == "USA")

                            .ToList());

                    var result = query.ToList();
                }


                using (var session = store.OpenSession())
                {
                    // May 17 2020, 00:00:00
                    var baseline = new DateTime(2020, 5, 17, 00, 00, 00);

                    #region ts_region_Filter-By-load-Tag-Raw-RQL
                    // Raw query with no aggregation - Select syntax
                    IRawDocumentQuery<TimeSeriesRawResult> nonAggregatedRawQuery =
                        session.Advanced.RawQuery<TimeSeriesRawResult>(@"
                            from Companies as c where c.Address.Country = 'USA'
                            select timeseries(
                                from StockPrices
                                   load Tag as emp
                                   where emp.Title == 'Sales Representative'
                            )");

                    var nonAggregatedRawQueryResult = nonAggregatedRawQuery.ToList();
                    #endregion
                }

                // Query - LINQ format - LoadByTag to find a stock broker
                using (var session = store.OpenSession())
                {
                    var baseline = new DateTime(2020, 5, 17, 00, 00, 00);

                    #region ts_region_Filter-By-LoadByTag-LINQ
                    IRavenQueryable<TimeSeriesRawResult> query =
                        (IRavenQueryable<TimeSeriesRawResult>)session.Query<Company>()

                            // Choose user profiles of users under the age of 30
                            .Where(c => c.Address.Country == "USA")
                            .Select(q => RavenQuery.TimeSeries(q, "StockPrices")

                            .LoadByTag<Employee>()
                            .Where((ts, src) => src.Title == "Sales Representative")

                            .ToList());

                    var result = query.ToList();
                    #endregion
                }

                /*
                                // Query - LINQ format
                                using (var session = store.OpenSession())
                                {
                                    //var baseline = DateTime.Today;
                                    var baseline = new DateTime(2020, 5, 17, 00, 00, 00);

                                    IRavenQueryable<TimeSeriesRawResult> query = 
                                        (IRavenQueryable <TimeSeriesRawResult>)session.Query<User>()
                                        .Where(u => u.Age < 30)
                                        .Select(q => RavenQuery.TimeSeries(q, "HeartRates", baseline, baseline.AddMonths(3))
                                            .Where(ts => ts.Tag == "watches/fitbit")
                                            //.GroupBy(g => g.Months(1))
                                            //.Select(g => new
                                            //{
                                                //Avg = g.Average(),
                                                //Cnt = g.Count()
                                            //})
                                            .ToList());

                                    var result = query.ToList();
                                }
                */

            }
        }

        // Time series Document Query examples
        public void TSDocumentQueries()
        {
            using (var store = getDocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region TS_DocQuery_1
                    var query = session.Advanced.DocumentQuery<User>()
                        .SelectTimeSeries(builder => builder
                            .From("Heartrate")
                            .ToList());
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region TS_DocQuery_2
                    var query = session.Advanced.DocumentQuery<User>()
                        .SelectTimeSeries(builder => builder
                            .From("Heartrate")
                            .Between(DateTime.Now, DateTime.Now.AddDays(1))
                            .ToList());
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region TS_DocQuery_3
                    var query1 = session.Advanced.DocumentQuery<User>()
                        .SelectTimeSeries(builder => builder
                            .From("Heartrate")
                            .FromFirst(x => x.Days(3))
                            .ToList());

                    var query2 = session.Advanced.DocumentQuery<User>()
                        .SelectTimeSeries(builder => builder
                            .From("Heartrate")
                            .FromLast(x => x.Days(3))
                            .ToList());
                    #endregion
                }

            }
        }

        //Various raw RQL queries
        [Fact]
        public void QueryRawRQLQueries()
        {
            using (var store = getDocumentStore())
            {
                // Create a document
                using (var session = store.OpenSession())
                {
                    var user = new User
                    {
                        Name = "John",
                        Age = 27
                    };
                    session.Store(user);
                    session.SaveChanges();
                }

                // Query for a document with the Name property "John" and append it a time point
                using (var session = store.OpenSession())
                {
                    var baseline = new DateTime(2020, 5, 17);

                    IRavenQueryable<User> query = session.Query<User>()
                        .Where(u => u.Name == "John");

                    var result = query.ToList();
                    Random randomValues = new Random();

                    for (var cnt = 0; cnt < 120; cnt++)
                    {
                        session.TimeSeriesFor(result[0], "HeartRates")
                            .Append(baseline.AddDays(cnt), (68 + Math.Round(19 * randomValues.NextDouble())), "watches/fitbit");
                    }

                    session.SaveChanges();

                }

                // Raw Query
                using (var session = store.OpenSession())
                {
                    var baseline = DateTime.Today;

                    // Raw query with a range selection
                    IRawDocumentQuery<TimeSeriesRawResult> nonAggregatedRawQuery =
                        session.Advanced.RawQuery<TimeSeriesRawResult>(@"
                            declare timeseries ts(jogger) 
                            {
                                from jogger.HeartRates 
                                    between $start and $end
                            }
                            from Users as jog where Age < 30
                            select ts(jog)
                            ")
                        .AddParameter("start", new DateTime(2020, 5, 17))
                        .AddParameter("end", new DateTime(2020, 5, 23));

                    var nonAggregatedRawQueryResult = nonAggregatedRawQuery.ToList();

                }

            }
        }



        // patching a document a single time-series entry
        // using PatchByQueryOperation
        [Fact]
        public async Task PatchTimeSerieshByQuery()
        {
            using (var store = getDocumentStore())
            {
                // Create a document
                using (var session = store.OpenSession())
                {
                    var user1 = new User
                    {
                        Name = "John"
                    };
                    session.Store(user1);

                    var user2 = new User
                    {
                        Name = "Mia"
                    };
                    session.Store(user2);

                    var user3 = new User
                    {
                        Name = "Emil"
                    };
                    session.Store(user3);

                    session.SaveChanges();
                }

                var baseline = DateTime.Today;

                #region TS_region-PatchByQueryOperation-Append-To-Multiple-Docs
                PatchByQueryOperation appendRestHeartRateOperation = new PatchByQueryOperation(new IndexQuery
                {
                    Query = @"from Users as u update
                                {
                                    timeseries(u, $name).append($time, $values, $tag)
                                }",
                    QueryParameters = new Parameters
                            {
                                { "name", "RestHeartRate" },
                                { "time", baseline.AddMinutes(1) },
                                { "values", new[]{59d} },
                                { "tag", "watches/fitbit1" }
                            }
                });
                store.Operations.Send(appendRestHeartRateOperation);
                #endregion

                // Append time series to multiple documents
                PatchByQueryOperation appendExerciseHeartRateOperation = new PatchByQueryOperation(new IndexQuery
                {
                    Query = @"from Users as u update
                                {
                                    timeseries(u, $name).append($time, $values, $tag)
                                }",
                    QueryParameters = new Parameters
                            {
                                { "name", "ExerciseHeartRate" },
                                { "time", baseline.AddMinutes(1) },
                                { "values", new[]{89d} },
                                { "tag", "watches/fitbit2" }
                            }
                });
                store.Operations.Send(appendExerciseHeartRateOperation);

                // Get time-series data from all users 
                PatchByQueryOperation getOperation = new PatchByQueryOperation(new IndexQuery
                {
                    Query = @"from users as u update
                                {
                                    timeseries(u, $name).get($from, $to)
                                }",
                    QueryParameters = new Parameters
                            {
                                { "name", "HeartRates" },
                                { "from", DateTime.MinValue },
                                { "to", DateTime.MaxValue }
                            }
                });
                Operation getop = store.Operations.Send(getOperation);
                var getResult = getop.WaitForCompletion();

                // Get and project chosen time-series data from all users 
                PatchByQueryOperation getExerciseHeartRateOperation = new PatchByQueryOperation(new IndexQuery
                {
                    Query = @"
		                declare function foo(doc){
			                var entries = timeseries(doc, $name).get($from, $to);
			                var differentTags = [];
			                for (var i = 0; i < entries.length; i++)
			                {
				                var e = entries[i];
				                if (e.Tag !== null)
				                {
					                if (!differentTags.includes(e.Tag))
					                {
						                differentTags.push(e.Tag);
					                }
				                }
			                }
			                doc.NumberOfUniqueTagsInTS = differentTags.length;
			                return doc;
		                }

		                from Users as u
		                update
		                {
			                put(id(u), foo(u))
		                }",

                    QueryParameters = new Parameters
                    {
                        { "name", "ExerciseHeartRate" },
                        { "from", DateTime.MinValue },
                        { "to", DateTime.MaxValue }
                    }
                });

                var result = store.Operations.Send(getExerciseHeartRateOperation).WaitForCompletion();

                #region TS_region-PatchByQueryOperation-Delete-From-Multiple-Docs
                // Delete time-series from all users
                PatchByQueryOperation deleteOperation = new PatchByQueryOperation(new IndexQuery
                {
                    Query = @"from Users as u
                                update
                                {
                                    timeseries(u, $name).delete($from, $to)
                                }",
                    QueryParameters = new Parameters
                            {
                                { "name", "HeartRates" },
                                { "from", DateTime.MinValue },
                                { "to", DateTime.MaxValue }
                            }
                });
                store.Operations.Send(deleteOperation);
                #endregion
            }
        }



        // patching a document a single time-series entry
        // using PatchByQueryOperation
        [Fact]
        public async Task PatchTimeSerieshByQueryWithGet()
        {
            using (var store = getDocumentStore())
            {
                // Create a document
                using (var session = store.OpenSession())
                {
                    var user1 = new User
                    {
                        Name = "John"
                    };
                    session.Store(user1);

                    var user2 = new User
                    {
                        Name = "Mia"
                    };
                    session.Store(user2);

                    var user3 = new User
                    {
                        Name = "Emil"
                    };
                    session.Store(user3);

                    var user4 = new User
                    {
                        Name = "shaya"
                    };
                    session.Store(user4);

                    session.SaveChanges();
                }

                var baseline = DateTime.Today;

                // get employees Id list
                List<string> usersIdList;
                using (var session = store.OpenSession())
                {
                    usersIdList = session
                        .Query<User>()
                        .Select(u => u.Id)
                        .ToList();
                }

                // Append each employee a week (168 hours) of random HeartRate values 
                var baseTime = new DateTime(2020, 5, 17);
                Random randomValues = new Random();
                using (var session = store.OpenSession())
                {
                    for (var user = 0; user < usersIdList.Count; user++)
                    {
                        for (var tse = 0; tse < 168; tse++)
                        {
                            session.TimeSeriesFor(usersIdList[user], "ExerciseHeartRate")
                            .Append(baseTime.AddHours(tse),
                                    (68 + Math.Round(19 * randomValues.NextDouble())),
                                    "watches/fitbit" + tse.ToString());
                        }
                    }
                    session.SaveChanges();
                }

                #region TS_region-PatchByQueryOperation-Get                
                PatchByQueryOperation patchNumOfUniqueTags = new PatchByQueryOperation(new IndexQuery
                {
                    Query = @"
		                declare function foo(doc){
			                var entries = timeseries(doc, $name).get($from, $to);
			                var differentTags = [];
			                for (var i = 0; i < entries.length; i++)
			                {
				                var e = entries[i];
				                if (e.Tag !== null)
				                {
					                if (!differentTags.includes(e.Tag))
					                {
						                differentTags.push(e.Tag);
					                }
				                }
			                }
			                doc.NumberOfUniqueTagsInTS = differentTags.length;
			                return doc;
		                }

		                from Users as u
		                update
		                {
			                put(id(u), foo(u))
		                }",

                    QueryParameters = new Parameters
                    {
                        { "name", "ExerciseHeartRate" },
                        { "from", DateTime.MinValue },
                        { "to", DateTime.MaxValue }
                    }
                });

                var result = store.Operations.Send(patchNumOfUniqueTags).WaitForCompletion();
                #endregion

                // Delete time-series from all users
                PatchByQueryOperation removeOperation = new PatchByQueryOperation(new IndexQuery
                {
                    Query = @"from Users as u
                                update
                                {
                                    timeseries(u, $name).delete($from, $to)
                                }",
                    QueryParameters = new Parameters
                            {
                                { "name", "HeartRates" },
                                { "from", DateTime.MinValue },
                                { "to", DateTime.MaxValue }
                            }
                });
                store.Operations.Send(removeOperation);

            }
        }

        // patch HeartRate TS to all employees
        // using PatchByQueryOperation
        // not that all employees get the same times-series entries.
        [Fact]
        public async Task PatchEmployeesHeartRateTS1()
        {
            using (var store = getDocumentStore())
            {
                // Create a document
                using (var session = store.OpenSession())
                {
                    var employee1 = new Employee
                    {
                        FirstName = "John"
                    };
                    session.Store(employee1);

                    var employee2 = new Employee
                    {
                        FirstName = "Mia"
                    };
                    session.Store(employee2);

                    var employee3 = new Employee
                    {
                        FirstName = "Emil"
                    };
                    session.Store(employee3);

                    session.SaveChanges();
                }

                var baseTime = new DateTime(2020, 5, 17);
                Random randomValues = new Random();

                // an array with a week of random hourly HeartRate values
                var valuesToAppend = Enumerable.Range(0, 168) // 168 hours a week
                    .Select(i =>
                    {
                        return new TimeSeriesEntry
                        {
                            Tag = "watches/fitbit",
                            Timestamp = baseTime.AddHours(i),
                            Values = new[] { 68 + Math.Round(19 * randomValues.NextDouble()) }
                        };
                    }).ToArray();

                // Append time-series to all employees
                PatchByQueryOperation appendHeartRate = new PatchByQueryOperation(new IndexQuery
                {
                    Query = @"from Employees as e update
                                {
                                    for(var i = 0; i < $valuesToAppend.length; i++){
                                        timeseries(e, $name)
                                        .append(
                                            $valuesToAppend[i].Timestamp, 
                                            $valuesToAppend[i].Values, 
                                            $valuesToAppend[i].Tag);
                                    }
                                }",
                    QueryParameters = new Parameters
                            {
                                {"valuesToAppend", valuesToAppend},
                                { "name", "HeartRates" },
                            }
                });

                store.Operations.Send(appendHeartRate);
            }
        }


        // Appending random time-series entries to all employees
        [Fact]
        public async Task PatchEmployeesHeartRateTS2()
        {
            using (var store = getDocumentStore())
            {
                // Create a document
                using (var session = store.OpenSession())
                {
                    var employee1 = new Employee
                    {
                        FirstName = "John"
                    };
                    session.Store(employee1);

                    var employee2 = new Employee
                    {
                        FirstName = "Mia"
                    };
                    session.Store(employee2);

                    var employee3 = new Employee
                    {
                        FirstName = "Emil"
                    };
                    session.Store(employee3);

                    session.SaveChanges();
                }

                // get employees Id list
                List<string> employeesIdList;
                using (var session = store.OpenSession())
                {
                    employeesIdList = session
                        .Query<Employee>()
                        .Select(e => e.Id)
                        .ToList();
                }

                // Append each employee a week (168 hours) of random HeartRate values 
                var baseTime = new DateTime(2020, 5, 17);
                Random randomValues = new Random();
                using (var session = store.OpenSession())
                {
                    for (var emp = 0; emp < employeesIdList.Count; emp++)
                    {
                        for (var tse = 0; tse < 168; tse++)
                        {
                            session.TimeSeriesFor(employeesIdList[emp], "HeartRates")
                            .Append(baseTime.AddHours(tse),
                                    (68 + Math.Round(19 * randomValues.NextDouble())),
                                    "watches/fitbit");
                        }
                    }
                    session.SaveChanges();
                }
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
                    "HeartRates",
                    timeSeries => from ts in timeSeries
                                  from entry in ts.Entries
                                  select new
                                  {
                                      HeartBeat =
                                        entry.Values[0],
                                      entry.Timestamp.Date,
                                      User = ts.DocumentId,
                                      Tag = entry.Tag
                                  });
            }
        }
        #endregion

        private struct HeartRate
        {
            [TimeSeriesValue(0)] public double HeartRateMeasure;
        }

        private struct Downloads
        {
            [TimeSeriesValue(0)] public double DownloadsCount;
        }

        #region Custom-Data-Type-1
        private struct StockPrice
        {
            [TimeSeriesValue(0)] public double Open;
            [TimeSeriesValue(1)] public double Close;
            [TimeSeriesValue(2)] public double High;
            [TimeSeriesValue(3)] public double Low;
            [TimeSeriesValue(4)] public double Volume;
        }
        #endregion

        #region Custom-Data-Type-2
        private struct RoutePoint
        {
            [TimeSeriesValue(0)] public double Latitude;
            [TimeSeriesValue(1)] public double Longitude;
        }
        #endregion

        public class User
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string LastName { get; set; }
            public string AddressId { get; set; }
            public int Count { get; set; }
            public int Age { get; set; }
        }

        public class Company
        {
            public string Id { get; set; }
            public string ExternalId { get; set; }
            public string Name { get; set; }
            public Contact Contact { get; set; }
            public Address Address { get; set; }
            public string Phone { get; set; }
            public string Fax { get; set; }
        }

        public class Address
        {
            public string Line1 { get; set; }
            public string Line2 { get; set; }
            public string City { get; set; }
            public string Region { get; set; }
            public string PostalCode { get; set; }
            public string Country { get; set; }
        }

        public class Contact
        {
            public string Name { get; set; }
            public string Title { get; set; }
        }

        public class Employee
        {
            public string Id { get; set; }
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public string Title { get; set; }
            public Address Address { get; set; }
            public DateTime HiredAt { get; set; }
            public DateTime Birthday { get; set; }
            public string HomePhone { get; set; }
            public string Extension { get; set; }
            public string ReportsTo { get; set; }
            public List<string> Notes { get; set; }
            public List<string> Territories { get; set; }
        }

    }

    public class SampleTimeSeriesDefinitions
    {
        private interface Foo
        {
            #region incremental_declaration_increment-values-array-at-provided-timestamp
            // Increment a time series entry's array of values at the provided timestamp
            void Increment(DateTime timestamp, IEnumerable<double> values);
            #endregion

            #region incremental_declaration_increment-values-array-at-current-time
            // Increment a time series entry's array of values at the current time
            void Increment(IEnumerable<double> values);
            #endregion

            #region incremental_declaration_increment-value-at-provided-timestamp
            // Increment an entry value at the provided timestamp
            void Increment(DateTime timestamp, double value);
            #endregion

            #region incremental_declaration_increment-value-at-current-time
            // Increment an entry value at the current time
            void Increment(double value);
            #endregion

            #region incremental_declaration_session-get-time-series
            // Return time series values for the provided range
            TimeSeriesEntry[] Get(DateTime? from = null, DateTime? to = null, int start = 0, int pageSize = int.MaxValue);
            #endregion

            #region incremental_delete-values-range-or-all-values
            // Delete incremental time series values range from .. to,
            // or, if values are omitted, delete the whole series.
            void Delete(DateTime? from = null, DateTime? to = null);
            #endregion

            #region incremental_delete-value-at-timestamp
            // Delete the entry value at the specified time stamp
            void Delete(DateTime at);
            #endregion

            public class GetTimeSeriesOperation
            {
                #region incremental_declaration_GetTimeSeriesOperation
                public GetTimeSeriesOperation(
                    string docId, string timeseries, DateTime? @from = null,
                    DateTime? to = null, int start = 0, int pageSize = int.MaxValue, 
                    bool returnFullResults = false)
                #endregion
                { }
            }


        }
    }

}


