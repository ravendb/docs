<?php

namespace RavenDB\Samples\ClientApi\Session\HowTo;

use Employee;
use RavenDB\Documents\DocumentStore;

interface FooInterface
{
    # region get_change_vector_1
    public function getChangeVectorFor(?object $instance): ?string;
    # endregion
}

class GetChangeVector
{
    public function example(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region get_change_vector_2
                $employee = $session->load(Employee::class, "employees/1-A");
                $changeVector = $session->advanced()->getChangeVectorFor($employee);
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }
}
