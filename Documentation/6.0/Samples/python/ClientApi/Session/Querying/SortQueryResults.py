from typing import Union, TypeVar

from ravendb import OrderingType, GroupByField, DocumentQuery
from ravendb.infrastructure.orders import Product

from examples_base import ExampleBase

_T = TypeVar("_T")


class SortQueryResult(ExampleBase):
    def setUp(self):
        super().setUp()
        with self.embedded_server.get_document_store("SortQueryResults") as store:
            with store.open_session() as session:
                session.store(
                    Product(
                        "products/1",
                        "Apple Vision Pro",
                        "suppliers/idream",
                        "categories/electronics",
                        "pcs",
                        5000,
                        12,
                        3,
                    )
                )
                session.store(
                    Product(
                        "products/2",
                        "Apple iCar",
                        "suppliers/2",
                        "categories/automotive",
                        "car",
                        99000,
                        57,
                        53,
                    )
                )

                session.store(
                    Product(
                        "products/4",
                        "Apple iJet",
                        "suppliers/3",
                        "categories/aircraft",
                        "plane",
                        14950000,
                        2,
                        1,
                    )
                )

                session.store(
                    Product(
                        "products/65",
                        "Apple iAirFry",
                        "suppliers/5",
                        "categories/kitchen",
                        "pcs",
                        14950,
                        52167,
                        1,
                        True,
                    )
                )
                session.store(
                    Product(
                        "products/65412",
                        "Apple iAntimatter",
                        "suppliers/x",
                        "categories/misc",
                        "fg",
                        600750399,
                        2,
                        0,
                        True,
                    )
                )
                session.save_changes()

    def test_sort_query_results(self):
        with self.embedded_server.get_document_store("SortQueryResults") as store:
            with store.open_session() as session:
                # region sort_1
                products = list(
                    session
                    # Make a dynamic query on the 'Products' collection
                    .query_collection("Products")
                    # Apply filtering (optional)
                    .where_greater_than("units_in_stock", 10)
                    # Call 'order_by'
                    # Pass the document-field by which to order the results and the ordering type
                    .order_by("units_in_stock", OrderingType.LONG)
                )

                # Results will be sorted by the 'units_in_stock' value in ascending order,
                # with smaller values listed first
                # endregion

            with store.open_session() as session:
                # region sort_4
                products = list(
                    session.query_collection("Products")
                    # Apply filtering
                    .where_less_than("units_in_stock", 5)
                    .or_else()
                    .where_equals("discontinued", True)
                    # Call 'order_by_score'
                    .order_by_score()
                )

                # Results will be sorted by the score value
                # with best matching documents (higher score values) listed first.
                # endregion

            with store.open_session() as session:
                # region sort_7
                products = list(
                    session.query_collection("Products").where_greater_than("units_in_stock", 10)
                    # Call 'random_ordering'
                    .random_ordering()
                    # An optional seed can be passed, e.g.:
                    # .random_ordering("someSeed")
                )

                # Results will be randomly ordered
                # endregion

            with store.open_session() as session:
                # region sort_10
                number_of_products_per_category = list(
                    session.query_collection("Products", Product)
                    # Group by category
                    .group_by("category").select_key("category")
                    # Count the number of product documents per category
                    .select_count()
                    # Order by the count value
                    .order_by("count", OrderingType.LONG)
                )

                # Results will contain the number of Product documents per category
                # ordered by that count in ascending order.
                # endregion

            with store.open_session() as session:
                # region sort_13
                number_of_units_in_stock_per_category = list(
                    session.query_collection("Products", Product)
                    # Group by category
                    .group_by("category").select_key("category")
                    # Sum the number of units in stock per category
                    .select_sum(GroupByField("units_in_stock", "sum"))
                    # Order by the sum value
                    .order_by("sum", OrderingType.LONG)
                )

                # Results will contain the total number of units in stock per category
                # ordered by that number in ascending order.
                # endregion

            with store.open_session() as session:
                # region sort_16
                products = list(
                    session.query_collection("products")
                    # Call 'order_by', order by field 'quantity_per_unit'
                    # Pass a second param, requesting to order the text alphanumerically
                    .order_by("quantity_per_unit", OrderingType.ALPHA_NUMERIC)
                )

                # endregion

                # region sort_6_results
                #  Running the above query on the NorthWind sample data,
                #  would produce the following order for the quantity_per_unit field:
                #  ================================================================
                #
                #  "1 kg pkg."
                #  "1k pkg."
                #  "2 kg box."
                #  "4 - 450 g glasses"
                #  "5 kg pkg."
                #  ...
                #
                #  While running with the default Lexicographical ordering would have produced:
                #  ============================================================================
                #
                #  "1 kg pkg."
                #  "10 - 200 g glasses"
                #  "10 - 4 oz boxes"
                #  "10 - 500 g pkgs."
                #  "10 - 500 g pkgs."
                #  ...

                # endregion

            with store.open_session() as session:
                # region sort_19
                products = list(
                    session.query_collection("Products").where_greater_than("units_in_stock", 10)
                    # Apply the primary sort by 'units_in_stock'
                    .order_by_descending("units_in_stock", OrderingType.LONG)
                    # Apply a secondary sort by the score
                    .order_by_score()
                    # Apply another sort by 'Name'
                    .order_by("name")
                )

                #  Results will be sorted by the 'units_in_stock' value (descending),
                #  then by score,
                #  and then by 'name' (ascending).
                # endregion

            with store.open_session() as session:
                # region sort_22
                products = list(
                    session.query(object_type=Product).where_greater_than("units_in_stock", 10)
                    # Order by field 'units_in_stock', pass the name of your custom sorter class
                    .order_by("units_in_stock", "MySorter")
                )

                # Results will be sorted by the 'units_in_stock' value
                # according to the logic from 'MySorter' class

            class LmaoAPI:
                # region syntax
                # order_by:
                def order_by(
                    self, field: str, sorter_name_or_ordering_type: Union[str, OrderingType] = OrderingType.STRING
                ) -> DocumentQuery[_T]: ...

                # order_by_descending:
                def order_by_descending(
                    self, field: str, sorter_name_or_ordering_type: Union[str, OrderingType] = OrderingType.STRING
                ) -> DocumentQuery[_T]: ...

                # endregion
