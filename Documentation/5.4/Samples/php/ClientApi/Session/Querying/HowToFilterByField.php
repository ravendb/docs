<?php


use RavenDB\Documents\Session\DocumentQueryInterface;
use RavenDB\Documents\Session\DocumentSession;
use RavenDB\Samples\Infrastructure\Orders\Employee;

interface FooInterface
{
    # region whereexists_1
    public function whereExists(?string $fieldName): DocumentQueryInterface;
    # endregion
}

class HowToFilterByField
{
    public function samples(DocumentSession $session): void
    {
        # region whereexists_2
        /** @var array<Employee> $results */
        $results = $session
            ->advanced()
            ->documentQuery(Employee::class)
            ->whereExists("FirstName")
            ->toList();
        # endregion

        # region whereexists_3
        $results = $session
            ->advanced()
            ->documentQuery(Employee::class)
            ->whereExists("Address.Location.Latitude")
            ->toList();
        # endregion
    }
}
