from examples_base import ExampleBase


class CountingRevisions(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_count_revisions(self):
        with self.embedded_server.get_document_store("CountingRevisions") as store:
            with store.open_session() as session:
                # region getCount
                # Get the number of revisions for document 'companies/1-A'
                revisions_count = session.advanced.revisions.get_count_for("companies/1-A")
                # endregion


# region syntax
def get_count_for(self, id_: str) -> int: ...


# endregion
