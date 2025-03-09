<?php

namespace RavenDB\Samples\DocumentExtensions\TimeSeries\Querying;

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Queries\TimeSeries\TimeSeriesAggregationResult;
use RavenDB\Documents\Queries\TimeSeries\TimeSeriesRawResult;
use RavenDB\Samples\Infrastructure\Orders\Company;
use RavenDB\Samples\Infrastructure\Orders\Employee;

class AggregatingValues
{
    public function samples(): void
    {
        $store = new DocumentStore();
        try {
            // Aggregate entries with single value
            $session = $store->openSession();
            try {
                # region aggregation_1
                // Define the time series query part (expressed in RQL):
                $tsQueryText = "
                    from HeartRates
                    // Use 'group by' to group the time series entries by the specified time frame
                    group by \"1 hour\"
                    // Use 'select' to choose aggregation functions that will be evaluated
                    // Project the lowest and highest value of each group
                    select min(), max()";

                $query = $session->query(Employee::class)
                        ->selectTimeSeries(TimeSeriesRawResult::class, function($b) use ($tsQueryText) { return $b->raw($tsQueryText);});

                // Execute the query
                /** @var array<TimeSeriesAggregationResult> $results */
                $results = $query->toList();
                # endregion
            } finally {
                $session->close();
            }

            // Aggregate entries with multiple values
            $session = $store->openSession();
            try {
                # region aggregation_2
                $tsQueryText = "
                    from StockPrices
                    // Query stock price behavior when trade volume is high
                    where Values[4] > 500000 
                    // Group entries into consecutive 7-day groups
                    group by \"7 day\"
                    // Project the lowest and highest value of each group         
                    select max(), min()";

                $query = $session->query(Company::class)
                    ->whereEquals("Address.Country", "USA")
                    ->selectTimeSeries(TimeSeriesRawResult::class, function($b) use ($tsQueryText) { return $b->raw($tsQueryText);});

                // Execute the query
                /** @var array<TimeSeriesAggregationResult> $results */
                $results = $query->toList();
                # endregion
            } finally {
                $session->close();
            }

            // Aggregate entries without grouping by time frame
            $session = $store->openSession();
            try {
                # region aggregation_3
                $tsQueryText = "
                    from StockPrices
                    where Values[4] > 500_000
                    select max(), min()";

                $query = $session->query(Company::class)
                    ->whereEquals("Address.Country", "USA")
                    ->selectTimeSeries(TimeSeriesRawResult::class, function($b) use ($tsQueryText) { return $b->raw($tsQueryText);});

                // Execute the query
                /** @var array<TimeSeriesAggregationResult> $results */
                $results = $query->toList();
                # endregion
            } finally {
                $session->close();
            }

            // Aggregate entries filtered by referenced document
            $session = $store->openSession();
            try {
                # region aggregation_4
                $tsQueryText = "
                    from StockPrices
                    // Load the referenced document into variable 'employee'
                    load Tag as employee
                    // Filter entries by the 'Title' field of the employee document
                    where employee.Title == \"Sales Representative\"
                    group by \"1 month\"
                    select min(), max()";

                $query = $session->query(Company::class)
                    ->selectTimeSeries(TimeSeriesRawResult::class, function($b) use ($tsQueryText) { return $b->raw($tsQueryText);});

                // Execute the query
                /** @var array<TimeSeriesAggregationResult> $results */
                $results = $query->toList();
                # endregion
            } finally {
                $session->close();
            }

            // Secondary aggregation by tag
            $session = $store->openSession();
            try {
                # region aggregation_5
                $tsQueryText = "
                    from StockPrices
                    // Use the 'tag' keyword to perform a secondary grouping by the entries' tags
                    // Group by months and by tag
                    group by \"6 months\", tag
                    // Project the highest and lowest values of each group  
                    select max(), min()";

                $query = $session->query(Company::class)
                    ->selectTimeSeries(TimeSeriesRawResult::class, function($b) use ($tsQueryText) { return $b->raw($tsQueryText);});

                // Execute the query
                /** @var array<TimeSeriesAggregationResult> $results */
                $results = $query->toList();
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }
}
