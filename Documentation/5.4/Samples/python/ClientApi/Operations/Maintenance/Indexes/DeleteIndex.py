from ravendb import DeleteIndexOperation
from ravendb.documents.operations.definitions import VoidMaintenanceOperation

from examples_base import ExampleBase


class DeleteIndex(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_delete_index(self):
        with self.embedded_server.get_document_store("DeleteIndex") as store:
            self.add_index_orders_totals(store)
            # region delete_index
            # Define the delete index operation, specify the index name
            delete_index_op = DeleteIndexOperation("Orders/Totals")

            # Execute the operation by passing it to maintenance.send
            store.maintenance.send(delete_index_op)
            # endregion


class Foo:
    # region syntax
    class DeleteIndexOperation(VoidMaintenanceOperation):
        def __init__(self, index_name: str): ...

    # endregion
