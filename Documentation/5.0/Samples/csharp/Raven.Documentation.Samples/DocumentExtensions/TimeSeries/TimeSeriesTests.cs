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
                    .Get(DateTime.MinValue, DateTime.MaxValue)
                    .Single();
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

            #region timeseries_region_Remove-TimeSeries-Range
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
}
