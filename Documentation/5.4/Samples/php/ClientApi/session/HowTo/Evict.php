<?php

use RavenDB\Documents\DocumentStore;

interface FooInterface
{
    # region evict_1
    public function evict(object $entity): void;
    # endregion
}

class Evict
{
    public function example(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region evict_2
                $employee1 = new Employee();
                $employee1->setFirstName("John");
                $employee1->setLastName("Doe");

                $employee2 = new Employee();
                $employee2->setFirstName("Joe");
                $employee2->setLastName("Shmoe");

                $session->store($employee1);
                $session->store($employee2);

                $session->advanced()->evict($employee1);

                $session->saveChanges(); // only 'Joe Shmoe' will be saved
                # endregion
            } finally {
                $session->close();
            }

                $session = $store->openSession();
                try {
                # region evict_3
                $employee = $session->load(Employee::class, "employees/1-A");//loading from serer
                $employee = $session->load(Employee::class, "employees/1-A"); // no server call
                $session->advanced()->evict($employee);
                $employee = $session->load(Employee::class, "employees/1-A"); // loading form server
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
