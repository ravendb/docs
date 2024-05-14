from ravendb import EnableIndexOperation
from ravendb.documents.operations.definitions import VoidMaintenanceOperation

from examples_base import ExampleBase


class Foo:
    # region syntax
    class EnableIndexOperation(VoidMaintenanceOperation):
        def __init__(self, index_name: str, cluster_wide: bool = False): ...

    # endregion


class EnableIndex(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_enable_index(self):
        with self.embedded_server.get_document_store("EnableIndex") as store:
            self.add_index_orders_totals(store)
            # region enable_1
            # Define the enable index operation
            # Use this args set to enable on a single node
            enable_index_op = EnableIndexOperation("Orders/Totals")

            # Execute the operation by passing it to maintenance.send
            store.maintenance.send(enable_index_op)

            # At this point, the index is enabled only on the 'preferred node'
            # New data will not be indexed on this node only
            # endregion

            # region enable_2
            # Define the enable index operation
            # Pass 'True' to enable the index on all nodes in the database-group
            enable_index_op = EnableIndexOperation("Orders/Totals", True)

            # Execute the operation by passing it to maintenance.send
            store.maintenance.send(enable_index_op)

            # At this point, the index is enabled on ALL nodes
            # New data will not be indexed
            # endregion
