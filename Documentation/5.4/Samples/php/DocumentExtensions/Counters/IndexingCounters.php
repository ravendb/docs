<?php

namespace RavenDB\Samples\DocumentExtensions\Counters;

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Indexes\AbstractIndexCreationTask;
use RavenDB\Documents\Indexes\Counters\AbstractCountersIndexCreationTask;
use RavenDB\Documents\Indexes\Counters\AbstractJavaScriptCountersIndexCreationTask;
use RavenDB\Samples\Infrastructure\Orders\Company;
use RavenDB\Type\StringArray;

/*
interface IFoo
{
    # region syntax
    List<String> counterNamesFor(Object doc);
    # endregion
}
*/

class IndexingCounters
{
    public function sample(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region query1
                // return all companies that have 'Likes' counter
                /** @var array<Company> $companies */
                $companies = $session
                    ->query(Companies_ByCounterNames_Result::class, Companies_ByCounterNames::class)
                    ->containsAny("CounterNames", ["Likes"])
                    ->ofType(Company::class)
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region query3
                // return all companies that have 'Likes' counter
                /** @var array<Company> $companies */
                $companies = $session
                        ->advanced()
                        ->documentQuery(Companies_ByCounterNames_Result::class, Companies_ByCounterNames::class)
                        ->containsAny("CounterNames", ["Likes"])
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

# region counter_entry
class CounterEntry
{
    private ?string $documentId = null;
    private ?string $name = null;
    private ?int $value = null;

    public function getDocumentId(): ?string
    {
        return $this->documentId;
    }

    public function setDocumentId(?string $documentId): void
    {
        $this->documentId = $documentId;
    }

    public function getName(): ?string
    {
        return $this->name;
    }

    public function setName(?string $name): void
    {
        $this->name = $name;
    }

    public function getValue(): ?int
    {
        return $this->value;
    }

    public function setValue(?int $value): void
    {
        $this->value = $value;
    }
}
# endregion

# region index_0
class Companies_ByCounterNames_Result
{
    public ?StringArray $counterNames = null;

    public function getCounterNames(): ?StringArray
    {
        return $this->counterNames;
    }

    public function setCounterNames(?StringArray $counterNames): void
    {
        $this->counterNames = $counterNames;
    }
}
class Companies_ByCounterNames extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->map = "from e in docs.Employees " .
            "let counterNames = counterNamesFor(e) " .
            "select new {" .
            "    CounterNames = counterNames.ToArray() " .
            "}";
    }
}
# endregion

# region index_1
class MyCounterIndex extends AbstractCountersIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->setMap(
            "from counter in counters " .
            "select new { " .
            "    Likes = counter.Value, " .
            "    Name = counter.Name, " .
            "    User = counter.DocumentId " .
            "}"
        );
    }
}
# endregion

# region index_3
class MyMultiMapCounterIndex extends AbstractJavaScriptCountersIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->setMaps([
            "counters.map('Blogposts', 'Likes', function (counter) {
                return {
                    Likes: counter.Value,
                    Name: counter.Name,
                    Blog Post: counter.DocumentId
                };
            })"
        ]);
    }
}
# endregion

