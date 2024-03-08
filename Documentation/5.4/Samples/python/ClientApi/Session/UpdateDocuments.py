from ravendb import DocumentStore
from ravendb.infrastructure.orders import Company, Address
from ravendb.serverwide.database_record import DatabaseRecord
from ravendb.serverwide.operations.common import DeleteDatabaseOperation, CreateDatabaseOperation
from examples_base import ExamplesBase


class StoringEntities(ExamplesBase):
    def setUp(self):
        super().setUp()

    def get_document_store(self) -> DocumentStore:
        store = self.embedded_server.get_document_store("TestDatabase")

        parameters = DeleteDatabaseOperation.Parameters(["TestDatabase"], True)
        store.maintenance.server.send(DeleteDatabaseOperation(parameters=parameters))
        store.maintenance.server.send(CreateDatabaseOperation(DatabaseRecord("TestDatabase")))
        return store

    def test_update_document_sync(self):
        with self.get_document_store() as store:
            # Open a session
            with store.open_session() as session:
                # Use the session to create a document
                session.store(
                    Company(name="KitchenAppliances", address=Address(postal_code="12345")),
                    "companies/1-A",
                )
                session.store(
                    Company(name="ShoeAppliances", address=Address(postal_code="12345")), "companies/ShoeAppliances"
                )
                session.store(
                    Company(name="CarAppliances", address=Address(postal_code="12345")), "companies/CarAppliances"
                )
                session.save_changes()

            # region load-company-and-update
            with store.open_session() as session:
                # Load a company document
                # The entity loaded from the document will be added to the Session's entities map
                company = session.load("companies/1-A", Company)

                # Update the company's postal_code
                company.address["postal_code"] = "TheNewPostalCode"

                # In Python client nested objects are loaded as dicts for convenience
                # You can customize and control your class (from/to) JSON conversion to fit any case
                # Implement classmethod 'from_json(json_dict) -> YourType' to control how the object its being read
                # Implement method 'to_json() -> Dict' to manage how it's serialized

                # Apply changes
                session.save_changes()
            # endregion

            # region query-companies-and-update
            with store.open_session() as session:
                # Query: find companies with the specified postal_code
                # The entities loaded from the matching documents will be added to the Session's entities map
                query = session.query(object_type=Company).where_equals("address.postal_code", "12345")

                matching_companies = list(query)

                # Update the postal_code for the resulting company documents
                for company in matching_companies:
                    company.address["postal_code"] = "TheNewPostalCode"

                # Apply changes
                session.save_changes()
            # endregion
