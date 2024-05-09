from typing import Dict

from ravendb.documents.operations.definitions import MaintenanceOperation
from ravendb.documents.operations.identities import GetIdentitiesOperation

from examples_base import ExampleBase, Company


class GetIdentities(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_get_identities(self):
        with self.embedded_server.get_document_store("GetIdentities") as store:
            # region get_identities
            # Create a document with an identity ID:
            # ======================================
            with store.open_session() as session:
                # Request the server to generate an identity ID for the new document. Pass:
                #    * The entity to store
                #    * The collection name with a pipe (|) postfix
                session.store(Company(name="RavenDB"), "companies|")

                #  If this is the first identity created for this collection,
                #  and if the identity value was not customized
                #  then a document with an identity ID "companies/1" will be created
                session.save_changes()

            # Get identities information:
            # ===========================

            # Define the get identities operation
            get_identities_op = GetIdentitiesOperation()

            # Execute the operation by passing it to maintenance.send
            identities = store.maintenance.send(get_identities_op)

            # Results
            latest_identity_value = identities["companies|"]  # => value will be 1
            # endregion


class Foo:
    # region syntax
    class GetIdentitiesOperation(MaintenanceOperation[Dict[str, int]]): ...


# endregion
