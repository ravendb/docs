<?php

use RavenDB\Documents\Indexes\AbstractIndexCreationTask;
use RavenDB\Documents\Queries\Suggestions\StringDistanceTypes;
use RavenDB\Documents\Queries\Suggestions\SuggestionBase;
use RavenDB\Documents\Queries\Suggestions\SuggestionDocumentQueryInterface;
use RavenDB\Documents\Queries\Suggestions\SuggestionOperationsInterface;
use RavenDB\Documents\Queries\Suggestions\SuggestionOptions;
use RavenDB\Documents\Queries\Suggestions\SuggestionResult;
use RavenDB\Documents\Queries\Suggestions\SuggestionSortMode;
use RavenDB\Documents\Queries\Suggestions\SuggestionWithTerm;
use RavenDB\Samples\Infrastructure\DocumentStoreHolder;
use RavenDB\Type\StringArray;

class Foo {
    # region suggest_7
    private int $pageSize = 15;
    private ?StringDistanceTypes $distance = null;
    private float $accuracy = 0.5;
    private ?SuggestionSortMode $sortMode = null;

    public function __construct()
    {
        $distance = StringDistanceTypes::levenshtein();
        $sortMode = SuggestionSortMode::popularity();
        ...
    }

    // getters and setters for fields listed above
    # endregion
}

interface FooInterface {
    # region suggest_1
    /**
     * Usage:
     *   - suggestUsing(SuggestionBase $suggestion);
     *   - suggestUsing(Closure $suggestionBuilder);
     *
     * @param SuggestionBase|Closure|null $suggestionOrBuilder
     * @return SuggestionDocumentQueryInterface
     */
    public function suggestUsing(null|SuggestionBase|Closure $suggestionOrBuilder): SuggestionDocumentQueryInterface;

    # endregion

    # region suggest_2
    /**
     * Usage:
     *   - byField("fieldName", "term");
     *   - byField("fieldName", ["term1", "term2"]);
     */
    function byField(?string $fieldName, null|string|StringArray|array $terms): SuggestionOperationsInterface;

    function withOptions(?SuggestionOptions $options): SuggestionOperationsInterface;
    # endregion
}

class Employees_ByFullName extends AbstractIndexCreationTask
{

}

class Employee {

}

class HowToWorkWithSuggestions
{
    public function examples(): void
    {
        $store = DocumentStoreHolder::getStore();
        try {
            $session = $store->openSession();
            try {
                # region suggest_5
                $options = new SuggestionOptions();
                $options->setAccuracy(0.4);
                $options->setPageSize(5);
                $options->setDistance(StringDistanceTypes::jaroWinkler());
                $options->setSortMode(SuggestionSortMode::popularity());

                /** @var array<SuggestionResult> $suggestions */
                $suggestions = $session
                    ->query(Employee::class, Employees_ByFullName::class)
                    ->suggestUsing(function($builder) use ($options) {
                        $builder->byField("FullName", "johne")
                            ->withOptions($options);
                    })
                    ->execute();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region suggest_8
                $suggestionWithTerm = new SuggestionWithTerm("FullName");
                $suggestionWithTerm->setTerm("johne");

                /** @var array<SuggestionResult> $suggestions */
                $suggestions = $session
                    ->query(Employee::class, Employees_ByFullName::class)
                    ->suggestUsing($suggestionWithTerm)
                    ->execute();
                # endregion
            } finally {
               $session->close();
            }
        } finally {
            $store->close();
        }
    }
}
