from ravendb import GetIndexingStatusOperation, StopIndexingOperation, StartIndexingOperation
from ravendb.documents.indexes.definitions import IndexRunningStatus
from ravendb.documents.operations.definitions import VoidMaintenanceOperation

from examples_base import ExampleBase


class Foo:
    # region syntax
    class StartIndexingOperation(VoidMaintenanceOperation):
        def __init__(self): ...

    # endregion


class ResumeIndexing(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_resume_indexing(self):
        with self.embedded_server.get_document_store("ResumeIndex") as store:
            self.add_index_orders_totals(store)
            store.maintenance.send(StopIndexingOperation())
            # region resume_indexing
            # Define the resume indexing operation
            resume_index_op = StartIndexingOperation()

            # Execute the operation by passing it to maintenance.send
            store.maintenance.send(resume_index_op)

            # At this point:
            # you can be sure that all indexes on the preferred node are 'running'

            # Can verify the index status on the preferred node by sending GetIndexingStatusOperation
            indexing_status = store.maintenance.send(GetIndexingStatusOperation())

            self.assertEqual(indexing_status.status, IndexRunningStatus.RUNNING)
            # endregion
