<?php

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Session\SessionOptions;
use RavenDB\Documents\Session\TransactionMode;
use RavenDB\Documents\Lazy;
use RavenDB\Samples\Infrastructure\Orders\Supplier;

# region lazy_productClass
class Product
{
    public ?string $id = null;
    public ?string $name = null;
    public ?string $supplierId = null;

    public function getId(): ?string
    {
        return $this->id;
    }

    public function setId(?string $id): void
    {
        $this->id = $id;
    }

    public function getName(): ?string
    {
        return $this->name;
    }

    public function setName(?string $name): void
    {
        $this->name = $name;
    }

    public function getSupplierId(): ?string
    {
        return $this->supplierId;
    }

    public function setSupplierId(?string $supplierId): void
    {
        $this->supplierId = $supplierId;
    } // The related document ID
}

# endregion

class LazyExample
{
    public function example(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region lazy_Load
                /** @var Lazy<Employee> $lazyEmployee */
                $lazyEmployee = $session
                    // Add a call to Lazily
                    ->advanced()->lazily()
                    // Document will Not be loaded from the database here, no server call is made
                    ->load(Employee::class, "employees/1-A");

                $employee = $lazyEmployee->getValue(); // 'Load' operation is executed here
                // The employee entity is now loaded & tracked by the session
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region lazy_LoadWithInclude
                /** @var Lazy<Product> $lazyProduct */
                $lazyProduct = $session
                    // Add a call to Lazily
                    ->advanced()->lazily()
                    // Request to include the related Supplier document
                    // Documents will Not be loaded from the database here, no server call is made
                    ->include("SupplierId")
                    ->load(Product::class, "products/1-A");

                // 'Load with include' operation will be executed here
                // Both documents will be retrieved from the database
                $product = $lazyProduct->getValue();
                // The product entity is now loaded & tracked by the session

                // Access the related document, no additional server call is made
                $supplier = $session->load(Supplier::class, $product->getSuppierId());
                // The supplier entity is now also loaded & tracked by the session
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region lazy_LoadStartingWith
                /** @var Lazy<array<Employee>> $lazyEmployees */
                $lazyEmployees = $session
                    // Add a call to Lazily
                    ->advanced()->lazily()
                    // Request to load entities whose ID starts with 'employees/'
                    // Documents will Not be loaded from the database here, no server call is made
                    ->loadStartingWith(Employee::class, "employees/");

                $employees = $lazyEmployees->getValue(); // 'Load' operation is executed here
                // The employee entities are now loaded & tracked by the session
                # endregion
            } finally {
                $session->close();
            }

            # region lazy_ConditionalLoad
            // Create document and get its change-vector:
            $changeVector = null;
            $session1 = $store->openSession();
            try {
                $employee = new Employee();
                $session1->store($employee, "employees/1-A");
                $session1->saveChanges();

                // Get the tracked entity change-vector
                $changeVector = $session1->advanced()->getChangeVectorFor($employee);
            } finally {
                $session1->close();
            }

            // Conditionally lazy-load the document:
            $session2 = $store->openSession();
            try {
                $lazyEmployee = $session2
                    // Add a call to Lazily
                    ->advanced()->lazily()
                    // Document will Not be loaded from the database here, no server call is made

                    ->conditionalLoad(Employee::class, "employees/1-A", $changeVector);

                var
                $loadedItem = $lazyEmployee->getValue(); // 'ConditionalLoad' operation is executed here
                $employee = $loadedItem->getEntity();

                // If ConditionalLoad has actually fetched the document from the server (logic described above)
                // then the employee entity is now loaded & tracked by the session
            } finally {
                $session2->close();
            }
            # endregion

            $session = $store->openSession();
            try {
                # region lazy_Query
                // Define a
                // lazy query:
                $lazyEmployees = $session
                    ->query(Employee::class)
                    ->whereEquals("FirstName", "John")
                    // Add a call to Lazily, the query will Not be executed here
                    ->lazily();

                $employees = $lazyEmployees->getValue(); // Query is executed here

                // Note: Since query results are not projected,
                // then the resulting employee entities will be tracked by the session.
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region lazy_Revisions
                /** @var Lazy<array<Employee>> $lazyRevisions */
                $lazyRevisions = $session
                    // Add a call to Lazily
                    ->advanced()->revisions()->lazily()
                    // Revisions will Not be fetched here, no server call is made
                    ->getFor(Employee::class, "employees/1-A");

                // Usage is the same for the other get revisions methods:
                // .Get()
                // .GetMetadataFor()

                /** @var array<Employee> $revisions */
                $revisions = $lazyRevisions->getValue(); // Getting revisions is executed here
                # endregion
            } finally {
                $session->close();
            }

            # region lazy_CompareExchange
            $sessionOptions = new SessionOptions();
            $sessionOptions->setTransactionMode(TransactionMode::clusterWide());
            $session = $store->openSession($sessionOptions);
            try {
                // Create compare-exchange value:
                $session->advanced()->clusterTransaction()
                    ->createCompareExchangeValue(key: "someKey", value: "someValue");
                $session->SaveChanges();

                // Get the compare-exchange value lazily:
                /** @var Lazy<CompareExchangeValue<string>> $lazyCmpXchg */
                $lazyCmpXchg = $session
                    // Add a call to Lazily
                    ->advanced()->clusterTransaction()->lazily()
                    // Compare-exchange values will Not be fetched here, no server call is made
                    ->getCompareExchangeValue(null, "someKey");

                // Usage is the same for the other method:
                // .GetCompareExchangeValues()

                /** @var CompareExchangeValue<string> $cmpXchgValue */
                $cmpXchgValue = $lazyCmpXchg->getValue(); // Getting compare-exchange value is executed here
            } finally {
                $session->close();
            }
            # endregion

            $session = $store->openSession();
            try {
                # region lazy_ExecuteAll_Implicit
                // Define multiple lazy requests
                $lazyUser1 = $session->advanced()->lazily()->load(User::class, "users/1-A");
                $lazyUser2 = $session->advanced()->lazily()->load(User::class, "users/2-A");

                $lazyEmployees = $session->query(Employee::class)
                    ->lazily();
                $lazyProducts = $session->query(Product::class)
                    ->search("Name", "Ch*")
                    ->lazily();

                // Accessing the value of ANY of the lazy instances will trigger
                // the execution of ALL pending lazy requests held up by the session
                // This is done in a SINGLE server call
                $user1 = $lazyUser1->getValue();

                // ALL the other values are now also available
                // No additional server calls are made when accessing these values
                $user2 = $lazyUser2->getValue();
                $employees = $lazyEmployees->getValue();
                $products = $lazyProducts->getValue();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                    # region lazy_ExecuteAll_Explicit
                    // Define multiple lazy requests
                    $lazyUser1 = $session->advanced()->lazily->load(User::class, "users/1-A");
                    $lazyUser2 = $session->advanced()->lazily->load(User::class, "users/2-A");

                    $lazyEmployees = $session->query(Employee::class)
                        ->lazily();
                    $lazyProducts = $session->query(Product::class)
                        ->search("Name", "Ch*")
                        ->lazily();

                    // Explicitly call 'ExecuteAllPendingLazyOperations'
                    // ALL pending lazy requests held up by the session will be executed in a SINGLE server call
                    $session->advanced()->eagerly()->executeAllPendingLazyOperations();

                    // ALL values are now available
                    // No additional server calls are made when accessing the values
                    $user1 = $lazyUser1->getValue();
                    $user2 = $lazyUser2->getValue();
                    $employees = $lazyEmployees->getValue();
                    $products = $lazyProducts->getValue();
                    # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }
}
