from ravendb import StartIndexOperation, GetIndexingStatusOperation, StopIndexingOperation, StopIndexOperation
from ravendb.documents.indexes.definitions import IndexRunningStatus
from ravendb.documents.operations.definitions import VoidMaintenanceOperation

from examples_base import ExampleBase


class Foo:
    # region syntax
    class StopIndexOperation(VoidMaintenanceOperation):
        # class name has "Stop", but this is ok, this is the "Pause" operation
        def __init__(self, index_name: str): ...

    # endregion


class Pause(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_pause_index(self):
        with self.embedded_server.get_document_store("ResumeIndex") as store:
            self.add_index_orders_totals(store)
            # region pause_index
            # Define the resume index operation, pass the index name
            pause_index_op = StopIndexOperation("Orders/Totals")

            # Execute the operation by passing it to maintenance.send
            store.maintenance.send(pause_index_op)

            # At this point:
            # Index 'Orders/Totals' is paused on the preferred node

            # Can verify the index status on the preferred node by sending GetIndexingStatusOperation
            indexing_status = store.maintenance.send(GetIndexingStatusOperation())

            index = [x for x in indexing_status.indexes if x.name == "Orders/Totals"][0]
            self.assertEqual(index.status, IndexRunningStatus.PAUSED)
            # endregion
