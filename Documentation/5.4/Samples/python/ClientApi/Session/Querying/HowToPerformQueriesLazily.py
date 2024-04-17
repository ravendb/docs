from typing import Optional, Callable, Dict, List, TypeVar

from ravendb import Facet, RangeFacet, Lazy, SuggestionResult, AbstractIndexCreationTask
from ravendb.documents.queries.facets.misc import FacetResult

from examples_base import ExampleBase, Employee, Category, Product

_T = TypeVar("_T")


class HowToPerformQueriesLazily(ExampleBase):
    def setUp(self):
        super().setUp()
        with self.embedded_server.get_document_store("QueriesLazily") as store:
            with store.open_session() as session:
                self.add_employees(session)
                self.add_companies(session)
                self.add_products(session)
                self.add_orders(session)
                self.add_categories(session)
                Products_ByCategoryAndPrice().execute(store)

    def test_how_to_perform_queries_lazily(self):
        with self.embedded_server.get_document_store("QueriesLazily") as store:
            with store.open_session() as session:
                # region lazy_1
                # Define a lazy query
                lazy_employees = (
                    session.query(object_type=Employee).where_equals("first_name", "Robert")
                    # Add a call to 'Lazily'
                    .lazily()
                )

                employees = lazy_employees.value  # Query is executed here
                # endregion

            with store.open_session() as session:
                # region lazy_4
                lazy_count = session.query(object_type=Employee).where_equals("first_name", "Robert").count_lazily()

                count = lazy_count.value  # Query is executed here
                # endregion

            with store.open_session() as session:
                # region lazy_7
                lazy_suggestions = (
                    session.query(object_type=Product).suggest_using(lambda builder: builder.by_field("name", "chaig"))
                    # Add a call to 'execute_lazy'
                    .execute_lazy()
                )

                suggest = lazy_suggestions.value  # Query is executed here
                suggestions = suggest["name"].suggestions
                # endregion

            # region the_facets
            # The facets definition used in the facets query
            facet = Facet(field_name="CategoryName")
            facet.display_field_name = "Product Category"

            range_facet = RangeFacet()
            range_facet.ranges = [
                "price_per_unit < 200",
                "price_per_unit between 200 and 400",
                "price_per_unit between 400 and 600",
                "price_per_unit between 600 and 800",
                "price_per_unit >= 800",
            ]
            range_facet.display_field_name = "Price per Unit"

            facets_definition = [facet, range_facet]

            with store.open_session() as session:
                # region lazy_10
                # Define a lazy facets query
                lazy_facets = (
                    session.query_index_type(
                        Products_ByCategoryAndPrice, Products_ByCategoryAndPrice.IndexEntry
                    ).aggregate_by_facets(facets_definition)
                    # Add a call to 'execute_lazy'
                    .execute_lazy()
                )

                facets = lazy_facets.value  # Query is executed here

                category_results = facets["Product Category"]
                price_results = facets["Price per Unit"]
                # endregion


class Foo:
    # region syntax_1
    # Lazy query
    def lazily(self, on_eval: Callable[[List[_T]], None] = None) -> Lazy[List[_T]]: ...

    # endregion

    # region syntax_2
    # Lazy count query
    def count_lazily(self) -> Lazy[int]: ...

    # endregion
    # region syntax_3
    # Lazy suggestions query
    def execute_lazy(
        self, on_eval: Optional[Callable[[Dict[str, SuggestionResult]], None]] = None
    ) -> "Lazy[Dict[str, SuggestionResult]]": ...

    # endregion
    # region syntax_4
    # Lazy facet query
    def execute_lazy(
        self, on_eval: Optional[Callable[[Dict[str, FacetResult]], None]] = None
    ) -> Lazy[Dict[str, FacetResult]]: ...

    # endregion


# region the_index
# The index definition used in the facets query
class Products_ByCategoryAndPrice(AbstractIndexCreationTask):
    def __init__(self):
        super().__init__()
        self.map = (
            "from p in docs.Products select new {"
            'CategoryName = LoadDocument(p.Category, "Categories").Name, PricePerUnit = p.PricePerUnit'
            "}"
        )

    class IndexEntry:
        def __init__(self, category_name: str, price_per_unit: int):
            self.category_name = category_name
            self.price_per_unit = price_per_unit


# endregion
