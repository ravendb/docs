from datetime import timedelta
from typing import Optional

from ravendb import InMemoryDocumentSessionOperations, SessionOptions, TransactionMode
from ravendb.infrastructure.orders import Employee

from ClientAPI.Session.opening_session import DocumentStoreFake
from examples_base import ExamplesBase


class StoringEntities(ExamplesBase):
    def setUp(self):
        super().setUp()

    class Interface:
        # region saving_changes_1
        def save_changes(self) -> None:
            ...

        # endregion

    def test_saving_changes_xy(self):
        with self.embedded_server.get_document_store("SavingChangesXY") as store:
            with store.open_session() as session:
                # region saving_changes_2
                session.store(Employee(first_name="John", last_name="Doe"))

                session.save_changes()
                # endregion

            with store.open_session() as session:
                # storing new entity
                # region saving_changes_3
                def _build_wait(idx_wait_opt_builder: InMemoryDocumentSessionOperations.IndexesWaitOptsBuilder) -> None:
                    idx_wait_opt_builder.with_timeout(timedelta(seconds=30))
                    idx_wait_opt_builder.throw_on_timeout(True)
                    idx_wait_opt_builder.wait_for_indexes("index/1", "index/2")

                # this function can be also passed as a lambda
                session.advanced.wait_for_indexes_after_save_changes(_build_wait)

                session.store(Employee(first_name="John", last_name="Doe"))

                session.save_changes()
                # endregion

            with store.open_session() as session:
                # storing new entity
                # region saving_changes_4

                def _build_wait(
                    repl_wait_builder: InMemoryDocumentSessionOperations.ReplicationWaitOptsBuilder,
                ) -> None:
                    repl_wait_builder.with_timeout(timedelta(seconds=30))
                    repl_wait_builder.throw_on_timeout(False)  # default True
                    repl_wait_builder.number_of_replicas(2)  # minimum replicas to replicate
                    repl_wait_builder.majority(False)

                # this function can be also passed as a lambda
                session.advanced.wait_for_replication_after_save_changes(_build_wait)

                session.store(Employee(first_name="John", last_name="Doe"))
                session.save_changes()
                # endregion

            # region cluster_store_with_compare_exchange
            DocumentStore = DocumentStoreFake
            with DocumentStore() as store:
                with store.open_session(
                    session_options=SessionOptions(
                        # default is:    TransactionMode.SINGLE_NODE
                        transaction_mode=TransactionMode.CLUSTER_WIDE
                    )
                ) as session:
                    user = Employee(first_name="John", last_name="Doe")
                    session.store(user)

                    # this transaction is now conditional on this being
                    # successfully created (so, no other users with this name)
                    # it also creates an association to the new user's id
                    session.advanced.cluster_transaction.create_compare_exchange_value("usernames/John", user.Id)

                    session.save_changes()
            # endregion
