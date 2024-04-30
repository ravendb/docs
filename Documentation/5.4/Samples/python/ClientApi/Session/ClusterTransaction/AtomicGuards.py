from ravendb import SessionOptions, TransactionMode, GetCompareExchangeValuesOperation
from ravendb.infrastructure.entities import User

from examples_base import ExampleBase


class LoadingEntities(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_create_atomic_guard(self):
        # nodes, leader = create_raft_cluster(3)
        with self.embedded_server.get_document_store("AtomicGuard") as store:
            # region atomic-guards-enabled
            with store.open_session(
                # Open a cluster-wide session
                session_options=SessionOptions(transaction_mode=TransactionMode.CLUSTER_WIDE)
            ) as session:
                session.store(User(), "users/johndoe")
                session.save_changes()
                # An atomic-guard is now automatically created for the new document "users/johndoe"

            # Open two more cluster-wide sessions
            with store.open_session(
                session_options=SessionOptions(transaction_mode=TransactionMode.CLUSTER_WIDE)
            ) as session1:
                with store.open_session(
                    session_options=SessionOptions(transaction_mode=TransactionMode.CLUSTER_WIDE)
                ) as session2:
                    # The two sessions will load the same document at the same time
                    loaded_user_1 = session1.load("users/johndoe", User)
                    loaded_user_1.name = "jindoe"
                    loaded_user_2 = session2.load("users/johndoe", User)
                    loaded_user_2.name = "jandoe"

                    # Session1 will save changes first, which triggers a change in the
                    # version number of the associated atomic-guard
                    session1.save_changes()

                    # session2.save_changes() will be rejected with ConcurrencyException
                    # since session1 already changed the atomic-guard version,
                    # and session2 save_changes uses the document version that it had when it loaded the document.
                    session2.save_changes()
            # endregion

            result = store.operations.send(GetCompareExchangeValuesOperation("", User))

    def test_do_not_create_atomic_guard(self):
        with self.embedded_server.get_document_store("NotAtomicGuard") as store:
            # region atomic-guards-disabled
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
