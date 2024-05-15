from typing import List

from ravendb import GetIndexOperation, GetIndexesOperation, IndexDefinition
from ravendb.documents.operations.definitions import MaintenanceOperation

from examples_base import ExampleBase


class GetIndex(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_get_index(self):
        with self.embedded_server.get_document_store("GetIndex") as store:
            # region get_index
            # Define the get index operation, pass the index name
            get_index_op = GetIndexOperation("Orders/Totals")

            # Execute the operation by passing it to maintenance.send
            index = store.maintenance.send(get_index_op)

            # Access the index definition
            state = index.state
            lock_mode = index.lock_mode
            deployment_mode = index.deployment_mode
            # etc.

            # endregion

            # region get_indexes
            # Define the get indexes operation
            # Pass number of indexes to skip & number of indexes to retrieve
            get_index_op = GetIndexesOperation(0, 10)

            # Execute the operation by passing it to maintenance.send
            indexes = store.maintenance.send(get_index_op)

            # indexes will contain the first 10 indexes, alphabetically ordered by index name
            # Access an index definition from the resulting list:
            name = indexes[0].name
            state = indexes[0].state
            lock_mode = indexes[0].lock_mode
            deployment_mode = indexes[0].deployment_mode
            # etc.
            # endregion

    class Foo:
        # region get_index_syntax

        class GetIndexOperation(MaintenanceOperation[IndexDefinition]):
            def __init__(self, index_name: str): ...

        # endregion
        # region get_indexes_syntax
        class GetIndexesOperation(MaintenanceOperation[List[IndexDefinition]]):
            def __init__(self, start: int, page_size: int): ...

        # endregion
