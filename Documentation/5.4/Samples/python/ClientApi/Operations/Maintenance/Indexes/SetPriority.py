from enum import Enum

from ravendb import SetIndexesPriorityOperation
from ravendb.documents.indexes.definitions import IndexPriority
from ravendb.documents.operations.definitions import VoidMaintenanceOperation

from examples_base import ExampleBase


class SetPriority(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_set_priority(self):
        with self.embedded_server.get_document_store("SetPriority") as store:
            # region set_priority_single
            # Define the set priority operation
            # Pass index name & priority
            set_priority_op = SetIndexesPriorityOperation(IndexPriority.HIGH, "Orders/Totals")

            # Execute the operation by passing it to maintenance.send
            # An exception will be thrown if index does not exist
            store.maintenance.send(set_priority_op)
            # endregion

            # region set_priority_multiple
            # Define the set priority operation, pass multiple index names
            set_priority_op = SetIndexesPriorityOperation(IndexPriority.LOW, "Orders/Totals", "Orders/ByCompany")

            # Execute the operation by passing it to maintenance.send
            # An exception will be thrown if any of the specified indexes do not exist
            store.maintenance.send(set_priority_op)
            # endregion

    class Foo:
        # region syntax_1
        class SetIndexesPriorityOperation(VoidMaintenanceOperation):
            def __init__(self, priority: IndexPriority, *index_names: str): ...

        # endregion

    class Priority:
        # region syntax_2
        class IndexPriority(Enum):
            LOW = "Low"
            NORMAL = "Normal"
            HIGH = "High"

        # endregion
