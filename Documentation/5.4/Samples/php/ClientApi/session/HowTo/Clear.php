<?php

use RavenDB\Documents\DocumentStore;

interface FooInterface {
    # region clear_1
    public function clear(): void;
    # endregion
}

class Clear
{
    public function example(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region clear_2
                $employee = new Employee();
                $employee->setFirstName("John");
                $employee->setLastName("Doe");
                $session->store($employee);

                $session->advanced()->clear();

                $session->saveChanges(); // nothing will hapen
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }
}

class Employee
{
    private ?string $firstName = null;
    private ?string $lastName = null;

    public function getFirstName(): ?string
    {
        return $this->firstName;
    }

    public function setFirstName(?string $firstName): void
    {
        $this->firstName = $firstName;
    }

    public function getLastName(): ?string
    {
        return $this->lastName;
    }

    public function setLastName(?string $lastName): void
    {
        $this->lastName = $lastName;
    }
}
