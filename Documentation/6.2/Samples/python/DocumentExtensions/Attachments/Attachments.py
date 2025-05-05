from typing import Union, List

import requests
from ravendb.documents.operations.attachments import CloseableAttachmentResult, AttachmentDetails, AttachmentName

from examples_base import ExampleBase


class Foo:
    # region StoreSyntax
    def store(
        self,
        entity_or_document_id: Union[object, str],
        name: str,
        stream: bytes,
        content_type: str = None,
        change_vector: str = None,
    ): ...

    # endregion
    # region GetSyntax
    def get(self, entity_or_document_id: str = None, name: str = None) -> CloseableAttachmentResult: ...

    class CloseableAttachmentResult:
        def __init__(self, response: requests.Response, details: AttachmentDetails):
            self.__details = details
            self.__response = response

    class AttachmentDetails(AttachmentName):
        def __init__(
            self, name: str, hash: str, content_type: str, size: int, change_vector: str = None, document_id: str = None
        ):
            super().__init__(...)
            ...

    class AttachmentName:
        def __init__(self, name: str, hash: str, content_type: str, size: int): ...

    def get_names(self, entity: object) -> List[AttachmentName]: ...

    def exists(self, document_id: str, name: str) -> bool: ...

    # endregion

    # region DeleteSyntax
    def delete(self, entity_or_document_id, name): ...

    # endregion


class Album:
    def __init__(self, Id: str = None, name: str = None, description: str = None, tags: List[str] = None):
        self.Id = Id
        self.name = name
        self.description = description
        self.tags = tags


class GetStoreDeleteAttachments(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_get_store_delete_attachments(self):
        with self.embedded_server.get_document_store("GetStoreDeleteAttachments") as store:
            # region StoreAttachment
            with store.open_session() as session:
                with open("001.jpg", "r") as file1:
                    file1_data = file1.read()
                    file2_data = b"file_2_content"  # Mock
                    file3_data = b"file_3_content"  # Mock
                    file4_data = b"file_4_content"  # Mock
                    album = Album(
                        name="Holidays",
                        description="Holidays travel pictures of the all family",
                        tags=["Holidays Travel", "All Family"],
                    )
                    session.store(album, "albums/1")

                    session.advanced.attachments.store("albums/1", "001.jpg", file1_data, "image/jpeg")
                    session.advanced.attachments.store("albums/1", "002.jpg", file2_data, "image/jpeg")
                    session.advanced.attachments.store("albums/1", "003.jpg", file3_data, "image/jpeg")
                    session.advanced.attachments.store("albums/1", "004.jpg", file4_data, "image/jpeg")

                    session.save_changes()
            # endregion

            # region GetAttachment
            with store.open_session() as session:
                album = session.load("albums/1", Album)

                with session.advanced.attachments.get(album, "001.jpg") as file1:
                    with session.advanced.attachments.get("albums/1", "002.jpg") as file2:
                        bytes_data = file1.data

                        attachment_details = file1.details
                        name = attachment_details.name
                        content_type = attachment_details.content_type
                        hash_ = attachment_details.hash
                        size = attachment_details.size
                        document_id = attachment_details.document_id
                        change_vector = attachment_details.change_vector

                attachment_names = session.advanced.attachments.get_names(album)
                for attachment_name in attachment_names:
                    name = attachment_name.name
                    content_type = attachment_name.content_type
                    hash_ = attachment_name.hash
                    size = attachment_name.size

                exists = session.advanced.attachments.exists("albums/1", "003.jpg")
            # endregion
            # region DeleteAttachment
            with store.open_session() as session:
                album = session.load("albums/1")
                session.advanced.attachments.delete(album, "001.jpg")
                session.advanced.attachments.delete("albums/1", "002.jpg")

                session.save_changes()
            # endregion
