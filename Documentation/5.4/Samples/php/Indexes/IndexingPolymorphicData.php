<?php

namespace RavenDB\Samples\Indexes;

use RavenDB\Documents\Conventions\DocumentConventions;
use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Indexes\AbstractMultiMapIndexCreationTask;

class IndexingPolymorphicData
{
    public function MultiMapIndexes(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region multi_map_2
                /** @var array<Animal> $results */
                $results = $session
                    ->advanced()
                    ->documentQuery(Animal::class, Animals_ByName::class)
                    ->whereEquals("Name", "Mitzy")
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region multi_map_3
                /** @var array<Animal> $results */
                $results = $session
                    ->query(Animal::class, Animals_ByName::class)
                    ->whereEquals("Name", "Mitzy")
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

# region multi_map_1
class Animals_ByName extends AbstractMultiMapIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->addMap("from c in docs.Cats select new { c.name }");
        $this->addMap("from d in docs.Dogs select new { d.name }");
    }
}
# endregion

class OtherWays
{
    public function sample(): void
    {
        # region other_ways_1
        $store = new DocumentStore();
        $store->getConventions()->setFindCollectionName(
            function (?string $className): string {
                if (is_a($className, Animal::class)) {
                    return "Animals";
                }
                return DocumentConventions::defaultGetCollectionName($className);
            }
        );
        # endregion
    }
}

class Animal
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

class Cat extends Animal
{
}

class Dog extends Animal
{
}
