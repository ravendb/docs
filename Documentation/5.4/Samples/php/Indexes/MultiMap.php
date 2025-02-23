<?php

namespace RavenDB\Samples\Indexes;

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Indexes\AbstractMultiMapIndexCreationTask;
use RavenDB\Documents\Indexes\FieldIndexing;
use RavenDB\Documents\Indexes\FieldStorage;

class MultiMap
{
    public function samples()
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region multi_map_7
                /** @var array<AnimalInterface> $results */
                $results = $session
                    ->query(AnimalInterface::class, Animals_ByName::class)
                    ->whereEquals("Name", "Mitzy")
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region multi_map_8
                /** @var array<AnimalInterface> $results */
                $results = $session
                    ->advanced()
                    ->documentQuery(AnimalInterface::class, Animals_ByName::class)
                    ->whereEquals("Name", "Mitzy")
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }

        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region multi_map_1_1
                /** @var array<Smart_Search_Projection> $results */
                $results = $session
                    ->documentQuery(Smart_Search_Result::class, Smart_Search::class)
                    ->search("Content", "Lau*")
                    ->selectFields(Smart_Search_Projection::class)
                    ->toList();

                /** @var Smart_Search_Projection $result */
                foreach ($results as $result)
                {
                    echo $result->getCollection() . ": " . $result->getDisplayName();
                    // Companies: Laughing Bacchus Wine Cellars
                    // Products: Laughing Lumberjack Lager
                    // Employees: Laura Callahan
                }
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
class Dog extends Animal
{

}
# endregion

# region multi_map_2
class Cat extends Animal
{

}
# endregion

# region multi_map_3
abstract class Animal implements AnimalInterface
{
    public ?string $name = null;

    public function getName(): ?string
    {
        return $this->name;
    }

    public function setName(?string $name): void
    {
        $this->name = $name;
    }
}
# endregion

# region multi_map_6
interface AnimalInterface
{
    public function getName(): ?string;
    public function setName(?string $name): void;
}
# endregion

# region multi_map_4
class Animals_ByName extends AbstractMultiMapIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->addMap( "docs.Cats.Select(c => new { " .
            "    Name = c.Name " .
            "})");

        $this->addMap( "docs.Dogs.Select(d => new { " .
            "    Name = d.Name " .
            "})");
    }
}
# endregion

# region multi_map_5
class Animals_ByName_ForAll extends AbstractMultiMapIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();
    }
}
# endregion

# region multi_map_1_0
class Smart_Search_Result
{
    private ?string $id = null;
    private ?string $displayName = null;
    private ?string $collection = null;
    private ?string $content = null;

    public function getId(): ?string
    {
        return $this->id;
    }

    public function setId(?string $id): void
    {
        $this->id = $id;
    }

    public function getDisplayName(): ?string
    {
        return $this->displayName;
    }

    public function setDisplayName(?string $displayName): void
    {
        $this->displayName = $displayName;
    }

    public function getCollection(): ?string
    {
        return $this->collection;
    }

    public function setCollection(?string $collection): void
    {
        $this->collection = $collection;
    }

    public function getContent(): ?string
    {
        return $this->content;
    }

    public function setContent(?string $content): void
    {
        $this->content = $content;
    }
}

class Smart_Search_Projection
{
    private ?string $id = null;
    private ?string $displayName = null;
    private ?string $collection = null;

    public function getId(): ?string
    {
        return $this->id;
    }

    public function setId(?string $id): void
    {
        $this->id = $id;
    }

    public function getDisplayName(): ?string
    {
        return $this->displayName;
    }

    public function setDisplayName(?string $displayName): void
    {
        $this->displayName = $displayName;
    }

    public function getCollection(): ?string
    {
        return $this->collection;
    }

    public function setCollection(?string $collection): void
    {
        $this->collection = $collection;
    }
}

class Smart_Search extends AbstractMultiMapIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->addMap("docs.Companies.Select(c => new { " .
            "    Id = Id(c), " .
            "    Content = new string[] { " .
            "        c.Name " .
            "    }, " .
            "    DisplayName = c.Name, " .
            "    Collection = this.MetadataFor(c)[\"@collection\"] " .
            "})");

        $this->addMap("docs.Products.Select(p => new { " .
            "    Id = Id(p), " .
            "    Content = new string[] { " .
            "        p.Name " .
            "    }, " .
            "    DisplayName = p.Name, " .
            "    Collection = this.MetadataFor(p)[\"@collection\"] " .
            "})");

        $this->addMap("docs.Employees.Select(e => new { " .
            "    Id = Id(e), " .
            "    Content = new string[] { " .
            "        e.FirstName, " .
            "        e.LastName " .
            "    }, " .
            "    DisplayName = (e.FirstName + \" \") + e.LastName, " .
            "    Collection = this.MetadataFor(e)[\"@collection\"] " .
            "})");

        // mark 'content' field as analyzed which enables full text search operations
        $this->index("Content", FieldIndexing::search());

        // storing fields so when projection (e.g. ProjectInto)
        // requests only those fields
        // then data will come from index only, not from storage
        $this->store("Id", FieldStorage::yes());
        $this->store("DisplayName", FieldStorage::yes());
        $this->store("Collection", FieldStorage::yes());

    }
}
# endregion
