<?php

namespace RavenDB\Samples\ClientApi\Session\HowTo;

use Employee;
use RavenDB\Documents\DocumentStore;
use RavenDB\Type\StringList;

interface FooInterface
{
    # region syntax
    public function getCountersFor(mixed $instance): ?StringList;
    # endregion
}

class GetCountersFor
{
    public function example(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region example
                // Load a document
                $employee = $session->load(Employee::class, "employees/1-A");

                // Get counter names from the loaded entity
                $counterNames = $session->advanced()->getCountersFor($employee);
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }
}
