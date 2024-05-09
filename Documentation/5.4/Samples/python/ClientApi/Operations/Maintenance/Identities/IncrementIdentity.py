from ravendb.documents.operations.definitions import MaintenanceOperation
from ravendb.documents.operations.identities import NextIdentityForOperation

from examples_base import ExampleBase, Company


class IncrementIdentity(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_increment_identity(self):
        with self.embedded_server.get_document_store("IncrementIdentity") as store:
            # region increment_identity
            # Create a document with an identity ID:
            # ======================================

            with store.open_session() as session:
                # Pass a collection name that ends with a pipe '|' to create an identity ID
                session.store(Company(name="RavenDB"), "companies|")
                session.save_changes()
                # => Document "companies/1" will be created

            # Increment the identity value on the server:
            # ===========================================

            # Define the next identity operation
            # Pass the collection name (can be with or without a pipe)
            next_identity_op = NextIdentityForOperation("companies|")

            #  Execute the operation by passing it to Maintenance.Send
            #  The latest value will be incremented to "2"
            #  and the next document created with an identity will be assigned "3"
            incremented_value = store.maintenance.send(next_identity_op)

            # Create another document with an identity ID:
            # ============================================

            with store.open_session() as session:
                session.store(Company(name="RavenDB"), "companies|")
                session.save_changes()
                # => Document "companies/3" will be created

            # endregion

    class Foo:
        # region syntax
        class NextIdentityForOperation(MaintenanceOperation[int]): ...

        # endregion
