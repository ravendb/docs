from typing import Any, Dict

from ravendb import AbstractIndexCreationTask
from ravendb.documents.indexes.definitions import FieldIndexing

from examples_base import ExampleBase, Product


# region index_1
class Products_ByUnitsInStock(AbstractIndexCreationTask):

    class IndexEntry:
        def __init__(self, units_in_stock: int = None):
            self.units_in_stock = units_in_stock

        # Handle different casing
        @classmethod
        def from_json(cls, json_dict: Dict[str, Any]):
            return cls(json_dict["UnitsInStock"])

    def __init__(self):
        super().__init__()
        self.map = "from p in products select new { UnitsInStock = p.UnitsInStock }"


# endregion


# region index_2
class Products_BySearchName(AbstractIndexCreationTask):
    class IndexEntry:
        def __init__(self, name: str = None, name_for_sorting: str = None):
            # Index-field 'Name' will be configured below for full-text search
            self.name = name

            # Index-field 'NameForSorting' will be used for ordering query results
            self.name_for_sorting = name_for_sorting

        @classmethod
        def from_json(cls, json_dict: Dict[str, Any]):
            return cls(json_dict["Name"], json_dict["NameForSorting"])

    def __init__(self):
        super().__init__()
        # Both index-fields are assigned the same content (The 'Name' from the document)
        self.map = "from p in products select new {Name = p.Name, NameForSorting = p.Name}"

        # Configure only the 'Name' index-field for FTS
        self._index("Name", FieldIndexing.SEARCH)


# endregion


class SortQueryResults(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_sort_query(self):
        with self.embedded_server.get_document_store("SortQueryResultsIndex") as store:
            with store.open_session() as session:
                # region sort_1
                products = list(
                    session
                    # Query the index
                    .query_index_type(Products_ByUnitsInStock, Products_ByUnitsInStock.IndexEntry)
                    # Apply filtering (optional)
                    .where_greater_than("UnitsInStock", 10)
                    # Call 'order_by_descending', pass the index-field by which to order the results
                    .order_by_descending("UnitsInStock").of_type(Product)
                )

                # Results will be sorted by the 'UnitsInStock' value in descending order,
                # with higher values listed first.
                # endregion

            with store.open_session() as session:
                # region sort_4
                products = list(
                    session
                    # Query the index
                    .query_index_type(Products_BySearchName, Products_BySearchName.IndexEntry)
                    # Call 'search':
                    # Pass the index-field that was configured for FTS and the term to search for.
                    # Here we search for terms that start with "ch" within index-field 'Name'.
                    .search("Name", "ch*")
                    # Call 'order_by':
                    # Pass the other index-field by which to order the results.
                    .order_by("NameForSorting").of_type(Product)
                )
                # Running the above query on the NorthWind sample data, ordering by 'NameForSorting' field,
                # we get the following order:
                # =========================================================================================

                # "Chai"
                # "Chang"
                # "Chartreuse verte"
                # "Chef Anton's Cajun Seasoning"
                # "Chef Anton's Gumbo Mix"
                # "Chocolade"
                # "Jack's New England Clam Chowder"
                # "Pâté chinois"
                # "Teatime Chocolate Biscuits"

                # While ordering by the searchable 'Name' field would have produced the following order:
                # ======================================================================================

                # "Chai"
                # "Chang"
                # "Chartreuse verte"
                # "Chef Anton's Cajun Seasoning"
                # "Pâté chinois"
                # "Chocolade"
                # "Teatime Chocolate Biscuits"
                # "Chef Anton's Gumbo Mix"
                # "Jack's New England Clam Chowder"

                # endregion
