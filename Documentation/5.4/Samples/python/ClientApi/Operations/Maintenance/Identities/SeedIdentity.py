from ravendb.documents.operations.definitions import MaintenanceOperation
from ravendb.documents.operations.identities import SeedIdentityForOperation

from examples_base import ExampleBase, Company


class SeedIdentity(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_seed_identity(self):
        with self.embedded_server.get_document_store("SeedIdentity") as store:
            # region seed_identity_1
            # Seed a higher identity value on the server:
            # ===========================================

            # Define the seed identity operation. Pass:
            # * The collection name (can be with or without a pipe)
            # * The new value to set
            seed_identity_op = SeedIdentityForOperation("companies|", 23)

            # Execute the operation by passing it to maintenance.send
            # The latest value on the server will be incremented to "23"
            # and the next document created with an identity will be assigned "24"
            seeded_value = store.maintenance.send(seed_identity_op)

            # Create a document with an identity ID:
            # ======================================

            with store.open_session() as session:
                session.store(Company(name="RavenDB"), "companies|")
                session.save_changes()
                # => Document "companies/24" will be created

            # endregion
            # region seed_identity_2
            # Force a smaller identity value on the server:
            # =============================================

            # Define the seed identity operation. Pass:
            #   * The collection name (can be with or without a pipe)
            #   * The new value to set
            #   * Set 'force_update' to True
            seed_identity_op = SeedIdentityForOperation("companies|", 5, force_update=True)

            # Execute the operation by passing it to maintenance.send
            # The latest value on the server will be decremented to "5"
            # and the next document created with an identity will be assigned "6"
            seeded_value = store.maintenance.send(seed_identity_op)

            # Create a document with an identity ID:
            # ======================================

            with store.open_session() as session:
                session.store(Company(name="RavenDB"), "companies|")
                session.save_changes()
                # => Document "companies/6" will be created

            # endregion

    class Foo:
        # region syntax
        class SeedIdentityForOperation(MaintenanceOperation[int]):
            def __init__(self, name: str, value: int, force_update: bool = False): ...

        # endregion
