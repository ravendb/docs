from typing import Optional

from ravendb import DeleteDocumentCommand
from ravendb.http.raven_command import VoidRavenCommand

from examples_base import ExampleBase


class Foo:
    class DeleteDocumentCommand:
        # region delete_interface
        class DeleteDocumentCommand(VoidRavenCommand):
            def __init__(self, key: str, change_vector: Optional[str] = None): ...

        # endregion


class DeleteSamples(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_delete(self):
        with self.embedded_server.get_document_store("DeleteDocumentCommand") as store:
            with store.open_session() as session:
                # region delete_sample
                command = DeleteDocumentCommand("employees/1-A", None)
                session.advanced.request_executor.execute_command(command)
                # endregion
