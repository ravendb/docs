<?php

namespace RavenDB\Samples\ClientApi\Session\HowTo;

use DateTimeInterface;
use Employee;
use RavenDB\Documents\DocumentStore;

interface FooInterface
{
    # region get_last_modified_1
    public function getLastModifiedFor($instance): ?DateTimeInterface;
    # endregion
}

class GetLastModified
{
    public function example(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region get_last_modified_2
                $employee = $session->load(Employee::class, "employees/1-A");
                $lastModified = $session->advanced()->getLastModifiedFor($employee);
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }
}
