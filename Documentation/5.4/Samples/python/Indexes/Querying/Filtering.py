from typing import Dict, Any

from ravendb import AbstractIndexCreationTask

from examples_base import ExampleBase, Employee, Product, Order


# region filtering_0_4
class Employees_ByFirstAndLastName(AbstractIndexCreationTask):
    def __init__(self):
        super().__init__()
        self.map = "from e in docs.Employees select new {FirstName = e.FirstName, LastName = e.LastName}"


# endregion


# region filtering_1_4
class Products_ByUnitsInStock(AbstractIndexCreationTask):
    def __init__(self):
        super().__init__()
        self.map = "from p in docs.Products select new {p.UnitsInStock}"


# endregion


# region filtering_7_4
class Orders_ByTotalPrice(AbstractIndexCreationTask):
    class Result:
        def __init__(self, total_price: int = None):
            self.total_price = total_price

        # The from_json method to handle different casing on the server
        @classmethod
        def from_json(cls, json_dict: Dict[str, Any]):
            return cls(json_dict["TotalPrice"])

    def __init__(self):
        super().__init__()
        self.map = "from o in docs.Orders select new { TotalPrice = order.Lines.Sum(x => (x.Quantity * x.PricePerUnit) * (1 - x.Discount)) }"


# endregion


# region filtering_2_4
class Order_ByOrderLinesCount(AbstractIndexCreationTask):
    def __init__(self):
        super().__init__()
        self.map = "from o in docs.Orders select new {Lines_Count = order.Lines.Count}"


# endregion


# region filtering_3_4
class Order_ByOrderLines_ProductName(AbstractIndexCreationTask):
    def __init__(self):
        super().__init__()
        self.map = "from o in docs.Orders select new {Lines_ProductName = order.Lines.Select(x => x.ProductName)}"


# endregion


# region filtering_5_4
class BlogPosts_ByTags(AbstractIndexCreationTask):
    def __init__(self):
        super().__init__()
        self.map = "from post in posts select new {post.Tags}"

    # endregion


class Filtering(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_filtering(self):
        with self.embedded_server.get_document_store("Filtering") as store:
            with store.open_session() as session:
                # region filtering_0_1
                results = list(  # Materialize query by sending it to server for processing
                    session.query_index_type(
                        Employees_ByFirstAndLastName, Employee
                    )  # query 'Employees/ByFirstAndLastName' index
                    .where_equals("FirstName", "Robert")  # filtering predicates
                    .and_also()
                    .where_equals("LastName", "King")
                )
                # endregion

                # region filtering_1_1
                results = list(  # Materialize query by sending it to server for processing
                    session.query_index_type(
                        Products_ByUnitsInStock, Product  # query 'Products/ByUnitsInStock' index
                    ).where_greater_than(
                        "UnitsInStock", 50
                    )  # filtering predicates
                )
                # endregion

                # region filtering_7_1
                results = list(  # Materialize query by sending it to server for processing
                    session.query_index_type(
                        Orders_ByTotalPrice, Orders_ByTotalPrice.Result
                    )  # query 'Orders/ByTotalPrice' index
                    .where_greater_than("TotalPrice", 50)  # filtering predicates
                    .of_type(Order)
                )
                # endregion

                # region filtering_2_1
                results = list(  # Materialize query by sending it to server for processing
                    session.query_index_type(
                        Order_ByOrderLinesCount, Order  # query 'Orders/ByOrderLinesCount' index
                    ).where_greater_than(
                        "Lines.Count", 50
                    )  # filtering predicates
                )
                # endregion

                # region filtering_3_1
                results = list(  # Materialize query by sending it to server for processing
                    session.query_index_type(
                        Order_ByOrderLines_ProductName, Order  # query 'Orders/ByOrderLinesCount' index
                    ).where_equals(
                        "Lines_ProductName", "Teatime Chocolate Biscuits"
                    )  # filtering predicates
                )
                # endregion

                # region filtering_4_1
                results = list(  # Materialize query by sending it to server for processing
                    session.query_index_type(
                        Employees_ByFirstAndLastName, Employee  # query 'Employees/ByFirstAndLastName' index
                    ).where_in(
                        "FirstName", ["Robert", "Nancy"]
                    )  # filtering predicates
                )
                # endregion

                # region filtering_5_1
                results = list(  # Materialize query by sending it to server for processing
                    session.query_index_type(BlogPosts_ByTags, BlogPost).contains_any(  # query 'BlogPosts/ByTags' index
                        "Tags", ["Development", "Research"]
                    )  # filtering predicates
                )
                # endregion

                # region filtering_6_1
                results = list(  # Materialize query by sending it to server for processing
                    session.query_index_type(BlogPosts_ByTags, BlogPost).contains_all(  # query 'BlogPosts/ByTags' index
                        "Tags", ["Development", "Research"]
                    )  # filtering predicates
                )
                # endregion

                # region filtering_8_1
                # return all products which name starts with 'ch'
                results = list(session.query(object_type=Product).where_starts_with("Name", "ch"))
                # endregion

                # region filtering_9_1
                results = list(session.query(object_type=Product).where_ends_with("Name", "ra"))
                # endregion

                # region filtering_10_1
                # return all orders that were shipped to 'Albuquerque'
                results = list(session.query(object_type=Order).where_equals("ShipTo.City", "Albuquerque"))
                # endregion

                # region filtering_11_1
                order = session.query(object_type=Order).where_equals("Id", "orders/1-A").first()
                # endregion

                # region filtering_12_1
                orders = list(session.query(object_type=Order).where_starts_with("Id", "orders/1"))
                # endregion
