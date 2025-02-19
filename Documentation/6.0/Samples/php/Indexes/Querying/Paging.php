<?php

namespace RavenDB\Samples\Indexes\Querying;

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Indexes\AbstractIndexCreationTask;
use RavenDB\Documents\Session\QueryStatistics;
use RavenDB\Samples\Infrastructure\Orders\Product;

class Paging
{
    public function Examples(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region paging_0_1
                // A simple query without paging:
                // ==============================

                /** @var array<Product> $allResults */
                $allResults = $session
                    ->query(Products_ByUnitsInStock_IndexEntry::class, Products_ByUnitsInStock::class)
                    ->whereGreaterThan("UnitsInStock", 10)
                    ->ofType(Product::class)
                    ->toList();

                // Executing the query on the Northwind sample data
                // will result in all 47 Product documents that match the query predicate.
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region paging_0_3
                // A simple DocumentQuery without paging:
                // ======================================

                /** @var array<Product> $allResults */
                $allResults = $session->advanced()
                    ->documentQuery(Products_ByUnitsInStock_IndexEntry::class, Products_ByUnitsInStock::class)
                    ->whereGreaterThan("UnitsInStock", 10)
                    ->ofType(Product::class)
                    ->toList();

                // Executing the query on the Northwind sample data
                // will result in all 47 Product documents that match the query predicate.
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region paging_1_1
                // Retrieve only the 3'rd page - when page size is 10:
                // ===================================================

                $stats = new QueryStatistics();

                /** @var array<Product> $thirdPageResults */
                $thirdPageResults = $session
                    ->query(Products_ByUnitsInStock_IndexEntry::class, Products_ByUnitsInStock::class)
                     // Get the query stats if you wish to know the TOTAL number of results
                    ->statistics($stats )
                     // Apply some filtering condition as needed
                    ->whereGreaterThan("UnitsInStock", 10)
                    ->ofType(Product::class)
                     // Call 'Skip', pass the number of items to skip from the beginning of the result set
                     // Skip the first 20 resulting documents
                    ->skip(20)
                     // Call 'Take' to define the number of documents to return
                     // Take up to 10 products => so 10 is the "Page Size"
                    ->take(10)
                    ->toList();

                // When executing this query on the Northwind sample data,
                // results will include only 10 Product documents ("products/45-A" to "products/54-A")

                $totalResults = $stats->getTotalResults();

                // While the query returns only 10 results,
                // `totalResults` will hold the total number of matching documents (47).
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region paging_1_3
                // Retrieve only the 3'rd page - when page size is 10:
                // ===================================================

                $stats = new QueryStatistics();

                /** @var array<Product> $thirdPageResults */
                $thirdPageResults = $session->advanced()
                    ->documentQuery(Products_ByUnitsInStock_IndexEntry::class, Products_ByUnitsInStock::class)
                     // Get the query stats if you wish to know the TOTAL number of results
                    ->statistics($stats)
                     // Apply some filtering condition as needed
                    ->whereGreaterThan("UnitsInStock", 10)
                    ->ofType(Product::class)
                     // Call 'Skip', pass the number of items to skip from the beginning of the result set
                     // Skip the first 20 resulting documents
                    ->skip(20)
                     // Call 'Take' to define the number of documents to return
                     // Take up to 10 products => so 10 is the "Page Size"
                    ->take(10)
                    ->toList();

                // When executing this query on the Northwind sample data,
                // results will include only 10 Product documents ("products/45-A" to "products/54-A")

                $totalResults = $stats->getTotalResults();

                // While the query returns only 10 results,
                // `totalResults` will hold the total number of matching documents (47).
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region paging_2_1
                // Query for all results - page by page:
                // =====================================

                $pagedResults = null;
                $pageNumber = 0;
                $pageSize = 10;

                do
                {
                    $pagedResults = $session
                        ->query(Products_ByUnitsInStock_IndexEntry::class, Products_ByUnitsInStock::class)
                         // Apply some filtering condition as needed
                        ->whereGreaterThan("UnitsInStock", 10)
                        ->ofType(Product::class)
                         // Skip the number of results that were already fetched
                        ->skip($pageNumber * $pageSize)
                         // Request to get 'pageSize' results
                        ->take($pageSize)
                        ->toList();

                    $pageNumber++;

                    // Make any processing needed with the current paged results here
                    // ...
                }
                while (count($pagedResults) > 0); // Fetch next results
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region paging_2_3
                // Query for all results - page by page:
                // =====================================

                $pagedResults = null;
                $pageNumber = 0;
                $pageSize = 10;

                do
                {
                    $pagedResults = $session->advanced()
                        ->documentQuery(Products_ByUnitsInStock_IndexEntry::class, Products_ByUnitsInStock::class)
                        // Apply some filtering condition as needed
                        ->whereGreaterThan("UnitsInStock", 10)
                        ->ofType(Product::class)
                        // Skip the number of results that were already fetched
                        ->skip($pageNumber * $pageSize)
                        // Request to get 'pageSize' results
                        ->take($pageSize)
                        ->toList();

                    $pageNumber++;

                    // Make any processing needed with the current paged results here
                    // ...
                }
                while (count($pagedResults) > 0); // Fetch next results

                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region paging_3_1
                $pagedResults = null;

                $totalResults = 0;
                $totalUniqueResults = 0;
                $skippedResults = 0;

                $pageNumber = 0;
                $pageSize = 10;

                do
                {
                    $pagedResults = $session
                            ->query(Products_ByUnitsInStock_IndexEntry::class, Products_ByUnitsInStock::class)
                            ->statistics($stats)
                            ->whereGreaterThan("UnitsInStock", 10)
                            ->ofType(Product::class)
                            // Define a projection
                            ->selectFields(ProjectedClass::class)
                             // Call Distinct to remove duplicate projected results
                            ->distinct()
                             // Add the number of skipped results to the "start location"
                            ->skip(($pageNumber * $pageSize) + $skippedResults)
                             // Define how many items to return
                            ->take($pageSize)
                            ->toList();

                    $totalResults = $stats->getTotalResults();        // Number of total matching documents (includes duplicates)
                    $skippedResults += $stats->getSkippedResults();   // Number of duplicate results that were skipped
                    $totalUniqueResults += count($pagedResults); // Number of unique results returned in this server call

                    $pageNumber++;
                }
                while (count($pagedResults) > 0); // Fetch next results

                // When executing the query on the Northwind sample data:
                // ======================================================

                // The total matching results reported in the stats is 47 (totalResults),
                // but the total unique objects returned while paging the results is only 29 (totalUniqueResults)
                // due to the 'Distinct' usage which removes duplicates.

                // This is solved by adding the skipped results count to Skip().
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region paging_3_3
                $pagedResults = null;

                $totalResults = 0;
                $totalUniqueResults = 0;
                $skippedResults = 0;

                $pageNumber = 0;
                $pageSize = 10;

                do
                {
                    $pagedResults = $session->advanced()
                        ->documentQuery(Products_ByUnitsInStock_IndexEntry::class, Products_ByUnitsInStock::class)
                        ->statistics($stats)
                        ->whereGreaterThan("UnitsInStock", 10)
                        ->ofType(Product::class)
                         // Define a projection
                        ->selectFields(ProjectedClass::class)
                         // Call Distinct to remove duplicate projected results
                        ->distinct()
                         // Add the number of skipped results to the "start location"
                        ->skip(($pageNumber * $pageSize) + $skippedResults)
                        ->take($pageSize)
                        ->toList();

                    $totalResults = $stats->getTotalResults();        // Number of total matching documents (includes duplicates)
                    $skippedResults += $stats->getSkippedResults();   // Number of duplicate results that were skipped
                    $totalUniqueResults += count($pagedResults);      // Number of unique results returned in this server call

                    $pageNumber++;
                }
                while (count($pagedResults) > 0); // Fetch next results

                // When executing the query on the Northwind sample data:
                // ======================================================

                // The total matching results reported in the stats is 47 (totalResults),
                // but the total unique objects returned while paging the results is only 29 (totalUniqueResults)
                // due to the 'Distinct' usage which removes duplicates.

                // This is solved by adding the skipped results count to Skip().
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region paging_4_1
                $pagedResults = null;

                $totalResults = 0;
                $totalUniqueResults = 0;
                $skippedResults = 0;

                $pageNumber = 0;
                $pageSize = 50;

                do
                {
                    $pagedResults = $session
                        ->query(Orders_ByProductName_IndexEntry::class, Orders_ByProductName::class)
                        ->statistics($stats)
                        ->ofType(Order::class)
                         // Add the number of skipped results to the "start location"
                        ->skip(($pageNumber * $pageSize) + $skippedResults)
                        ->take($pageSize)
                        ->toList();

                    $totalResults = $stats->getTotalResults();
                    $skippedResults += $stats->getSkippedResults();
                    $totalUniqueResults += count($pagedResults);

                    $pageNumber++;
                }
                while (count($pagedResults) > 0); // Fetch next results

                // When executing the query on the Northwind sample data:
                // ======================================================

                // The total results reported in the stats is 2155 (totalResults),
                // which represent the multiple index-entries generated as defined by the fanout index.

                // By adding the skipped results count to the Skip() method,
                // we get the correct total unique results which is 830 Order documents.
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region paging_4_3
                $pagedResults = null;

                $totalResults = 0;
                $totalUniqueResults = 0;
                $skippedResults = 0;

                $pageNumber = 0;
                $pageSize = 50;

                do
                {
                    $pagedResults = $session->advanced()
                        ->documentQuery(Orders_ByProductName_IndexEntry::class, Orders_ByProductName::class)
                        ->statistics($stats)
                        ->ofType(Order::class)
                         // Add the number of skipped results to the "start location"
                        ->skip(($pageNumber * $pageSize) + $skippedResults)
                        ->take($pageSize)
                        ->toList();

                    $totalResults = $stats->getTotalResults();
                    $skippedResults += $stats->getSkippedResults();
                    $totalUniqueResults += count($pagedResults);

                    $pageNumber++;
                }
                while (count($pagedResults) > 0); // Fetch next results

                // When executing the query on the Northwind sample data:
                // ======================================================

                // The total results reported in the stats is 2155 (totalResults),
                // which represent the multiple index-entries generated as defined by the fanout index.

                // By adding the skipped results count to the Skip() method,
                // we get the correct total unique results which is 830 Order documents.
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }
}

# region projected_class
class ProjectedClass
{
    public ?string $category = null;
    public ?string $supplier = null;

    public function getCategory(): ?string
    {
        return $this->category;
    }

    public function setCategory(?string $category): void
    {
        $this->category = $category;
    }

    public function getSupplier(): ?string
    {
        return $this->supplier;
    }

    public function setSupplier(?string $supplier): void
    {
        $this->supplier = $supplier;
    }
}
# endregion

# region index_0
class Products_ByUnitsInStock_IndexEntry
{
    private ?int $unitsInStock = null;

    public function getUnitsInStock(): ?int
    {
        return $this->unitsInStock;
    }

    public function setUnitsInStock(?int $unitsInStock): void
    {
        $this->unitsInStock = $unitsInStock;
    }
}

class Products_ByUnitsInStock extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->map = "docs.Products.Select(product => new {" .
            "    UnitsInStock = product.UnitsInStock" .
            "})";
    }
}
# endregion

# region index_1
// A fanout index - creating MULTIPLE index-entries per document:
// ==============================================================

class Orders_ByProductName_IndexEntry
{
    private ?string $productName = null;

    public function getProductName(): ?string
    {
        return $this->productName;
    }

    public function setProductName(?string $productName): void
    {
        $this->productName = $productName;
    }
}
class Orders_ByProductName extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->map = "docs.Orders.SelectMany(order => order.Lines, (order, line) => new {" .
                "    Product = line.ProductName " .
                "})";
    }
}
# endregion
