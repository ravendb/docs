from ravendb import StopIndexingOperation, GetIndexingStatusOperation, IndexingStatus
from ravendb.documents.indexes.definitions import IndexRunningStatus
from ravendb.documents.operations.definitions import VoidMaintenanceOperation

from examples_base import ExampleBase


class Foo:
    # region syntax
    # class name has "Stop", but this is ok, this is the "Pause" operation
    class StopIndexingOperation(VoidMaintenanceOperation):
        def __init__(self): ...

    # endregion


class PauseIndexing(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_pause_indexing(self) -> None:
        with self.embedded_server.get_document_store("PauseIndexing") as store:
            # region pause_indexing
            # Define the pause indexing operation
            pause_indexing_op = StopIndexingOperation()

            # Execute the operation by passing it to maintenance.send
            store.maintenance.send(pause_indexing_op)

            # At this point:
            # All indexes in the default database will be 'paused' on the preferred node

            # Can verify indexing status on the preferred node by sending GetIndexingStatusOperation
            indexing_status = store.maintenance.send(GetIndexingStatusOperation())
            self.assertEqual(indexing_status.status, IndexRunningStatus.PAUSED)
            # endregion
