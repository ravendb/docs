import datetime
from typing import Any, Dict, List

from ravendb import AbstractIndexCreationTask
from ravendb.documents.indexes.abstract_index_creation_tasks import AbstractJavaScriptIndexCreationTask
from ravendb.documents.indexes.definitions import FieldIndexing, IndexFieldOptions
from ravendb.tools.utils import Utils

from examples_base import ExampleBase, Employee, Company


# region indexes_1
class Employees_ByFirstAndLastName(AbstractIndexCreationTask):
    def __init__(self):
        super().__init__()
        # ...


# endregion
# region javaScriptindexes_1
class Employees_ByFirstAndLastName(AbstractJavaScriptIndexCreationTask):
    def __init__(self):
        super().__init__()
        # ...


# endregion
# region indexes_2
class Employees_ByFirstAndLastName(AbstractIndexCreationTask):
    def __init__(self):
        super().__init__()
        self.map = "from employee in docs.Employees select new { FirstName = employee.FirstName, LastName = employee.LastName }"


# endregion
# region indexes_3
class Employees_ByFirstAndLastName(AbstractIndexCreationTask):
    def __init__(self):
        super().__init__()
        self.map = "from employee in docs.Employees select new { FirstName = employee.FirstName, LastName = employee.LastName }"


# endregion


# region javaScriptindexes_2
class Employees_ByFirstAndLastName(AbstractJavaScriptIndexCreationTask):
    def __init__(self):
        super().__init__()
        self.maps = {
            """
            map('Employees', function (employee){ 
                return { 
                    FirstName : employee.FirstName, 
                    LastName : employee.LastName
                };
            })
            """
        }


# endregion


# region indexes_6
class Employees_ByFirstAndLastName(AbstractIndexCreationTask):
    def __init__(self):
        super().__init__()
        self.map = "from employee in docs.Employees select new { FirstName = employee.FirstName, LastName = employee.LastName }"


# endregion


# region javaScriptindexes_6
class Employees_ByFirstAndLastName(AbstractJavaScriptIndexCreationTask):
    def __init__(self):
        super().__init__()
        self.maps = {
            """
            map('Employees', function (employee){ 
                return { 
                    FirstName : employee.FirstName, 
                    LastName : employee.LastName
                };
            })
            """
        }


# endregion


# region indexes_7
class Employees_ByFullName(AbstractIndexCreationTask):
    class Result:
        def __init__(self, full_name: str = None):
            self.full_name = full_name

    def __init__(self):
        super().__init__()
        self.map = (
            'from employee in docs.Employees select new { full_name = employee.FirstName + " " + employee.LastName }'
        )


# endregion


# region javaScriptindexes_7
class Employees_ByFullName(AbstractJavaScriptIndexCreationTask):
    class Result:
        def __init__(self, full_name: str = None):
            self.full_name = full_name

    def __init__(self):
        super().__init__()
        self.maps = {
            """
            map('Employees', function (employee){ 
                return { 
                    FullName  : employee.FirstName + ' ' + employee.LastName
                };
            })
            """
        }


# endregion


# region indexes_1_0
class Employees_ByYearOfBirth(AbstractIndexCreationTask):
    class Result:
        def __init__(self, year_of_birth: int = None):
            self.year_of_birth = year_of_birth

    def __init__(self):
        super().__init__()
        self.map = "from employee in docs.Employees select new { year_of_birth = employee.Birthday.Year }"


# endregion


# region javaScriptindexes_1_0
class Employees_ByYearOfBirth(AbstractJavaScriptIndexCreationTask):
    class Result:
        def __init__(self, year_of_birth: int = None):
            self.year = year_of_birth

    def __init__(self):
        super().__init__()
        self.maps = {
            """
            map('Employees', function (employee){ 
                return {
                    Birthday : employee.Birthday.Year 
                } 
            })
            """
        }


# endregion


# region indexes_1_6
class Companies_ByAddress_Country(AbstractIndexCreationTask):
    class Result:
        def __init__(self, city: str = None, company: str = None, phone: str = None):
            self.city = city
            self.company = company
            self.phone = phone

    def __init__(self):
        super().__init__()
        self.map = (
            'from company in docs.Companies where company.Address.Country == "USA"'
            "select new { company = company.Name, city = company.Address.City, phone = company.Phone }"
        )


# endregion
# region indexes_1_7
class Companies_ByAddress_Latitude(AbstractIndexCreationTask):
    class Result:
        def __init__(
            self,
            latitude: float = None,
            longitude: float = None,
            company_name: str = None,
            company_address: str = None,
            company_phone: str = None,
        ):
            self.latitude = latitude
            self.longitude = longitude
            self.company_name = company_name
            self.company_address = company_address
            self.company_phone = company_phone

    def __init__(self):
        super().__init__()
        self.map = (
            "from company in companies"
            "where (company.Address.Location.Latitude > 20 && company.Address.Location.Latitude < 50"
            "select new"
            "{"
            "    latitude = company.Address.Location.Latitude,"
            "    longitude = company.Address.Location.Longitude,"
            "    company_name = company.Name,"
            "    company_address = company.Address,"
            "    company_phone = company.Phone"
            "}"
        )


# endregion


# region indexes_1_2
class Employees_ByBirthday(AbstractIndexCreationTask):
    class Result:
        def __init__(self, birthday: datetime.datetime = None):
            self.birthday = birthday

        @classmethod
        def from_json(cls, json_dict: Dict[str, Any]) -> "Employees_ByBirthday.Result":
            # import 'Utils' from 'ravendb.tools.utils' to convert C# datetime strings to Python datetime objects
            return cls(Utils.string_to_datetime(json_dict["Birthday"]))


# endregion


# region javaScriptindexes_1_2
class Employees_ByBirthday(AbstractJavaScriptIndexCreationTask):
    class Result:
        def __init__(self, birthday: datetime.datetime = None):
            self.birthday = birthday

    def __init__(self):
        super().__init__()
        self.maps = {
            """
            map('Employees', function (employee){ 
                return {
                    Birthday : employee.Birthday 
                } 
            })
            """
        }


# endregion
# region indexes_1_4
class Employees_ByCountry(AbstractIndexCreationTask):
    class Result:
        def __init__(self, country: str = None):
            self.country = country

    def __init__(self):
        super().__init__()
        self.map = "from employee in docs.Employees select new { country = employee.Address.Country }"


# endregion
# region javaScriptindexes_1_4
class Employees_ByCountry(AbstractJavaScriptIndexCreationTask):

    class Result:
        def __init__(self, country: str = None):
            self.country = country

    def __init__(self):
        super().__init__()
        self.maps = {
            """
            map('Employees', function (employee){ 
                return {
                    Country : employee.Address.Country 
                 } 
            })
            """
        }


# endregion
# region indexes_1_6
class Employees_Query(AbstractIndexCreationTask):
    class Result:
        def __init__(self, query: List[str] = None):
            self.query = query

    def __init__(self):
        super().__init__()
        self.map = (
            "from employee in docs.Employees select new { query = new[]"
            "{"
            "    employee.FirstName,"
            "    employee.LastName,"
            "    employee.Title,"
            "    employee.Address.City,"
            "}"
            " }"
        )
        self._index("query", FieldIndexing.SEARCH)


# endregion
# region javaScriptindexes_1_6
class Employees_ByCity(AbstractJavaScriptIndexCreationTask):
    class Result:
        def __init__(self, query: List[str] = None):
            self.query = query

    def __init__(self):
        super().__init__()
        self.maps = {
            """
            map('Employees', function (employee) { 
            return { 
                query : [employee.FirstName, 
                         employee.LastName,
                         employee.Title,
                         employee.Address.City] } })
            """
        }
        self.fields = {"query": IndexFieldOptions(indexing=FieldIndexing.SEARCH)}


# endregion


class MapExample(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_map_indexes(self):
        with self.embedded_server.get_document_store("MapIndexes") as store:
            with store.open_session() as session:
                # region indexes_4
                employees_1 = list(
                    session.query_index_type(Employees_ByFirstAndLastName, Employee).where_equals(
                        "first_name", "Robert"
                    )
                )
                employees_2 = list(
                    session.query_index("Employees/ByFirstAndLastName", Employee).where_equals("first_name", "Robert")
                )
                # endregion

                # region indexes_8
                employees = list(
                    session.query_index_type(Employees_ByFullName, Employee).where_equals("full_name", "Robert King")
                )
                # endregion

                # region indexes_6_1
                employees = list(
                    session.query_index_type(Employees_ByYearOfBirth, Employees_ByYearOfBirth.Result)
                    .where_equals("year_of_birth", 1963)
                    .of_type(Employee)
                )
                # endregion

                # region indexes_5_1
                start_date = datetime.datetime(1963, 1, 1)
                end_date = start_date + datetime.timedelta(days=365) - datetime.timedelta(milliseconds=1)
                employees = list(
                    session.query_index_type(Employees_ByBirthday, Employees_ByBirthday.Result)
                    .where_between("birthday", start_date, end_date)
                    .of_type(Employee)
                )
                # endregion

                # region indexes_7_1
                employees = list(
                    session.query_index_type(Employees_ByCountry, Employees_ByCountry.Result)
                    .where_equals("country", "USA")
                    .of_type(Employee)
                )
                # endregion

                # region indexes_1_7_2
                employees = list(
                    session.query_index_type(Employees_Query, Employees_Query.Result)
                    .search("query", "John Doe")
                    .of_type(Employee)
                )
                # endregion

                # region indexes_query_1_6
                orders = list(
                    session.query_index_type(Companies_ByAddress_Country, Companies_ByAddress_Country.Result).of_type(
                        Company
                    )
                )
                # endregion

                # region indexes_query_1_7
                orders = list(
                    session.query_index_type(Companies_ByAddress_Latitude, Companies_ByAddress_Latitude.Result).of_type(
                        Company
                    )
                )
                # endregion
