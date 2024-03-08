from ravendb.infrastructure.orders import Company
from examples_base import ExamplesBase


class WhatIsSession(ExamplesBase):
    def test_samples(self):
        with self.embedded_server.get_document_store("WhatIsSession") as store:
            # A client-side copy of the document ID.
            company_id = "companies/1-A"

            # region session_usage_1
            with store.open_session() as session:
                # Create a new entity
                entity = Company(name="CompanyName")

                # Store the entity in the Session's internal map
                session.store(entity)
                # From now on, any changes that will be made to the entity will be tracked by the Session.
                # However, the changes will be persisted to the server only when 'save_changes()' is called.

                session.save_changes()
                # At this point the entity is persisted to the database as a new document.
                # Since no database was specified when opening the Session, the Default Database is used.

            # endregion

            # region session_usage_2
            # Open a session
            with store.open_session() as session:
                # Load an existing document to the Session using its ID
                # The loaded entity will be added to the session's internal map
                entity = session.load(company_id, Company)

                # Edit the entity, the Session will track this change
                entity.name = "NewCompanyName"

                session.save_changes()
                # At this point, the change made is persisted to the existing document in the database
            # endregion

            with store.open_session() as session:
                # region session_usage_3
                # A document is fetched from the server
                entity1 = session.load(company_id, Company)

                # Loading the same document will now retrieve its entity from the Session's map
                entity2 = session.load(company_id, Company)

                # This command will not throw an exception
                self.assertEqual(entity1, entity2)
                # endregion
