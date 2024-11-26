<?php

use RavenDB\Documents\Session\DocumentQueryInterface;
use RavenDB\Documents\Session\RawDocumentQueryInterface;
use RavenDB\Samples\Infrastructure\DocumentStoreHolder;
use RavenDB\Samples\Infrastructure\Orders\Employee;
use RavenDB\Samples\Infrastructure\Orders\Product;

class HowToQuery
{
    public function samples(): void
    {
        $store = DocumentStoreHolder::getStore();
        try {
            $session = $store->openSession();
            try {
                # region query_1_1
                // This is a Full Collection Query
                // No auto-index is created since no filtering is applied

                $allEmployees = $session
                    ->query(Employee::class)  // Query for all documents from 'Employees' collection
                    ->toList();                         // Execute the query

                // All 'Employee' entities are loaded and will be tracked by the session
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region query_2_1
                // Query collection by document ID
                // No auto-index is created when querying only by ID

                $employee = $session
                    ->query(Employee::class)
                    ->whereEquals("Id", "employees/1-A") // Query for specific document from 'Employees' collection
                    ->firstOrDefault();                                 // Execute the query

                // The resulting 'Employee' entity is loaded and will be tracked by the session
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region query_3_1
                // Query collection - filter by document field

                // An auto-index will be created if there isn't already an existing auto-index
                // that indexes this document field

                $employees = $session
                    ->query(Employee::class)
                    ->whereEquals("FirstName", "Robert") // Query for all 'Employee' documents that match this predicate
                    ->ToList();                                         // Execute the query

                // The resulting 'Employee' entities are loaded and will be tracked by the session
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region query_4_1
                // Query collection - page results
                // No auto-index is created since no filtering is applied

                $products = $session
                    ->query(Product::class)
                    ->skip(5)   // Skip first 5 results
                    ->take(10)  // Load up to 10 entities from 'Products' collection
                    ->toList(); // Execute the query

                // The resulting 'Product' entities are loaded and will be tracked by the session
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region query_5_1
                // Query with DocumentQuery - filter by document field

                // An auto-index will be created if there isn't already an existing auto-index
                // that indexes this document field

                $employees = $session
                    ->advanced()->documentQuery(Employee::class) // Use DocumentQuery
                    ->whereEquals("FirstName", "Robert")    // Query for all 'Employee' documents that match this predicate
                    ->toList();                                            // Execute the query

                // The resulting 'Employee' entities are loaded and will be tracked by the session
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region query_6_1
                // Query with RawQuery - filter by document field

                // An auto-index will be created if there isn't already an existing auto-index
                // that indexes this document field

                $employees = $session
                    // Provide RQL to RawQuery
                    ->advanced()->rawQuery(Employee::class, "from 'Employees' where FirstName = 'Robert'")
                    // Execute the query
                    ->toList();

                // The resulting 'Employee' entities are loaded and will be tracked by the session
                # endregion
            } finally {
                $session->close();
            }

        } finally {
            $store->close();
        }
    }
}

interface FooInterface
{
    # region syntax
    // Methods for querying a collection OR an index:
    // ================================================

    public function query(?string $className, $collectionOrIndexName = null): DocumentQueryInterface;

    public function documentQuery(?string $className, $indexNameOrClass = null, ?string $collectionName = null, bool $isMapReduce = false): DocumentQueryInterface;

    // RawQuery:
    // =========
    public function rawQuery(?string $className, string $query): RawDocumentQueryInterface;
    # endregion
}
