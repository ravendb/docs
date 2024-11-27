<?php

use RavenDB\Documents\Indexes\AbstractIndexCreationTask;
use RavenDB\Samples\Infrastructure\DocumentStoreHolder;

class FilterByNonExistingField
{
    public function examples(): void
    {
        $store = DocumentStoreHolder::getStore();
        try {
            $session = $store->openSession();
            try {
                # region whereNotExists_1
                /** @var array<Order> $ordersWithoutFreightField */
                $ordersWithoutFreightField = $session
                        ->advanced()
                         // Define a DocumentQuery on 'Orders' collection
                        ->documentQuery(Order::class)
                         // Search for documents that do Not contain field 'Freight'
                        ->not()
                        ->whereExists("Freight")
                         // Execute the query
                        ->toList();

                // Results will be only the documents that do Not contain the 'Freight' field in 'Orders' collection
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region whereNotexists_2
                // Query the index
                // ===============

                /** @var array<Order> $ordersWithoutFreightField */
                $ordersWithoutFreightField = $session
                    ->advanced()
                    // Define a DocumentQuery on the index
                    ->documentQuery(IndexEntry::class, Orders_ByFright::class)
                    // Verify the index is not stale (optional)
                    ->waitForNonStaleResults()
                    // Search for documents that do Not contain field 'Freight'
                    ->not()
                    ->whereExists("Freight")
                     // Execute the query
                    ->toList();

                // Results will be only the documents that do Not contain the 'Freight' field in 'Orders' collection
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }
}

# region the_index
// Define a static index on the 'Orders' collection
// ================================================

class IndexEntry
{
    // Define the index-fields
    public ?float $freight = null;
    public ?string $id = null;

    public function getFreight(): float
    {
        return $this->freight;
    }

    public function setFreight(float $freight): void
    {
        $this->freight = $freight;
    }

    public function getId(): ?string
    {
        return $this->id;
    }

    public function setId(?string $id): void
    {
        $this->id = $id;
    }
}

class Orders_ByFright extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();
        // Define the index Map function
        $this->map = "orders => from doc in orders select new {\n" .
            "    freight = doc.name, \n" .
            "    id = doc.id\n" .
            "})";

        }
    }
# endregion
