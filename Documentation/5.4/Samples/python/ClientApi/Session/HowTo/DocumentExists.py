from examples_base import ExampleBase


class Exists:
    # region exists_1
    def exists(self, key: str) -> bool: ...

    # endregion


class DocumentExists(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_document_exists(self):
        with self.embedded_server.get_document_store("DocumentExists") as store:
            with store.open_session() as session:
                # region exists_2
                exists = session.advanced.exists("employees/1-A")

                if exists:
                    ...  # Document 'employees/1-A' exists
                # endregion
