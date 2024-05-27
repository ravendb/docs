from ravendb import IndexDefinition, IndexHasChangedOperation
from ravendb.documents.operations.definitions import MaintenanceOperation

from examples_base import ExampleBase


class IndexHasChanged(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_index_has_changed(self):
        with self.embedded_server.get_document_store("IndexHasChanged") as store:
            # region index_has_changed
            # Some index definition
            index_definition = IndexDefinition(
                name="UsersByName", maps={"from user in docs.Users select new { user.Name }"}
            )

            # Define the has-changed operation, pass the index definition
            index_has_changed_op = IndexHasChangedOperation(index_definition)

            # Execute the operation by passing it to maintenance.send
            index_has_changed = store.maintenance.send(index_has_changed_op)

            # Return values:
            # False: The definition of the index passed is the SAME as the one deployed on the server
            # True:  The definition of the index passed is DIFFERENT from the one deployed on the server
            #        Or - index does not exist
            # endregion

    class Foo:
        # region syntax
        class IndexHasChangedOperation(MaintenanceOperation[bool]):
            def __init__(self, index: IndexDefinition): ...

        # endregion
