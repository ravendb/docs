<?php

use RavenDB\Documents\Session\DocumentQueryInterface;
use RavenDB\Samples\Infrastructure\DocumentStoreHolder;

interface FooInterface
{
    # region syntax
    public function proximity(int $proximity): DocumentQueryInterface;
    # endregion
}

class ProximitySearch
{
    public function examples(): void
    {
        $store = DocumentStoreHolder::getStore();
        try {
            $session = $store->openSession();
            try {
                # region proximity_2
                $session->advanced()
                    ->documentQuery(Fox::class)
                    ->search("name", "quick fox")
                    ->proximity(2)
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region proximity_1
                /** @var array<Employee> $employees */
                $employees = $session->advanced()
                    ->documentQuery(Employee::class)
                    // Make a full-text search with search terms
                    ->search("Notes", "fluent french")
                    // Call 'Proximity' with 0 distance
                    ->proximity(0)
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region proximity_3
                /** @var array<Employee> $employees */
                $employees = $session->advanced()
                    ->documentQuery(Employee::class)
                    // Make a full-text search with search terms
                    ->search("Notes", "fluent french")
                    // Call 'Proximity' with distance 5
                    ->proximity(4)
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

class Fox
{
    private ?string $name = null;

    public function getName(): ?string
    {
        return $this->name;
    }

    public function setName(?string $name): void
    {
        $this->name = $name;
    }
}
