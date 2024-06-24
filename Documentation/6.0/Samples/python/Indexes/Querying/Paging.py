from typing import Dict, Any, List

from ravendb import QueryStatistics, AbstractIndexCreationTask

from examples_base import ExampleBase, Product, Order


class Paging(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_example(self):
        with self.embedded_server.get_document_store("IndexQueryPaging") as store:
            with store.open_session() as session:
                # region paging_0_1
                # A simple query without paging:
                # ==============================
                all_results = list(
                    session.query_index_type(Products_ByUnitsInStock, Products_ByUnitsInStock.IndexEntry)
                    .where_greater_than("units_in_stock", 10)
                    .of_type(Product)
                )

                # Executing the query on the Northwind sample data
                # will result in all 47 Product documents that match the query predicate.
                # endregion

            with store.open_session() as session:
                # region paging_1_1
                # Retrieve only the 3'rd page - when page size is 10:
                # ===================================================
                def __stats_callback(statistics: QueryStatistics):
                    total_results = statistics.total_results
                    # While the query below returns only 10 results,
                    # 'total_results' will hold the total number of matching documents (47).

                third_page_results = list(
                    session.query_index_type(Products_ByUnitsInStock, Products_ByUnitsInStock.IndexEntry)
                    # Get the query stats if you wish to know the TOTAL number of results
                    .statistics(__stats_callback)
                    # Apply some filtering condition as needed
                    .where_greater_than("units_in_stock", 10).of_type(Product)
                    # Call 'skip', pass the number of items to skip from the beginning of the result set
                    # Skip the first 20 resulting documents
                    .skip(20)
                    # Call 'take' to define the number of documents to return
                    # Take up to 10 products => so 10 is the "Page Size"
                    .take(10)
                )

            # When executing this query on the Northwind sample data,
            # results will include only 10 Product documents ("products/45-A" to "products/54-A")

            with store.open_session() as session:
                # region paging_2_1
                # Query for all results - page by page:
                # =====================================
                paged_results: List[Product] = []
                page_number = 0
                page_size = 10

                while True:
                    paged_results = list(
                        session.query_index_type(Products_ByUnitsInStock, Products_ByUnitsInStock.IndexEntry)
                        # Apply some filtering condition as needed
                        .where_greater_than("units_in_stock", 10).of_type(Product)
                        # Skip the number of results that were already fetched
                        .skip(page_number * page_size)
                        # Request to get 'page_size' results
                        .take(page_size)
                    )
                    page_number += 1

                    if len(paged_results) == 0:
                        break

                    # Make any processing needed with the current paged results here
                    # ...

                # endregion

            with store.open_session() as session:
                # region paging_3_3
                paged_results: List[ProjectedClass] = []

                total_results = 0
                total_unique_results = 0
                skipped_results = 0

                page_number = 0
                page_size = 10

                def __stats_callback(statistics: QueryStatistics):
                    total_results = statistics.total_results
                    nonlocal skipped_results
                    skipped_results += statistics.skipped_results

                while True:
                    paged_results = list(
                        session.query_index_type(Products_ByUnitsInStock, Products_ByUnitsInStock.IndexEntry)
                        .statistics(__stats_callback)
                        .where_greater_than("units_in_stock", 10)
                        .of_type(Product)
                        # Define a projection
                        .select_fields(ProjectedClass)
                        # Call distinct to remove duplicate projected results
                        .distinct()
                        # Add the number of skipped results to the "start location"
                        .skip((page_size * page_size) + skipped_results)
                        .take(page_size)
                    )

                    total_unique_results += len(paged_results)

                    if len(paged_results) == 0:
                        break

                # When executing the query on the Northwind sample data:
                # ======================================================

                # The total matching results reported in the stats is 47 (totalResults),
                # but the total unique objects returned while paging the results is only 29 (totalUniqueResults)
                # due to the 'Distinct' usage which removes duplicates.

                # This is solved by adding the skipped results count to Skip().
                # endregion

            with store.open_session() as session:
                # region paging_4_1
                paged_results: List[Order] = []

                total_results = 0
                total_unique_results = 0
                skipped_results = 0

                page_number = 0
                page_size = 50

                def __stats_callback(statistics: QueryStatistics):
                    nonlocal skipped_results
                    skipped_results += statistics.skipped_results
                    total_results = statistics.total_results

                while True:
                    paged_results = list(
                        session.query_index_type(Orders_ByProductName, Orders_ByProductName.IndexEntry)
                        .statistics(__stats_callback)
                        .of_type(Order)
                        # Add the number of skipped results to the "start location"
                        .skip((page_size * page_size) + skipped_results)
                        .take(page_size)
                    )

                    total_unique_results += len(paged_results)

                    if len(paged_results) == 0:
                        break

                # When executing the query on the Northwind sample data:
                # ======================================================

                # The total results reported in the stats is 2155 (total_results),
                # which represent the multiple index-entries generated as defined by the fanout index.

                # By adding the skipped results count to the skip() method,
                # we get the correct total unique results which is 830 Order documents.
                # endregion


# region projected_class
class ProjectedClass:
    def __init__(self, category: str = None, supplier: str = None):
        self.category = category
        self.supplier = supplier

    # Handle different casing by implementing from_json class method
    @classmethod
    def from_json(cls, json_dict: Dict[str, Any]):
        return cls(json_dict["Category"], json_dict["Supplier"])


# endregion


# region index_0
class Products_ByUnitsInStock(AbstractIndexCreationTask):
    def __init__(self):
        super().__init__()
        self.map = "from product in docs.Products select new { units_in_stock = product.UnitsInStock }"

    class IndexEntry:
        def __init__(self, units_in_stock: int = None):
            self.units_in_stock = units_in_stock


# endregion


# region index_1
# A fanout index - creating MULTIPLE index-entries per document:
# ==============================================================
class Orders_ByProductName(AbstractIndexCreationTask):
    class IndexEntry:
        def __init__(self, product_name: str = None):
            self.product_name = product_name

    def __init__(self):
        super().__init__()
        self.map = "from order in docs.Orders from line in order.Lines select new { product_name = line.ProductName }"


# endregion
