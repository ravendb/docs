<?php

namespace RavenDB\Samples\DocumentExtensions\TimeSeries;

use Cassandra\Date;
use DateInterval;
use DateTime;
use DateTimeInterface;
use RavenDB\Constants\PhpClient;
use RavenDB\Documents\Commands\Batches\PatchCommandData;
use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Indexes\TimeSeries\AbstractTimeSeriesIndexCreationTask;
use RavenDB\Documents\Operations\Indexes\StartIndexingOperation;
use RavenDB\Documents\Operations\Indexes\StopIndexingOperation;
use RavenDB\Documents\Operations\PatchByQueryOperation;
use RavenDB\Documents\Operations\PatchOperation;
use RavenDB\Documents\Operations\PatchRequest;
use RavenDB\Documents\Operations\TimeSeries\AbstractTimeSeriesRange;
use RavenDB\Documents\Operations\TimeSeries\AppendOperation;
use RavenDB\Documents\Operations\TimeSeries\DeleteOperation;
use RavenDB\Documents\Operations\TimeSeries\GetMultipleTimeSeriesOperation;
use RavenDB\Documents\Operations\TimeSeries\GetTimeSeriesOperation;
use RavenDB\Documents\Operations\TimeSeries\TimeSeriesBatchOperation;
use RavenDB\Documents\Operations\TimeSeries\TimeSeriesDetails;
use RavenDB\Documents\Operations\TimeSeries\TimeSeriesOperation;
use RavenDB\Documents\Operations\TimeSeries\TimeSeriesRange;
use RavenDB\Documents\Operations\TimeSeries\TimeSeriesRangeList;
use RavenDB\Documents\Operations\TimeSeries\TimeSeriesRangeResult;
use RavenDB\Documents\Operations\TimeSeries\TimeSeriesRangeResultListArray;
use RavenDB\Documents\Queries\IndexQuery;
use RavenDB\Documents\Queries\Query;
use RavenDB\Documents\Queries\QueryOperationOptions;
use RavenDB\Documents\Queries\TimeSeries\TimeSeriesAggregationResult;
use RavenDB\Documents\Queries\TimeSeries\TimeSeriesRawResult;
use RavenDB\Documents\Session\DocumentQueryInterface;
use RavenDB\Documents\Session\RawDocumentQueryInterface;
use RavenDB\Documents\Session\SessionDocumentTypedTimeSeriesInterface;
use RavenDB\Documents\Session\TimeSeries\TimeSeriesEntryArray;
use RavenDB\Documents\Session\TimeSeries\TimeSeriesValue;
use RavenDB\Documents\Session\TimeSeries\TypedTimeSeriesEntryArray;
use RavenDB\Parameters;
use RavenDB\Primitives\NetISO8601Utils;
use RavenDB\ServerWide\DatabaseRecord;
use RavenDB\ServerWide\Operations\CreateDatabaseOperation;
use RavenDB\ServerWide\Operations\DeleteDatabaseCommandParameters;
use RavenDB\ServerWide\Operations\DeleteDatabasesOperation;
use RavenDB\Type\Duration;
use RavenDB\Type\ObjectMap;
use RavenDB\Type\StringArray;
use RavenDB\Utils\DateUtils;

class TimeSeriesTest
{
    public function getDocumentStore(): DocumentStore
    {
        $store = new DocumentStore("http://localhost:8080", "TestDatabase");
        $store->initialize();

        $parameters = new DeleteDatabaseCommandParameters();
        $parameters->setDatabaseNames(["TestDatabase"]);
        $parameters->setHardDelete(true);

        $store->maintenance()->server()->send(new DeleteDatabasesOperation($parameters));
        $store->maintenance()->server()->send(new CreateDatabaseOperation(new DatabaseRecord("TestDatabase")));

        return $store;
    }

    public function canCreateSimpleTimeSeries(): void
    {
        $store = $this->getDocumentStore();
        try {
             $baseTime =  DateUtils::today();

            // Open a session
            $session = $store->openSession();
            try {
                // Use the session to create a document
                $user = new User();
                $user->setName("John");
                $session->store($user, "users/john");

                // Create an instance of TimeSeriesFor
                // Pass an explicit document ID to the TimeSeriesFor constructor
                // Append a HeartRate of 70 at the first-minute timestamp
                $session
                    ->timeSeriesFor("users/john", "HeartRates")
                    ->append((clone $baseTime)->add(new DateInterval("PT1M")), 70, "watches/fitbit");

                $session->saveChanges();
            } finally {
                $session->close();
            }

            // Get time series names
            # region timeseries_region_Retrieve-TimeSeries-Names
            // Open a session
            $session = $store->openSession();
            try {
                // Load a document entity to the session
                $user = $session->load(User::class, "users/john");

                // Call GetTimeSeriesFor, pass the entity
                $tsNames = $session->advanced()->getTimeSeriesFor($user);

                // Results will include the names of all time series associated with document 'users/john'
            } finally {
                $session->close();
            }
            # endregion

            $session = $store->openSession();
            try {
                // Use the session to load a document
                $user = $session->load(User::class, "users/john");

                // Pass the document object returned from session.Load as a param
                // Retrieve a single value from the "HeartRates" time series
                /** @var array<TimeSeriesEntry> $val */
                $val = $session->timeSeriesFor($user, "HeartRates")
                    ->get();
            } finally {
                $session->close();
            }

            # region timeseries_region_Delete-TimeSeriesFor-Single-Time-Point
            // Delete a single entry
            $session = $store->openSession();
            try {
                $session->timeSeriesFor("users/john", "HeartRates")
                ->delete((clone $baseTime)->add(new DateInterval("PT1M")));

                $session->saveChanges();
            } finally {
                $session->close();
            }
            # endregion
        } finally {
            $store->close();
        }
    }

    public function stronglyTypes(): void
    {
        $store = $this->getDocumentStore();
        try {
            $store->timeSeries()->register(User::class, HeartRate::class);
            # region timeseries_region_Named-Values-Register
            // Register the StockPrice class type on the server
            $store->timeSeries()->register(Company::class, StockPrice::class);
            # endregion
            $store->timeSeries()->register(User::class, RoutePoint::class);

            $baseTime = DateUtils::today();

            // Append entries
            $session = $store->openSession();
            try {
                $user = new User();
                $user->setName("John");
                $session->store($user, "users/john");

                # region timeseries_region_Append-Named-Values-1
                // Append coordinates
                $session->typedTimeSeriesFor(RoutePoint::class, "users/john")
                    ->append(
                         (clone $baseTime)->add(new DateInterval("PT1H")),
                        new RoutePoint(latitude: 40.712776, longitude: -74.005974),
                        "devices/Navigator"
                    );
                # endregion

                $session->typedTimeSeriesFor(RoutePoint::class, "users/john")
                    ->append(
                         (clone $baseTime)->add(new DateInterval("PT2H")),
                        new RoutePoint(latitude: 40.712781, longitude: -74.005979),
                        "devices/Navigator"
                    );

                $session->typedTimeSeriesFor(RoutePoint::class, "users/john")
                    ->append(
                         (clone $baseTime)->add(new DateInterval("PT3H")),
                        new RoutePoint(latitude: 40.712789, longitude: -74.005987),
                        "devices/Navigator"
                    );

                $session->typedTimeSeriesFor(RoutePoint::class, "users/john")
                    ->append(
                         (clone $baseTime)->add(new DateInterval("PT4H")),
                        new RoutePoint(latitude: 40.712792, longitude: -74.006002),
                        "devices/Navigator"
                    );

                $session->saveChanges();
            } finally {
                $session->close();
            }


            // Get entries
            $session = $store->openSession();
            try {
                // Use the session to load a document
                $user = $session->load(User::class, "users/john");

                // Pass the document object returned from session.Load as a param
                // Retrieve a single value from the "HeartRates" time series

                /** @var TimeSeriesEntryArray<RoutePoint> $results */
                $results = $session
                    ->typedTimeSeriesFor(RoutePoint::class, "users/john")
                    ->get();
            } finally {
                $session->close();
            }

            //append entries
            $session = $store->openSession();
            try {
                $user = new User();
                $user->setName("John");
                $session->store($user, "users/john");

                // Append a HeartRate entry
                $session->timeSeriesFor("users/john", "HeartRates")
                    ->append ((clone $baseTime)->add(new DateInterval("PT1M")), 70, "watches/fitbit");

                $session->saveChanges();
            } finally {
                $session->close();
            }

            // append entries using a registered time series type
            $session = $store->openSession();
            try {
                $user = new User();
                $user->setName("John");
                $session->store($user, "users/john");

                //store.TimeSeries.Register<User, HeartRate>();

                $hr = new HeartRate;
                $hr->setHeartRateMeasure(80);

                $session->typedTimeSeriesFor(HeartRate::class, "users/john")
                    ->append(new DateTime(), $hr, "watches/anotherFirm");

                $session->saveChanges();
            } finally {
                $session->close();
            }

            // append multi-value entries by name
            # region timeseries_region_Append-Named-Values-2
            $session = $store->openSession();
            try {
                $user = new User();
                $user->setName("John");
                $session->store($user, "users/john");

                // Call 'Append' with the custom StockPrice class
                $sp = new StockPrice();
                $sp->setOpen(52);
                $sp->setClose(54);
                $sp->setHigh(63.5);
                $sp->setLow(51.4);
                $sp->setVolume(9824);

                $session->typedTimeSeriesFor(StockPrice::class, "users/john")
                ->append(
                     (clone $baseTime)->add(new DateInterval("P1D")),
                    $sp,
                    "companies/kitchenAppliances"
                );

                $sp = new StockPrice();
                $sp->setOpen(54);
                $sp->setClose(55);
                $sp->setHigh(61.5);
                $sp->setLow(49.4);
                $sp->setVolume(8400);
                $session->typedTimeSeriesFor(StockPrice::class, "users/john")
                    ->append(
                         (clone $baseTime)->add(new DateInterval("P2D")),
                        $sp,
                        "companies/kitchenAppliances"
                    );

                $sp = new StockPrice();
                $sp->setOpen(55);
                $sp->setClose(57);
                $sp->setHigh(65.5);
                $sp->setLow(50);
                $sp->setVolume(9020);
                $session->typedTimeSeriesFor(StockPrice::class, "users/john")
                    ->append(
                         (clone $baseTime)->add(new DateInterval("P3D")),
                        $sp,
                        "companies/kitchenAppliances"
                    );

                $session->saveChanges();
            } finally {
                $session->close();
            }
            # endregion

            # region timeseries_region_Append-Unnamed-Values-2
            $session = $store->openSession();
            try {
                $user = new User();
                $user->setName("John");
                $session->store($user, "users/john");

                $session->timeSeriesFor("users/john", "StockPrices")
                ->append(
                     (clone $baseTime)->add(new DateInterval("P1D")),
                    [ 52, 54, 63.5, 51.4, 9824 ],
                    "companies/kitchenAppliances"
                );

                $session->timeSeriesFor("users/john", "StockPrices")
                    ->append(
                         (clone $baseTime)->add(new DateInterval("P2D")),
                        [ 54, 55, 61.5, 49.4, 8400 ],
                        "companies/kitchenAppliances"
                    );

                $session->timeSeriesFor("users/john", "StockPrices")
                ->append(
                     (clone $baseTime)->add(new DateInterval("P3D")),
                    [ 55, 57, 65.5, 50, 9020 ],
                    "companies/kitchenAppliances"
                );

                $session->saveChanges();
            } finally {
                $session->close();
            }
            # endregion

            // append multi-value entries using a registered time series type
            $session = $store->openSession();
            try {
                $company = new Company();
                $company->setName("kitchenAppliances");

                $address = new Address();
                $address->setCity("New York");
                $company->setAddress($address);
                $session->store($company, "companies/kitchenAppliances");

                $sp = new StockPrice();
                $sp->setOpen(52);
                $sp->setClose(54);
                $sp->setHigh(63.5);
                $sp->setLow(51.4);
                $sp->setVolume(9824);
                $session->typedTimeSeriesFor(StockPrice::class, "companies/kitchenAppliances")
                    ->append(
                         (clone $baseTime)->add(new DateInterval("P1D")),
                        $sp,
                        "companies/kitchenAppliances"
                    );

                $sp = new StockPrice();
                $sp->setOpen(54);
                $sp->setClose(55);
                $sp->setHigh(61.5);
                $sp->setLow(49.4);
                $sp->setVolume(8400);
                $session->typedTimeSeriesFor(StockPrice::class, "companies/kitchenAppliances")
                    ->append(
                         (clone $baseTime)->add(new DateInterval("P2D")),
                        $sp,
                        "companies/kitchenAppliances"
                    );

                $sp = new StockPrice();
                $sp->setOpen(55);
                $sp->setClose(57);
                $sp->setHigh(65.5);
                $sp->setLow(50);
                $sp->setVolume(9020);
                $session->typedTimeSeriesFor(StockPrice::class, "companies/kitchenAppliances")
                    ->append(
                         (clone $baseTime)->add(new DateInterval("P3D")),
                        $sp,
                        "companies/kitchenAppliances"
                    );

                $session->saveChanges();
            } finally {
                $session->close();
            }

            # region timeseries_region_Named-Values-Query
            $session = $store->openSession();
            try {
                $startTime = new DateTime();
                $endTime = (new DateTime())->add(new DateInterval("P3D"));

                $tsQueryText = "
                    from StockPrices
                    between \$start and \$end
                    where Tag == \"AppleTech\"";

                $query = $session->query(Company::class)
                    ->whereEquals("Address.City", "New York")
                    ->selectTimeSeries(TimeSeriesAggregationResult::class, function ($b) use ($tsQueryText) {
                        return $b->raw($tsQueryText);
                    })
                    ->addParameter("start", $startTime)
                    ->addParameter("end", $endTime);

                /** @var array<TimeSeriesAggregationResult> $queryResults */
                $queryResults = $query->toList();

                /** @var TimeSeriesEntryArray<StockPrice> $tsEntries */
                $tsEntries = $queryResults[0]->asTypedResult(StockPrice::class);

                $volumeDay1 = $tsEntries[0]->getValue()->getVolume();
                $volumeDay2 = $tsEntries[1]->getValue()->getVolume();
                $volumeDay3 = $tsEntries[2]->getValue()->getVolume();
            } finally {
                $session->close();
            }
            # endregion

            # region timeseries_region_Unnamed-Values-Query
            $session = $store->openSession();
            try {
                $startTime = new DateTime();
                $endTime = (new DateTime())->add(new DateInterval("P3D"));

                $tsQueryText = "
                    from StockPrices
                    between \$start and \$end
                    where Tag == \"AppleTech\"";

                $query = $session->query(Company::class)
                    ->whereEquals("Address.City", "New York")
                    ->selectTimeSeries(TimeSeriesRawResult::class, function ($b) use ($tsQueryText) {
                        return $b->raw($tsQueryText);
                    })
                    ->addParameter("start", $startTime)
                    ->addParameter("end", $endTime);

                /** @var array<TimeSeriesRawResult> $queryResults */
                $queryResults = $query->toList();

                /** @var TimeSeriesEntryArray $tsEntries */
                $tsEntries = $queryResults[0]->getResults();

                $volumeDay1 = $tsEntries[0]->getValues()[4];
                $volumeDay2 = $tsEntries[1]->getValues()[4];
                $volumeDay3 = $tsEntries[2]->getValues()[4];
            } finally {
                $session->close();
            }
            # endregion

            // get entries
            $session = $store->openSession();
            try {
                // Use the session to load a document
                $user = $session->load(User::class, "users/john");

                // Pass the document object returned from session.Load as a param
                // Retrieve a single value from the "HeartRates" time series
                /** @var TimeSeriesEntryArray $val */
                $val = $session->timeSeriesFor($user, "HeartRates")
                    ->get();
            } finally {
                $session->close();
            }


            # region timeseries_region_Get-NO-Named-Values
            // Use Get without a named type
            // Is the stock's closing-price rising?
            $goingUp = false;

            $session = $store->openSession();
            try {
                /** @var TimeSeriesEntryArray $val */
                $val = $session->timeSeriesFor("users/john", "StockPrices")
                    ->get();

                $closePriceDay1 = $val[0]->getValues()[1];
                $closePriceDay2 = $val[1]->getValues()[1];
                $closePriceDay3 = $val[2]->getValues()[1];

                if (($closePriceDay2 > $closePriceDay1)
                    &&
                    ($closePriceDay3 > $closePriceDay2))
                    $goingUp = true;
            } finally {
                $session->close();
            }
            # endregion

            # region timeseries_region_Get-Named-Values
            $goingUp = false;

            $session = $store->openSession();
            try {
                // Call 'Get' with the custom StockPrice class type
                /** @var TimeSeriesEntryArray<StockPrice> $val */
                $val = $session->typedTimeSeriesFor(StockPrice::class, "users/john")
                    ->get();

                $closePriceDay1 = $val[0]->getValue()->getClose();
                $closePriceDay2 = $val[1]->getValue()->getClose();
                $closePriceDay3 = $val[2]->getValue()->getClose();

                if (($closePriceDay2 > $closePriceDay1)
                    &&
                    ($closePriceDay3 > $closePriceDay2))
                    $goingUp = true;
            } finally {
                $session->close();
            }
            # endregion


            // remove entries
            /*
            $session = $store->openSession();
            try {
                $session->timeSeriesFor("users/john", "HeartRates")
                    ->delete((clone $baseTime)->add(new DateInterval("PT1M"));

                $session->saveChanges();
            } finally {
                $session->close();
            }*/

            // remove entries using a registered time series type
            $session = $store->openSession();
            try {
                $session->timeSeriesFor("users/john", "HeartRates")
                    ->delete ((clone $baseTime)->add(new DateInterval("PT1M")));

                $session->typedTimeSeriesFor(StockPrice::class, "users/john")
                    ->delete ((clone $baseTime)->add(new DateInterval("P1D")),  (clone $baseTime)->add(new DateInterval("P2D")));

                $session->saveChanges();
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }

    public function canAppendAndGetUsingQuery(): void
    {
        $store = $this->getDocumentStore();
        try {
            // Create a document
            $session = $store->openSession();
            try {
                $user = new User();
                $user->setName("John");
                
                $session->store($user);
                $session->saveChanges();
            } finally {
                $session->close();
            }

            // Query for a document with the Name property "John" and append it a time point
            $session = $store->openSession();
            try {
                 $baseTime = DateUtils::today();

                $query = $session->query(User::class)
                    ->whereEquals("Name", "John");

                $result = $query->toList();

                $session->timeSeriesFor($result[0], "HeartRates")
                    ->append((clone $baseTime)->add(new DateInterval("PT1M")), 72, "watches/fitbit");

                $session->saveChanges();
            } finally {
                $session->close();
            }

            # region timeseries_region_Pass-TimeSeriesFor-Get-Query-Results
            // Query for a document with the Name property "John"
            // and get its HeartRates time-series values
            $session = $store->openSession();
            try {
                 $baseTime = DateUtils::today();

                $query = $session->query(User::class)
                    ->whereEquals("Name", "John");

                $result = $query->toList();

                /** @var TimeSeriesEntryArray $val */
                $val = $session->timeSeriesFor($result[0], "HeartRates")
                    ->get();

                $session->saveChanges();
            } finally {
                $session->close();
            }
            # endregion
      } finally {
          $store->close();
      }
    }

    public function canIncludeTimeSeriesData(): void
    {
        $store = $this->getDocumentStore();
        try {
            // Create a document
            $session = $store->openSession();
            try {
                $user = new User();
                $user->setName("John");

                $session->store($user);
                $session->saveChanges();
            } finally {
                $session->close();
            }

            // Query for a document with the Name property "John" and append it a time point
            $session = $store->openSession();
            try {
                 $baseTime = DateUtils::today();

                $query = $session->query(User::class)
                    ->whereEquals("Name", "John");

                $result = $query->toList();

                for ($cnt = 0; $cnt < 10; $cnt++)
                {
                    $session->timeSeriesFor($result[0], "HeartRates")
                        ->append((clone $baseTime)->add(new DateInterval("PT" . $cnt . "M")), 72, "watches/fitbit");
                }

                $session->saveChanges();
            } finally {
                $session->close();
            }

            # region timeseries_region_Load-Document-And-Include-TimeSeries
            $session = $store->openSession();
            try {
                 $baseTime = DateUtils::today();

                // Load a document
                /** @var User $user */
                $user = $session->load(User::class, "users/john", function($includeBuilder) use ( $baseTime) {
                    // Call 'IncludeTimeSeries' to include time series entries, pass:
                    // * The time series name
                    // * Start and end timestamps indicating the range of entries to include
                    return $includeBuilder->includeTimeSeries("HeartRates",   (clone $baseTime)->add(new DateInterval("PT3M")),   (clone $baseTime)->add(new DateInterval("PT8M")));
                });

                // The following call to 'Get' will Not trigger a server request,
                // the entries will be retrieved from the session's cache.
                /** @var TimeSeriesEntryArray $entries */
                $entries = $session->timeSeriesFor("users/john", "HeartRates")
                    ->get((clone $baseTime)->add(new DateInterval("PT3M")),   (clone $baseTime)->add(new DateInterval("PT8M")));
            } finally {
                $session->close();
            }
            # endregion

            # region timeseries_region_Query-Document-And-Include-TimeSeries
            $session = $store->openSession();
            try {
                // Query for a document and include a whole time-series
                /** @var User $user */
                $user = $session->query(User::class)
                    ->whereEquals("Name", "John")
                        ->include(function($includeBuilder) { return $includeBuilder->includeTimeSeries("HeartRates"); })
                    ->firstOrDefault();

                // The following call to 'Get' will Not trigger a server request,
                // the entries will be retrieved from the session's cache.
                /** @var TimeSeriesEntryArray $entries */
                $entries = $session->timeSeriesFor($user, "HeartRates")
                    ->get();
            } finally {
                $session->close();
            }
            # endregion

            # region timeseries_region_Raw-Query-Document-And-Include-TimeSeries
            $session = $store->openSession();
            try {
                $baseTime = DateUtils::today();

                $from = $baseTime;
                $to =  (clone $baseTime)->add(new DateInterval("PT5M"));

                // Define the Raw Query:
                $query = $session->advanced()->rawQuery(User::class,
                           // Use 'include timeseries' in the RQL
                    "from Users include timeseries('HeartRates', \$from, \$to)")
                           // Pass optional parameters
                          ->addParameter("from", $from)
                          ->addParameter("to", $to);

                // Execute the query:
                // For each document in the query results,
                // the time series entries will be 'loaded' to the session along with the document
                $users = $query->toList();

                // The following call to 'Get' will Not trigger a server request,
                // the entries will be retrieved from the session's cache.
                /** @var TimeSeriesEntryArray $entries */
                $entries = $session->timeSeriesFor($users[0], "HeartRates")
                    ->get($from, $to);
            } finally {
                $session->close();
            }
            # endregion
      } finally {
          $store->close();
      }
    }

    public function appendWithIEnumerable(): void
    {
        $store = $this->getDocumentStore();
        try {
             $baseTime = DateUtils::today();

            // Open a session
            $session = $store->openSession();
            try {
                // Use the session to create a document
                $user = new User();
                $user->setName("John");
                $session->store($user, "users/john");

                $session->timeSeriesFor("users/john", "HeartRates")
                ->append((clone $baseTime)->add(new DateInterval("PT1M")),
                        [ 65, 52, 72 ],
                        "watches/fitbit");

                $session->saveChanges();
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                // Use the session to load a document
                $user = $session->load(User::class, "users/john");

                // Pass the document object returned from session.Load as a param
                // Retrieve a single value from the "HeartRates" time series
                /** @var TimeSeriesEntry $val */
                $val = $session->timeSeriesFor($user, "HeartRates")
                    ->get();

            } finally {
                $session->close();
            }

            // Get time series HeartRates' time points data
            $session = $store->openSession();
            try {

                # region timeseries_region_Get-All-Entries-Using-Document-ID
                // Get all time series entries
                /** @var TimeSeriesEntryArray $val */
                $val = $session->timeSeriesFor("users/john", "HeartRates")
                    ->get();
                # endregion
            } finally {
                $session->close();
            }

            // Get time series HeartRates's time points data
            $session = $store->openSession();
            try {
                # region IncludeParentAndTaggedDocuments
                // Get all time series entries
                /** @var TimeSeriesEntryArray $entries */
                $entries =
                    $session->timeSeriesFor("users/john", "HeartRates")
                        ->get(null, null,
                            includes: function($builder) {
                                return $builder
                                    // Include documents referred-to by entry tags
                                    ->includeTags()
                                    // Include Parent Document
                                    ->includeDocument();
                            });
                # endregion
            } finally {
                $session->close();
            }
      } finally {
          $store->close();
      }
    }

    public function removeRange(): void
    {
        $store = $this->getDocumentStore();
        try {
            # region timeseries_region_TimeSeriesFor-Append-TimeSeries-Range
            $baseTime = DateUtils::today();

            // Append 10 HeartRate values
            $session = $store->openSession();
            try {
                $user = new User();
                $user->setName("John");
                $session->store($user, "users/john");

                $tsf = $session->timeSeriesFor("users/john", "HeartRates");

                for ($i = 0; $i < 10; $i++)
                {
                    $tsf->append((clone $baseTime)->add(new DateInterval("PT" . $i . "S")), [ 67 ], "watches/fitbit");
                }

                $session->saveChanges();
            } finally {
                $session->close();
            }
            # endregion

            # region timeseries_region_TimeSeriesFor-Delete-Time-Points-Range
            // Delete a range of entries from the time series
            $session = $store->openSession();
            try {
                $session->timeSeriesFor("users/john", "HeartRates")
                    ->delete((clone $baseTime)->add(new DateInterval("PT0S")),   (clone $baseTime)->add(new DateInterval("PT9S")));

                $session->saveChanges();
            } finally {
                $session->close();
            }
            # endregion
      } finally {
          $store->close();
      }
    }

    public function useGetTimeSeriesOperation(): void
    {
        $store = $this->getDocumentStore();
        try {
            // Create a document
            $session = $store->openSession();
            try {
                $employee1 = new Employee();
                $employee1->setFirstName("John");
                $session->store($employee1);

                $employee2 = new Employee();
                $employee2->setFirstName("Mia");
                $session->store($employee2);

                $employee3 = new Employee();
                $employee3->setFirstName("Emil");
                $session->store($employee3);

                $session->saveChanges();
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                $employeesIdList = [];

                $employeeList = $session
                    ->query(Employee::class)
                    ->toList();

                foreach($employeeList as $employee) {
                    $employeesIdList[] = $employee->getId();
                }
            } finally {
                $session->close();
            }

            // Append each employee a week (168 hours) of random exercise HeartRate values
            // and a week (168 hours) of random rest HeartRate values
            $baseTime = new DateTime("2020-05-17");
            $session = $store->openSession();
            try {
                foreach ($employeesIdList as $employeeId)
                {
                    for ($tse = 0; $tse < 168; $tse++)
                    {
                        $session->timeSeriesFor($employeeId, "ExerciseHeartRate")
                        ->append ((clone $baseTime)->add(new DateInterval("PT". $tse . "H")),
                                68 + rand(0, 19),
                                "watches/fitbit");

                        $session->timeSeriesFor($employeeId, "RestHeartRate")
                        ->append ((clone $baseTime)->add(new DateInterval("PT". $tse . "H")),
                                52 + rand(0, 19),
                                "watches/fitbit");
                    }
                }
                $session->saveChanges();
            } finally {
                $session->close();
            }

            # region timeseries_region_Get-Single-Time-Series
            // Define the get operation
            $getTimeSeriesOp = new GetTimeSeriesOperation(
                "employees/1-A", // The document ID
                "HeartRates", // The time series name
                null, // Entries range start - use null to go from beginning of time
                null    // Entries range end - use null to go 'till the end of time
            );

            // Execute the operation by passing it to 'Operations.Send'
            /** @var TimeSeriesRangeResult $timeSeriesEntries */
            $timeSeriesEntries = $store->operations()->send($getTimeSeriesOp);

            // Access entries
            $firstEntryReturned = $timeSeriesEntries->getEntries()[0];
            # endregion

            # region timeseries_region_Get-Multiple-Time-Series
            // Define the get operation
            $getMultipleTimeSeriesOp = new GetMultipleTimeSeriesOperation("employees/1-A",
                [
                    new TimeSeriesRange("ExerciseHeartRates",  (clone $baseTime)->add(new DateInterval("PT1H")),  (clone $baseTime)->add(new DateInterval("PT10H"))),
                    new TimeSeriesRange("RestHeartRates",  (clone $baseTime)->add(new DateInterval("PT11H")),  (clone $baseTime)->add(new DateInterval("PT20H")))
                ]);

            // Execute the operation by passing it to 'Operations.Send'
            /** @var TimeSeriesDetails $timesSeriesEntries */
            $timesSeriesEntries = $store->operations()->send($getMultipleTimeSeriesOp);

            // Access entries
            $timeSeriesEntry = $timesSeriesEntries->getValues()["ExerciseHeartRates"][0]->getEntries()[0];
            # endregion
      } finally {
          $store->close();
      }
    }

    public function useTimeSeriesBatchOperation(): void
    {
        $store = $this->getDocumentStore();
        try {
            # region timeseries_region_Append-Using-TimeSeriesBatchOperation
            $baseTime = DateUtils::today();

            // Define the Append operations:
            // =============================
            $appendOp1 = new AppendOperation(
                 (clone $baseTime)->add(new DateInterval("PT1M")),
                [ 79 ],
                "watches/fitbit"
            );

            $appendOp2 = new AppendOperation(
                 (clone $baseTime)->add(new DateInterval("PT2M")),
                [ 82 ],
                "watches/fitbit"
            );

            $appendOp3 = new AppendOperation(
                 (clone $baseTime)->add(new DateInterval("PT3M")),
                [ 80 ],
                "watches/fitbit"
            );

            $appendOp4 = new AppendOperation(
                 (clone $baseTime)->add(new DateInterval("PT4M")),
                [ 78 ],
                "watches/fitbit"
            );

            // Define 'TimeSeriesOperation' and add the Append operations:
            // ===========================================================
            $timeSeriesOp = new TimeSeriesOperation("HeartRates");

            $timeSeriesOp->append($appendOp1);
            $timeSeriesOp->append($appendOp2);
            $timeSeriesOp->append($appendOp3);
            $timeSeriesOp->append($appendOp4);


            // Define 'TimeSeriesBatchOperation' and execute:
            // ==============================================
            $timeSeriesBatchOp = new TimeSeriesBatchOperation("users/john", $timeSeriesOp);
            $store->operations()->send($timeSeriesBatchOp);
            # endregion
      } finally {
          $store->close();
      }

        $store = $this->getDocumentStore();
        try {
            # region timeseries_region_Delete-Range-Using-TimeSeriesBatchOperation
            $baseTime = DateUtils::today();

            $deleteOp = new DeleteOperation(
                 (clone $baseTime)->add(new DateInterval("PT2M")),
                 (clone $baseTime)->add(new DateInterval("PT3M"))
            );

            $timeSeriesOp = new TimeSeriesOperation("HeartRates");

            $timeSeriesOp->delete($deleteOp);

            $timeSeriesBatchOp = new TimeSeriesBatchOperation("users/john", $timeSeriesOp);

            $store->operations()->send($timeSeriesBatchOp);
            # endregion
      } finally {
          $store->close();
      }

        $store = $this->getDocumentStore();
        try {
            # region timeseries_region-Append-and-Delete-TimeSeriesBatchOperation
            $baseTime = DateUtils::today();

            // Define some Append operations:
            $appendOp1 = new AppendOperation(
                 (clone $baseTime)->add(new DateInterval("PT1M")),
                [ 79 ],
                "watches/fitbit"
            );

            $appendOp2 = new AppendOperation(
                 (clone $baseTime)->add(new DateInterval("PT2M")),
                [ 82 ],
                "watches/fitbit"
            );

            $appendOp3 = new AppendOperation(
                 (clone $baseTime)->add(new DateInterval("PT3M")),
                [ 80 ],
                "watches/fitbit"
            );

            // Define a Delete operation:
            $deleteOp = new DeleteOperation(
                 (clone $baseTime)->add(new DateInterval("PT2M")),
                 (clone $baseTime)->add(new DateInterval("PT3M"))
            );

            $timeSeriesOp = new TimeSeriesOperation("HeartRates");

            // Add the Append & Delete operations to the list of actions
            // Note: the Delete action will be executed BEFORE all the Append actions
            //       even though it is added last
            $timeSeriesOp->append($appendOp1);
            $timeSeriesOp->append($appendOp2);
            $timeSeriesOp->append($appendOp3);
            $timeSeriesOp->delete($deleteOp);

            $timeSeriesBatchOp = new TimeSeriesBatchOperation("users/john", $timeSeriesOp);

            $store->operations()->send($timeSeriesBatchOp);

            // Results:
            // All 3 entries that were appended will exist and are not deleted.
            // This is because the Delete action occurs first, before all Append actions.
            # endregion
      } finally {
          $store->close();
      }
    }

    public function patchTimeSeries(): void
    {
        $store = $this->getDocumentStore();
        try {
            // Create a document
            $session = $store->openSession();
            try {
                $user = new User();
                $user->setName("John");
                
                $session->store($user);
                $session->saveChanges();
            } finally {
                $session->close();
            }

            // Patch a time-series to a document whose Name property is "John"
            $session = $store->openSession();
            try {
                 $baseTime = DateUtils::today();

                $query = $session->query(User::class)
                    ->whereEquals("Name", "John");
                /** @var array<User> $result */
                $result = $query->toList();
                $documentId = $result[0]->getId();

                $values = [ 59 ];
                $tag = "watches/fitbit";
                $timeseries = "HeartRates";

                $patchRequest = new PatchRequest();
                $patchRequest->setScript(
                    "timeseries(this, \$timeseries)
                                 ->append(
                                    \$timestamp,
                                    \$values,
                                    \$tag
                                  );"
                );

                $patchRequest->setValues(
                    [
                        "timeseries" => $timeseries,
                        "timestamp" =>   (clone $baseTime)->add(new DateInterval("PT1M")),
                        "values" => $values,
                        "tag" => $tag
                    ]
                );

                $session->advanced()->defer(new PatchCommandData($documentId, null, $patchRequest, null));
                $session->saveChanges();

            } finally {
                $session->close();
            }
      } finally {
          $store->close();
      }
    }

    // patching a document a single time-series entry
    // using $session->advanced().Defer
    public function patchSingleEntryUsingSessionDefer(): void
    {
        $store = $this->getDocumentStore();
        try {
            // Create a document
            $session = $store->openSession();
            try {
                $user = new User();
                $user->setName("John");

                $session->store($user);
                $session->saveChanges();
            } finally {
                $session->close();
            }

            // Patch a document a single time-series entry
            $session = $store->openSession();
            try {
                 $baseTime = DateUtils::today();

                $patchRequest = new PatchRequest();
                $patchRequest->setScript("timeseries(this, \$timeseries)
                                 ->append(
                                     \$timestamp,
                                     \$values,
                                     \$tag
                                   );");
                $patchRequest->setValues([
                    "timeseries" => "HeartRates",
                    "timestamp" =>   (clone $baseTime)->add(new DateInterval("PT1M")),
                    "values" => 59,
                    "tag" => "watches/fitbit"
                ]);

                $session->advanced()->defer(new PatchCommandData("users/1-A", null, $patchRequest, null));
                $session->saveChanges();
            } finally {
                $session->close();
            }
      } finally {
          $store->close();
      }
    }

    // patching a document a single time-series entry
    // using PatchOperation
    public function patchSingleEntryUsingPatchOperation(): void
    {
        $store = $this->getDocumentStore();
        try {
            // Create a document
            $session = $store->openSession();
            try {
                $user = new User();
                $user->setName("John");
                
                $session->store($user);
                $session->saveChanges();
            } finally {
                $session->close();
            }

            # region TS_region-Operation_Patch-Append-Single-TS-Entry
            $baseTime = DateUtils::now();

            $patchRequest = new PatchRequest();
            // Define the patch request using JavaScript:
            $patchRequest->setScript("timeseries(this, \$timeseries)->append(\$timestamp, \$values, \$tag);");

            // Provide values for the parameters in the script:
            $patchRequest->setValues([
               "timeseries" => "HeartRates",
                "timestamp" =>  (clone $baseTime)->add(new DateInterval("PT1M")),
                "values"=> 59,
                "tag" => "watches/fitbit"
            ]);

            // Define the patch operation;
            $patchOp = new PatchOperation("users/john", null, $patchRequest);

            // Execute the operation:
            $store->operations()->send($patchOp);
            # endregion
      } finally {
          $store->close();
      }
    }

    // Patching: Append and Remove multiple time-series entries
    // Using $session->advanced().Defer
    public function patchAndDeleteMultipleEntriesSession(): void
    {
        $store = $this->getDocumentStore();
        try {
            // Create a document
            $session = $store->openSession();
            try {
                $user = new User();
                $user->setName("John");

                $session->store($user);
                $session->saveChanges();
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region TS_region-Session_Patch-Append-100-Random-TS-Entries
                 $baseTime = DateUtils::today();

                // Create arrays of timestamps and random values to patch
                $values = [];
                $timeStamps = [];

                for ($i = 0; $i < 100; $i++)
                {
                    $values[] = 68 + rand(0, 19);
                    $timeStamps[] =   (clone $baseTime)->add(new DateInterval("PT" . $i . "S"));
                }

                $patchRequest = new PatchRequest();
                $patchRequest->setScript("
                            var i = 0;
                            for(i = 0; i < \$values.length; i++)
                            {
                                timeseries(id(this), \$timeseries)
                                .append (
                                  new Date(\$timeStamps[i]),
                                  \$values[i],
                                  \$tag);
                            }");
                $patchRequest->setValues([
                        "timeseries" => "HeartRates",
                        "timeStamps" => $timeStamps,
                        "values" => $values,
                        "tag" => "watches/fitbit"
                ]);

                $session->advanced()->defer(new PatchCommandData("users/1-A", null, $patchRequest, null));

                $session->saveChanges();
                # endregion

                # region TS_region-Session_Patch-Delete-50-TS-Entries
                // Delete time-series entries
                $patchRequest = new PatchRequest();
                $patchRequest->setScript("timeseries(this, \$timeseries)
                                 .delete(
                                    \$from,
                                    \$to
                                  );");
                $patchRequest->setValues([
                    "timeseries" => "HeartRates",
                    "from" =>  $baseTime,
                    "to" =>   (clone $baseTime)->add(new DateInterval("PT49S"))
                ]);

                $session->advanced()->defer(new PatchCommandData("users/1-A", null, $patchRequest, null));

                $session->saveChanges();
                # endregion
            } finally {
                $session->close();
            }
      } finally {
          $store->close();
      }
    }

    // Patching:multiple time-series entries Using $session->advanced().Defer
    public function patcMultipleEntriesSession(): void
    {
        $store = $this->getDocumentStore();
        try {
            // Create a document
            $session = $store->openSession();
            try {
                $user = new User();
                $user->setName("John");

                $session->store($user, "users/john");
                $session->saveChanges();
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region TS_region-Session_Patch-Append-TS-Entries
                $baseTime = DateUtils::now();

                // Create arrays of timestamps and random values to patch
                $values = [];
                $timeStamps = [];

                for ($i = 0; $i < 100; $i++)
                {
                    $values[] = 68 + rand(0, 19);;
                    $timeStamps[] =  (clone $baseTime)->add(new DateInterval("PT" . $i . "S"));
                }

                $patchRequest = new PatchRequest();
                $patchRequest->setScript("
                            var i = 0;
                            for(i = 0; i < \$values.length; i++)
                            {
                                timeseries(id(this), \$timeseries)
                                .append (
                                    new Date(\$timeStamps[i]),
                                    \$values[i],
                                    \$tag);
                            }");
                $patchRequest->setValues([
                    "timeseries" => "HeartRates" ,
                    "timeStamps" => $timeStamps ,
                    "values" => $values ,
                    "tag" => "watches/fitbit"
                ]);

                $session->advanced()->defer(new PatchCommandData("users/john", null, $patchRequest, null));

                $session->saveChanges();
                # endregion
            } finally {
                $session->close();
            }
      } finally {
          $store->close();
      }
    }

    // Patching: Append and Remove multiple time-series entries
    // Using PatchOperation
    public function patchAndDeleteMultipleEntriesOperation(): void
    {
        $store = $this->getDocumentStore();
        try {
            // Create a document
            $session = $store->openSession();
            try {
                $user = new User();
                $user->setName("John");

                $session->store($user);
                $session->saveChanges();
            } finally {
                $session->close();
            }

            // Patch a document 100 time-series entries
            $session = $store->openSession();
            try {
                # region TS_region-Operation_Patch-Append-100-TS-Entries
                $baseTime = DateUtils::now();

                // Create arrays of timestamps and random values to patch
                $values = [];
                $timeStamps = [];

                for ($i = 0; $i < 100; $i++)
                {
                    $values[] = 68 + rand(0, 19);
                    $timeStamps[] =  (clone $baseTime)->add(new DateInterval("PT". $i . "M"));
                }

                $patchRequest = new PatchRequest();
                $patchRequest->setScript("var i = 0;
                               for (i = 0; i < \$values.length; i++) {
                                   timeseries(id(this), \$timeseries).append (
                                       \$timeStamps[i],
                                       \$values[i],
                                       \$tag);
                               }");
                $patchRequest->setValues([
                    "timeseries" => "HeartRates",
                    "timeStamps" => $timeStamps,
                    "values" => $values,
                    "tag" => "watches/fitbit"
                ]);

                $patchOp = new PatchOperation("users/john", null, $patchRequest);
                $store->operations()->send($patchOp);
                # endregion

                # region TS_region-Operation_Patch-Delete-50-TS-Entries
                $patchRequest = new PatchRequest();
                $patchRequest->setScript("timeseries(this, \$timeseries).delete(\$from, \$to);");
                $patchRequest->setValues([
                    "timeseries" => "HeartRates",
                    "from" => $baseTime,
                    "to" =>  (clone $baseTime)->add(new DateInterval("PT49M"))
                ]);

                $store->operations()->send(new PatchOperation("users/john", null, $patchRequest));
                # endregion
            } finally {
                $session->close();
            }
      } finally {
          $store->close();
      }
    }

    //Query Time-Series Using Raw RQL
    public function queryTimeSeriesUsingRawRQL(): void
    {
        $store = $this->getDocumentStore();
        try {
            // Create a document
            $session = $store->openSession();
            try {
                $user = new User();
                $user->setName("John");

                $session->store($user);
                $session->saveChanges();
            } finally {
                $session->close();
            }

            // Query for a document with the Name property "John" and append it a time point
            $session = $store->openSession();
            try {
                 $baseTime = DateUtils::today();

                $query = $session->query(User::class)
                    ->whereEquals("Name", "John");

                $result = $query->toList();

                for ($cnt = 0; $cnt < 120; $cnt++)
                {
                    $session->timeSeriesFor($result[0], "HeartRates")
                        ->append((clone $baseTime)->add(new DateInterval("P". $cnt . "D")), 72, "watches/fitbit");
                }

                $session->saveChanges();

            } finally {
                $session->close();
            }

            // Raw Query
            $session = $store->openSession();
            try {
                 $baseTime = DateUtils::today();

                $start =  $baseTime;
                $end =   (clone $baseTime)->add(new DateInterval("PT1H"));

                $query = $session->advanced()->rawQuery(User::class, "from Users include timeseries('HeartRates', \$start, \$end)")
                    ->addParameter("start", $start)
                    ->addParameter("end", $end);

                // Raw Query with aggregation
                $aggregatedRawQuery =
                    $session->advanced()->rawQuery(TimeSeriesAggregationResult::class, "
                        from Users as u where Age < 30
                        select timeseries(
                            from HeartRates between
                                '2020-05-27T00:00:00.0000000Z'
                                    and '2020-06-23T00:00:00.0000000Z'
                            group by '7 days'
                            select min(), max())
                        ");

                $aggregatedRawQueryResult = $aggregatedRawQuery->toList();
            } finally {
                $session->close();
            }

      } finally {
          $store->close();
      }
    }


    //Raw RQL and LINQ aggregation and projection queries
    public function aggregationQueries(): void
    {
        $store = $this->getDocumentStore();
        try {
            // Create user documents and time-series
            $session = $store->openSession();
            try {
                $employee1 = new User();
                $employee1->setName("John");
                $employee1->setAge(22);
                $session->store($employee1);

                $employee2 = new User();
                $employee2->setName("Mia");
                $employee2->setAge(26);
                $session->store($employee2);

                $employee3 = new User();
                $employee3->setName("Emil");
                $employee3->setAge(29);
                $session->store($employee3);

                $session->saveChanges();
            } finally {
                $session->close();
            }

            // get employees Id list
            $usersIdList = [];
            $session = $store->openSession();
            try {
                $usersIdList = $session
                    ->query(User::class)
                    ->selectFields(User::class)
                    ->toList();
            } finally {
                $session->close();
            }

            // Append each employee a week (168 hours) of random HeartRate values
            $baseTime = new DateTime("2020-05-17");
            $session = $store->openSession();
            try {
                for ($emp = 0; $emp < count($usersIdList); $emp++) {
                    for ($tse = 0; $tse < 168; $tse++) {
                        $session->timeSeriesFor($usersIdList[$emp]->getId(), "HeartRates")
                            ->append ((clone $baseTime)->add(new DateInterval("PT" . $tse . "H")), 68 + rand(0, 19), "watches/fitbit");
                    }
                }
                $session->saveChanges();
            } finally {
                $session->close();
            }

            // Raw Query - HeartRates using "where Tag in"
            $session = $store->openSession();
            try {
                 $baseTime = new DateTime("2020-05-17");

                $start =  $baseTime;
                $end =   (clone $baseTime)->add(new DateInterval("PT1H"));

                // Raw Query with aggregation
                $aggregatedRawQuery = $session->advanced()->rawQuery(TimeSeriesAggregationResult::class, "
                        from Users as u where Age < 30
                        select timeseries(
                            from HeartRates between
                                '2020-05-17T00:00:00.0000000Z'
                                and '2020-05-23T00:00:00.0000000Z'
                                where Tag in ('watches/Letsfit', 'watches/Willful', 'watches/Lintelek')
                            group by '1 days'
                            select min(), max()
                        )
                        ");

                $aggregatedRawQueryResult = $aggregatedRawQuery->toList();
            } finally {
                $session->close();
            }


            // Raw Query - HeartRates using "where Tag =="
            $session = $store->openSession();
            try {
                 $baseTime = new DateTime("2020-05-17");

                $start =  $baseTime;
                $end =   (clone $baseTime)->add(new DateInterval("PT1H"));

                // Raw Query with aggregation
                $aggregatedRawQuery = $session->advanced()->rawQuery(TimeSeriesAggregationResult::class, "
                        from Users as u where Age < 30
                        select timeseries(
                            from HeartRates between
                                '2020-05-17T00:00:00.0000000Z'
                                and '2020-05-23T00:00:00.0000000Z'
                                where Tag == 'watches/fitbit'
                            group by '1 days'
                            select min(), max()
                        )
                        ");

                $aggregatedRawQueryResult = $aggregatedRawQuery->toList();

            } finally {
                $session->close();
            }


            // Raw Query - StockPrice - Select Syntax
            $session = $store->openSession();
            try {
                 $baseTime = new DateTime("2020-05-17");

                $start =  $baseTime;
                $end =   (clone $baseTime)->add(new DateInterval("PT1H"));

                // Select Syntax
                # region ts_region_Raw-RQL-Select-Syntax-Aggregation-and-Projections-StockPrice
                $aggregatedRawQuery = $session->advanced()->rawQuery(TimeSeriesAggregationResult::class, "
                        from Companies as c
                            where c.Address.Country = 'USA'
                            select timeseries (
                                from StockPrices
                                where Values[4] > 500000
                                    group by '7 day'
                                    select max(), min()
                            )
                        ");

                $aggregatedRawQueryResult = $aggregatedRawQuery->toList();
                # endregion
            } finally {
                $session->close();
            }

            // Raw Query - StockPrice
            $session = $store->openSession();
            try {
                 $baseTime = new DateTime("2020-05-17");

                $start =  $baseTime;
                $end =   (clone $baseTime)->add(new DateInterval("PT1H"));

                // Select Syntax
                # region ts_region_Raw-RQL-Declare-Syntax-Aggregation-and-Projections-StockPrice
                $aggregatedRawQuery = $session->advanced()->rawQuery(TimeSeriesAggregationResult::class, "
                        declare timeseries SP(c) {
                            from c.StockPrices
                            where Values[4] > 500000
                            group by '7 day'
                            select max(), min()
                        }
                        from Companies as c
                        where c.Address.Country = 'USA'
                        select c.Name, SP(c)"
                        );

                $aggregatedRawQueryResult = $aggregatedRawQuery->toList();
                # endregion

            } finally {
                $session->close();
            }
      } finally {
          $store->close();
      }
    }

    //Query Time-Series Using Raw RQL
    public function queryRawRQLNoAggregation(): void
    {
        $store = $this->getDocumentStore();
        try {
            // Create a document
            $session = $store->openSession();
            try {
                $user = new User();
                $user->setName("John");
                $user->setAge(27);

                $session->store($user);
                $session->saveChanges();
            } finally {
                $session->close();
            }

            // Query for a document with the Name property "John" and append it a time point
            $session = $store->openSession();
            try {
                // May 17 2020, 18:00:00
                 $baseTime = new DateTime("2020-05-17T00:00:00");

                $query = $session->query(User::class)
                    ->whereEquals("Name", "John");

                $result = $query->toList();

                // Two weeks of hourly HeartRate values
                for ($cnt = 0; $cnt < 336; $cnt++)
                {
                    $session->timeSeriesFor($result[0], "HeartRates")
                        ->append((clone $baseTime)->add(new DateInterval("PT" . $cnt . "H")), 72, "watches/fitbit");
                }

                $session->saveChanges();
            } finally {
                $session->close();
            }

            // Raw Query
            $session = $store->openSession();
            try {
                # region ts_region_Raw-Query-Non-Aggregated-Declare-Syntax
                $baseTime = new DateTime("2020-05-17T00:00:00"); // May 17 2020, 00:00:00

                // Raw query with no aggregation - Declare syntax
                $query = $session->advanced()->rawQuery(TimeSeriesRawResult::class, "
                        declare timeseries getHeartRates(user)
                        {
                            from user.HeartRates
                                between \$from and \$to
                                offset '02:00'
                        }
                        from Users as u where Age < 30
                        select getHeartRates(u)
                        ")
                    ->addParameter("from", $baseTime)
                    ->addParameter("to",  (clone $baseTime)->add(new DateInterval("PT24H")));

                $results = $query->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region ts_region_Raw-Query-Non-Aggregated-Select-Syntax
                 $baseTime = new DateTime("2020-05-17T00:00:00"); // May 17 2020, 00:00:00

                // Raw query with no aggregation - Select syntax
                $query = $session->advanced()->rawQuery(TimeSeriesRawResult::class, "
                        from Users as u where Age < 30
                        select timeseries (
                            from HeartRates
                                between \$from and \$to
                                offset '02:00'
                        )")
                    ->addParameter("from",  $baseTime)
                    ->addParameter("to",   (clone $baseTime)->add(new DateInterval("PT24H")));

                $results = $query->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region ts_region_Raw-Query-Aggregated
                 $baseTime = new DateTime("2020-05-17T00:00:00"); // May 17 2020, 00:00:00

                // Raw Query with aggregation
                $query =
                    $session->advanced()->rawQuery(TimeSeriesAggregationResult::class, "
                        from Users as u
                        select timeseries(
                            from HeartRates
                                between \$start and \$end
                            group by '1 day'
                            select min(), max()
                            offset '03:00')
                        ")
                    ->addParameter("start",  $baseTime)
                    ->addParameter("end",   (clone $baseTime)->add(new DateInterval("P7D")));

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

    // simple RQL query and its LINQ equivalent
    public function rawRqlAndLinqqueries(): void
    {
        $store = $this->getDocumentStore();
        try {
            // Create a document
            $session = $store->openSession();
            try {
                $user = new User();
                $user->setName("John");
                $user->setAge(28);

                $session->store($user);
                $session->saveChanges();
            } finally {
                $session->close();
            }

            // Query for a document with the Name property "John" and append it a time point
            $session = $store->openSession();
            try {
                 $baseTime = new DateTime("2020-05-17T00:00:00"); // May 17 2020, 00:00:00

                $query = $session->query(User::class)
                    ->whereEquals("Name", "John");

                $result = $query->toList();

                // Two weeks of hourly HeartRate values
                for ($cnt = 0; $cnt < 336; $cnt++)
                {
                    $session->timeSeriesFor($result[0], "HeartRates")
                        ->append((clone $baseTime)->add(new DateInterval("PT" . $cnt . "H")), 72, "watches/fitbit");
                }

                $session->saveChanges();

            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region ts_region_LINQ-2-RQL-Equivalent
                // Raw query with no aggregation - Select syntax
                $query = $session->advanced()->rawQuery(TimeSeriesRawResult::class, "
                        from Users where Age < 30
                        select timeseries (
                            from HeartRates
                        )");

                $results = $query->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region choose_range_1
                $tsQueryText = "
                    from HeartRates
                    between \"2020-05-17T00:00:00.0000000\"
                    and \"2020-05-17T00:10:00.0000000\"
                    offset \"03:00\"
                ";

                $query = $session
                    ->query(Employee::class)
                    ->whereEquals("Address.Country", "UK")
                    ->selectTimeSeries(TimeSeriesRawResult::class, function ($builder) use ($tsQueryText) {
                        return $builder->raw($tsQueryText);
                    });

                // Execute the query
                $result = $query->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region choose_range_2
                $baseTime = new DateTime("2020-05-17T00:00:00");
                $from = $baseTime;
                $to =  (clone $baseTime)->add(new DateInterval("PT10M"));

                $tsQueryText = "
                    from HeartRates
                    between" . NetISO8601Utils::format($from) . " and " . NetISO8601Utils::format($to) . "
                    offset \"03:00\"
                ";

                $query = $session
                    ->query(Employee::class)
                    ->whereEquals("Address.Country", "UK")
                    ->selectTimeSeries(TimeSeriesRawResult::class, function ($builder) use ($tsQueryText) {
                        return $builder->raw($tsQueryText);
                    });

                // Execute the query
                $result = $query->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region choose_range_3
                $baseTime = new DateTime("2020-05-17T00:00:00");

                $query = $session->advanced()
                    ->rawQuery(TimeSeriesRawResult::class, "
                        from Employees
                        where Address.Country == 'UK'
                        select timeseries (
                            from HeartRates
                            between \$from and \$to
                            offset '03:00'
                        )")
                    ->addParameter("from", $baseTime)
                    ->addParameter("to",  (clone $baseTime)->add(new DateInterval("PT10M")));

                // Execute the query
                $results = $query->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region choose_range_4
                $baseTime = new DateTime("2020-05-17T00:00:00");

                $query = $session->advanced()
                    ->rawQuery(TimeSeriesRawResult::class, "
                        from \"Employees\" as employee
                        where employee.Address.Country == \"UK\"
                        select timeseries(
                            from employee.HeartRates
                            between \$from and \$to
                            offset \"03:00\"
                        )")
                    ->addParameter("from", $baseTime)
                    ->addParameter("to",  (clone $baseTime)->add(new DateInterval("PT10M")));

                // Execute the query
                $results = $query->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region choose_range_5
                // Define the time series query part (expressed in RQL):
                $tsQueryText = "
                    from HeartRates
                    last 30 min
                    offset \"03:00\"
                ";

                $query = $session
                    ->query(Employee::class)
                    ->whereEquals("Address.Country", "UK")
                    ->selectTimeSeries(TimeSeriesRawResult::class, function ($builder) use ($tsQueryText) {
                        return $builder->raw($tsQueryText);
                    });

                // Execute the query
                $result = $query->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region choose_range_6
                $query = $session->advanced()
                     // Provide the raw RQL to the RawQuery method:
                    ->rawQuery(TimeSeriesRawResult::class, "
                        from Employees
                        select timeseries (
                            from HeartRates
                            last 30 min
                            offset '03:00'
                        )");

                // Execute the query
                $results = $query->toList();
                # endregion
            } finally {
                $session->close();
            }

      } finally {
          $store->close();
      }
    }

    // Time series Document Query examples
    public function tsDocumentQueries(): void
    {
        $store = $this->getDocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region TS_DocQuery_1
                // Define the query:
                $query = $session->advanced()->documentQuery(User::class)
                    ->selectTimeSeries(TimeSeriesRawResult::class, function ($builder) {
                        return $builder->raw("from HeartRates");
                    });

                // Execute the query:
                // The following call to 'ToList' will trigger query execution
                $results = $query->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region TS_DocQuery_2
                $baseTime = DateUtils::now();
                $from = $baseTime;
                $to =  (clone $baseTime)->add(new DateInterval("P1D"));

                // Define the query:
                $query = $session->advanced()->documentQuery(User::class)
                    ->selectTimeSeries(TimeSeriesRawResult::class, function ($builder) {
                        return $builder->raw("from HeartRates between \$from and \$to");
                    })
                    ->addParameter("from", $from)
                    ->addParameter("to", $to);

                $results = $query->toList();
                # endregion
            } finally {
                $session->close();
            }

    // Various raw RQL queries
    public function queryRawRQLQueries(): void
    {
        $store = $this->getDocumentStore();
        try {
            // Create a document
            $session = $store->openSession();
            try {
                $user = new User();
                $user->setName("John");
                $user->setAge(27);

                $session->store($user);
                $session->saveChanges();
            } finally {
                $session->close();
            }

            // Query for a document with the Name property "John" and append it a time point
            $session = $store->openSession();
            try {
                 $baseTime = new DateTime("2020-05-17");

                $query = $session->query(User::class)
                    ->whereEquals("Name", "John");

                $result = $query->toList();

                for ($cnt = 0; $cnt < 120; $cnt++)
                {
                    $session->timeSeriesFor($result[0], "HeartRates")
                        ->append((clone $baseTime)->add(new DateInterval("P". $cnt . "D")), 68 + rand(0,19), "watches/fitbit");
                }

                $session->saveChanges();

            } finally {
                $session->close();
            }

            // Raw Query
            $session = $store->openSession();
            try {
                 $baseTime = DateUtils::today();

                // Raw query with a range selection
                $nonAggregatedRawQuery =
                    $session->advanced()->rawQuery(TimeSeriesRawResult::class, "
                        declare timeseries ts(jogger)
                        {
                            from jogger.HeartRates
                                between \$start and \$end
                        }
                        from Users as jog where Age < 30
                        select ts(jog)
                        ")
                    ->addParameter("start", new DateTime("2020-05-17"))
                    ->addParameter("end", new DateTime("2020-05-23"));

                $nonAggregatedRawQueryResult = $nonAggregatedRawQuery->toList();

            } finally {
                $session->close();
            }

      } finally {
          $store->close();
      }
    }

    // patching a document a single time-series entry
    // using PatchByQueryOperation
    public function patchTimeSerieshByQuery(): void
    {
        $store = $this->getDocumentStore();
        try {
            // Create a document
            $session = $store->openSession();
            try {
                $user1 = new User();
                $user1->setName("John");
                $session->store($user1);

                $user2 = new User();
                $user2->setName("Mia");
                $session->store($user2);

                $user3 = new User();
                $user3->setName("Emil");
                $session->store($user3);

                $session->saveChanges();
            } finally {
                $session->close();
            }

            $baseTime = DateUtils::today();

            # region TS_region-PatchByQueryOperation-Append-To-Multiple-Docs
            $indexQuery = new IndexQuery();
            // Define the query and the patching action that follows the 'update' keyword:
            $indexQuery->setQuery("from Users as u
                          update
                          {
                              timeseries(u, \$name)->append(\$time, \$values, \$tag)
                          }");

            // Provide values for the parameters in the script:
            $parameters = Parameters::fromArray([
                "name" => "HeartRates",
                "time" =>  (clone $baseTime)->add(new DateInterval("PT1M")),
                "values" => [ 59 ],
                "tag" => "watches/fitbit"
            ]);
            $indexQuery->setQueryParameters($parameters);


            // Define the patch operation:
            $patchByQueryOp = new PatchByQueryOperation($indexQuery);

            // Execute the operation:
            $store->operations()->send($patchByQueryOp);
            # endregion

            // Append time series to multiple documents
            $indexQuery = new IndexQuery();
            $indexQuery->setQuery("from Users as u update
                            {
                                timeseries(u, \$name)->append(\$time, \$values, \$tag)
                            }");

            $parameters = Parameters::fromArray([
                "name" => "ExerciseHeartRate",
                "time" =>   (clone $baseTime)->add(new DateInterval("PT1M")),
                "values" => [ 89 ],
                "tag" => "watches/fitbit2"
            ]);
            $indexQuery->setQueryParameters($parameters);

            $appendExerciseHeartRateOperation = new PatchByQueryOperation($indexQuery);
            $store->operations()->send($appendExerciseHeartRateOperation);

            // Get time-series data from all users
            $indexQuery = new IndexQuery();
            $indexQuery->setQuery("from users as u update
                            {
                                timeseries(u, \$name).get(\$from, \$to)
                            }");
            $parameters = Parameters::fromArray([
                "name" => "HeartRates",
                "from" => null,
                "to" => null
            ]);
            $indexQuery->setQueryParameters($parameters);
            $getOperation = new PatchByQueryOperation($indexQuery);

            $getResult = $store->operations()->send($getOperation);

            // Get and project chosen time-series data from all users
            $indexQuery = new IndexQuery();
            $indexQuery->setQuery("
                    declare function foo(doc){
                        var entries = timeseries(doc, \$name).get(\$from, \$to);
                        var differentTags = [];
                        for (var i = 0; i < entries.length; i++)
                        {
                            var e = entries[i];
                            if (e.Tag !== null)
                            {
                                if (!differentTags.includes(e.Tag))
                                {
                                    differentTags.push(e.Tag);
                                }
                            }
                        }
                        doc.NumberOfUniqueTagsInTS = differentTags.length;
                        return doc;
                    }

                    from Users as u
                    update
                    {
                        put(id(u), foo(u))
                    }");
            $indexQuery->setQueryParameters(Parameters::fromArray([
                    "name" => "ExerciseHeartRate",
                    "from" => null,
                    "to" => null
            ]));
            $getExerciseHeartRateOperation = new PatchByQueryOperation($indexQuery);

            $result = $store->operations()->send($getExerciseHeartRateOperation);

            # region TS_region-PatchByQueryOperation-Delete-From-Multiple-Docs
            $indexQuery = new IndexQuery();
            $indexQuery->setQuery("from Users as u
                          where u.Age < 30
                          update
                          {
                              timeseries(u, \$name).delete(\$from, \$to)
                          }");
            $indexQuery->setQueryParameters(Parameters::fromArray([
                "name" => "HeartRates",
                "from" => null,
                "to" => null
            ]));
            $deleteByQueryOp = new PatchByQueryOperation($indexQuery);

            // Execute the operation:
            // Time series "HeartRates" will be deleted for all users with age < 30
            $store->operations()->send($deleteByQueryOp);
            # endregion
      } finally {
          $store->close();
      }
    }

    // patching a document a single time-series entry
    // using PatchByQueryOperation
    public function patchTimeSerieshByQueryWithGet(): void
    {
        $store = $this->getDocumentStore();
        try {
            // Create a document
            $session = $store->openSession();
            try {
                $user1 = new User();
                $user1->setName("John");
                $session->store($user1);

                $user2 = new User();
                $user2->setName("Mia");
                $session->store($user2);

                $user3 = new User();
                $user3->setName("Emil");
                $session->store($user3);

                $user4 = new User();
                $user4->setName("shaya");
                $session->store($user4);

                $session->saveChanges();
            } finally {
                $session->close();
            }

             $baseTime = DateUtils::today();

            // get employees Id list
            $session = $store->openSession();
            try {
                $users = $session
                    ->query(User::class)
                    ->toList();
            } finally {
                $session->close();
            }

            $usersIdList = [];

            /** @var User $user */
            foreach ($users as $user) {
                $usersIdList[] = $user->getId();
            }

            // Append each employee a week (168 hours) of random HeartRate values
            $baseTime = new DateTime("2020-05-17");
            $session = $store->openSession();
            try {
                for ($user = 0; $user < count($usersIdList); $user++)
                {
                    for ($tse = 0; $tse < 168; $tse++)
                    {
                        $session->timeSeriesFor($usersIdList[$user], "ExerciseHeartRate")
                        ->append ((clone $baseTime)->add(new DateInterval("PT". $tse . "H")),
                                68 + rand(0,19),
                                "watches/fitbit" . $tse);
                    }
                }
                $session->saveChanges();
            } finally {
                $session->close();
            }

            # region TS_region-PatchByQueryOperation-Get
            $indexQuery = new IndexQuery();
            $indexQuery->setQuery("
                    declare function patchDocumentField(doc) {
                        var differentTags = [];
                        var entries = timeseries(doc, \$name).get(\$from, \$to);

                        for (var i = 0; i < entries.length; i++) {
                            var e = entries[i];

                            if (e.Tag !== null) {
                                if (!differentTags.includes(e.Tag)) {
                                    differentTags.push(e.Tag);
                                }
                            }
                        }

                        doc.NumberOfUniqueTagsInTS = differentTags.length;
                        return doc;
                    }

                    from Users as u
                    update {
                        put(id(u), patchDocumentField(u))
                    }");

            $indexQuery->setQueryParameters(Parameters::fromArray([
                "name" => "HeartRates",
                "from" => null,
                "to" => null
            ]));

            $patchNumberOfUniqueTags = new PatchByQueryOperation($indexQuery);

            // Execute the operation and Wait for completion:
            $result = $store->operations()->send($patchNumberOfUniqueTags);
            # endregion

            // Delete time-series from all users
            $indexQuery = new IndexQuery();
            $indexQuery->setQuery("from Users as u
                            update
                            {
                                timeseries(u, \$name).delete(\$from, \$to)
                            }");
            $indexQuery->setQueryParameters(Parameters::fromArray([
                "name" => "HeartRates",
                "from" => null,
                "to" => null
            ]));

            $removeOperation = new PatchByQueryOperation($indexQuery);

            $store->operations()->send($removeOperation);
      } finally {
          $store->close();
      }
    }

    // patch HeartRate TS to all employees
    // using PatchByQueryOperation
    // not that all employees get the same times-series entries.
    public function patchEmployeesHeartRateTS1(): void
    {
        $store = $this->getDocumentStore();
        try {
            // Create a document
            $session = $store->openSession();
            try {
                $employee1 = new Employee();
                $employee1->setFirstName("John");
                $session->store($employee1);

                $employee2 = new Employee();
                $employee2->setFirstName("Mia");
                $session->store($employee2);

                $employee3 = new Employee();
                $employee3->setFirstName("Emil");
                $session->store($employee3);

                $session->saveChanges();
            } finally {
                $session->close();
            }

            $baseTime = new DateTime("2020-05-17");

            // an array with a week of random hourly HeartRate values
            for ($i = 0; $i < 168; $i++) { // Loop for 168 hours
                $ts = new TimeSeriesEntry();
                $ts->setTimeStamp((clone $baseTime)->add(new DateInterval("PT" . $i . "H")));
                $ts->setValues([ 68 + rand(0, 19) ]);
                $ts->setTag("watches/fitbit");

                $valuesToAppend[] = $ts;
            }

            // Append time-series to all employees
            $appendHeartRate = new PatchByQueryOperation();
            $indexQuery = new IndexQuery();
            $indexQuery->setQuery("from Employees as e update
                            {
                                for(var i = 0; i < \$valuesToAppend.length; i++){
                                    timeseries(e, \$name)
                                    ->append(
                                        \$valuesToAppend[i].Timestamp,
                                        \$valuesToAppend[i].Values,
                                        \$valuesToAppend[i].Tag);
                                }
                            }");
            $indexQuery->setQueryParameters(Parameters::fromArray([
                "valuesToAppend" => $valuesToAppend,
                "name" => "HeartRates"
            ]));


            $store->operations()->send($appendHeartRate);
      } finally {
          $store->close();
      }
    }


    // Appending random time-series entries to all employees
    public function patchEmployeesHeartRateTS2(): void
    {
        $store = $this->getDocumentStore();
        try {
            // Create a document
            $session = $store->openSession();
            try {
                $employee1 = new Employee();
                $employee1->setFirstName("John");
                $session->store($employee1);

                $employee2 = new Employee();
                $employee2->setFirstName("Mia");
                $session->store($employee2);

                $employee3 = new Employee();
                $employee3->setFirstName("Emil");
                $session->store($employee3);

                $session->saveChanges();
            } finally {
                $session->close();
            }

            // get employees list
            $employees = [];
            $session = $store->openSession();
            try {
                $employees = $session
                    ->query(Employee::class)
                    ->toList();
            } finally {
                $session->close();
            }

            // Append each employee a week (168 hours) of random HeartRate values
            $baseTime = new DateTime("2020-05-17");
            $session = $store->openSession();
            try {
                /** @var Employee $employee */
                foreach($employees as $employee)
                {
                    for ($tse = 0; $tse < 168; $tse++)
                    {
                        $session->timeSeriesFor($employee->getId(), "HeartRates")
                            ->append(
                                (clone $baseTime)->add(new DateInterval("PT" . $tse . "H")),
                                68 + rand(0, 19),
                                "watches/fitbit"
                            );
                    }
                }
                $session->saveChanges();
            } finally {
                $session->close();
            }
      } finally {
          $store->close();
      }
    }

    // Query an index
    public function indexQuery(): void
    {
        $store = $this->getDocumentStore();
        try {
            // Create a document
            $session = $store->openSession();
            try {
                $employee1 = new Employee();
                $employee1->setFirstName("John");
                $session->store($employee1);

                $employee2 = new Employee();
                $employee2->setFirstName("Mia");
                $session->store($employee2);

                $employee3 = new Employee();
                $employee3->setFirstName("Emil");
                $session->store($employee3);

                $session->saveChanges();
            } finally {
                $session->close();
            }

            // get employees list
            $employees = [];
            $session = $store->openSession();
            try {
                $employees = $session
                    ->query(Employee::class)
                    ->toList();
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                $baseTime = new DateTime("2020-05-17");

                // Append each employee a week (168 hours) of random HeartRate values
                /** @var Employee $employee */
                foreach($employees as $employee)
                {
                    for ($tse = 0; $tse < 168; $tse++)
                    {
                        $session->timeSeriesFor($employee->getId(), "HeartRates")
                            ->append(
                                (clone $baseTime)->add(new DateInterval("PT" . $tse . "H")),
                                68 + rand(0, 19),
                                "watches/fitbit"
                            );
                    }
                }
                $session->saveChanges();
            } finally {
                $session->close();
            }

            $store->maintenance()->send(new StopIndexingOperation());

            $timeSeriesIndex = new TsIndex();
            $indexDefinition = $timeSeriesIndex->createIndexDefinition();

            $timeSeriesIndex->execute($store);

            $store->maintenance()->send(new StartIndexingOperation());

            //WaitForIndexing(store);
      } finally {
          $store->close();
      }
    }

# region sample_ts_index
class TsIndex_IndexEntry
{
    // The index-fields:
    private ?float $BPM = null;
    private ?DateTime $date = null;
    private ?string $tag = null;
    private ?string $employeeID = null;
    private ?string $employeeName = null;

    public function getBPM(): ?float
    {
        return $this->BPM;
    }

    public function setBPM(?float $BPM): void
    {
        $this->BPM = $BPM;
    }

    public function getDate(): ?DateTime
    {
        return $this->date;
    }

    public function setDate(?DateTime $date): void
    {
        $this->date = $date;
    }

    public function getTag(): ?string
    {
        return $this->tag;
    }

    public function setTag(?string $tag): void
    {
        $this->tag = $tag;
    }

    public function getEmployeeID(): ?string
    {
        return $this->employeeID;
    }

    public function setEmployeeID(?string $employeeID): void
    {
        $this->employeeID = $employeeID;
    }

    public function getEmployeeName(): ?string
    {
        return $this->employeeName;
    }

    public function setEmployeeName(?string $employeeName): void
    {
        $this->employeeName = $employeeName;
    }
}

class TsIndex extends AbstractTimeSeriesIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->map =  "
            from ts in timeSeries.Employees.HeartRates
            from entry in ts.Entries
            let employee = LoadDocument(ts.DocumentId, \"Employees\")
            select new 
            {
                BPM = entry.Values[0],
                Date = entry.Timestamp.Date,
                Tag = entry.Tag,
                EmployeeId = ts.DocumentId,
                EmployeeName = employee.FirstName + ' ' + employee.LastName
            }
        ";
    }
}
# endregion

# region DefineCustomFunctions_ModifiedTimeSeriesEntry
class ModifiedTimeSeriesEntry
{
    private ?DateTime $timestamp = null;
    private ?float $value = null;
    private ?string $tag = null;

    // ... getters and setters ...
}
# endregion

class HeartRate
{
    #[TimeSeriesValue(0)]
    private ?float $heartRateMeasure = null;

    public function getHeartRateMeasure(): ?float
    {
        return $this->heartRateMeasure;
    }

    public function setHeartRateMeasure(?float $heartRateMeasure): void
    {
        $this->heartRateMeasure = $heartRateMeasure;
    }
}

# region Custom-Data-Type-1
class StockPrice
{
    #[TimeSeriesValue(0)]
    private ?float $open = null;

    #[TimeSeriesValue(1)]
    private ?float $close = null;

    #[TimeSeriesValue(2)]
    private ?float $high = null;

    #[TimeSeriesValue(3)]
    private ?float $low = null;

    #[TimeSeriesValue(4)]
    private ?float $volume = null;

    public function getOpen(): ?float
    {
        return $this->open;
    }

    public function setOpen(?float $open): void
    {
        $this->open = $open;
    }

    public function getClose(): ?float
    {
        return $this->close;
    }

    public function setClose(?float $close): void
    {
        $this->close = $close;
    }

    public function getHigh(): ?float
    {
        return $this->high;
    }

    public function setHigh(?float $high): void
    {
        $this->high = $high;
    }

    public function getLow(): ?float
    {
        return $this->low;
    }

    public function setLow(?float $low): void
    {
        $this->low = $low;
    }

    public function getVolume(): ?float
    {
        return $this->volume;
    }

    public function setVolume(?float $volume): void
    {
        $this->volume = $volume;
    }
}
# endregion

# region Custom-Data-Type-2
class RoutePoint
{
    // The Latitude and Longitude properties will contain the time series entry values.
    // The names for these values will be "Latitude" and "Longitude" respectively.
    #[TimeSeriesValue(0)]
    private ?float $latitude = null;
    #[TimeSeriesValue(1)]
    private ?float $longitude = null;

    public function __construct(?float $latitude, ?float $longitude)
    {
        $this->latitude = $latitude;
        $this->longitude = $longitude;
    }

    public function getLatitude(): ?float
    {
        return $this->latitude;
    }

    public function setLatitude(?float $latitude): void
    {
        $this->latitude = $latitude;
    }

    public function getLongitude(): ?float
    {
        return $this->longitude;
    }

    public function setLongitude(?float $longitude): void
    {
        $this->longitude = $longitude;
    }
}
# endregion

class User
{
    private ?string $id = null;
    private ?string $name = null;
    private ?string $lastName = null;
    private ?string $addressId = null;
    private ?int $count = null;
    private ?int $age = null;

    public function getId(): ?string
    {
        return $this->id;
    }

    public function setId(?string $id): void
    {
        $this->id = $id;
    }

    public function getName(): ?string
    {
        return $this->name;
    }

    public function setName(?string $name): void
    {
        $this->name = $name;
    }

    public function getLastName(): ?string
    {
        return $this->lastName;
    }

    public function setLastName(?string $lastName): void
    {
        $this->lastName = $lastName;
    }

    public function getAddressId(): ?string
    {
        return $this->addressId;
    }

    public function setAddressId(?string $addressId): void
    {
        $this->addressId = $addressId;
    }

    public function getCount(): ?int
    {
        return $this->count;
    }

    public function setCount(?int $count): void
    {
        $this->count = $count;
    }

    public function getAge(): ?int
    {
        return $this->age;
    }

    public function setAge(?int $age): void
    {
        $this->age = $age;
    }
}

class Person
{
    private ?string $id = null;
    private ?string $name = null;
    private ?string $lastName = null;
    private ?int $age = null;
    private ?string $worksAt = null;

    public function getId(): ?string
    {
        return $this->id;
    }

    public function setId(?string $id): void
    {
        $this->id = $id;
    }

    public function getName(): ?string
    {
        return $this->name;
    }

    public function setName(?string $name): void
    {
        $this->name = $name;
    }

    public function getLastName(): ?string
    {
        return $this->lastName;
    }

    public function setLastName(?string $lastName): void
    {
        $this->lastName = $lastName;
    }

    public function getAge(): ?int
    {
        return $this->age;
    }

    public function setAge(?int $age): void
    {
        $this->age = $age;
    }

    public function getWorksAt(): ?string
    {
        return $this->worksAt;
    }

    public function setWorksAt(?string $worksAt): void
    {
        $this->worksAt = $worksAt;
    }
}

class Company
{
    private ?string $id = null;
    private ?string $externalId = null;
    private ?string $name = null;
    private ?Contact $contact = null;
    private ?Address $address = null;
    private ?string $phone = null;
    private ?string $fax = null;

    public function getId(): ?string
    {
        return $this->id;
    }

    public function setId(?string $id): void
    {
        $this->id = $id;
    }

    public function getExternalId(): ?string
    {
        return $this->externalId;
    }

    public function setExternalId(?string $externalId): void
    {
        $this->externalId = $externalId;
    }

    public function getName(): ?string
    {
        return $this->name;
    }

    public function setName(?string $name): void
    {
        $this->name = $name;
    }

    public function getContact(): ?Contact
    {
        return $this->contact;
    }

    public function setContact(?Contact $contact): void
    {
        $this->contact = $contact;
    }

    public function getAddress(): ?Address
    {
        return $this->address;
    }

    public function setAddress(?Address $address): void
    {
        $this->address = $address;
    }

    public function getPhone(): ?string
    {
        return $this->phone;
    }

    public function setPhone(?string $phone): void
    {
        $this->phone = $phone;
    }

    public function getFax(): ?string
    {
        return $this->fax;
    }

    public function setFax(?string $fax): void
    {
        $this->fax = $fax;
    }
}

class Address
{
    private ?string $line1 = null;
    private ?string $line2 = null;
    private ?string $city = null;
    private ?string $region = null;
    private ?string $postalCode = null;
    private ?string $country = null;

    public function getLine1(): ?string
    {
        return $this->line1;
    }

    public function setLine1(?string $line1): void
    {
        $this->line1 = $line1;
    }

    public function getLine2(): ?string
    {
        return $this->line2;
    }

    public function setLine2(?string $line2): void
    {
        $this->line2 = $line2;
    }

    public function getCity(): ?string
    {
        return $this->city;
    }

    public function setCity(?string $city): void
    {
        $this->city = $city;
    }

    public function getRegion(): ?string
    {
        return $this->region;
    }

    public function setRegion(?string $region): void
    {
        $this->region = $region;
    }

    public function getPostalCode(): ?string
    {
        return $this->postalCode;
    }

    public function setPostalCode(?string $postalCode): void
    {
        $this->postalCode = $postalCode;
    }

    public function getCountry(): ?string
    {
        return $this->country;
    }

    public function setCountry(?string $country): void
    {
        $this->country = $country;
    }
}

class Contact
{
    private ?string $name = null;
    private ?string $title = null;

    public function getName(): ?string
    {
        return $this->name;
    }

    public function setName(?string $name): void
    {
        $this->name = $name;
    }

    public function getTitle(): ?string
    {
        return $this->title;
    }

    public function setTitle(?string $title): void
    {
        $this->title = $title;
    }
}

class Employee
{
    private ?string $id = null;
    private ?string $lastName = null;
    private ?string $firstName = null;
    private ?string $title = null;
    private ?Address $address = null;
    private ?DateTime $hiredAt = null;
    private ?DateTime $birthday = null;
    private ?string $homePhone = null;
    private ?string $extension = null;
    private ?string $reportsTo = null;
    private ?StringArray $notes = null;
    private ?StringArray $territories = null;

    public function getId(): ?string
    {
        return $this->id;
    }

    public function setId(?string $id): void
    {
        $this->id = $id;
    }

    public function getLastName(): ?string
    {
        return $this->lastName;
    }

    public function setLastName(?string $lastName): void
    {
        $this->lastName = $lastName;
    }

    public function getFirstName(): ?string
    {
        return $this->firstName;
    }

    public function setFirstName(?string $firstName): void
    {
        $this->firstName = $firstName;
    }

    public function getTitle(): ?string
    {
        return $this->title;
    }

    public function setTitle(?string $title): void
    {
        $this->title = $title;
    }

    public function getAddress(): ?Address
    {
        return $this->address;
    }

    public function setAddress(?Address $address): void
    {
        $this->address = $address;
    }

    public function getHiredAt(): ?DateTime
    {
        return $this->hiredAt;
    }

    public function setHiredAt(?DateTime $hiredAt): void
    {
        $this->hiredAt = $hiredAt;
    }

    public function getBirthday(): ?DateTime
    {
        return $this->birthday;
    }

    public function setBirthday(?DateTime $birthday): void
    {
        $this->birthday = $birthday;
    }

    public function getHomePhone(): ?string
    {
        return $this->homePhone;
    }

    public function setHomePhone(?string $homePhone): void
    {
        $this->homePhone = $homePhone;
    }

    public function getExtension(): ?string
    {
        return $this->extension;
    }

    public function setExtension(?string $extension): void
    {
        $this->extension = $extension;
    }

    public function getReportsTo(): ?string
    {
        return $this->reportsTo;
    }

    public function setReportsTo(?string $reportsTo): void
    {
        $this->reportsTo = $reportsTo;
    }

    public function getNotes(): ?StringArray
    {
        return $this->notes;
    }

    public function setNotes(?StringArray $notes): void
    {
        $this->notes = $notes;
    }

    public function getTerritories(): ?StringArray
    {
        return $this->territories;
    }

    public function setTerritories(?StringArray $territories): void
    {
        $this->territories = $territories;
    }
}

# region TimeSeriesEntry-Definition
class TimeSeriesEntry
{
    private ?DateTime $timestamp = null;
    private ?string $tag = null;
    private ?array $values = null;
    private bool $rollup = false;

    private ?array $nodeValues = null; // Map<String, Double[]>


    //..
}
# endregion

interface Foo
{
    # region TimeSeriesFor-Append-definition
    // Append an entry with a single value or multiple values
    function append(DateTime $timestamp, float|array $values, ?string $tag = null): void;
    # endregion

    # region TimeSeriesFor-Delete-definition
    // Delete a time-series entries
    function delete(?DateTimeInterface $from = null, ?DateTimeInterface $to = null): void;
    # endregion

    # region TimeSeriesFor-Get-definition
    public function get(?DateTimeInterface $from = null, ?DateTimeInterface $to = null, ?Closure $includes = null, int $start = 0, int $pageSize = PHP_INT_MAX): ?TimeSeriesEntryArray;
    # endregion
}

interface SessionDocumentTypedTimeSeriesInterface2
{
    # region TimeSeriesFor-Get-Named-Values
    // The stongly-typed API is used, to address time series values by name.

    /**
     * Return the time series values for the provided range
     *
     * @param DateTimeInterface|null $from
     * @param DateTimeInterface|null $to
     * @param int $start
     * @param int $pageSize
     * @return TypedTimeSeriesEntryArray|null
     */
    public function get(?DateTimeInterface $from = null, ?DateTimeInterface $to = null, int $start = 0, int $pageSize = PHP_INT_MAX): ?TypedTimeSeriesEntryArray;
    # endregion
}

interface DocumentSessionInterfaceForInclude
{
    # region Load-definition
    /**
     *  - load(string $className, string $id, Closure $includes) ?Object;
     *  - load(string $className, StringArray $ids, Closure $includes): ObjectArray;
     *  - load(string $className, array $ids, Closure $includes): ObjectArray;
     */
    public function load(?string $className, ...$params): mixed;
    # endregion
}

interface TimeSeriesIncludeBuilderInterface
{
    # region IncludeTimeSeries-definition
    public function includeTimeSeries(?string $name, ?DateTimeInterface $from = null, ?DateTimeInterface $to = null): IncludeBuilderInterface;
    # endregion
}

interface RawQueryDefinitionInterface
{
    # region RawQuery-definition
    public function rawQuery(string $className, string $query): RawDocumentQueryInterface;
    # endregion
}

/*
# region PatchCommandData-definition
new PatchCommandData(?string $id, ?string $changeVector, ?PatchRequest $patch, ?PatchRequest $patchIfMissing = null);
# endregion
*/

/*
# region PatchRequest-definition
class PatchRequest
{
    // The patching script
    private ?string $script = null;

    // Values for the parameters used by the patching script
    private ?ObjectMap $values = null;

    // ... getters and setters ...
}
# endregion
*/

/*
# region TimeSeriesBatchOperation-definition
new TimeSeriesBatchOperation(?string $documentId, ?TimeSeriesOperation $operation);
# endregion
*/

/*
# region GetTimeSeriesOperation-Definition
new GetTimeSeriesOperation(?string $docId, ?string $timeseries, ?DateTimeInterface $from = null, ?DateTimeInterface $to = null, int $start = 0, int $pageSize = PHP_INT_MAX, ?Closure $includes = null, bool $returnFullResults = false);
# endregion
*/

/*
# region TimeSeriesRangeResult-class
class TimeSeriesRangeResult
{
    // Timestamp of first entry returned
    private ?DateTimeInterface $from = null;

    // Timestamp of last entry returned
    private ?DateTimeInterface $to = null;

    // The resulting entries
    // Will be empty if requesting an entries range that does Not exist
    private ?TimeSeriesEntryArray $entries = null;

    // The number of entries returned
    // Will be undefined if not all entries of this time series were returned
    private ?int $totalResults = null;

    private ?array $includes = null;

    public function __construct(?array $data = null) {}
}
# endregion
*/

/*
# region GetMultipleTimeSeriesOperation-Definition
new GetMultipleTimeSeriesOperation(?string $docId, null|TimeSeriesRangeList|array $ranges, int $start = 0, int $pageSize = PhpClient::INT_MAX_VALUE, ?Closure $includes = null);
# endregion
*/

/*
# region TimeSeriesRange-class
class TimeSeriesRange extends AbstractTimeSeriesRange
{
    private ?string $name = '';         // Name of time series
    private ?DateTimeInterface $from;   // Get time series entries starting from this timestamp (inclusive).
    private ?DateTimeInterface $to;     // Get time series entries ending at this timestamp (inclusive).

    // ... getters and setters ...
}
# endregion
*/

/*
# region TimeSeriesDetails-class
class TimeSeriesDetails
{
    // The document ID
    private ?string $id = null;

    // Dictionary of time series name to the time series results
    private TimeSeriesRangeResultListArray $values;

    // ... getters and setters ...
}
# endregion
*/

/*
# region PatchOperation-Definition
new PatchOperation(
    ?string $id,
    ?string $changeVector,
    ?PatchRequest $patch,
    ?PatchRequest $patchIfMissing = null,
    bool $skipPatchIfChangeVectorMismatch = false
);
# endregion
*/

/*
# region PatchByQueryOperation-Definition-1
new PatchByQueryOperation(null|IndexQuery|string $queryToUpdate, ?QueryOperationOptions $options = null)
# endregion
*/

interface TimeSeriesBulkInsert
{
    # region Append-Operation-Definition
    // Append a single value
    public function append(DateTimeInterface $timestamp, float|array $values, ?string $tag = null): void;
    # endregion
}

interface TimeSeriesOperations
{
    # region Register-Definition
    public function register(
        string                        $collectionClassOrCollection,
        string                        $timeSeriesEntryClassOrName,
        null|string|StringArray|array $nameOrValuesName = null
    ): void;
    # endregion
}

/*
# region Query-definition
public function query(string $className, Query|null|string $collectionOrIndexName = null): DocumentQueryInterface;
# endregion
*/

//Watch class for TS Document Query documentation
# region TS_DocQuery_class
class Monitor
{
    private ?float $accuracy = null;

    public function getAccuracy(): ?float
    {
        return $this->accuracy;
    }

    public function setAccuracy(?float $accuracy): void
    {
        $this->accuracy = $accuracy;
    }
}
# endregion
