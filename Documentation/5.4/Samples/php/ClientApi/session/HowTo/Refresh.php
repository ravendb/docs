<?php

use PHPUnit\Framework\TestCase;
use RavenDB\Documents\DocumentStore;

interface FooInterface
{
    # region refresh_1
    public function refresh(object $entity): void;
    # endregion
}

class Refresh extends TestCase
{
    public function example(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region refresh_2
                $employee = $session->load(Employee::class, "employees/1");
                $this->assertEquals("Doe", $employee->getLastName());

                // LastName changed to 'Shmoe'

                $session->advanced()->refresh($employee);

                $this->assertEquals("Shmoe", $employee->getLastName());
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }
}
