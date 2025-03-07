<?php

namespace RavenDB\Samples\DocumentExtensions\Counters;

use DateTime;
use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Session\SessionDocumentCountersInterface;
use RavenDB\Documents\Smuggler\DatabaseItemType;
use RavenDB\Documents\Smuggler\DatabaseItemTypeSet;
use RavenDB\Type\StringList;

interface IFoo
{
    # region CountersFor-definition
    public function countersFor(string|object $idOrEntity): SessionDocumentCountersInterface;
    # endregion
    
    # region Increment-definition
    public function increment(?string $counter, int $delta = 1): void;
    # endregion

    # region Delete-definition
    public function delete(?string $counter): void;
    # endregion

    # region Get-definition
    public function get(string|StringList|array $counters): null|int|array;
    # endregion

    # region GetAll-definition
    public function getAll(): array;
    # endregion
}

class Counters
{
    public function examples(): void
    {
        $docStore = new DocumentStore( ["http://localhost:8080"],  "products");
        $docStore->initialize();

        try {
            # region counters_region_CountersFor_with_document_load
            // Use CountersFor by passing it a document object

            // 1. Open a session
            $session = $docStore->openSession();
            try {
                // 2. Use the session to load a document.
                $document = $session->load(Product::class, "products/1-C");

                // 3. Create an instance of `countersFor`
                // Pass the document object returned from session.load as a param.
                $documentCounters = $session->countersFor($document);

                // 4. Use `countersFor` methods to manage the product document's Counters
                $documentCounters->delete("ProductLikes"); // delete the "ProductLikes" Counter
                $documentCounters->increment("ProductModified", 15); // Add 15 to Counter "ProductModified"
                $counter = $documentCounters->get("DaysLeftForSale"); // Get value of "DaysLeftForSale"

                // 5. Execute all changes by calling SaveChanges
                $session->saveChanges();
            } finally {
                $session->close();
            }
            # endregion

            # region counters_region_CountersFor_without_document_load
            // Use countersFor without loading a document

            // 1. Open a session
            $session = $docStore->openSession();
            try {
                // 2. pass an explicit document ID to the CountersFor constructor
                $documentCounters = $session->countersFor("products/1-C");

                // 3. Use `countersFor` methods to manage the product document's Counters
                $documentCounters->delete("ProductLikes"); // delete the "ProductLikes" Counter
                $documentCounters->increment("ProductModified", 15); // Add 15 to Counter "ProductModified"
                $counter = $documentCounters->get("DaysLeftForSale"); // Get "DaysLeftForSale"'s value

                // 4. Execute all changes by calling saveChanges
                $session->saveChanges();
            } finally {
                $session->close();
            }
            # endregion

            // remove a counter from a document
            # region counters_region_Delete
            // 1. Open a session
            $session = $docStore->openSession();
            try {
                // 2. pass countersFor's constructor a document ID
                $documentCounters = $session->countersFor("products/1-C");

                // 3. delete the "ProductLikes" Counter
                $documentCounters->delete("ProductLikes");

                // 4. The 'delete' is executed upon calling SaveChanges
                $session->saveChanges();
            } finally {
                $session->close();
            }
            # endregion

            // increment a counter's value
            # region counters_region_Increment
            // Open a session
            $session = $docStore->openSession();
            try {
                // Pass the countersFor constructor a document ID
                $documentCounters = $session->countersFor("products/1-A");

                // Use `countersFor.increment`:
                // ============================

                // Increase "ProductLikes" by 1, or create it if doesn't exist with a value of 1
                $documentCounters->increment("ProductLikes");

                // Increase "ProductPageViews" by 15, or create it if doesn't exist with a value of 15
                $documentCounters->increment("ProductPageViews", 15);

                // Decrease "DaysLeftForSale" by 10, or create it if doesn't exist with a value of -10
                $documentCounters->increment("DaysLeftForSale", -10);

                // Execute all changes by calling SaveChanges
                $session->saveChanges();
            } finally {
                $session->close();
            }
            # endregion

            // get a counter's value by the counter's name
            # region counters_region_Get
            // 1. Open a session
            $session = $docStore->openSession();
            try {
                // 2. pass the countersFor constructor a document ID
                $documentCounters = $session->countersFor("products/1-C");

                // 3. Use `countersFor.get` to retrieve a counter's value
                $daysLeft = $documentCounters->get("DaysLeftForSale");

                echo "Days Left For Sale: " . $daysLeft . PHP_EOL;
            } finally {
                $session->close();
            }
            # endregion

            // GetAll
            # region counters_region_GetAll
            // 1. Open a session
            $session = $docStore->openSession();
            try {
                // 2. Pass the countersFor constructor a document ID
                $documentCounters = $session->countersFor("products/1-C");

                // 3. Use getAll to retrieve all of the document's Counters' names and values
                $counters = $documentCounters->getAll();

                // list counters' names and values

                foreach ($counters as $counterKey => $counterValue)
                {
                    echo "counter name: " . $counterKey . ", counter value: " . $counterValue;
                }
            } finally {
                $session->close();
            }
            # endregion

            //Query a collection for documents with a Counter named "ProductLikes"
            $session = $docStore->openSession();
            try {
                # region counters_region_load_include1
                //include single Counters
                $productPage = $session->load(Product::class, "products/1-C", function($includeBuilder) {
                    return $includeBuilder
                            ->includeCounter("ProductLikes")
                            ->includeCounter("ProductDislikes")
                            ->includeCounter("ProductDownloads");
                });
                # endregion
            } finally {
                $session->close();
            }

            $session = $docStore->openSession();
            try {
                # region counters_region_load_include2
                //include multiple Counters
                //note that you can combine the inclusion of Counters and documents.
                $productPage = $session->load(Product::class, "products/1-C", function($includeBuilder) {
                    return $includeBuilder
                        ->includeDocuments("products/1-C")
                        ->includeCounters(["ProductLikes", "ProductDislikes"]);
                });
                # endregion
            } finally {
                $session->close();
            }

            $session = $docStore->openSession();
            try {
                # region counters_region_query_include_single_Counter
                //include a single Counter
                $query = $session->query(Product::class)
                        ->include(function($includeBuilder) {
                            return $includeBuilder->includeCounter("ProductLikes");
                        });
                # endregion
            } finally {
                $session->close();
            }

            $session = $docStore->openSession();
            try {
                # region counters_region_query_include_multiple_Counters
                //include multiple Counters
                $query = $session->query(Product::class)
                        ->include(function($includeBuilder){
                            return $includeBuilder->includeCounters(["ProductLikes", "ProductDownloads"]);
                        });
                # endregion
            } finally {
                $session->close();
            }

            $session = $docStore->openSession();
            try {
                # region counters_region_rawqueries_counter
                //Various RQL expressions sent to the server using counter()
                //Returned Counter value is accumulated
                $rawQuery1 = $session->advanced()
                    ->rawQuery(CounterResult::class, "from products as p select counter(p, \"ProductLikes\")")
                    ->toList();

                $rawQuery2 = $session->advanced()
                    ->rawQuery(CounterResult::class,"from products select counter(\"ProductLikes\") as ProductLikesCount")
                    ->toList();

                $rawQuery3 = $session->advanced()
                    ->rawQuery(CounterResult::class,"from products where PricePerUnit > 50 select Name, counter(\"ProductLikes\")")
                    ->toList();
                # endregion

                # region counters_region_rawqueries_counterRaw
                //An RQL expression sent to the server using counterRaw()
                //Returned Counter value is distributed
                $query = $session->advanced()
                    ->rawQuery(CounterResultRaw::class, "from users as u select counterRaw(u, \"downloads\")")
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $docStore->close();
        }
    }


    public function sample(): void
        {
            $store = new DocumentStore();
            try {
                $exportOptions = new DatabaseSmugglerExportOptions();
                # region smuggler_options
                $set = new DatabaseItemTypeSet();
                $set->append(DatabaseItemType::indexes());
                $set->append(DatabaseItemType::documents());
                $set->append(DatabaseItemType::counters());
                $exportOptions->setOperateOnTypes($set);
                # endregion
            } finally {
                $store->close();
            }
        }
}

class CounterResult
{
    private ?int $productPrice = null;
    private ?int $productLikes = null;
    private ?string $productSection = null;

    // ... getters and setters ...
}

class CounterResultRaw
{
    private ?array $downloads = null;

    // ... getters and setters ...
}

class Product
{
    private ?string $id = null;
    private ?string $customerId = null;
    private ?DateTime $started = null;
    private ?DateTime $ended = null;
    private ?float $pricePerUnit = null;
    private ?string $issue = null;
    private ?int $votes = null;

    // ... getters and setters ...
}

# region counters_region_CounterItem
// The value given to a Counter by each node, is placed in a CounterItem object.
class CounterItem {
    private ?string $name = null;
    private ?string $docId = null;
    private ?string $changeVector = null;
    private ?int $value = null;

    // ... getters and setters ...
}
# endregion
