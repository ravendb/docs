from typing import List

from ravendb.documents.operations.definitions import VoidMaintenanceOperation
from ravendb.documents.operations.indexes import DeleteIndexErrorsOperation

from examples_base import ExampleBase


class DeleteIndexErrors(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_delete_index_errors(self):
        with self.embedded_server.get_document_store("DeleteIndexErrors") as store:
            self.add_index_orders_totals(store)
            # region delete_errors_all
            # Define the delete index errors operation
            delete_index_errors_op = DeleteIndexErrorsOperation()

            # Execute the operation by passing it to maintenance.send
            store.maintenance.send(delete_index_errors_op)

            # All errors from ALL indexes are deleted
            # endregion

            # region delete_errors_specific
            # Define the delete index errors operation from specific indexes
            delete_index_errors_op = DeleteIndexErrorsOperation(["Orders/Totals"])

            # Execute the operation by passing it to maintenance.send
            # An exception will be thrown if any of the specified indexes do not exist
            store.maintenance.send(delete_index_errors_op)

            # Only errors from index "Orders/Totals" are deleted
            # endregion


class Foo:
    # region syntax

    class DeleteIndexErrorsOperation(VoidMaintenanceOperation):
        def __init__(self, index_names: List[str] = None): ...

    # endregion
