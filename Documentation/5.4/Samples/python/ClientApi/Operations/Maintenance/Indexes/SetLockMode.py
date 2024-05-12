from ravendb import SetIndexesLockOperation
from ravendb.documents.indexes.definitions import IndexLockMode

from examples_base import ExampleBase


class SetLockMode(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_set_lock_mode(self):
        with self.embedded_server.get_document_store("SetLockMode") as store:
            # region set_lock_single
            # Define the set lock mode operation
            # Pass index name & lock mode
            set_lock_mode_op = SetIndexesLockOperation(IndexLockMode.LOCKED_IGNORE, "Orders/Totals")

            # Execute the operation by passing it to maintenance.send
            # An exception will be thrown if index does not exist
            store.maintenance.send(set_lock_mode_op)

            # Lock mode is now set to 'LockedIgnore'
            # Any modification done now to the index will Not be applied, and will Not throw
            # endregion

            # region set_lock_multiple
            # Define the set lock mode operation, pass the parameters
            set_lock_mode_op = SetIndexesLockOperation(IndexLockMode.LOCKED_ERROR, "Orders/Totals", "Orders/ByCompany")

            # Execute the operation by passing it to maintenance.send
            # An exception will be thrown if any of the specified indexes does not exist
            store.maintenance.send(set_lock_mode_op)

            # Lock mode is now set to 'LockedError' on both indexes
            # Any modifications done now to either index will throw
            # endregion
