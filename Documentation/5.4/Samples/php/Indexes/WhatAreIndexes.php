<?php

namespace RavenDB\Samples\Indexes;

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Indexes\AbstractIndexCreationTask;
use RavenDB\Samples\Infrastructure\Orders\Employee;

# region indexes_1
class Employees_ByFirstAndLastName extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->map = "docs.Employees.Select(employee => new {" .
            "    FirstName = employee.FirstName," .
            "    LastName = employee.LastName" .
            "})";
    }
}
# endregion


class WhatAreIndexes
{
    public function samples(): void
    {
        $store = new DocumentStore();

        try {
            # region indexes_2
            // save index on server
            (new Employees_ByFirstAndLastName())->execute($store);
            # endregion

            $session = $store->openSession();
            try {
                # region indexes_3
                /** @var array<Employee> $results */
                $results = $session->query(Employee::class, Employees_ByFirstAndLastName::class)
                        ->whereEquals('FirstName', "Robert")
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
