<?php

use RavenDB\Documents\Session\DocumentQueryInterface;
use RavenDB\Samples\Infrastructure\DocumentStoreHolder;

interface FooInterface
{
    # region syntax
    function whereEquals(string $fieldName, $value, bool $exact = false): DocumentQueryInterface;
    function whereNotEquals(string $fieldName, $value, bool $exact = false): DocumentQueryInterface;

    // ... rest of where methods
    # endregion
}

class ExactMatch
{
    public function examples(): void
    {
        $store = DocumentStoreHolder::getStore();
        try {
            $session = $store->openSession();
            try {
                # region exact_1
                // load all entities from 'Employees' collection
                // where FirstName field's contents equals 'Robert' (case sensitive match)

                /** @var array<Employee> $employees */
                $employees = $session->query(Employee::class)
                    ->whereEquals("FirstName", "Robert", true)
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region exact_4
                // return all entities from the 'Orders' collection which contain 
                // at least one order line with 'Teatime Chocolate Biscuits' product
                // perform a case-sensitive match
                /** @var array<Order> $orders */
                $orders = $session->query(Order::class)
                    ->whereEquals("Lines[].ProductName", "Teatime Chocolate Biscuits", true)
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

class Employee
{
}

class Order
{
}
