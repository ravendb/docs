<?php

use RavenDB\Documents\Indexes\AbstractIndexCreationTask;
use RavenDB\Documents\Indexes\FieldIndexing;
use RavenDB\Documents\Indexes\FieldStorage;
use RavenDB\Documents\Indexes\FieldTermVector;
use RavenDB\Documents\Queries\Highlighting\HighlightingOptions;
use RavenDB\Documents\Queries\Highlighting\Highlightings;
use RavenDB\Documents\Queries\QueryData;
use RavenDB\Documents\Session\DocumentQueryInterface;
use RavenDB\Samples\Infrastructure\DocumentStoreHolder;
use RavenDB\Type\StringArray;


class ContentSearchIndex extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->map = "docs.Select(doc => new { " .
            "  text = doc.text" .
            "})";

        $this->index("text", FieldIndexing::search());
        $this->store("text", FieldStorage::yes());
        $this->termVector("text", FieldTermVector::withPositionsAndOffsets());
    }
}

class SearchItem
{
    private ?string $id = null;
    private ?string $text = null;

    public function getId(): ?string
    {
        return $this->id;
    }

    public function setId(?string $id): void
    {
        $this->id = $id;
    }

    public function getText(): ?string
    {
        return $this->text;
    }

    public function setText(?string $text): void
    {
        $this->text = $text;
    }
}

class EmployeeDetails
{
    private ?string $name = null;
    private ?string $title = null;

    public function getName(): ?string
    {
        return $this->name;
    }

    public function setName(?string $name): void
    {
        $this->name = $name;
    }

    public function getTitle(): ?string
    {
        return $this->title;
    }

    public function setTitle(?string $title): void
    {
        $this->title = $title;
    }
}

interface FooInterface
{
    # region syntax_1
    function highlight(
        ?string              $fieldName,
        int                  $fragmentLength,
        int                  $fragmentCount,
        ?HighlightingOptions $options,
        Highlightings        &$highlightings
    ): DocumentQueryInterface;
    # endregion
}

class Foo
{
    # region syntax_2
    private ?string $groupKey;
    private ?StringArray $preTags = null;
    private ?StringArray $postTags = null;

    // getters and setters
    # endregion
}


class HighlightingsClass
{
    # region syntax_3
    private ?string $fieldName = null;
    public function getResultIndents(): array;
    # endregion
}

interface FooInterface2
{
    # region syntax_4
    public function getFragments(?string $key): array;
    # endregion
}

class HighlightQueryResults
{
    public function examples(): void
    {
        $store = DocumentStoreHolder::getStore();
        try {
            $session = $store->openSession();
            try {
                # region highlight_1
                // Make a full-text search dynamic query:
                // ======================================

                $highlightings = new Highlightings();

                /** @var array<Employee> $employeesResults */
                $employeesResults = $session
                    // Make a dynamic query on 'Employees' collection
                    ->query(Employee::class)
                    // Search for documents containing the term 'sales' in their 'Notes' field
                    ->search("Notes", "sales")
                    // Request to highlight the searched term by calling 'highlight()'
                    ->highlight(
                        "Notes", // The document-field name in which we search
                        35,           // Max length of each text fragment
                        4,            // Max number of fragments to return per document
                        null,           // Put null to use default options
                        $highlightings) // An out param for getting the highlighted text fragments

                    // Execute the query
                    ->toList();
                # endregion

                # region fragments_1
                // Process results:
                // ================

                // 'employeesResults' contains all Employee DOCUMENTS that have 'sales' in their 'Notes' field.
                // 'salesHighlights' contains the text FRAGMENTS that highlight the 'sales' term.

                $builder = '<ul>';

                /** @var SearchItem $employee */
                foreach ($employeesResults as $employee) {
                    // Call 'GetFragments' to get all fragments for the specified employee Id
                    $fragments = $highlightings->getFragments($employee->getId());
                    foreach ($fragments as $fragment) {
                        $builder .= '<li>Doc: ' . $employee->getId() . ' Fragment: ' . $fragment . '</li>';
                    }
                }

                $builder .= '</ul>';
                $fragmentsHtml = $builder;

                // The resulting fragmentsHtml:
                // ============================

                // <ul>
                //   <li>Doc: employees/2-A Fragment: company as a <b style="background:yellow">sales</b></li>
                //   <li>Doc: employees/2-A Fragment: promoted to <b style="background:yellow">sales</b> manager in</li>
                //   <li>Doc: employees/2-A Fragment: president of <b style="background:yellow">sales</b> in March 1993</li>
                //   <li>Doc: employees/2-A Fragment: member of the <b style="background:yellow">Sales</b> Management</li>
                //   <li>Doc: employees/3-A Fragment: hired as a <b style="background:yellow">sales</b> associate in</li>
                //   <li>Doc: employees/3-A Fragment: promoted to <b style="background:yellow">sales</b> representativ</li>
                //   <li>Doc: employees/5-A Fragment: company as a <b style="background:yellow">sales</b> representativ</li>
                //   <li>Doc: employees/5-A Fragment: promoted to <b style="background:yellow">sales</b> manager in</li>
                //   <li>Doc: employees/5-A Fragment: <b style="background:yellow">Sales</b> Management." </li>
                //   <li>Doc: employees/6-A Fragment: for the <b style="background:yellow">Sales</b> Professional.</li>
                // </ul>
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region highlight_4
                // Define customized tags to use for highlighting the searched terms
                // =================================================================

                $salesHighlights = new Highlightings();
                $managerHighlights = new Highlightings();

                $tagsToUse = new HighlightingOptions();
                // Provide strings of your choice to 'PreTags' & 'PostTags', e.g.:
                // the first term searched for will be wrapped with '+++'
                // the second term searched for will be wrapped with '<<<' & '>>>'
                $tagsToUse->setPreTags(["+++", "<<<"]);
                $tagsToUse->setPostTags(["+++", ">>>"]);

                // Make a full-text search dynamic query:
                // ======================================
                $employeesResults = $session
                    ->query(Employee::class)
                    // Search for:
                    //   * documents containing the term 'sales' in their 'Notes' field
                    //   * OR for documents containing the term 'manager' in their 'Title' field
                    ->search("Notes", "sales")
                    ->search("Title", "manager")
                    // Call 'Highlight' for each field searched
                    // Pass 'tagsToUse' to OVERRIDE the default tags used
                    ->highlight("Notes", 35, 1, $tagsToUse, $salesHighlights)
                    ->highlight("Title", 35, 1, $tagsToUse, $managerHighlights)
                    ->toList();
                # endregion

                # region fragments_2
                // The resulting salesHighlights fragments:
                // ========================================

                // "for the +++Sales+++ Professional."
                // "hired as a +++sales+++ associate in"
                // "company as a +++sales+++"
                // "company as a +++sales+++ representativ"

                // The resulting managerHighlights fragments:
                // ==========================================

                // "Sales <<<Manager>>>"
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region highlight_6
                // Make a full-text search dynamic query & project results:
                // ========================================================
                $termsHighlights = new Highlightings();

                /** @var array<Employee> $employeesProjectedResults */
                $employeesProjectedResults = $session
                    ->query(Employee::class)
                    // Search for documents containing 'sales' or 'german' in their 'Notes' field
                    ->search("Notes", "manager german")
                    // Request to highlight the searched terms from the 'Notes' field
                    ->highlight("Notes", 35, 2, null, $termsHighlights)
                    // Define the projection
                    ->selectFields(EmployeeDetails::class, QueryData::customFunction("o", "{ name: o.FirstName + ' ' + o.LastName, title: o.Title }"))
                    ->toList();
                # endregion

                # region fragments_3
                // The resulting fragments from termsHighlights:
                // =============================================

                // "to sales <b style=\"background:yellow\">manager</b> in March"
                // "and reads <b style=\"background:lawngreen\">German</b>.  He joined"
                // "to sales <b style=\"background:yellow\">manager</b> in January"
                // "in French and <b style=\"background:lawngreen\">German</b>."

                // NOTE: each search term is wrapped with a different color
                // 'manager' is wrapped with yellow
                // 'german' is wrapped with lawngreen
                # endregion
            } finally {
                $session->close();
            }

        } finally {
            $store->close();
        }
    }
}

