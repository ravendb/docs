from ravendb import SessionOptions, TransactionMode, GetCompareExchangeValuesOperation
from ravendb.infrastructure.entities import User

from examples_base import ExampleBase


class LoadingEntities(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_create_atomic_guard(self):
        # nodes, leader = create_raft_cluster(3)
        with self.embedded_server.get_document_store("AtomicGuard") as store:
            # region atomic_guards_enabled
            with store.open_session(
                # Open a cluster-wide session:
                session_options=SessionOptions(transaction_mode=TransactionMode.CLUSTER_WIDE)
            ) as session:
                session.store(User(), "users/johndoe")
                session.save_changes()
                # An atomic-guard is now automatically created for the new document "users/johndoe"

            # Open two concurrent cluster-wide sessions:
            with store.open_session(
                session_options=SessionOptions(transaction_mode=TransactionMode.CLUSTER_WIDE)
            ) as session1:
                with store.open_session(
                    session_options=SessionOptions(transaction_mode=TransactionMode.CLUSTER_WIDE)
                ) as session2:
                    # Both sessions load the same document:
                    loaded_user_1 = session1.load("users/johndoe", User)
                    loaded_user_1.name = "jindoe"
                    loaded_user_2 = session2.load("users/johndoe", User)
                    loaded_user_2.name = "jandoe"

                    # session1 saves its changes first —
                    # this increments the Raft index of the associated atomic guard.
                    session1.save_changes()

                    # session2 tries to save using an outdated atomic guard version
                    # and fails with a ConcurrencyException.
                    session2.save_changes()
            # endregion

            result = store.operations.send(GetCompareExchangeValuesOperation("", User))

    def test_do_not_create_atomic_guard(self):
        with self.embedded_server.get_document_store("NotAtomicGuard") as store:
            # region atomic_guards_disabled
            with store.open_session(
                # Open a cluster-wide session
                session_options=SessionOptions(
                    transaction_mode=TransactionMode.CLUSTER_WIDE,
                    disable_atomic_document_writes_in_cluster_wide_transaction=True,
                )
            ) as session:
                session.store(User(), "users/johndoe")

                # No atomic-guard will be created upon save_changes
                session.save_changes()
            # endregion

            result = store.operations.send(GetCompareExchangeValuesOperation("", User))
            
    def load_before_storing(self):
            with self.embedded_server.get_document_store("LoadBeforeStore") as store:
                # region load_before_storing
                with store.open_session(
                    session_options=SessionOptions(
                        # Open a cluster-wide session
                        transaction_mode=TransactionMode.CLUSTER_WIDE
                    )
                ) as session:
                    # Load the user document BEFORE creating or updating
                    user = session.load("users/johndoe", User)
        
                    if user is None:
                        # Document doesn't exist => create a new document
                        new_user = User()
                        new_user.id = "users/johndoe"
                        new_user.name = "John Doe"
                        # ... initialize other properties
        
                        # Store the new user document in the session
                        session.store(new_user)
                    else:
                        # Document exists => apply your modifications
                        user.name = "New name"
                        # ... make any other updates
                        
                        # No need to call store() again
                        # RavenDB tracks changes on loaded entities
        
                    # Commit your changes
                    session.save_changes()
                # endregion
