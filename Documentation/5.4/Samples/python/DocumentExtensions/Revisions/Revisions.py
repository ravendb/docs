from examples_base import ExampleBase, Company

class Force(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_force_revision_creation_for(self):
        with self.embedded_server.get_document_store("ForceRevision") as store:
            with store.open_session() as session:
                self.add_companies(session)
            company_id = "companies/1"
            # region force_revision_creation_for
            with store.open_session() as session:
                company = session.load(company_id, Company)
                company.name = "HR V2"

                session.advanced.revisions.force_revision_creation_for(company)
                session.save_changes()

                revisions = session.advanced.revisions.get_for(company.Id, Company)
                revisions_count = len(revisions)

                self.assertEqual(1, revisions_count)
                # Assert revision contains the value 'Before' the changes...
                self.assertEqual("HR V2", revisions[1].name)
            # endregion
