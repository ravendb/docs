from typing import Any, Dict, List

from ravendb import AbstractIndexCreationTask, SearchOperator
from ravendb.documents.indexes.definitions import FieldIndexing

from examples_base import ExampleBase, Employee

# region index_1
class Employees_ByNotes(AbstractIndexCreationTask):
    # The IndexEntry class defines the index-fields
    class IndexEntry:
        def __init__(self, employee_notes: str = None):
            self.employee_notes = employee_notes

    def __init__(self):
        super().__init__()
        # The 'Map' function defines the content of the index-fields
        self.map = "from employee in docs.Employees " "select new " "{ " " employee_notes = employee.Notes[0]" "}"

        # Configure the index-field for FTS:
        # Set 'FieldIndexing.Search' on index-field 'employee_notes'
        self._index("employee_notes", FieldIndexing.SEARCH)

        # Optionally: Set your choice of analyzer for the index-field:
        # Here the text from index-field 'EmployeeNotes' will be tokenized by 'WhitespaceAnalyzer'.
        self._analyze("employee_notes", "WhitespaceAnalyzer")

        # Note:
        # If no analyzer is set then the default 'RavenStandardAnalyzer' is used.
# endregion

# region index_2
class Employees_ByEmployeeData(AbstractIndexCreationTask):
    class IndexEntry:
        def __init__(self, employee_data: List = None):
            self.employee_data = employee_data

    def __init__(self):
        super().__init__()
        self.map = (
            "from employee in docs.Employees "
            "select new {"
            "  employee_data = "
            "  {"
            # Multiple document-fields can be indexed
            # into the single index-field 'employee_data'
            "    employee.FirstName,"
            "    employee.LastName,"
            "    employee.Title,"
            "    employee.Notes"
            "  }"
            "}"
        )
        # Configure the index-field for FTS:
        # Set 'FieldIndexing.SEARCH' on index-field 'employee_data'
        self._index("employee_data", FieldIndexing.SEARCH)

        # Note:
        # Since no analyzer is set, the default 'RavenStandardAnalyzer' is used.
# endregion

# region index_3
class Products_ByAllValues(AbstractIndexCreationTask):
    class IndexEntry:
        def __init__(self, all_values: str = None):
            self.all_values = all_values

    def __init__(self):
        super().__init__()
        self.map = (
            "docs.Products.Select(product => new { "
            # Use the 'AsJson' method to convert the document into a JSON-like structure
            # and call 'Select' to extract only the values of each property
            "    all_values = this.AsJson(product).Select(x => x.Value) "
            "})"
        )
        
        # Configure the index-field for FTS:
        # Set 'FieldIndexing.SEARCH' on index-field 'all_values'
        self._index("all_values", FieldIndexing.SEARCH)
        
        # Note:
        # Since no analyzer is set, the default 'RavenStandardAnalyzer' is used.
        
        # Set the search engine type to Lucene:
        self.search_engine_type = SearchEngineType.LUCENE
# endregion

class Searching(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_searching(self):
        with self.embedded_server.get_document_store() as store:
            with store.open_session() as session:
                # region search_1
                employees = list(
                    session
                    # Query the index
                    .query_index_type(Employees_ByNotes, Employees_ByNotes.IndexEntry)
                    # Call 'search':
                    # pass the index field that was configured for FTS and the term to search for.
                    .search("employee_notes", "French").of_type(Employee)
                )
                # * Results will contain all Employee documents that have 'French' in their 'Notes' field.

                # * Search is case-sensitive since field was indexed using the 'WhitespaceAnalyzer'
                #   which preserves casing.
                # endregion

                # region search_4
                employees = list(
                    session
                    # Query the static-index
                    .query_index_type(Employees_ByEmployeeData, Employees_ByEmployeeData.IndexEntry)
                    .open_subclause()
                    # A logical OR is applied between the following two search calls
                    .search("employee_data", "Manager")
                    # A logical AND is applied between the following two terms
                    .search("employee_data", "French Spanish", operator=SearchOperator.AND)
                    .close_subclause()
                    .of_type(Employee)
                )

                # * Results will contain all Employee documents that have:
                #   ('Manager' in any of the 4 document-fields that were indexed)
                #   OR
                #   ('French' AND 'Spanish' in any of the 4 document-fields that were indexed)

                # * Search is case-insensitive since the default analyzer is used
                # endregion

                # region search_5
                products = list(
                    session.query_index_type(Products_ByAllValues, Products_ByAllValues.IndexEntry)
                    .search("all_values", "tofu")
                    .of_type(Product)
                )
                
                # * Results will contain all Product documents that have 'tofu'
                #   in ANY of their fields.
                #
                # * Search is case-insensitive since the default analyzer is used.
                # endregion
