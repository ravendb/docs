import datetime
from typing import List

from ravendb import QueryData
from ravendb.infrastructure.orders import Company, Address, Order, OrderLine

from examples_base import ExampleBase, Employee


class HowToProjectQueryResults(ExampleBase):
    def setUp(self):
        super().setUp()
        with self.embedded_server.get_document_store("ProjectQueryResults") as store:
            with store.open_session() as session:
                self.add_orders(session)
                self.add_companies(session)
                self.add_employees(session)

    def test_examples(self):
        with self.embedded_server.get_document_store("ProjectQueryResults") as store:
            with store.open_session() as session:
                # region projections_1

                class CompanyNameCityAndCountry:
                    def __init__(self, name: str = None, city: str = None, country: str = None):
                        self.name = name
                        self.city = city
                        self.country = country

                query_data = QueryData(["name", "address.city", "address.country"], ["name", "city", "country"])
                results = list(
                    session.query(object_type=Company).select_fields_query_data(CompanyNameCityAndCountry, query_data)
                )

                # Each resulting object in the list is not a 'Company' entity, it is a new object containing ONLY the
                # fields specified in the query_data
                # endregion

            with store.open_session() as session:
                # region projections_2
                class OrderShippingAddressAndProductNames:
                    def __init__(self, ship_to: str = None, product_names: List[str] = None):
                        self.ship_to = ship_to
                        self.product_names = product_names

                # Retrieve all product names from the Lines array in an Order document
                query_data = QueryData(["ship_to", "lines[].product_name"], ["ship_to", "product_names"])

                projected_results = list(
                    session.query(object_type=Order).select_fields_query_data(
                        OrderShippingAddressAndProductNames, query_data
                    )
                )
                # endregion

            with store.open_session() as session:
                # region projections_3
                class EmployeeFullName:
                    def __init__(self, full_name: str):
                        self.full_name = full_name

                # Use custom function in query data or raw query
                query_data = QueryData.custom_function("o", "{ full_name: o.first_name + ' ' + o.last_name }")
                projected_results = list(
                    session.query(object_type=Employee).select_fields_query_data(EmployeeFullName, query_data)
                )
                # endregion

            with store.open_session() as session:
                # region projections_4
                class ProductsRaport:
                    def __init__(
                        self, total_products: int = None, total_discounted_products: int = None, total_price: int = None
                    ):
                        self.total_products = total_products
                        self.total_discounted_products = total_discounted_products
                        self.total_price = total_price

                # Use custom function in query data or raw query
                query_data = QueryData.custom_function(
                    "o",
                    "{"
                    "total_products: o.lines.length,"
                    " total_discounted_products: o.lines.filter((line) => line.discount > 0).length,"
                    " total_price: o.lines.reduce("
                    "(accumulator, line) => accumulator + line.price_per_unit * line.quantity, 0) "
                    "}",
                )
                projected_results = list(
                    session.query(object_type=Order).select_fields_query_data(ProductsRaport, query_data)
                )
                # endregion

            with store.open_session() as session:
                # region projections_7
                class EmployeeAgeDetails:
                    def __init__(self, day_of_birth: str = None, month_of_birth: str = None, age: str = None):
                        self.day_of_birth = day_of_birth
                        self.month_of_birth = month_of_birth
                        self.age = age

                # Use custom function in query data or raw query
                results = session.advanced.raw_query(
                    "from Employees as e select {"
                    ' "day_of_birth : new Date(Date.parse(e.birthday)).getDate(),'
                    " month_of_birth : new Date(Date.parse(e.birthday)).getMonth() + 1,"
                    " age : new Date().getFullYear() - new Date(Date.parse(e.birthday)).getFullYear()"
                    "}",
                    EmployeeAgeDetails,
                )
                # endregion

            with store.open_session() as session:
                # region projections_8
                class EmployeeBirthdayAndName:
                    def __init__(self, date: str = None, name: str = None):
                        self.date = date
                        self.name = name

                # Use custom function in query data or raw query
                results = list(
                    session.advanced.raw_query(
                        "from Employees as e select {"
                        "date: new Date(Date.parse(e.birthday)),"
                        "name: e.first_name.substr(0,3)"
                        "}",
                        EmployeeBirthdayAndName,
                    )
                )
                # endregion

            with store.open_session() as session:

                class EmployeeNameAndMetadata:
                    def __init__(self, name: str = None, metadata: str = None):
                        self.name = name
                        self.metadata = metadata

                # region projections_9
                projected_results = list(
                    session.advanced.raw_query(
                        "from Employees as e "
                        + "select {"
                        + "     name : e.first_name, "
                        + "     metadata : getMetadata(e)"
                        + "}",
                        EmployeeNameAndMetadata,
                    )
                )
                # endregion

            with store.open_session() as session:
                # region projections_10
                projected_results = list(
                    session.query(object_type=Company)
                    # Pass the projection class
                    .select_fields(ContactDetails)
                )
                # endregion

            with store.open_session() as session:
                # region projections_12
                # Lets define an array with the field names that will be projected
                # (its optional, you can pass field names as args loosely)
                projection_fields = ["name", "phone"]
                # Make a query
                projected_results = list(
                    session.advanced.document_query(object_type=Company)
                    # Call 'select_fields'
                    # Pass the projection class type & the fields to be projected from it
                    .select_fields(ContactDetails, *projection_fields)
                )

                # Each resulting object in the list is not a 'Company' entity
                # it is an object of type 'ContactDetails' containing data ONLY for the specified fields
                # endregion

            with store.open_session() as session:
                # region projections_13
                # For example:
                query_data = QueryData(["name"], ["funny_name"])
                try:
                    projected_results = list(
                        session.query(object_type=Company)
                        # Make a first projection
                        .select_fields(ContactDetails)
                        # A second projection is not supported and will raise an error
                        .select_fields_query_data(CompanyNameCityAndCountry, query_data)
                    )
                except Exception as e:
                    pass
                    # The following exception will be raised:
                    # "Projection is already done. You should not project your result twice."
                # endregion


# region projection_class
class ContactDetails:
    # The projection class contains field names from the 'Company' document
    def __init__(self, name: str = None, phone: str = None, fax: str = None):
        self.name = name
        self.phone = phone
        self.fax = fax


# endregion
