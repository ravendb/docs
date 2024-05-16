from typing import Optional

from ravendb import PutDocumentCommand, DocumentInfo
from ravendb.documents.commands.crud import PutResult
from ravendb.http.raven_command import VoidRavenCommand, RavenCommand

from examples_base import ExampleBase, Category


class Foo:
    class PutDocumentCommand:
        # region put_interface
        class PutDocumentCommand(RavenCommand[PutResult]):
            def __init__(self, key: str, change_vector: Optional[str], document: dict): ...

        # endregion


class PutSamples(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_put(self):
        with self.embedded_server.get_document_store("PutDocumentCommand") as store:
            with store.open_session() as session:
                # region put_sample
                # Create a new document
                doc = Category(name="My category", description="My category description")

                # Create metadata on the document
                doc_info = DocumentInfo(collection="Categories")

                # Convert your entity to a dict
                dict_doc = session.entity_to_json.convert_entity_to_json_static(doc, session.conventions, doc_info)

                # The put command (parameters are document ID, change vector check is None, the document to store)
                command = PutDocumentCommand("employees/1-A", None, dict_doc)
                # Request executor sends the command to the server
                session.advanced.request_executor.execute_command(command)
                # endregion
