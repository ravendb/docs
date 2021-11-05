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
                var entries = store.Operations
                                  .Send(new GetTimeSeriesOperation("users/ayende",
                                   "INC:Downloads", start: 0, pageSize: pageSize / 2, 
                                   returnFullResults: true));

                // we get 50 unique entries but we read 100 entries from the segment
                // so next call we should start from position 101: numberOfUniqueEntries + skippedResults 
                var nextStart = entries.Entries.Length + entries.SkippedResults;

                entries = store.Operations
                                  .Send(new GetTimeSeriesOperation("users/ayende",
                                   "INC:Downloads", start: (int)nextStart,
                                   pageSize: pageSize / 2, returnFullResults: true));
                #endregion

            }
        }
        
   


        private struct Downloads
        {
            [TimeSeriesValue(0)] public double DownloadsCount;
        }

        public class User
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string LastName { get; set; }
            public string AddressId { get; set; }
            public int Count { get; set; }
            public int Age { get; set; }
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


