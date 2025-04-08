<?php

namespace RavenDB\Samples\Indexes\Querying;

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Indexes\AbstractIndexCreationTask;
use RavenDB\Documents\Indexes\FieldIndexing;
use RavenDB\Documents\Queries\SearchOperator;
use RavenDB\Samples\Infrastructure\Orders\Employee;
use RavenDB\Type\TypedArray;

class Searching
{
    public function examples(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region search_1
                /** @var array<Employee> $employees */
                $employees = $session
                     // Query the index
                    ->query(Employees_ByNotes_IndexEntry::class, Employees_ByNotes::class)
                     // Call 'Search':
                     // pass the index field that was configured for FTS and the term to search for.
                    ->search("EmployeeNotes", "French")
                    ->ofType(Employee::class)
                    ->toList();

                // * Results will contain all Employee documents that have 'French' in their 'Notes' field.
                //
                // * Search is case-sensitive since field was indexed using the 'WhitespaceAnalyzer'
                //   which preserves casing.
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region search_3
                /** @var array<Employee> $employees */
                $employees = $session->advanced()
                     // Query the index
                    ->documentQuery(Employees_ByNotes_IndexEntry::class, Employees_ByNotes::class)
                     // Call 'Search':
                     // pass the index field that was configured for FTS and the term to search for.
                    ->search("EmployeeNotes", "French")
                    ->ofType(Employee::class)
                    ->toList();

                // * Results will contain all Employee documents that have 'French' in their 'Notes' field.
                //
                // * Search is case-sensitive since field was indexed using the 'WhitespaceAnalyzer'
                //   which preserves casing.
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region search_4
                /** @var array<Employee> $employees */
                $employees = $session
                     // Query the static-index
                    ->query(Employees_ByEmployeeData_IndexEntry::class, Employees_ByEmployeeData::class)
                     // A logical OR is applied between the following two Search calls:
                    ->search("EmployeeData", "Manager")
                     // A logical AND is applied between the following two terms:
                    ->search("EmployeeData", "French Spanish", SearchOperator::and())
                    ->ofType(Employee::class)
                    ->toList();

                // * Results will contain all Employee documents that have:
                //   ('Manager' in any of the 4 document-fields that were indexed)
                //   OR
                //   ('French' AND 'Spanish' in any of the 4 document-fields that were indexed)
                //
                // * Search is case-insensitive since the default analyzer is used
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region search_6
                /** @var array<Employee> $employees */
                $employees = $session->advanced()
                     // Query the static-index
                    ->documentQuery(Employees_ByEmployeeData_IndexEntry::class, Employees_ByEmployeeData::class)
                    ->openSubclause()
                     // A logical OR is applied between the following two Search calls:
                    ->search("EmployeeData", "Manager")
                     // A logical AND is applied between the following two terms:
                    ->search("EmployeeData", "French Spanish", SearchOperator::and())
                    ->closeSubclause()
                    ->ofType(Employee::class)
                    ->toList();

                // * Results will contain all Employee documents that have:
                //   ('Manager' in any of the 4 document-fields that were indexed)
                //   OR
                //   ('French' AND 'Spanish' in any of the 4 document-fields that were indexed)
                //
                // * Search is case-insensitive since the default analyzer is used
                # endregion
            } finally {
                $session->close();
            }
        
            $session = $store->openSession();
            try {
                # region search_7
                $results = $session->query(Products_ByAllValues_IndexEntry::class, Products_ByAllValues::class)
                        ->search("allValues", "tofu")
                        ->ofType(Product::class)
                        ->toList();
                        
                // * Results will contain all Product documents that have 'tofu'
                //   in ANY of their fields.
                //
                // * Search is case-insensitive since the default analyzer is used.
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }
}

# region index_1
// The IndexEntry class defines the index-fields
class Employees_ByNotes_IndexEntry
{
    private ?string $employeeNotes = null;

    public function getEmployeeNotes(): ?string
    {
        return $this->employeeNotes;
    }

    public function setEmployeeNotes(?string $employeeNotes): void
    {
        $this->employeeNotes = $employeeNotes;
    }
}
class Employees_ByNotes extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        // The 'Map' function defines the content of the index-fields
        $this->map =
            "from employee in docs.Employees " .
            "select new " .
            "{ " .
               " employee_notes = employee.Notes[0]" .
            "}";

        # Configure the index-field for FTS:
        # Set 'FieldIndexing.Search' on index-field 'employee_notes'
        $this->index("employee_notes", FieldIndexing::search());

        # Optionally: Set your choice of analyzer for the index-field:
        # Here the text from index-field 'EmployeeNotes' will be tokenized by 'WhitespaceAnalyzer'.
        $this->analyze("employee_notes", "WhitespaceAnalyzer");

        # Note:
        # If no analyzer is set then the default 'RavenStandardAnalyzer' is used.
    }
}
# endregion

# region index_2
class EmployeeData
{
    private ?string $firstName = null;
    private ?string $lastName = null;
    private ?string $title = null;
    private ?string $notes = null;

    // ... getters and setters
}

class EmployeeDataArray extends TypedArray
{
    protected function __construct()
    {
        parent::__construct(EmployeeData::class);
    }
}

class Employees_ByEmployeeData_IndexEntry
{
    public ?EmployeeDataArray  $employeeData = null;
}
class Employees_ByEmployeeData extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->map =
            "from employee in docs.Employees " .
            "select new {" .
            "  EmployeeData = " .
            "  {" .
            # Multiple document-fields can be indexed
            # into the single index-field 'employee_data'
            "    employee.FirstName," .
            "    employee.LastName," .
            "    employee.Title," .
            "    employee.Notes" .
            "  }" .
            "}";

        // Configure the index-field for FTS:
        // Set 'FieldIndexing.Search' on index-field 'EmployeeData'
        $this->index("EmployeeData", FieldIndexing::search());

        // Note:
        // Since no analyzer is set then the default 'RavenStandardAnalyzer' is used.
    }
}
# endregion

# region index_3
class Products_ByAllValues_IndexEntry
{
    public ?string $allValues = null;
    public function getAllValues(): ?string
    {
        return $this->allValues;
    }
    public function setAllValues(?string $allValues): void
    {
        $this->allValues = $allValues
    }
}

class Products_ByAllValues extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->map = "docs.Products.Select(product => new { " .
            # Use the 'AsJson' method to convert the document into a JSON-like structure
            # and call 'Select' to extract only the values of each property
            "    allValues = this.AsJson(product).Select(x => x.Value) " .
            "})";

        # Configure the index-field for FTS:
        # Set 'FieldIndexing::search' on index-field 'allValues'
        $this->index("allValues", FieldIndexing::search());
        
        # Set the search engine type to Lucene:
        $this->setSearchEngineType(SearchEngineType::lucene());
    }
}
# endregion
