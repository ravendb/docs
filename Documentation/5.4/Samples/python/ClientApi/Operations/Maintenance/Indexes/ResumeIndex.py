from ravendb import StartIndexOperation, GetIndexingStatusOperation, StopIndexingOperation
from ravendb.documents.indexes.definitions import IndexRunningStatus
from ravendb.documents.operations.definitions import VoidMaintenanceOperation

from examples_base import ExampleBase


class Foo:
    # region syntax
    class StartIndexOperation(VoidMaintenanceOperation):
        def __init__(self, index_name: str): ...

    # endregion


class ResumeIndex(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_resume_index(self):
        with self.embedded_server.get_document_store("ResumeIndex") as store:
            self.add_index_orders_totals(store)
            store.maintenance.send(StopIndexingOperation())
            # region resume_index
            # Define the resume index operation, pass the index name
            resume_index_op = StartIndexOperation("Orders/Totals")

            # Execute the operation by passing it to maintenance.send
            store.maintenance.send(resume_index_op)

            # At this point:
            # Index 'Orders/Totals' is resumed on the preferred node

            # Can verify the index status on the preferred node by sending GetIndexingStatusOperation
            indexing_status = store.maintenance.send(GetIndexingStatusOperation())

            index = [x for x in indexing_status.indexes if x.name == "Orders/Totals"][0]
            self.assertEqual(index.status, IndexRunningStatus.RUNNING)
            # endregion
