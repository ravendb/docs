from ravendb import DisableIndexOperation
from ravendb.documents.operations.definitions import VoidMaintenanceOperation

from examples_base import ExampleBase


class Foo:
    # region syntax
    class DisableIndexOperation(VoidMaintenanceOperation):
        def __init__(self, index_name: str, cluster_wide: bool = False): ...

    # endregion


class DisableIndex(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_disable_index(self):
        with self.embedded_server.get_document_store("DisableIndex") as store:
            self.add_index_orders_totals(store)
            # region disable_1
            # Define the disable index operation
            # Use this args set to disable on a single node
            disable_index_op = DisableIndexOperation("Orders/Totals")

            # Execute the operation by passing it to maintenance.send
            store.maintenance.send(disable_index_op)

            # At this point, the index is disabled only on the 'preferred node'
            # New data will not be indexed on this node only
            # endregion

            # region disable_2
            # Define the disable index operation
            # Pass 'True' to disable the index on all nodes in the database-group
            disable_index_op = DisableIndexOperation("Orders/Totals", True)

            # Execute the operation by passing it to maintenance.send
            store.maintenance.send(disable_index_op)

            # At this point, the index is disabled on ALL nodes
            # New data will not be indexed
            # endregion
