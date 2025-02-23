<?php

namespace RavenDB\Samples\Indexes;

use DateTime;
use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Indexes\AbstractIndexCreationTask;
use RavenDB\Documents\Indexes\FieldIndexing;
use RavenDB\Samples\Infrastructure\Orders\Product;

# region indexes_1
class Products_AllProperties_Result
{
    public ?string $query = null;
    public function getQuery(): ?string
    {
        return $this->query;
    }
    public function setQuery(?string $query): void
    {
        $this->query = $query;
    }
}

class Products_AllProperties extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->map = "docs.Products.Select(product => new { " .
            // convert product to JSON and select all properties from it
            "    Query = this.AsJson(product).Select(x => x.Value) " .
            "})";

        // mark 'query' field as analyzed which enables full text search operations
        $this->index("Query", FieldIndexing::search());
    }
}
# endregion

# region indexes_3
class Products_WithMetadata_Result
{
    public ?DateTime $lastModified = null;

    public function getLastModified(): ?DateTime
    {
        return $this->lastModified;
    }

    public function setLastModified(?DateTime $lastModified): void
    {
        $this->lastModified = $lastModified;
    }
}

class Products_WithMetadata extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->map = "docs.Products.Select(product => new { " .
            "    Product = Product, " .
            "    Metadata = this.MetadataFor(product) " .
            "}).Select(this0 => new { " .
            "    LastModified = this0.metadata.Value\<DateTime\>(\"Last-Modified\") " .
            "})";
    }
}
# endregion

class Metadata
{
    public function samples(): void
    {
        $store = new DocumentStore();
        try {

            $session = $store->openSession();
            try {
                # region indexes_2
                $results = $session->query(Products_AllProperties_Result::class, Products_AllProperties::class)
                        ->whereEquals("Query", "Chocolade")
                        ->ofType(Product::class)
                        ->toList();
				# endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region indexes_4
                $results = $session
                    ->query(Products_WithMetadata_Result::class, Products_WithMetadata::class)
                    ->orderByDescending("LastModified")
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
