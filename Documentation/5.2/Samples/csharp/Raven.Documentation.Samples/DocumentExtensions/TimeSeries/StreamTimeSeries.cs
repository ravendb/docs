using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Commands;
using Raven.Client.Documents.Queries.TimeSeries;
using Raven.Client.Documents.Session;
using Raven.Client.Documents.Session.TimeSeries;
// using Raven.Documentation.Samples.DocumentExtensions.TimeSeries.Indexing;

namespace Raven.Documentation.Samples.DocumentExtensions.TimeSeries
{
    class StreamTimeSeries
    {
        private interface IFoo<T>
        {
            #region stream_methods
            IEnumerator<TimeSeriesStreamResult<T>> Stream(
                IQueryable<T> query);

            IEnumerator<TimeSeriesStreamResult<T>> Stream(
                IQueryable<T> query,
                out StreamQueryStatistics streamQueryStats);

            IEnumerator<TimeSeriesStreamResult<T>> Stream(
                IDocumentQuery<T> query);

            IEnumerator<TimeSeriesStreamResult<T>> Stream(
                IRawDocumentQuery<T> query);

            IEnumerator<TimeSeriesStreamResult<T>> Stream(
                IRawDocumentQuery<T> query,
                out StreamQueryStatistics streamQueryStats);

            IEnumerator<TimeSeriesStreamResult<T>> Stream(
                IDocumentQuery<T> query,
                out StreamQueryStatistics streamQueryStats);
            #endregion
        }

        public async Task Examples()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region direct
                    var timeseries = session.TimeSeriesFor<HeartRate>("user/1-A");
                    var results = new List<TimeSeriesEntry>();

                    using (var TSstream = timeseries.Stream())
                    {
                        while (TSstream.MoveNext())
                        {
                            results.Add(TSstream.Current);
                        }
                    }
                    #endregion
                }

                using (var session = store.OpenAsyncSession())
                {
                    #region direct_async
                    var timeseries = session.TimeSeriesFor<HeartRate>("user/1-A");
                    var results = new List<TimeSeriesEntry>();
                    var TSstream = await timeseries.StreamAsync();

                    await using (TSstream)
                    {
                        while (await TSstream.MoveNextAsync())
                        {
                            results.Add(TSstream.Current);
                        }
                    }
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region query
                    var query = session.Advanced.RawQuery<TimeSeriesRawResult>(@"
                        from Users
                        select timeseries (
                            from HeartRate
                        )");

                    var results = new List<TimeSeriesEntry>();

                    using (var docStream = session.Advanced.Stream(query))
                    {
                        while (docStream.MoveNext())
                        {
                            var document = docStream.Current.Result;
                            var timeseries = document.Stream;
                            while (timeseries.MoveNext())
                            {
                                results.Add(timeseries.Current);
                            }
                        }
                    }
                    #endregion
                }

                using (var session = store.OpenAsyncSession())
                {
                    #region query_async
                    var query = session.Advanced.AsyncRawQuery<TimeSeriesRawResult>(@"
                        from Users
                        select timeseries (
                            from HeartRate
                        )");

                    var results = new List<TimeSeriesEntry>();

                    await using (var docStream = await session.Advanced.StreamAsync(query))
                    {
                        while (await docStream.MoveNextAsync())
                        {
                            var document = docStream.Current.Result;
                            var timeseries = document.StreamAsync;
                            while (await timeseries.MoveNextAsync())
                            {
                                results.Add(timeseries.Current);
                            }
                        }
                    }
                    #endregion
                }
            }
        }
    }

    class HeartRate
    {
    }
}
