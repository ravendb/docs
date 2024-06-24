from ravendb import AbstractIndexCreationTask, PutIndexesOperation
from ravendb.documents.indexes.definitions import FieldStorage, IndexDefinition, IndexFieldOptions

from examples_base import ExampleBase


# region storing_1
class Employees_ByFirstAndLastName(AbstractIndexCreationTask):
    def __init__(self):
        super().__init__()
        self.map = "from employee in docs.Employees select new { employee.FirstName, employee.LastName }"
        self._store("FirstName", FieldStorage.YES)
        self._store("LastName", FieldStorage.YES)


# endregion


class Storing(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_storing(self):
        with self.embedded_server.get_document_store("Storing") as store:
            with store.open_session() as session:
                # region storing_2
                store.maintenance.send(
                    PutIndexesOperation(
                        IndexDefinition(
                            name="Employees_ByFirstAndLastName",
                            maps={
                                """
                    from employee in docs.Employees
                    select new
                    {
                        employee.FirstName,
                        employee.LastName
                    }
                    """
                            },
                            fields={
                                "FirstName": IndexFieldOptions(storage=FieldStorage.YES),
                                "LastName": IndexFieldOptions(storage=FieldStorage.YES),
                            },
                        )
                    )
                )
                # endregion
