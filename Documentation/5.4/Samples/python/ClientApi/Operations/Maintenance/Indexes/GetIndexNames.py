from ravendb import GetIndexNamesOperation
from ravendb.documents.operations.definitions import MaintenanceOperation

from examples_base import ExampleBase


class GetIndexNames(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_get_index_names(self):
        with self.embedded_server.get_document_store("IndexNames") as store:
            # region get_index_names
            # Define the get index names operation
            # Pass number of indexes to skip & number of indexes to retrieve
            get_index_names_op = GetIndexNamesOperation(0, 10)

            # Execute the operation by passing it to maintenance.send
            index_names = store.maintenance.send(get_index_names_op)

            # index_names will contain the first 10 indexes, alphabetically ordered
            # endregion

    class Foo:
        # region get_index_names_syntax
        class GetIndexNamesOperation(MaintenanceOperation):
            def __init__(self, start: int, page_size: int): ...

        # endregion
