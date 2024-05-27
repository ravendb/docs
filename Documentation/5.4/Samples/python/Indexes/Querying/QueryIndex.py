from typing import Dict, Any

from ravendb import AbstractIndexCreationTask

from examples_base import ExampleBase, Employee


# region the_index
# The index definition:
class Employees_ByName(AbstractIndexCreationTask):
    # The IndexEntry class defines the index-fields
    class IndexEntry:
        def __init__(self, first_name: str = None, last_name: str = None):
            self.first_name = first_name
            self.last_name = last_name

        # The from_json method to handle different casing on the server
        @classmethod
        def from_json(cls, json_dict: Dict[str, Any]) -> "Employees_ByName.IndexEntry":
            return cls(json_dict["FirstName"], json_dict["LastName"])

    def __init__(self):
        super().__init__()
        # The 'map' function defines the content of the INDEX-fields
        # * The content of INDEX-fields 'FirstName' & 'LastName'
        #   is composed of the relevant DOCUMENT-fields.
        self.map = """from e in docs.Employees select new {FirstName = e.FirstName, LastName = e.LastName}"""
        # * The index-fields can be queried on to fetch matching documents.
        #   You can query and filter Employee documents based on their first or last names.

        # * Employee documents that do Not contain both 'FirstName' and 'LastName' fields
        #   will Not be indexed.

        # * Note: the INDEX-field name does Not have to be exactly the same
        #   as the DOCUMENT-field name.


# endregion


class QueryIndex(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_query_index(self):
        with self.embedded_server.get_document_store("QueryIndex") as store:
            Employees_ByName().execute(store)
            with store.open_session() as session:
                self.add_employees(session)
                session.store(Employee(last_name="King", first_name="Jerry", notes=["Lmao"]))
                session.save_changes()
                # region index_query_1_1
                # Query the 'Employees' collection using the index - without filtering
                # (Open the 'Index' tab to view the index class definition)
                employees = list(
                    session
                    # Pass the queried collection as the first generic parameter
                    # Pass the index class as the second generic parameter
                    .query_index_type(Employees_ByName, Employee)
                )

                # All 'Employee' documents that contain DOCUMENT-fields 'FirstName' and\or 'LastName' will be returned
                # endregion

                # region index_query_1_3
                # Query the 'Employees' collection using the index - without filtering
                employees = list(
                    session
                    # Pass the index name as a parameter
                    # Use slash '/' in the index name, replacing the underscore '_' from the index class definition
                    .query_index("Employees/ByName")
                )
                # All 'Employee' documents that contain DOCUMENT-fields 'FirstName' and\or 'LastName' will be returned
                # endregion
                # region index_query_2_1
                # Query the 'Employees' collection using the index - filter by INDEX-field

                employees = list(
                    session
                    # Pass the index class as the first parameter
                    # Pass the IndexEntry class as the second parameter
                    .query_index_type(Employees_ByName, Employees_ByName.IndexEntry)
                    # Filter the retrieved documents by some predicate on an INDEX-field
                    .where_equals("LastName", "King")
                    # Specify the type of the returned document entities
                    .of_type(Employee)
                )

                # Results will include all documents from 'Employees' collection whose 'LastName' equals to 'King'
                # endregion

                # region index_query_3_1
                # Query the 'Employees' collection using the index - page results

                # This example is based on the previous filtering example
                employees = list(
                    session.query_index_type(Employees_ByName, Employees_ByName.IndexEntry)
                    .where_equals("LastName", "King")
                    .skip(5)  # Skip first 5 results
                    .take(10)  # Retrieve up to 10 documents
                    .of_type(Employee)
                )

                # Results will include up to 10 matching documents
                # endregion
                # region index_query_4_1
                # Query the 'Employees' collection using the index - filter by INDEX-field

                employees = list(
                    session
                    # Pass the Index class as the first parameter
                    # Pass the IndexEntry class as the second parameter
                    .query_index_type(Employees_ByName, Employees_ByName.IndexEntry)
                    # Filter the retrieved documents by some predicate on an INDEX-field
                    .where_equals("LastName", "King")
                    # Specify the type of the returned document entities
                    .of_type(Employee)
                )
                # Results will include all documents from 'Employees' collection whose 'LastName' equals to 'King'
                # endregion
                # region index_query_4_3
                # Query the 'Employees' collection using the index - filter by INDEX-field

                employees = list(
                    session
                    # Pass the IndexEntry class as the param
                    # Pass the index name as the param
                    # Use slash '/' in the index name, replacing the underscore `_` from the index class definition
                    .query_index("Employees/ByName", Employees_ByName.IndexEntry)
                    # Filter the retrieved documents by some predicate on an INDEX-field
                    .where_equals("LastName", "King")
                    # Specify the type of the returned documents entities
                    .of_type(Employee)
                )
                # Results will include all documents from 'Employees' collection whose 'LastName' equals to 'King'
                # endregion
                # region index_query_5_1
                # Query with RawQuery - filter by INDEX-field

                employees = list(
                    session.advanced
                    # Provide RQL to raw_query
                    .raw_query("from index 'Employees/ByName' where LastName == 'King'", Employee)
                )
                # Results will include all documents from 'Employees' collection whose 'LastName' equals to 'King'.
                # endregion
