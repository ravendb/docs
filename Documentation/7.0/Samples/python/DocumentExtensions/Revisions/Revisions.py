from ravendb.primitives import constants

from examples_base import ExampleBase

class Force(ExampleBase):
    def setUp(self):
        super().setUp()

        # region force_revision_creation_for
        with self.store.open_session() as session:
            company = session.load(company_id, Company)
            company.name = "HR V2"

            session.advanced.revisions.force_revision_creation_for(company)
            session.save_changes()

            revisions = session.advanced.revisions.get_for(company.Id, Company)
            revisions_count = len(revisions)

            self.assertEqual(1, revisions_count)
            # Assert revision contains the value 'Before' the changes...
            self.assertEqual("HR", revisions[0].name)
        # endregion
