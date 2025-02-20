<?php

namespace RavenDB\Samples\Indexes\Querying;

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Indexes\AbstractIndexCreationTask;
use RavenDB\Documents\Indexes\FieldIndexing;
use RavenDB\Samples\Infrastructure\Orders\Product;

class Sorting
{
    public function canOrderResults(): void
    {
        $store = new DocumentStore();
        try {
            // Order by index-field value
            // ==========================

            $session = $store->openSession();
            try {
                # region sort_1
                /** @var array<Product> $products */
                $products = $session
                     // Query the index
                    ->query(Products_ByUnitsInStock_IndexEntry::class, Products_ByUnitsInStock::class)
                     // Apply filtering (optional)
                    ->whereGreaterThan("UnitsInStock", 10)
                     // Call 'OrderByDescending', pass the index-field by which to order the results
                    ->orderByDescending("UnitsInStock")
                    ->ofType(Product::class)
                    ->toList();

                // Results will be sorted by the 'UnitsInStock' value in descending order,
                // with higher values listed first.
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region sort_3
                /** @var array<Product> $products */
                $products = $session->advanced()
                     // Query the index
                    ->documentQuery(Products_ByUnitsInStock_IndexEntry::class, Products_ByUnitsInStock::class)
                     // Apply filtering (optional)
                    ->whereGreaterThan("UnitsInStock", 10)
                     // Call 'OrderByDescending', pass the index-field by which to order the results
                    ->orderByDescending("UnitsInStock")
                    ->ofType(Product::class)
                    ->toList();

                // Results will be sorted by the 'UnitsInStock' value in descending order,
                // with higher values listed first.
                # endregion
            } finally {
                $session->close();
            }

            // Order when field is searchable
            // ==============================
            $session = $store->openSession();
            try {
                # region sort_4
                /** @var array<Product> $products */
                $products = $session
                     // Query the index
                    ->query(Products_BySearchName_IndexEntry::class, Products_BySearchName::class)
                     // Call 'Search':
                     // Pass the index-field that was configured for FTS and the term to search for.
                     // Here we search for terms that start with "ch" within index-field 'Name'.
                    ->search("Name", "ch*")
                     // Call 'OrderBy':
                     // Pass the other index-field by which to order the results.
                    ->orderBy("NameForSorting")
                    ->ofType(Product::class)
                    ->toList();

                // Running the above query on the NorthWind sample data, ordering by 'NameForSorting' field,
                // we get the following order:
                // =========================================================================================

                // "Chai"
                // "Chang"
                // "Chartreuse verte"
                // "Chef Anton's Cajun Seasoning"
                // "Chef Anton's Gumbo Mix"
                // "Chocolade"
                // "Jack's New England Clam Chowder"
                // "Pâté chinois"
                // "Teatime Chocolate Biscuits"

                // While ordering by the searchable 'Name' field would have produced the following order:
                // ======================================================================================

                // "Chai"
                // "Chang"
                // "Chartreuse verte"
                // "Chef Anton's Cajun Seasoning"
                // "Pâté chinois"
                // "Chocolade"
                // "Teatime Chocolate Biscuits"
                // "Chef Anton's Gumbo Mix"
                // "Jack's New England Clam Chowder"
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region sort_6
                /** @var array<Product> $products */
                $products = $session->advanced()
                     // Query the index
                    ->documentQuery(Products_BySearchName_IndexEntry::class, Products_BySearchName::class)
                     // Call 'Search':
                     // Pass the index-field that was configured for FTS and the term to search for.
                     // Here we search for terms that start with "ch" within index-field 'Name'.
                    ->search("Name", "ch*")
                     // Call 'OrderBy':
                     // Pass the other index-field by which to order the results.
                    ->orderBy("NameForSorting")
                    ->ofType(Product::class)
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }
}

# region index_1
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

# region index_2
class Products_BySearchName_IndexEntry
{
    // Index-field 'Name' will be configured below for full-text search
    private ?string $name = null;

    // Index-field 'NameForSorting' will be used for ordering query results
    private ?string $nameForSorting = null;

    public function getName(): ?string
    {
        return $this->name;
    }

    public function setName(?string $name): void
    {
        $this->name = $name;
    }

    public function getNameForSorting(): ?string
    {
        return $this->nameForSorting;
    }

    public function setNameForSorting(?string $nameForSorting): void
    {
        $this->nameForSorting = $nameForSorting;
    }
}
class Products_BySearchName extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->map = "docs.Products.Select(product => new {" .
            "    Name = product.Name," .
            "    NameForSorting = product.Name" .
            "})";

        $this->index("Name", FieldIndexing::search());
    }
}
# endregion
