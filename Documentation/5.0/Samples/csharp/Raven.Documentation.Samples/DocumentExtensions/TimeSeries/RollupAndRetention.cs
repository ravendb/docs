using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.TimeSeries;
using Sparrow;
using Xunit;

namespace Raven.Documentation.Samples.DocumentExtensions.TimeSeries
{
    class rollup_and_retention
    {
        public void Examples()
        {
            var store = new DocumentStore
            {
                Urls = new[] { "http://localhost:8080" },
                Database = "Examples"
            };
            store.Initialize();

            using (var session = store.OpenSession())
            {
                #region rollup_and_retention_0
                //Policy for the original ("raw") time-series,
                //to keep the data for one week
                var rawRetention = new RawTimeSeriesPolicy(TimeValue.FromDays(7));

                //Roll-up the data for each day,
                //and keep the results for one year
                var dailyRollup = new TimeSeriesPolicy("DailyRollupForOneYear",
                                                       TimeValue.FromDays(1),
                                                       TimeValue.FromYears(1));

                //Enter the above policies into a 
                //time-series collection configuration
                //for the collection 'Sales'
                var salesTSConfig = new TimeSeriesCollectionConfiguration
                {
                    Policies = new List<TimeSeriesPolicy>
                    {
                        dailyRollup
                    },
                    RawPolicy = rawRetention
                };

                //Enter the configuration for the Sales collection
                //into a time-series configuration for the whole database
                var DatabaseTSConfig = new TimeSeriesConfiguration();
                DatabaseTSConfig.Collections["Sales"] = salesTSConfig;

                //Send the time-series configuration to the server
                store.Maintenance.Send(new ConfigureTimeSeriesOperation(DatabaseTSConfig));
                #endregion

                #region rollup_and_retention_1
                //Create local instance of the time-series "rawSales"
                //in the document "sales/1"
                var rawTS = session.TimeSeriesFor("sales/1", "rawSales");

                //Create local instance of the rollup time-series - first method:
                var dailyRollupTS = session.TimeSeriesFor("sales/1",
                                                              "rawSales@DailyRollupForOneYear");

                //Create local instance of the rollup time-series - second method:
                //using the rollup policy itself and the raw time-series' name
                var rollupTimeSeries2 = session.TimeSeriesFor("sales/1",
                                                      dailyRollup.GetTimeSeriesName("rawSales"));

                //Retrieve all the data from both time-series
                var rawData = rawTS.Get(DateTime.MinValue, DateTime.MaxValue).ToList();
                var rollupData = dailyRollupTS.Get(DateTime.MinValue, DateTime.MaxValue).ToList();
                #endregion
            }
        }
    }
}
