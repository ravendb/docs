<?php

namespace RavenDB\Samples\ClientApi\Session\Querying;

use RavenDB\Constants\DocumentsMetadata;
use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Session\DocumentQueryInterface;
use RavenDB\Documents\Session\GroupByField;
use RavenDB\Documents\Session\OrderingType;
use RavenDB\Samples\Infrastructure\Orders\Product;

interface FooInterface
{
    # region syntax
    /**
     * Usage:
     *   - orderBy("lastName"); // same as call: orderBy("lastName", OrderingType::string())
     *   - orderBy("lastName", OrderingType::string());
     *
     *   - orderBy("units_in_stock", "MySorter");
     *     // Results will be sorted by the 'units_in_stock' value according to the logic from 'MySorter' class
     */
    function orderBy(string $field, $sorterNameOrOrdering = null): DocumentQueryInterface;

    /**
     * Usage:
     *   - orderByDescending("lastName"); // same as call: orderBy("lastName", OrderingType::string())
     *   - orderByDescending("lastName", OrderingType::string());
     *
     *   - orderByDescending("units_in_stock", "MySorter");
     *     // Results will be sorted by the 'units_in_stock' value according to the logic from 'MySorter' class
     */
    function orderByDescending(string $field, $sorterNameOrOrdering = null): DocumentQueryInterface;
    # endregion
}

class SortQueryResults
{
    public function examples(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                // Order by document field
                // =======================

                # region sort_1
                /** @var array<Product> $products */
                $products = $session
                    // Make a dynamic query on the Products collection
                    ->query(Product::class)
                    // Apply filtering (optional)
                    ->whereGreaterThan("UnitsInStock", 10)
                    // Call 'OrderBy', pass the document-field by which to order the results
                    ->orderBy("UnitsInStock")
                    ->toList();

                // Results will be sorted by the 'UnitsInStock' value in ascending order,
                // with smaller values listed first.
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region sort_3
                /** @var array<Product> $products */
                $products = $session->advanced()
                    // Make a DocumentQuery on the Products collection
                    ->documentQuery(Product::class)
                    // Apply filtering (optional)
                    ->whereGreaterThan("UnitsInStock", 10)
                    // Call 'OrderBy', pass the document-field by which to order the results
                    ->orderBy("UnitsInStock")
                    ->toList();

                // Results will be sorted by the 'UnitsInStock' value in ascending order,
                // with smaller values listed first.
                # endregion
            } finally {
                $session->close();
            }

            // Order by score
            // ==============

            $session = $store->openSession();
            try {
                # region sort_4
                /** @var array<Product> $products */
                $products = $session
                    ->query(Product::class)
                    // Apply filtering
                    ->whereLessThan("UnitsInStock", 5)
                    ->orElse()
                    ->whereEquals("Discontinued", true)
                    // Call 'orderByScore'
                    ->orderByScore()
                    ->toList();

                // Results will be sorted by the score value
                // with best matching documents (higher score values) listed first.
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region sort_6
                /** @var array<Product> $products */
                $products = $session->advanced()
                    ->documentQuery(Product::class)
                    // Apply filtering
                    ->whereLessThan("UnitsInStock", 5)
                    ->orElse()
                    ->whereEquals("Discontinued", true)
                    // Call 'orderByScore'
                    ->orderByScore()
                    ->toList();

                // Results will be sorted by the score value
                // with best matching documents (higher score values) listed first.
                # endregion
            } finally {
                $session->close();
            }

            // Order by random
            // ===============

            $session = $store->openSession();
            try {
                # region sort_7
                /** @var array<Product> $products */
                $products = $session->query(Product::class)
                    ->whereGreaterThan("UnitsInStock", 10)
                    // Call 'randomOrdering'
                    ->randomOrdering()
                    // An optional seed can be passed, e.g.:
                    // ->randomOrdering('someSeed')
                    ->toList();

                // Results will be randomly ordered.
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region sort_9
                /** @var array<Product> $products */
                $products = $session->advanced()
                    ->documentQuery(Product::class)
                    ->whereGreaterThan("UnitsInStock", 10)
                    // Call 'randomOrdering'
                    ->randomOrdering()
                    // An optional seed can be passed, e.g.:
                    // ->randomOrdering('someSeed')
                    ->toList();

                // Results will be randomly ordered.
                # endregion
            } finally {
                $session->close();
            }

            // Order by Count
            // ==============

            $session = $store->openSession();
            try {
                # region sort_10
                $numberOfProductsPerCategory = $session
                    ->query(Product::class)
                    // Make an aggregation query
                    ->groupBy("Category")
                    ->selectKey("Category")
                    // Count the number of product documents per category
                    ->selectCount()
                    // Order by the Count value
                    // Here you need to specify the ordering type explicitly
                    ->orderBy("Count", OrderingType::long())
                    ->toList();

                // Results will contain the number of Product documents per category
                // ordered by that count in ascending order.
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region sort_12
                $numberOfProductsPerCategory = $session->advanced()
                    ->documentQuery(Product::class)
                    // Group by Category
                    ->groupBy("Category")
                    ->selectKey("Category")
                    // Count the number of product documents per category
                    ->selectCount()
                    // Order by the Count value
                    // Here you need to specify the ordering type explicitly
                    ->orderBy("Count", OrderingType::long())
                    ->toList();

                // Results will contain the number of Product documents per category
                // ordered by that count in ascending order.
                # endregion
            } finally {
                $session->close();
            }

            // Order by Sum
            // ============

            $session = $store->openSession();
            try {
                # region sort_13
                $numberOfUnitsInStockPerCategory = $session
                    ->query(Product::class)
                    // Make an aggregation query
                    // Group by Category
                    ->groupBy("Category")
                    // Order by the Sum value
                    ->selectKey("Category")
                    ->selectSum(new GroupByField("UnitsInStock", "Sum"))
                    ->orderBy("Sum")
                    ->toList();

                // Results will contain the total number of units in stock per category
                // ordered by that number in ascending order.
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region sort_15
                $numberOfUnitsInStockPerCategory = $session->advanced()
                    ->documentQuery(Product::class)
                    // Group by Category
                    ->groupBy("Category")
                    ->selectKey("Category")
                    // Sum the number of units in stock per category
                    ->selectSum(new GroupByField("UnitsInStock", "Sum"))
                    // Order by the Sum value
                    // Here you need to specify the ordering type explicitly
                    ->orderBy("Sum", OrderingType::long())
                    ->toList();

                // Results will contain the total number of units in stock per category
                // ordered by that number in ascending order.
                # endregion
            } finally {
                $session->close();
            }

            // Force ordering type
            // ===================

            $session = $store->openSession();
            try {
                # region sort_16
                /** @var array<Product> $products */
                $products = $session
                    ->query(Product::class)
                    // Call 'OrderBy', order by field 'QuantityPerUnit'
                    // Pass a second param, requesting to order the text alphanumerically
                    ->orderBy("QuantityPerUnit", OrderingType::alphaNumeric())
                    ->toList();
                # endregion

                # region sort_16_results
                // Running the above query on the NorthWind sample data,
                // would produce the following order for the QuantityPerUnit field:
                // ================================================================

                // "1 kg pkg."
                // "1k pkg."
                // "2 kg box."
                // "4 - 450 g glasses"
                // "5 kg pkg."
                // ...

                // While running with the default Lexicographical ordering would have produced:
                // ============================================================================

                // "1 kg pkg."
                // "10 - 200 g glasses"
                // "10 - 4 oz boxes"
                // "10 - 500 g pkgs."
                // "10 - 500 g pkgs."
                // ...
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region sort_18
                /** @var array<Product> $products */
                $products = $session->advanced()
                    ->documentQuery(Product::class)
                    // Call 'OrderBy', order by field 'QuantityPerUnit'
                    // Pass a second param, requesting to order the text alphanumerically
                    ->orderBy("QuantityPerUnit", OrderingType::alphaNumeric())
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            // Chain ordering
            // ==============

            $session = $store->openSession();
            try {
                # region sort_19
                /** @var array<Product> $products */
                $products = $session
                    ->query(Product::class)
                    ->whereGreaterThan("UnitsInStock", 10)
                    // Apply the primary sort by 'UnitsInStock'
                    ->orderByDescending("UnitsInStock")
                    // Apply a secondary sort by the score (for products with the same # of units in stock)
                    ->orderByScore()
                    // Apply another sort by 'Name' (for products with same # of units in stock and same score)
                    ->orderBy("Name")
                    ->toList();

                // Results will be sorted by the 'UnitsInStock' value (descending),
                // then by score,
                // and then by 'Name' (ascending).
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region sort_21
                /** @var array<Product> $products */
                $products = $session->advanced()
                    ->documentQuery(Product::class)
                    ->whereGreaterThan("UnitsInStock", 10)
                    // Apply the primary sort by 'UnitsInStock'
                    ->orderByDescending("UnitsInStock")
                    // Apply a secondary sort by the score
                    ->orderByScore()
                    // Apply another sort by 'Name'
                    ->orderBy("Name")
                    ->toList();

                // Results will be sorted by the 'UnitsInStock' value (descending),
                // then by score,
                // and then by 'Name' (ascending).
                # endregion
            } finally {
                $session->close();
            }

            // Custom sorters
            // ==============

            $session = $store->openSession();
            try {
                # region sort_22
                /** @var array<Product> $products */
                $products = $session
                    ->query(Product::class)
                    ->whereGreaterThan("UnitsInStock", 10)
                    // Order by field 'UnitsInStock', pass the name of your custom sorter class
                    ->orderBy("UnitsInStock", "MySorter")
                    ->toList();

                // Results will be sorted by the 'UnitsInStock' value
                // according to the logic from 'MySorter' class
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region sort_24
                /** @var array<Product> $products */
                $products = $session->advanced()
                    ->documentQuery(Product::class)
                    ->whereGreaterThan("UnitsInStock", 10)
                    // Order by field 'UnitsInStock', pass the name of your custom sorter class
                    ->orderBy("UnitsInStock", "MySorter")
                    ->toList();

                // Results will be sorted by the 'UnitsInStock' value
                // according to the logic from 'MySorter' class
                # endregion
            } finally {
                $session->close();
            }

            // Get score from metadata
            // =======================

            $session = $store->openSession();
            try {
                # region get_score_from_metadata
                // Make a query:
                // =============

                $employees = $session
                    ->query(Employee::class)
                    ->search("Notes", "English")
                    ->search("Notes", "Italian")
                    ->boost(10)
                    ->toList();

                // Get the score:
                // ==============

                // Call 'GetMetadataFor', pass an entity from the resulting employees list
                $metadata = $session->advanced()->getMetadataFor($employees[0]);

                // Score is available in the 'INDEX_SCORE' metadata property
                $score = $metadata[DocumentsMetadata::INDEX_SCORE];
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }
}
