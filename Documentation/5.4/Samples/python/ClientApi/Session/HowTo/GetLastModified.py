from datetime import datetime

from examples_base import ExampleBase


class Foo:
    # region get_last_modified_1
    def get_last_modified_for(self, entity: object) -> datetime: ...

    # endregion


class GetLastModified(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_get_last_modified(self):
        with self.embedded_server.get_document_store("GetLastModified") as store:
            with store.open_session() as session:
                # region get_last_modified_2
                employee = session.load("employees/1-A")
                last_modified = session.advanced.get_last_modified_for(employee)
                # endregion
