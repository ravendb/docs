<?php

namespace RavenDB\Samples\DocumentExtensions\TimeSeries;

use PHPUnit\Framework\TestCase;
use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Operations\TimeSeries\ConfigureTimeSeriesOperation;
use RavenDB\Documents\Operations\TimeSeries\RawTimeSeriesPolicy;
use RavenDB\Documents\Operations\TimeSeries\TimeSeriesCollectionConfiguration;
use RavenDB\Documents\Operations\TimeSeries\TimeSeriesConfiguration;
use RavenDB\Documents\Operations\TimeSeries\TimeSeriesPolicy;
use RavenDB\Documents\Operations\TimeSeries\TimeSeriesPolicyArray;
use RavenDB\Primitives\TimeValue;

class RollupAndRetention extends TestCase
{
    public function samples(): void
    {
        $store = new DocumentStore("http://localhost:8080", "Examples");
        $store->initialize();

        try {
            $session = $store->openSession();
            try {
                # region rollup_and_retention_0
                $oneWeek = TimeValue::ofDays(7);
                $fiveYears = TimeValue::ofYears(5);

                // Define a policy on the RAW time series data:
                // ============================================
                $rawPolicy = new RawTimeSeriesPolicy($fiveYears); // Retain entries for five years

                // Define a ROLLUP policy:
                // =======================
                $rollupPolicy = new TimeSeriesPolicy(
                        "By1WeekFor1Year", // Name of policy
                        $oneWeek, // Aggregation time, roll-up the data for each week
                        $fiveYears); // Retention time, keep data for five years

                // Define the time series configuration for collection "Companies" (use above policies):
                // =====================================================================================
                $companyConfig = new TimeSeriesCollectionConfiguration();
                $companyConfig->setPolicies(TimeSeriesPolicyArray::fromArray([ $rollupPolicy ]));
                $companyConfig->setRawPolicy($rawPolicy);

                $timeSeriesConfig = new TimeSeriesConfiguration();
                $timeSeriesConfig->setCollections([
                    "Companies" => $companyConfig
                ]);

                // Deploy the time series configuration to the server
                // by sending the 'ConfigureTimeSeriesOperation' operation:
                // ========================================================
                $store->maintenance()->send(new ConfigureTimeSeriesOperation($timeSeriesConfig));

                // NOTE:
                // The time series entries in the RavenDB sample data are dated up to the year 2020.
                // To ensure that you see the rollup time series created when running this example,
                // the retention time should be set to exceed that year.
                # endregion

                # region rollup_and_retention_1
                // Get all data from the RAW time series:
                // ======================================

                $rawData = $session
                    ->timeSeriesFor("companies/91-A", "StockPrices")
                    ->get();

                // Get all data from the ROLLUP time series:
                // =========================================

                // Either - pass the rollup name explicitly to 'TimeSeriesFor':
                $rollupData = $session
                    ->timeSeriesFor("companies/91-A", "StockPrices@By1WeekFor1Year")
                    ->get();

                // Or - get the rollup name by calling 'GetTimeSeriesName':
                $rollupData = $session
                    ->timeSeriesFor("companies/91-A", $rollupPolicy->GetTimeSeriesName("StockPrices"))
                    ->get();

                // The raw time series has 100 entries
                $this->assertCount(100, $rawData);
                $this->assertFalse($rawData[0]->isRollup());

                // The rollup time series has only 22 entries
                // as each entry aggregates 1 week's data from the raw time series
                $this->assertCount(22, $rollupData);
                $this->assertTrue($rollupData[0]->isRollup());
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }
}
