from examples_base import ExampleBase


class Exists:
    # region syntax
    def exists(self, document_id: str, name: str) -> bool: ...

    # endregion


class AttachmentExists(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_attachment_exists(self):
        with self.embedded_server.get_document_store("AttachmentExists") as store:
            with store.open_session() as session:
                # region exists
                exists = session.advanced.attachments.exists("categories/1-A", "image.jpg")
                if exists:
                    ...  # attachment 'image.jpg' exists on document 'categories/1-A'
                # endregion
