using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client.Documents;
using Raven.Client.Documents.Queries;
using Raven.Client.Documents.Queries.TimeSeries;

namespace Raven.Documentation.Samples.DocumentExtensions.TimeSeries
{
    class Statistics
    {
        private void Examples()
        {
            using (var store = new DocumentStore
            {
                Urls = new[] { "http://localhost:8080" },
                Database = "Examples"
            }.Initialize())
            {
                using (var session = store.OpenSession())
                {
                    #region RQL_percentile
                    var query = session.Advanced.RawQuery<TimeSeriesAggregationResult>(@"
                        from Employees as e
                        select timeseries(
                            from e.HeartRate 
                            select percentile(90)
                        )
                    ");
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region LINQ_percentile
                    var query = session.Query<Employee>()
                        .Select(p => RavenQuery.TimeSeries(p, "HeartRates")
                            .Select(x => new
                            {
                                P = x.Percentile(90)
                            }
                        )
                    );
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region RQL_slope
                    var query = session.Advanced.RawQuery<TimeSeriesAggregationResult>(@"
                        from Employees as e 
                        select timeseries(
                            from e.HeartRates
                            group by 1 hour
                            select slope()
                        )
                    ");
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region LINQ_slope
                    var query = session.Query<Employee>()
                        .Select(p => RavenQuery.TimeSeries(p, "HeartRates")
                            .GroupBy(g => g.Hours(1))
                            .Select(x => new
                            {
                                Slope = x.Slope()
                            }
                        )
                    );
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region RQL_stddev
                    //Example query with defined range
                    var date = DateTime.Today;

                    var query = session.Advanced.RawQuery<TimeSeriesAggregationResult>(@"
                        from Employees as e
                        select timeseries(
                            from e.HeartRates 
                            between $start and $end
                            select stddev()
                        )
                    ")
                    .AddParameter("start", date)
                    .AddParameter("end", date.AddDays(1));
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region LINQ_stddev
                    //Example query with defined range
                    var date = DateTime.Today;

                    var query = session.Query<Employee>()
                        .Select(p => RavenQuery.TimeSeries(p, "HeartRates", date, date.AddDays(1))
                            .Select(x => new
                            {
                                StdDev = x.StandardDeviation()
                            }
                        )
                    );
                    #endregion
                }
            }
        }
    }

    internal class Employee
    {
    }
}
