<?php

namespace RavenDB\Samples\DocumentExtensions\TimeSeries;

use DateInterval;
use DateTime;
use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Queries\TimeSeries\TimeSeriesRawResult;
use RavenDB\Samples\Infrastructure\Orders\Company;
use RavenDB\Samples\Infrastructure\Orders\Employee;

class FilterTimeSeriesQuery
{
    public function samples(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region filter_entries_1
                // For example, in the "HeartRates" time series,
                // retrieve only entries where the value exceeds 75 BPM

                $tsQueryText =
                    "from HeartRates" .
                    "between \"2020-05-17T00:00:00.0000000\"" .
                    "and \"2020-05-17T00:10:00.0000000\"" .
                    // Use the "where Value" clause to filter entries by the value
                    "where Value > 75";

                $query = $session->advanced()->query(Employee::class)
                    ->selectTimeSeries(TimeSeriesRawResult::class, function($builder) use ($tsQueryText) {
                        return $builder->raw($tsQueryText);
                    });

                $results = $query->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region filter_entries_2
                // For example, in the "HeartRates" time series,
                // retrieve only entries where the value exceeds 75 BPM

                $tsQueryText =
                    "from HeartRates" .
                    "between \"2020-05-17T00:00:00.0000000\"" .
                    "and \"2020-05-17T00:10:00.0000000\"" .
                    // Use the "where Value" clause to filter entries by the value
                    "where Value > 75";

                $query = $session->advanced()->documentQuery(Employee::class)
                    ->selectTimeSeries(TimeSeriesRawResult::class, function($builder) use ($tsQueryText) {
                        return $builder->raw($tsQueryText);
                    });

                $results = $query->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region filter_entries_3
                // For example, in the "HeartRates" time series,
                // retrieve only entries where the value exceeds 75 BPM

                $baseTime = new DateTime("2020-05-17");
                $from = $baseTime;
                $to =  (clone $baseTime)->add(new DateInterval("PT10M"));

                $query = $session->advanced()->rawQuery(TimeSeriesRawResult::class, "
                    from Employees
                    select timeseries (
                        from HeartRates
                        between \$from and \$to
                        // Use the 'where Value' clause to filter by the value
                        where Value > 75
                    )")
                    ->addParameter("from", $from)
                    ->addParameter("to", $to);

                $results = $query->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region filter_entries_4
                // Retrieve only entries where the tag string content is "watches/fitbit"

                $tsQueryText =
                    "from HeartRates" .
                    "between \"2020-05-17T00:00:00.0000000\""  .
                    "and \"2020-05-17T00:10:00.0000000\""  .
                    // Use the "where Tag" clause to filter entries by the tag string content"  .
                    "where Tag == \"watches/fitbit\";";

                $query = $session->advanced()->query(Employee::class)
                    ->selectTimeSeries(TimeSeriesRawResult::class, function($builder) use ($tsQueryText) {
                        return $builder->raw($tsQueryText);
                    });

                $results = $query->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region filter_entries_5
                // Retrieve only entries where the tag string content is "watches/fitbit"

                $tsQueryText =
                    "from HeartRates" .
                    "between \"2020-05-17T00:00:00.0000000\""  .
                    "and \"2020-05-17T00:10:00.0000000\""  .
                    // Use the "where Tag" clause to filter entries by the tag string content"  .
                    "where Tag == \"watches/fitbit\";";

                $query = $session->advanced()->documentQuery(Employee::class)
                    ->selectTimeSeries(TimeSeriesRawResult::class, function($builder) use ($tsQueryText) {
                        return $builder->raw($tsQueryText);
                    });

                $results = $query->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region filter_entries_6
                // Retrieve only entries where the tag string content is "watches/fitbit"

                $baseTime = new DateTime("2020-05-17");
                $from = $baseTime;
                $to =  (clone $baseTime)->add(new DateInterval("PT10M"));

                $query = $session->advanced()->rawQuery(TimeSeriesRawResult::class, "
                    from Employees
                    select timeseries (
                        from HeartRates
                        between \$from and \$to
                        // Use the 'where Tag' clause to filter entries by the tag string content
                        where Tag == 'watches/fitbit'
                    )")
                    ->addParameter("from", $from)
                    ->addParameter("to", $to);

                $results = $query->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region filter_entries_7
                // Retrieve only entries where the tag string content is one of several options

                $tsQueryText =
                    "from HeartRates" .
                    "between \"2020-05-17T00:00:00.0000000\"" .
                    "and \"2020-05-17T00:10:00.0000000\"" .
                    // Use the "where Tag in" clause to filter by various tag options
                    "where Tag in (\"watches/apple\", \"watches/samsung\", \"watches/xiaomi\")";

                $query = $session->advanced()->query(Employee::class)
                    ->selectTimeSeries(TimeSeriesRawResult::class, function($builder) use ($tsQueryText) {
                        return $builder->raw($tsQueryText);
                    });

                $results = $query->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region filter_entries_8
                // Retrieve only entries where the tag string content is one of several options

                $tsQueryText =
                    "from HeartRates" .
                    "between \"2020-05-17T00:00:00.0000000\"" .
                    "and \"2020-05-17T00:10:00.0000000\"" .
                    // Use the "where Tag in" clause to filter by various tag options
                    "where Tag in (\"watches/apple\", \"watches/samsung\", \"watches/xiaomi\")";

                $query = $session->advanced()->documentQuery(Employee::class)
                    ->selectTimeSeries(TimeSeriesRawResult::class, function($builder) use ($tsQueryText) {
                        return $builder->raw($tsQueryText);
                    });

                $results = $query->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region filter_entries_9
                // Retrieve only entries where the tag string content is one of several options

                $baseTime = new DateTime("2020-05-17");
                $from = $baseTime;
                $to =  (clone $baseTime)->add(new DateInterval("PT10M"));

                $optionalTags = [ "watches/apple", "watches/samsung", "watches/xiaomi" ];

                $query = $session->advanced()->rawQuery(TimeSeriesRawResult::class, "
                    from Employees
                    select timeseries (
                        from HeartRates
                        between \$from and \$to
                        // Use the 'where Tag in' clause to filter by various tag options
                        where Tag in (\$optionalTags)
                    )")
                    ->addParameter("from", $from)
                    ->addParameter("to", $to)
                    ->addParameter("optionalTags", $optionalTags);

                $results = $query->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region filter_entries_10
                // Retrieve entries that reference a document that has "Sales Manager" in its 'Title' property

                $tsQueryText =
                    "from StockPrices" .
                    // Use 'load Tag' to load the employee document referenced in the tag
                    "load Tag as employeeDoc" .
                    // Use 'where <property>' to filter entries by the properties of the loaded document
                    "where employeeDoc.Title == \"Sales Manager\"";

                $query = $session->advanced()->query(Company::class)
                    // Query companies from USA
                    ->whereEquals("Address.Country", "USA")
                    ->selectTimeSeries(TimeSeriesRawResult::class, function ($builder) use ($tsQueryText) {
                        return $builder->raw($tsQueryText);
                    });

                $results = $query->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region filter_entries_11
                // Retrieve entries that reference a document that has "Sales Manager" in its 'Title' property

                $tsQueryText =
                    "from StockPrices" .
                    // Use 'load Tag' to load the employee document referenced in the tag
                    "load Tag as employeeDoc" .
                    // Use 'where <property>' to filter entries by the properties of the loaded document
                    "where employeeDoc.Title == \"Sales Manager\"";

                $query = $session->advanced()->documentQuery(Company::class)
                    // Query companies from USA
                    ->whereEquals("Address.Country", "USA")
                    ->selectTimeSeries(TimeSeriesRawResult::class, function ($builder) use ($tsQueryText) {
                        return $builder->raw($tsQueryText);
                    });

                $results = $query->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region filter_entries_12
                // Retrieve entries that reference a document that has "Sales Manager" in its 'Title' property

                $query = $session->advanced()->rawQuery(Company::class, "
                    from Companies
                    where Address.Country == 'USA'
                    select timeseries (
                        from StockPrices
                        // Use 'load Tag' to load the employee document referenced in the tag
                        load Tag as employeeDoc
                        // Use 'where \<property\>' to filter entries by the properties of the loaded document
                        where employeeDoc.Title == 'Sales Manager'
                    )"
                );

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
