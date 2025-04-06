<?php

namespace RavenDB\Samples\Indexes;

use DateTime;
use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Indexes\AbstractIndexCreationTask;
use RavenDB\Documents\Indexes\FieldIndexing;
use RavenDB\Samples\Infrastructure\Orders\Product;

# region index_1
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
                # region query_1
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
