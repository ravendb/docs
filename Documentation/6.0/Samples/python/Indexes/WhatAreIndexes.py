from ravendb import AbstractIndexCreationTask

from examples_base import ExampleBase, Employee


# region indexes_1
# Define the index:
# =================

class Employees_ByNameAndCountry(AbstractIndexCreationTask):
    def __init__(self):
        super().__init__()
        
        self.map = """
        from employee in docs.Employees 
        select new {
            LastName = employee.LastName, 
            FullName = employee.FirstName + " " + employee.LastName,
            Country = employee.Address.country
        }
        """
# endregion


class WhatAreIndexes(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_what_are_indexes(self):
        with self.embedded_server.get_document_store("WhatAreIndexes") as store:
            with store.open_session() as session:
                # region indexes_2
                # Deploy the index to the server:
                # ===============================
                
                Employees_ByNameAndCountry().execute(store)
                # endregion

                with store.open_session() as session:
                    # region indexes_3
                    # Query the database using the index:
                    # ===================================
                    
                    employeesFromUK = list(
                        session.query_index_type(Employees_ByNameAndCountry, Employee)
                         # Here we query for all Employee documents that are from the UK
                         # and have 'King' in their LastName field:
                        .where_equals("Country", "UK")
                        .where_equals("LastName", "King")
                    )
                    # endregion
