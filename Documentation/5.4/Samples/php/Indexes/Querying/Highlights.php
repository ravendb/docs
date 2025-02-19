<?php

namespace RavenDB\Samples\Indexes\Querying;

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Indexes\AbstractIndexCreationTask;
use RavenDB\Documents\Indexes\FieldIndexing;
use RavenDB\Documents\Indexes\FieldStorage;
use RavenDB\Documents\Indexes\FieldTermVector;
use RavenDB\Documents\Queries\Highlighting\HighlightingOptions;
use RavenDB\Documents\Queries\Highlighting\Highlightings;
use RavenDB\Samples\Infrastructure\Orders\Employee;

class Highlights
{
    public function examples(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region highlight_1

                $managerHighlights = new Highlightings();

                /** @var array<Employee> $employeesResults */
                $employeesResults = $session
                     // Query the map index
                    ->query(Employees_ByNotes_IndexEntry::class, Employees_ByNotes::class)
                     // Search for documents containing the term 'manager'
                    ->search("EmployeeNotes", "manager")
                     // Request to highlight the searched term by calling 'Highlight'
                    ->highlight("EmployeeNotes", 35, 2, null, $managerHighlights)
                    ->ofType(Employee::class)
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region highlight_3

                $managerHighlights = new Highlightings();

                /** @var array<Employee> $employeesResults */
                $employeesResults = $session->advanced()
                     // Query the map index
                    ->documentQuery(Employees_ByNotes_IndexEntry::class, Employees_ByNotes::class)
                     // Search for documents containing the term 'manager'
                    ->search("EmployeeNotes", "manager")
                     // Request to highlight the searched term by calling 'Highlight'
                    ->highlight("EmployeeNotes", 35, 2, null, $managerHighlights)
                    ->ofType(Employee::class)
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region highlight_4

                $managerHighlights = new Highlightings();

                /** @var array<Employee> $employeesResults */
                $employeesResults = $session
                     // Query the map index
                    ->query(Employees_ByNotes_IndexEntry::class, Employees_ByNotes::class)
                     // Request to highlight the searched term by calling 'Highlight'
                    ->highlight("EmployeeNotes", 35, 2, null, $managerHighlights)
                     // Search for documents containing the term 'manager'
                    ->whereEquals("EmployeeNotes", "manager")
                    ->ofType(Employee::class)
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region highlight_6

                $managerHighlights = new Highlightings();

                /** @var array<Employee> $employeesResults */
                $employeesResults = $session->advanced()
                     // Query the map index
                    ->documentQuery(Employees_ByNotes_IndexEntry::class, Employees_ByNotes::class)
                     // Request to highlight the searched term by calling 'Highlight'
                    ->highlight("EmployeeNotes", 35, 2, null, $managerHighlights)
                     // Search for documents containing the term 'manager'
                    ->whereEquals("EmployeeNotes", "manager")
                    ->ofType(Employee::class)
                    ->toList();
                # endregion

                # region highlight_7
                // 'employeesResults' contains all Employee DOCUMENTS that contain the term 'manager'.
                // 'managerHighlights' contains the text FRAGMENTS that highlight the 'manager' term.

                $builder = "<ul>";

                foreach ($employeesResults as $employee)
                {
                    // Call 'GetFragments' to get all fragments for the specified employee Id
                    $fragments = $managerHighlights->getFragments($employee->getId());
                    foreach ($fragments as $fragment)
                    {
                        $builder .= "<li>Doc: " . $employee->getId() . "</li>";
                        $builder .= "<li>Fragment: " . $fragment . "</li>";
                        $builder .= "<li></li>";
                    }
                }

                $fragmentsHtml = $builder . "</ul>";

                // The resulting fragmentsHtml:
                // ============================

                // <ul>
                //   <li>Doc: employees/2-A</li>
                //   <li>Fragment:  to sales <b style="background:yellow">manager</b> in January</li>
                //   <li>Doc: employees/5-A</li>
                //   <li>Fragment:  to sales <b style="background:yellow">manager</b> in March</li>
                //   <li></li>
                // </ul>
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region highlight_8
                // Define the key by which the resulting fragments are grouped:
                // ============================================================
                $options = new HighlightingOptions();
                // Set 'GroupKey' to be the index's group-by key
                // The resulting fragments will be grouped per 'Country'
                $options->setGroupKey("Country");

                $agentHighlights = new Highlightings();

                // Query the map-reduce index:
                // ===========================
                /** @var array<ContactDetailsPerCountry_IndexEntry> $detailsPerCountry */
                $detailsPerCountry = $session
                    ->query(ContactDetailsPerCountry_IndexEntry::class, ContactDetailsPerCountry::class)
                     // Search for results containing the term 'agent'
                    ->search("ContactDetails", "agent")
                     // Request to highlight the searched term by calling 'Highlight'
                     // Pass the defined 'options'
                    ->highlight("ContactDetails", 35, 2, $options, $agentHighlights)
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region highlight_10
                // Define the key by which the resulting fragments are grouped
                // ===========================================================
                $options = new HighlightingOptions();

                // Set 'GroupKey' to be the index's group-by key
                // The resulting fragments will be grouped per 'Country'
                $options->setGroupKey("Country");

                $agentHighlights = new Highlightings();

                // Query the map-reduce index:
                // ===========================
                /** @var array<ContactDetailsPerCountry_IndexEntry> $detailsPerCountry */
                $detailsPerCountry = $session->advanced()
                    ->documentQuery(ContactDetailsPerCountry_IndexEntry::class, ContactDetailsPerCountry::class)
                     // Search for results containing the term 'agent'
                    ->search("ContactDetails", "agent")
                     // Request to highlight the searched term by calling 'Highlight'
                     // Pass the defined 'options'
                    ->highlight("ContactDetails", 35, 2, $options, $agentHighlights)
                    ->toList();
                # endregion

                # region highlight_11
                // 'detailsPerCountry' contains the contacts details grouped per country.
                // 'agentHighlights' contains the text FRAGMENTS that highlight the 'agent' term.

                $builder = "<ul>";

                foreach ($detailsPerCountry as $item)  {
                    // Call 'GetFragments' to get all fragments for the specified country key
                    $fragments = $agentHighlights->getFragments($item->getCountry());
                    foreach ($fragments as $fragment)
                    {
                        $builder .= "<li>Country: " . $item->getCountry() . "</li>";
                        $builder .= "<li>Fragment: " . $fragment . "</li>";
                        $builder .= "<li></li>";
                    }
                }

                $fragmentsHtml = $builder . "</ul>";

                // The resulting fragmentsHtml:
                // ============================

                // <ul>
                //   <li>Country: UK</li>
                //   <li>Fragment: Devon Sales <b style="background:yellow">Agent</b> Helen Bennett</li>
                //   <li></li>
                //   <li>Country: France</li>
                //   <li>Fragment: Sales <b style="background:yellow">Agent</b> Carine Schmit</li>
                //   <li></li>
                //   <li>Country: France</li>
                //   <li>Fragment: Saveley Sales <b style="background:yellow">Agent</b> Paul Henriot</li>
                //   <li></li>
                //   <li>Country: Argentina</li>
                //   <li>Fragment: Simpson Sales <b style="background:yellow">Agent</b> Yvonne Moncad</li>
                //   <li></li>
                //   <li>Country: Argentina</li>
                //   <li>Fragment: Moncada Sales <b style="background:yellow">Agent</b> Sergio</li>
                //   <li></li>
                //   <li>Country: Brazil</li>
                //   <li>Fragment: Sales <b style="background:yellow">Agent</b> Anabela</li>
                //   <li></li>
                //   <li>Country: Belgium</li>
                //   <li>Fragment: Dewey Sales <b style="background:yellow">Agent</b> Pascale</li>
                //   <li></li>
                // </ul>
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
// Define a Map index:
// ===================
// The IndexEntry class defines index-field 'EmployeeNotes'
class Employees_ByNotes_IndexEntry
{
    public ?string $employeeNotes = null;

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

        // The 'Map' function defines the content of index-field 'EmployeeNotes'
        $this->map =
            "from employee in docs.Employees " .
            "select new { " .
            "   EmployeeNotes = employee.notes[0] " .
            "}";

        // Configure index-field 'EmployeeNotes' for highlighting:
        // =======================================================
        $this->store("EmployeeNotes", FieldStorage::yes());
        $this->index("EmployeeNotes", FieldIndexing::search());
        $this->termVector("EmployeeNotes", FieldTermVector::withPositionsAndOffsets());
    }
}
# endregion

# region index_2
// Define a Map-Reduce index:
// ==========================

// The IndexEntry class defines the index-fields
class ContactDetailsPerCountry_IndexEntry
{
    private ?string $country = null;
    private ?string $contactDetails = null;

    public function getCountry(): ?string
    {
        return $this->country;
    }

    public function setCountry(?string $country): void
    {
        $this->country = $country;
    }

    public function getContactDetails(): ?string
    {
        return $this->contactDetails;
    }

    public function setContactDetails(?string $contactDetails): void
    {
        $this->contactDetails = $contactDetails;
    }
}
class ContactDetailsPerCountry extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();
        
        // The 'Map' function defines what will be indexed from each document in the collection
        $this->map =
            "from company in docs.Companies " .
            "select new { " .
            "   Country = company.Address.Country, " .
            "   ContactDetails = company.Contact.Name + ' ' + company.Contact.Title ".
            "}";

        // The 'Reduce' function specifies how data is grouped and aggregated
        $this->reduce =
            "from result in results " .
            "group result by result.country into g " .
            "select new { " .
            // Set 'Country' as the group-by key
            // 'ContactDetails' will be grouped per 'Country'
            "   Country = g.key, " .
            // Specify the aggregation
            // here we use string.Join as the aggregation function
            "   ContactDetails = string.Join(\" \", g.Select(x => x.contact_details) )" .
            "}" ;

        // Configure index-field 'Country' for Highlighting:
        // =================================================
        $this->store("Country", FieldStorage::yes());

        // Configure index-field 'ContactDetails' for Highlighting:
        // ========================================================
        $this->store("ContactDetails", FieldStorage::yes());
        $this->index("ContactDetails", FieldIndexing::search());
        $this->termVector("ContactDetails", FieldTermVector::withPositionsAndOffsets());
    }
}
# endregion
