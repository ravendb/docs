<?php

namespace RavenDB\Samples\Indexes\Querying;

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Indexes\AbstractIndexCreationTask;
use RavenDB\Samples\Infrastructure\Orders\Employee;

# region the_index
// The IndexEntry class defines the index-fields
class Employees_ByName_IndexEntry
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


// The index definition:
class Employees_ByName extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        // The 'Map' function defines the content of the INDEX-fields
        // * The content of INDEX-fields 'FirstName' & 'LastName'
        //   is composed of the relevant DOCUMENT-fields.

        $this->map = "from e in docs.Employees select new {FirstName = e.FirstName, LastName = e.LastName}";

        // * The index-fields can be queried on to fetch matching documents.
        //   You can query and filter Employee documents based on their first or last names.

        // * Employee documents that do Not contain both 'FirstName' and 'LastName' fields
        //   will Not be indexed.

        // * Note: the INDEX-field name does Not have to be exactly the same
        //   as the DOCUMENT-field name.
    }
}
# endregion

class QueryIndex
{
    public function Sample(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region index_query_1_1
                // Query the 'Employees' collection using the index - without filtering
                // (Open the 'Index' tab to view the index class definition)
                /** @var array<Employee> $employees */
                $employees = $session
                     // Pass the queried collection as the first generic parameter
                     // Pass the index class as the second generic parameter
                    ->query(Employee::class, Employees_ByName::class)
                     // Execute the query
                    ->toList();

                // All 'Employee' documents that contain DOCUMENT-fields 'FirstName' and\or 'LastName' will be returned
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region index_query_1_3
                // Query the 'Employees' collection using the index - without filtering
                /** @var array<Employee> $employees */
                $employees = $session
                     // Pass the index name as a parameter
                     // Use slash `/` in the index name, replacing the underscore `_` from the index class definition
                    ->query(Employee::class, "Employees/ByName")
                     // Execute the query
                    ->toList();

                // All 'Employee' documents that contain DOCUMENT-fields 'FirstName' and\or 'LastName' will be returned
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region index_query_2_1
                // Query the 'Employees' collection using the index - filter by INDEX-field
                /** @var array<Employee> $employees */
                $employees = $session
                     // Pass the IndexEntry class as the first generic parameter
                     // Pass the index class as the second generic parameter
                    ->query(Employees_ByName_IndexEntry::class, Employees_ByName::class)
                     // Filter the retrieved documents by some predicate on an INDEX-field
                    ->whereEquals("LastName", "King")
                     // Specify the type of the returned document entities
                    ->ofType(Employee::class)
                     // Execute the query
                    ->toList();

                // Results will include all documents from 'Employees' collection whose 'LastName' equals to 'King'.
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region index_query_3_1
                // Query the 'Employees' collection using the index - page results

                // This example is based on the previous filtering example
                /** @var array<Employee> $employees */
                $employees = $session
                    ->query(Employees_ByName_IndexEntry::class, Employees_ByName::class)
                    ->whereEquals("LastName", "King")
                    ->skip(5)  // Skip first 5 results
                    ->take(10) // Retrieve up to 10 documents
                    ->ofType(Employee::class)
                    ->toList();

                // Results will include up to 10 matching documents
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region index_query_4_1
                // Query the 'Employees' collection using the index - filter by INDEX-field

                /** @var array<Employee> $employees */
                $employees = $session->advanced()
                     // Pass the IndexEntry class as the first generic parameter
                     // Pass the index class as the second generic parameter
                    ->documentQuery(Employees_ByName_IndexEntry::class, Employees_ByName::class)
                     // Filter the retrieved documents by some predicate on an INDEX-field
                    ->whereEquals("LastName", "King")
                     // Specify the type of the returned document entities
                    ->ofType(Employee::class)
                     // Execute the query
                    ->toList();

                // Results will include all documents from 'Employees' collection whose 'LastName' equals to 'King'.
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region index_query_4_3
                // Query the 'Employees' collection using the index - filter by INDEX-field
                /** @var array<Employee> $employees */
                $employees = $session->advanced()
                     // Pass the IndexEntry class as the generic param
                     // Pass the index name as the param
                     // Use slash `/` in the index name, replacing the underscore `_` from the index class definition
                    ->documentQuery(Employees_ByName_IndexEntry::class, "Employees/ByName")
                     // Filter the retrieved documents by some predicate on an INDEX-field
                    ->whereEquals("LastName", "King")
                     // Specify the type of the returned document entities
                    ->ofType(Employee::class)
                     // Execute the query
                    ->toList();

                // Results will include all documents from 'Employees' collection whose 'LastName' equals to 'King'.
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region index_query_5_1
                // Query with RawQuery - filter by INDEX-field

                /** @var array<Employee> $employees */
                $employees = $session->advanced()
                     // Provide RQL to RawQuery
                    ->rawQuery(Employee::class, "from index 'Employees/ByName' where LastName == 'King'")
                     // Execute the query
                    ->toList();

                // Results will include all documents from 'Employees' collection whose 'LastName' equals to 'King'.
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }
}

