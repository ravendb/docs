using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client.Documents;
using Raven.Client.Documents.Queries;
using Raven.Client.Documents.Queries.TimeSeries;

namespace Raven.Documentation.Samples.DocumentExtensions.TimeSeries.Querying
{
    class GapFilling
    {
        public void Examples()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region RQL_Query
                    var query = session.Advanced.RawQuery<TimeSeriesAggregationResult>(@"
                        from People
                        select timeseries(
                            from HeartRate
                            group by 1 second
                            with interpolation(linear)
                        ");
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region LINQ_Query
                    var query = session.Query<People>()
                        .Select(p => RavenQuery.TimeSeries(p, "HeartRate")
                            .GroupBy(g => g
                                .Hours(1)
                                .WithOptions(new TimeSeriesAggregationOptions
                                {
                                    Interpolation = InterpolationType.Linear
                                }))
                            .ToList());
                    #endregion
                }
            }
        }
    }

    internal class People
    {
    }
}
