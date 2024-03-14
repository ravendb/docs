from __future__ import annotations
from typing import Dict, Any, List

from ravendb import GroupByField
from ravendb.infrastructure.orders import Order, Address, OrderLine

from examples_base import ExampleBase

from ravendb.documents.queries.group_by import GroupBy


class CountryAndQuantity:
    def __init__(self, country: str = None, ordered_quantity: int = None):
        self.country = country
        self.ordered_quantity = ordered_quantity


class EmployeeAndCompany:
    def __init__(self, employee: str = None, company: str = None):
        self.employee = employee
        self.company = company

    @classmethod
    def from_json(cls, json_dict: Dict[str, Any]) -> EmployeeAndCompany:
        return cls(json_dict["employee"], json_dict["company"])


class CountByCompanyAndEmployee:
    def __init__(self, count: int = None, company: str = None, employee_identifier: str = None):
        self.count = count
        self.company = company
        self.employee_identifier = employee_identifier


class CountOfEmployeeAndCompanyPairs:
    def __init__(self, employee_company_pair: EmployeeAndCompany, count: int):
        self.employee_company_pair = employee_company_pair
        self.count = count

    @classmethod
    def from_json(cls, json_dict: Dict[str, Any]) -> CountOfEmployeeAndCompanyPairs:
        return cls(EmployeeAndCompany.from_json(json_dict["employee_company_pair"]), json_dict["count"])


class ProductInfo:
    def __init__(self, count: int, product: str, quantity: int):
        self.count = count
        self.product = product
        self.quantity = quantity

    @classmethod
    def from_json(cls, json_dict: Dict[str, Any]) -> ProductInfo:
        return cls(json_dict["count"], json_dict["product"], json_dict["quantity"] if "quantity" in json_dict else 0)


class ProductsInfo:
    def __init__(self, count: int, products: List[str], quantity: int = None):
        self.count = count
        self.products = products
        self.quantity = quantity

    @classmethod
    def from_json(cls, json_dict: Dict[str, Any]) -> ProductsInfo:
        return cls(json_dict["count"], json_dict["products"], json_dict["quantity"] if "quantity" in json_dict else 0)


class HowToPerformGroupByQuery(ExampleBase):
    def setUp(self):
        super().setUp()
        with self.embedded_server.get_document_store("GroupByQuery") as store:
            with store.open_session() as session:
                session.store(
                    Order(
                        "Funny Order",
                        "companies/1",
                        "employees/1",
                        ship_to=Address(country="Chad"),
                        lines=[
                            OrderLine("products/1", "iPhone 15", 199.9, 20),
                            OrderLine("products/2", "Apple Vision Pro", 6666, 2),
                        ],
                    )
                )
                session.store(
                    Order(
                        "Even More Funny Order",
                        "companies/1",
                        "employees/3",
                        ship_to=Address(country="Poland"),
                        lines=[
                            OrderLine("products/3", "Grain", 2, 5000),
                            OrderLine("products/1", "iPhone 15", 1999.9, 500),
                        ],
                    )
                )
                session.save_changes()

    def test_how_to_perform_group_by_query(self):
        with self.embedded_server.get_document_store("GroupByQuery") as store:
            with store.open_session() as session:
                # region group_by_1
                orders = list(
                    session.query(object_type=Order)
                    .group_by("ship_to.country")
                    .select_key("ship_to.country", "country")
                    .select_sum(GroupByField("lines[].quantity", "ordered_quantity"))
                    .of_type(CountryAndQuantity)
                )
                # endregion

            with store.open_session() as session:
                # region group_by_2
                results = list(
                    session.query(object_type=Order)
                    .group_by("employee", "company")
                    .select_key("employee", "employee_identifier")
                    .select_key("company")
                    .select_count()
                    .of_type(CountByCompanyAndEmployee)
                )
                # endregion

            with store.open_session() as session:
                # region group_by_3
                orders = list(
                    session.query(object_type=Order)
                    .group_by("employee", "company")
                    .select_key("key()", "employee_company_pair")
                    .select_count("count")
                    .of_type(CountOfEmployeeAndCompanyPairs)
                )
                # endregion

            with store.open_session() as session:
                # region group_by_4
                products = list(
                    session.query(object_type=Order)
                    .group_by(GroupBy.array("lines[].product"))
                    .select_key("key()", "products")
                    .select_count()
                    .of_type(ProductsInfo)
                )
                # endregion

            with store.open_session() as session:
                # region group_by_5
                products = list(
                    session.advanced.document_query(object_type=Order)
                    .group_by("lines[].product", "ship_to.country")
                    .select_key("lines[].product", "product")
                    .select_key("ship_to.country", "country")
                    .select_count()
                    .of_type(ProductInfo)
                )
                # endregion

            with store.open_session() as session:
                # region group_by_6
                results = list(
                    session.query(object_type=Order)
                    .group_by(GroupBy.array("lines[].product"), GroupBy.array("lines[].quantity"))
                    .select_key("lines[].product", "product")
                    .select_key("lines[].quantity", "quantity")
                    .select_count()
                    .of_type(ProductInfo)
                )
                # endregion

            with store.open_session() as session:
                # region group_by_7
                results = list(
                    session.query(object_type=Order)
                    .group_by(GroupBy.array("lines[].product"))
                    .select_key("key()", "products")
                    .select_count()
                    .of_type(ProductsInfo)
                )
                # endregion

            with store.open_session() as session:
                # region group_by_8
                results = list(
                    session.query(object_type=Order)
                    .group_by(GroupBy.array("lines[].product"), GroupBy.field("ship_to.country"))
                    .select_key("lines[].product", "products")
                    .select_key("ship_to.country", "country")
                    .select_count()
                    .of_type(ProductsInfo)
                )
                # endregion

            with store.open_session() as session:
                # region group_by_9
                results = list(
                    session.query(object_type=Order)
                    .group_by(GroupBy.array("lines[].product"), GroupBy.array("lines[].quantity"))
                    .select_key("lines[].product", "products")
                    .select_key("lines[].quantity", "quantities")
                    .select_count()
                    .of_type(ProductsInfo)
                )
                # endregion
