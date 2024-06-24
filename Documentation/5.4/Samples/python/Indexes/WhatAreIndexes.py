from ravendb import AbstractIndexCreationTask

from examples_base import ExampleBase, Employee


# region indexes_1
class Employees_ByFirstAndLastName(AbstractIndexCreationTask):
    def __init__(self):
        super().__init__()
        self.map = "from employee in docs.Employees select new {first_name = employee.FirstName, last_name = employee.LastName}"


# endregion


class WhatAreIndexes(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_what_are_indexes(self):
        with self.embedded_server.get_document_store("WhatAreIndexes") as store:
            with store.open_session() as session:
                # region indexes_2
                # save index on server
                Employees_ByFirstAndLastName().execute(store)
                # endregion

                with store.open_session() as session:
                    # region indexes_3
                    results = list(
                        session.query_index_type(Employees_ByFirstAndLastName, Employee).where_equals(
                            "first_name", "Robert"
                        )
                    )
                    # endregion
