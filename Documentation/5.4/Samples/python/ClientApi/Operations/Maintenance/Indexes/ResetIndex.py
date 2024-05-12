from ravendb.documents.operations.definitions import VoidMaintenanceOperation
from ravendb.documents.operations.indexes import ResetIndexOperation

from examples_base import ExampleBase


class ResetIndex(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_reset_index(self):
        with self.embedded_server.get_document_store("ResetIndex") as store:
            # region reset
            # Define the reset index operation, pass index name
            reset_index_op = ResetIndexOperation("Orders/Totals")

            # Execute the operation by passing it to maintenance.send
            # An exception will be thrown if index does not exist
            store.operations.send(reset_index_op)
            # endregion


class Foo:
    # region syntax
    class ResetIndexOperation(VoidMaintenanceOperation):
        def __init__(self, index_name: str): ...

    # endregion
