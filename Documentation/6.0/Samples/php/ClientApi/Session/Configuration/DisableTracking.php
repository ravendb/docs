<?php

use PHPUnit\Framework\TestCase;
use RavenDB\Documents\Conventions\DocumentConventions;
use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Session\SessionOptions;
use RavenDB\Samples\Infrastructure\Orders\Product;

class DisableTracking extends TestCase
{
    public function example(): void
    {
        $store = new DocumentStore();

        try {
            $session = $store->openSession();
            try {
                # region disable_tracking_1
                // Load a product entity - the session will track the entity by default
                /** @var Product $product */
                $product = $session->load(Product::class, "products/1-A");

                // Call 'ignoreChangesFor' to instruct the session to ignore changes made to this entity
                $session->advanced()->ignoreChangesFor($product);

                // The following change will be ignored by saveChanges - it will not be persisted
                $product->setUnitsInStock($product->getUnitsInStock() + 1);

                $session->saveChanges();
                # endregion
            } finally {
                $session->close();
            }

            # region disable_tracking_2
            $sessionOptions = new SessionOptions();
            // Disable tracking for all entities in the session's options
            $sessionOptions->setNoTracking(true);

            $session = $store->openSession($sessionOptions);
            try {
                // Load any entity, it will Not be tracked by the session
                /** @var Employee $employee1 */
                $employee1 = $session->load(Employee::class, "employees/1-A");

                // Loading again from same document will result in a new entity instance
                $employee2 = $session->load(Employee::class, "employees/1-A");

                // Entities instances are not the same
                $this->assertNotEquals($employee1, $employee2);
            } finally {
                $session->close();
            }
            # endregion

            # region disable_tracking_3
            $session = $store->openSession();
            try {
                // Define a query
                /** @var array<Employee> $employeesResults */
                $employeesResults = $session->query(Employee::class)
                    // Set NoTracking, all resulting entities will not be tracked
                    ->noTracking()
                    ->whereEquals("FirstName", "Robert")
                    ->toList();

                // The following modification will not be tracked for SaveChanges
                $firstEmployee = $employeesResults[0];
                $firstEmployee->setLastName("NewName");

                // Change to 'firstEmployee' will not be persisted
                $session->saveChanges();
            } finally {
                $session->close();
            }
            # endregion

            # region disable_tracking_3_documentQuery
            $session = $store->openSession();
            try {
                // Define a query
                /** @var array<Employee> $employeesResults */
                $employeesResults = $session->advanced()->documentQuery(Employee::class)
                    // Set NoTracking, all resulting entities will not be tracked
                    ->noTracking()
                    ->whereEquals("FirstName", "Robert")
                    ->toList();

                // The following modification will not be tracked for SaveChanges
                $firstEmployee = $employeesResults[0];
                $firstEmployee->setLastName("NewName");

                // Change to 'firstEmployee' will not be persisted
                $session->saveChanges();
            } finally {
                $session->close();
            }
            # endregion
        } finally {
            $store->close();
        }


        # region disable_tracking_4
        // Define the 'ignore' convention on your document store
        $conventions = new DocumentConventions();
        $conventions->setShouldIgnoreEntityChanges(
        // Define for which entities tracking should be disabled
        // Tracking will be disabled ONLY for entities of type Employee whose FirstName is Bob
            function ($session, $entity, $id) {
                return $entity instanceof Employee && $entity->getFirstName() == "Bob";
            }
        );

        $store = new DocumentStore();
        $store->setConventions($conventions);
        $store->initialize();
        try {
            $session = $store->openSession();
            try {
                $employee1 = new Employee();
                $employee1->setId("employees/1");
                $employee1->setFirstName("Alice");

                $employee2 = new Employee();
                $employee2->setId("employees/2");
                $employee2->setFirstName("Bob");

                $session->store($employee1);      // This entity will be tracked
                $session->store($employee2);      // Changes to this entity will be ignored

                $session->saveChanges();          // Only employee1 will be persisted

                $employee1->setFirstName("Bob");  // Changes to this entity will now be ignored
                $employee2->setFirstName("Alice");// This entity will now be tracked

                $session->saveChanges();          // Only employee2 is persisted
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
        # endregion

    }
}
