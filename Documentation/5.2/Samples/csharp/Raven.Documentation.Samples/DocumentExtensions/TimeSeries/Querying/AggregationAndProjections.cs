using System;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Queries;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Queries.TimeSeries;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.DocumentExtensions.TimeSeries.Querying
{
    class AggregationAndProjections
    {
        public void Examples()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    var input = "month";

                    #region GroupBy_Switch
                    var groupingAction = input switch //input is a string that represents some client input
                    {
                        "year" => (Action<ITimePeriodBuilder>)(builder => builder.Years(1)),
                        "month" => (Action<ITimePeriodBuilder>)(builder => builder.Months(1)),
                        "day" => (Action<ITimePeriodBuilder>)(builder => builder.Days(1))
                    };

                    var stocks = session.Query<Company>()
                        .Select(c => RavenQuery.TimeSeries(c, "StockPrices")
                            .GroupBy(groupingAction)
                            .Select(g => new
                            {
                                Min = g.Min(),
                                Max = g.Max()
                            })
                            .ToList());
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    var input = "month";
                    /*
                    #region GroupBy_Function
                    var stocks = session.Query<Company>()
                        .Select(c => RavenQuery.TimeSeries(c, "StockPrices")
                            .GroupBy(g => GroupingFunction(g, input)) // input is a string that represents some client input
                            .Select(g => new
                            {
                                Min = g.Min(),
                                Max = g.Max()
                            })
                            .ToList());

                    private static ITimeSeriesAggregationOperations GroupingFunction(ITimePeriodBuilder builder, string input)
                    {
                        if (input == "year")
                        {
                            return builder.Years(1);
                        }
                        else if (input == "month")
                        {
                            return builder.Months(1);
                        }
                        else
                        {
                            return builder.Days(1);
                        }
                    }
                    #endregion
                    */
                }
            }
        }
    }

    public interface Foo
    {
        #region GroupBy
        ITimeSeriesAggregationQueryable GroupBy(string s);

        ITimeSeriesAggregationQueryable GroupBy(Action<ITimePeriodBuilder> timePeriod);
        #endregion
    }



    /*
    #region Builder
    public interface ITimePeriodBuilder
    {
        ITimeSeriesAggregationOperations Milliseconds(int duration);
        ITimeSeriesAggregationOperations Seconds(int duration);
        ITimeSeriesAggregationOperations Minutes(int duration);
        ITimeSeriesAggregationOperations Hours(int duration);
        ITimeSeriesAggregationOperations Days(int duration);
        ITimeSeriesAggregationOperations Months(int duration);
        ITimeSeriesAggregationOperations Quarters(int duration);
        ITimeSeriesAggregationOperations Years(int duration);
    }
    #endregion
    */
}
