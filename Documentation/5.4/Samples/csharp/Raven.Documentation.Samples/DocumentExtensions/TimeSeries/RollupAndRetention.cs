using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.TimeSeries;
using Sparrow;
using Xunit;

namespace Raven.Documentation.Samples.DocumentExtensions.TimeSeries
{
    class rollups_and_retention
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
                var oneWeek = TimeValue.FromDays(7);
                var fiveYears = TimeValue.FromYears(5);
                
                // Define a policy on the RAW time series data:
                // ============================================
                var rawPolicy = new RawTimeSeriesPolicy(fiveYears); // Retain entries for five years

                // Define a ROLLUP policy: 
                // =======================
                var rollupPolicy = new TimeSeriesPolicy(
                        "By1WeekFor1Year", // Name of policy
                        oneWeek, // Aggregation time, roll-up the data for each week
                        fiveYears); // Retention time, keep data for five years
                
                // Define the time series configuration for collection "Companies" (use above policies):
                // =====================================================================================
                var timeSeriesConfig = new TimeSeriesConfiguration();
                timeSeriesConfig.Collections["Companies"] = new TimeSeriesCollectionConfiguration
                {
                    Policies = new List<TimeSeriesPolicy> { rollupPolicy },
                    RawPolicy = rawPolicy
                };
               
                // Deploy the time series configuration to the server
                // by sending the 'ConfigureTimeSeriesOperation' operation:
                // ========================================================
                store.Maintenance.Send(new ConfigureTimeSeriesOperation(timeSeriesConfig));
                
                // NOTE:
                // The time series entries in the RavenDB sample data are dated up to the year 2020.
                // To ensure that you see the rollup time series created when running this example,
                // the retention time should be set to exceed that year.
                #endregion

                #region rollup_and_retention_1
                // Get all data from the RAW time series:
                // ======================================
                
                var rawData = session
                    .TimeSeriesFor("companies/91-A", "StockPrices")
                    .Get(DateTime.MinValue, DateTime.MaxValue);

                // Get all data from the ROLLUP time series:
                // =========================================
                
                // Either - pass the rollup name explicitly to 'TimeSeriesFor':
                var rollupData = session
                    .TimeSeriesFor("companies/91-A", "StockPrices@By1WeekFor1Year")
                    .Get(DateTime.MinValue, DateTime.MaxValue);

                // Or - get the rollup name by calling 'GetTimeSeriesName':
                rollupData = session
                    .TimeSeriesFor("companies/91-A", rollupPolicy.GetTimeSeriesName("StockPrices"))
                    .Get(DateTime.MinValue, DateTime.MaxValue);
                
                // The raw time series has 100 entries
                Assert.Equal(rawData.Length, 100);
                Assert.Equal(rawData[0].IsRollup, false);
                
                // The rollup time series has only 22 entries
                // as each entry aggregates 1 week's data from the raw time series
                Assert.Equal(rollupData.Length, 22);
                Assert.Equal(rollupData[0].IsRollup, true);
                #endregion
            }
        }
    }
}
