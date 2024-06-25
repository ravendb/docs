using System;
using System.Linq;
using Raven.Client.Documents;
using System.Threading.Tasks;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Queries;
using Raven.Client.Documents.Queries.TimeSeries;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.DocumentExtensions.TimeSeries
{
    public class FilterTimeSeriesQuery
    {
        public async Task FilterTimeSeries()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region filter_entries_1
                    // For example, in the "HeartRates" time series,
                    // retrieve only entries where the value exceeds 75 BPM 

                    var baseTime = new DateTime(2020, 5, 17, 00, 00, 00);
                    var from = baseTime;
                    var to = baseTime.AddMinutes(10);

                    var query = session.Query<Employee>()
                        .Select(employee => RavenQuery
                            .TimeSeries(employee, "HeartRates", from, to)
                            // Call 'Where' to filter entries by the value
                            .Where(ts => ts.Value > 75)
                            .ToList());

                    var results = query.ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region filter_entries_2
                    // For example, in the "HeartRates" time series,
                    // retrieve only entries where the value exceeds 75 BPM

                    var baseTime = new DateTime(2020, 5, 17, 00, 00, 00);
                    var from = baseTime;
                    var to = baseTime.AddMinutes(10);

                    var query = session.Advanced.DocumentQuery<Employee>()
                        .SelectTimeSeries(builder => builder.From("HeartRates")
                            .Between(from, to)
                            // Call 'Where' to filter entries by the value
                            .Where(ts => ts.Value > 75)
                            .ToList());

                    var results = query.ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region filter_entries_3
                    // For example, in the "HeartRates" time series,
                    // retrieve only entries where the value exceeds 75 BPM

                    var baseTime = new DateTime(2020, 5, 17, 00, 00, 00);
                    var from = baseTime;
                    var to = baseTime.AddMinutes(10);

                    var query = session.Advanced.RawQuery<TimeSeriesRawResult>(@"
                        from Employees
                        select timeseries (
                            from HeartRates
                            between $from and $to
                            // Use the 'where Value' clause to filter by the value
                            where Value > 75
                        )")
                        .AddParameter("from", from)
                        .AddParameter("to", to);

                    var results = query.ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region filter_entries_4
                    // Retrieve only entries where the tag string content is "watches/fitbit"

                    var baseTime = new DateTime(2020, 5, 17, 00, 00, 00);
                    var from = baseTime;
                    var to = baseTime.AddMinutes(10);

                    var query = session.Query<Employee>()
                        .Select(employee => RavenQuery
                            .TimeSeries(employee, "HeartRates", from, to)
                            // Call 'Where' to filter entries by the tag
                            .Where(ts => ts.Tag == "watches/fitbit")
                            .ToList());

                    var results = query.ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region filter_entries_5
                    // Retrieve only entries where the tag string content is "watches/fitbit"

                    var baseTime = new DateTime(2020, 5, 17, 00, 00, 00);
                    var from = baseTime;
                    var to = baseTime.AddMinutes(10);

                    var query = session.Advanced.DocumentQuery<Employee>()
                        .SelectTimeSeries(builder => builder.From("HeartRates")
                            .Between(from, to)
                            // Call 'Where' to filter entries by the tag
                            .Where(ts => ts.Tag == "watches/fitbit")
                            .ToList());

                    var results = query.ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region filter_entries_6
                    // Retrieve only entries where the tag string content is "watches/fitbit"

                    var baseTime = new DateTime(2020, 5, 17, 00, 00, 00);
                    var from = baseTime;
                    var to = baseTime.AddMinutes(10);

                    var query = session.Advanced.RawQuery<TimeSeriesRawResult>(@"
                        from Employees
                        select timeseries (
                            from HeartRates
                            between $from and $to
                            // Use the 'where Tag' clause to filter entries by the tag string content
                            where Tag == 'watches/fitbit'
                        )")
                        .AddParameter("from", from)
                        .AddParameter("to", to);

                    var results = query.ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region filter_entries_7
                    // Retrieve only entries where the tag string content is one of several options

                    var baseTime = new DateTime(2020, 5, 17, 00, 00, 00);
                    var from = baseTime;
                    var to = baseTime.AddMinutes(10);

                    var optionalTags = new[] {"watches/apple", "watches/samsung", "watches/xiaomi"};

                    var query = session.Query<Employee>()
                        .Select(employee => RavenQuery
                            .TimeSeries(employee, "HeartRates", from, to)
                            // Call 'Where' to filter entries by the tag
                            .Where(ts =>
                                ts.Tag == "watches/apple" || ts.Tag == "watches/samsung" || ts.Tag == "watches/xiaomi")
                            .ToList());

                    var results = query.ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region filter_entries_8
                    // Retrieve only entries where the tag string content is one of several options

                    var baseTime = new DateTime(2020, 5, 17, 00, 00, 00);
                    var from = baseTime;
                    var to = baseTime.AddMinutes(10);

                    var optionalTags = new[] {"watches/apple", "watches/samsung", "watches/xiaomi"};

                    var query = session.Advanced.DocumentQuery<Employee>()
                        .SelectTimeSeries(builder => builder.From("HeartRates")
                            .Between(from, to)
                            // Call 'Where' to filter entries by the tag
                            .Where(ts =>
                                ts.Tag == "watches/apple" || ts.Tag == "watches/samsung" || ts.Tag == "watches/xiaomi")
                            .ToList());

                    var results = query.ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region filter_entries_9
                    // Retrieve only entries where the tag string content is one of several options

                    var baseTime = new DateTime(2020, 5, 17, 00, 00, 00);
                    var from = baseTime;
                    var to = baseTime.AddMinutes(10);

                    var optionalTags = new[] {"watches/apple", "watches/samsung", "watches/xiaomi"};

                    var query = session.Advanced.RawQuery<TimeSeriesRawResult>(@"
                        from Employees
                        select timeseries (
                            from HeartRates
                            between $from and $to
                            // Use the 'where Tag in' clause to filter by various tag options
                            where Tag in ($optionalTags)
                        )")
                        .AddParameter("from", from)
                        .AddParameter("to", to)
                        .AddParameter("optionalTags", optionalTags);

                    var results = query.ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region filter_entries_10
                    // Retrieve entries that reference a document that has "Sales Manager" in its 'Title' property

                    var query = session.Query<Company>()
                        // Query companies from USA
                        .Where(c => c.Address.Country == "USA")
                        .Select(company => RavenQuery
                            .TimeSeries(company, "StockPrices")
                            // Use 'LoadByTag' to load the employee document referenced in the tag
                            .LoadByTag<Employee>()
                            // Use 'Where' to filter the entries by the 'Title' property of the loaded document
                            .Where((ts, employeeDoc) => employeeDoc.Title == "Sales Manager")
                            .ToList());

                    var results = query.ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region filter_entries_11
                    // Retrieve entries that reference a document that has "Sales Manager" in its 'Title' property

                    var query = session.Advanced.DocumentQuery<Company>()
                        // Query companies from USA
                        .WhereEquals(company => company.Address.Country, "USA")
                        .SelectTimeSeries(builder => builder.From("StockPrices")
                            // Use 'LoadByTag' to load the employee document referenced in the tag
                            .LoadByTag<Employee>()
                            // Use 'Where' to filter the entries by the 'Title' property of the loaded document
                            .Where((ts, employeeDoc) => employeeDoc.Title == "Sales Manager")
                            .ToList());

                    var results = query.ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region filter_entries_12
                    // Retrieve entries that reference a document that has "Sales Manager" in its 'Title' property

                    var query = session.Advanced.RawQuery<Company>(@"
                        from Companies
                        where Address.Country == 'USA'
                        select timeseries (
                            from StockPrices
                            // Use 'load Tag' to load the employee document referenced in the tag
                            load Tag as employeeDoc
                            // Use 'where <property>' to filter entries by the properties of the loaded document 
                            where employeeDoc.Title == 'Sales Manager'
                        )"
                    );

                    var results = query.ToList();
                    #endregion
                }
            }
        }
    }
}
