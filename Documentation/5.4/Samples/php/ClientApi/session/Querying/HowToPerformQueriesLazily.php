<?php

use RavenDB\Documents\Lazy;
use RavenDB\Documents\Queries\Facets\FacetResult;
use RavenDB\Documents\Queries\Query;
use RavenDB\Documents\Queries\Suggestions\SuggestionResult;
use RavenDB\Samples\Infrastructure\DocumentStoreHolder;

interface FooInterface
{
    # region lazy_1
    /**
     * Usage
     *   - lazily();
     *   - lazily(Closure $onEval)
     */
    function lazily(?Closure $onEval = null): Lazy;
    # endregion

    # region lazy_4
    function countLazily(): Lazy;
    # endregion

    # region lazy_6
    /**
     * Usage
     *   - executeLazy();
     *   - executeLazy(Closure $onEval)
     */
    public function executeLazy(?Closure $onEval = null): Lazy;
    # endregion
}

class Employee {

}

class Camera  {

}

class HowToPerformQueriesLazily
{
    public function examples(): void
    {
        $store = DocumentStoreHolder::getStore();
        try {

            $session = $store->openSession();
            try {
                # region lazy_2
                /** @var Lazy<array<Employee>> $employeesLazy */
                $employeesLazy = $session
                    ->query(Employee::class)
                    ->whereEquals("FirstName", "Robert")
                    ->lazily();

                /** @var array<Employee> $employees */
                $employees = $employeesLazy->getValue(); // query will be executed here
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region lazy_5
                /** @var Lazy<int> $countLazy */
                $countLazy = $session
                    ->query(Employee::class)
                    ->whereEquals("FirstName", "Robert")
                    ->countLazily();

                /** @var int $count */
                $count = $countLazy->getValue(); // query will be executed here
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region lazy_7
                /** @var Lazy<array<string, SuggestionResult>> $suggestLazy */
                $suggestLazy = $session
                    ->query(Employee::class, Query::index("Employees_ByFullName"))
                    ->suggestUsing(function($builder) { $builder->byField("FullName", "johne"); })
                    ->executeLazy();

                /** @var array<string, SuggestionResult> $suggest */
                $suggest = $suggestLazy->getValue(); // query will be executed here

                /** @var array<string> $suggestions */
                $suggestions = $suggest["FullName"]->getSuggestions();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region lazy_9
                /** @var Lazy<array<string, FacetResult>> $facetsLazy */
                $facetsLazy = $session
                    ->query(Camera::class, Query::index("Camera/Costs"))
                    ->aggregateUsing("facets/CameraFacets")
                    ->executeLazy();

                /** @var array<string, FacetResult> $facets */
                $facets = $facetsLazy->getValue(); // query will be executed here

                /** @var FacetResult $results */
                $results = $facets["manufacturer"];
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }
}
