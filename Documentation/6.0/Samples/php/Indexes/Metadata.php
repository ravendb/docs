<?php

namespace RavenDB\Samples\Indexes;

use DateTime;
use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Indexes\AbstractIndexCreationTask;
use RavenDB\Documents\Indexes\FieldIndexing;
use RavenDB\Samples\Infrastructure\Orders\Product;

# region index_1
class Products_ByMetadata_AccessViaValue_IndexEntry
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

    public ?bool $hasCounters = null;
    public function getHasCounters(): ?bool
    {
        return $this->hasCounters;
    }
    public function setHasCounters(?bool $hasCounters): void
    {
        $this->hasCounters = $hasCounters;
    }
}

class Products_ByMetadata_AccessViaValue extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->map = "from product in docs.Products\n" .
                     "let metadata = MetadataFor(product)\n" .
                     "select new { " .
                     "    lastModified = metadata.Value<DateTime>(\"@last-modified\"), " .
                     "    hasCounters = metadata.Value<object>(\"@counters\") != null " .
                     "}";
    }
}
# endregion

# region index_2
class Products_ByMetadata_AccessViaIndexer_IndexEntry
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

    public ?bool $hasCounters = null;
    public function getHasCounters(): ?bool
    {
        return $this->hasCounters;
    }
    public function setHasCounters(?bool $hasCounters): void
    {
        $this->hasCounters = $hasCounters;
    }
}

class Products_ByMetadata_AccessViaIndexer extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->map = "from product in docs.Products " .
                     "let metadata = MetadataFor(product) " .
                     "select new {\n" .
                     "    lastModified = (DateTime)metadata[\"@last-modified\"],\n" .
                     "    hasCounters = metadata[\"@counters\"] != null }";
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
                    ->query(
                            Products_ByMetadata_AccessViaValue_IndexEntry::class,
                            Products_ByMetadata_AccessViaValue::class
                        )
                    ->whereEquals("hasCounters", true)
                    ->orderByDescending("lastModified")
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
