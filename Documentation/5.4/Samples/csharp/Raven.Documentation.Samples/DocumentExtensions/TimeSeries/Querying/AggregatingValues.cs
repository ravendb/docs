using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Queries;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Queries.TimeSeries;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.DocumentExtensions.TimeSeries.Querying
{
    class AggregatingValues
    {
        #region grouping_enum
        public enum GroupingInterval
        {
            Hour,
            Day,
            Month,
            Year
        }
        #endregion
        
        #region grouping_function
        public static ITimeSeriesAggregationOperations GroupingFunction(ITimePeriodBuilder builder,
            string input)
        {
            if (input == "year")
            {
                return builder.Years(1);
            }
            
            if (input == "month")
            {
                return builder.Months(1);
            }
            
            return builder.Days(1);
        }
        #endregion
        
        public void Examples()
        {
            using (var store = new DocumentStore())
            {
                // Aggregate entries with single value
                using (var session = store.OpenSession())
                {
                    #region aggregation_1
                    var query = session.Query<Employee>()
                        .Select(u => RavenQuery
                            .TimeSeries(u, "HeartRates")
                             // Call 'GroupBy' to group the time series entries by a time frame
                            .GroupBy(g => g.Hours(1))
                             // Call 'Select' to choose aggregation functions that will be evaluated for each group
                            .Select(g => new
                            {
                                // Project the lowest and highest value of each group
                                Min = g.Min(),
                                Max = g.Max()
                            })
                            .ToList());

                    // Execute the query
                    List<TimeSeriesAggregationResult> results = query.ToList();
                    #endregion
                }
                
                // Aggregate entries with multiple values  
                using (var session = store.OpenSession())
                {
                    #region aggregation_2
                    var query = session.Query<Company>()
                         // Query only USA companies:
                        .Where(c => c.Address.Country == "USA")
                        .Select(u => RavenQuery
                             .TimeSeries(u, "StockPrices")
                             // Query stock price behavior when trade volume is high
                            .Where(ts => ts.Values[4] > 500000)
                             // Group entries into consecutive 7-day groups
                            .GroupBy(g => g.Days(7))
                             // Project the lowest and highest value of each group
                            .Select(g => new
                            {
                                Min = g.Min(),
                                Max = g.Max()
                            })
                            .ToList());

                    // Execute the query
                    List<TimeSeriesAggregationResult> results = query.ToList();
                    #endregion
                }
                
                // Aggregate entries without grouping by time frame  
                using (var session = store.OpenSession())
                {
                    #region aggregation_3
                    var query = session.Query<Company>()
                        .Where(c => c.Address.Country == "USA")
                        .Select(u => RavenQuery
                            .TimeSeries(u, "StockPrices")
                            .Where(ts => ts.Values[4] > 500000)
                            // Project the lowest and highest value of ALL entries that match the query criteria
                            .Select(g => new
                            {
                                Min = g.Min(),
                                Max = g.Max()
                            })
                            .ToList());

                    // Execute the query
                    List<TimeSeriesAggregationResult> results = query.ToList();
                    #endregion
                }
                
                // Aggregate entries filtered by referenced document
                using (var session = store.OpenSession())
                {
                    #region aggregation_4
                    var query = session.Query<Company>()
                        .Select(u => RavenQuery
                            .TimeSeries(u, "StockPrices")
                             // Call 'LoadByTag' to load the document referenced by the tag
                            .LoadByTag<Employee>()
                             // Filter entries: 
                             // Process only entries that reference an employee with the Sales title
                            .Where((entry, employee) => employee.Title == "Sales Representative")
                            .GroupBy(g =>g.Months(1))
                            .Select(g => new
                            {
                                Min = g.Min(),
                                Max = g.Max()
                            })
                            .ToList());

                    // Execute the query
                    List<TimeSeriesAggregationResult> results = query.ToList();
                    #endregion
                }
                
                // Secondary aggregation by tag
                using (var session = store.OpenSession())
                {
                    #region aggregation_5
                    var query = session.Query<Company>()
                        .Select(u => RavenQuery
                            .TimeSeries(u, "StockPrices")
                            .GroupBy(g => g
                                 // First group by 6 months   
                                .Months(6)
                                 // Then group by tag
                                .ByTag())
                            .Select(g => new
                            {  
                                // Project the highest and lowest values of each group
                                Min = g.Min(),
                                Max = g.Max()
                            })
                            .ToList());

                    // Execute the query
                    List<TimeSeriesAggregationResult> results = query.ToList();
                    #endregion
                }
                
                // Group by dynamic criteria 1
                using (var session = store.OpenSession())
                {
                    #region aggregation_6
                    var grouping = GroupingInterval.Year; // Dynamic input from the client
                    
                    var groupingAction = grouping switch 
                    {
                        GroupingInterval.Year => (Action<ITimePeriodBuilder>)(builder => builder.Years(1)),
                        GroupingInterval.Month=> (Action<ITimePeriodBuilder>)(builder => builder.Months(1)),
                        GroupingInterval.Day => (Action<ITimePeriodBuilder>)(builder => builder.Days(1))
                    };

                    var query = session.Query<Company>()
                        .Select(c => RavenQuery
                            .TimeSeries(c, "StockPrices")
                            .GroupBy(groupingAction)
                            .Select(g => new
                            {
                                Ave = g.Average()
                            })
                            .ToList());
                    
                    // Execute the query
                    List<TimeSeriesAggregationResult> results = query.ToList();
                    #endregion
                }
                
                // Group by dynamic criteria 2
                using (var session = store.OpenSession())
                {
                    #region aggregation_7
                    var query = session.Query<Company>()
                        .Select(c => RavenQuery
                            .TimeSeries(c, "StockPrices")
                            .GroupBy(builder => GroupingFunction(builder, "year"))
                            .Select(g => new
                            {
                                Ave = g.Average()
                            })
                            .ToList());
                    
                    // Execute the query
                    List<TimeSeriesAggregationResult> results = query.ToList();
                    #endregion
                }
                
                // Project document data as well
                using (var session = store.OpenSession())
                {
                    #region aggregation_8
                    var query = session.Query<Company>()
                        .Select(company => new
                        {
                            // Projecting time series data:
                            MinMaxValues = RavenQuery.TimeSeries(company, "StockPrices")
                                .Where(e => e.Values[4] > 500_000)
                                .GroupBy(g => g.Days(7))
                                .Select(x => new
                                {
                                    Min = x.Min(),
                                    Max = x.Max(),
                                })
                                .ToList(),
                            
                            // Projecting the company name:
                            CompanyName = company.Name 
                        });
                    
                    // Execute the query
                    var results = query.ToList();
                    #endregion
                }
            }
        }
    }
    /*
    #region syntax_1
    public interface ITimeSeriesQueryable
    {
       
        ITimeSeriesQueryable Where(Expression<Func<TimeSeriesEntry, bool>> predicate);

        ITimeSeriesQueryable Offset(TimeSpan offset);

        ITimeSeriesQueryable Scale(double value);

        ITimeSeriesQueryable FromLast(Action<ITimePeriodBuilder> timePeriod);

        ITimeSeriesQueryable FromFirst(Action<ITimePeriodBuilder> timePeriod);

        ITimeSeriesLoadQueryable<TEntity> LoadByTag<TEntity>();

        // GroupBy overloads:
        ITimeSeriesAggregationQueryable GroupBy(string s);
        ITimeSeriesAggregationQueryable GroupBy(Action<ITimePeriodBuilder> timePeriod);

        // Select method:
        ITimeSeriesAggregationQueryable Select(
            Expression<Func<ITimeSeriesGrouping, object>> selector);

        TimeSeriesRawResult ToList();
    }
    #endregion
    */
    
    /*
    #region syntax_2
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
    
    /*
    #region syntax_3
    public interface ITimeSeriesGrouping
    {
        double[] Max();
        double[] Min();
        double[] Sum();
        double[] Average();
        double[] First();
        double[] Last();
        long[] Count();
        double[] Percentile(double number);
        double[] Slope();
        double[] StandardDeviation();
    } 
    #endregion
    */
}
