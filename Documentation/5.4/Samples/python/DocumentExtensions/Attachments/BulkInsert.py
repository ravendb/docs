from examples_base import ExampleBase, User


class BulkInsertAttachments(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_append_using_bulk_insert(self):
        with self.embedded_server.get_document_store("BulkInsertAttachments") as store:
            # Create documents to bulk-insert to
            with store.open_session() as session:
                user1 = User("users/1-A", "Lilly", 20)
                user2 = User("users/2-A", "Betty", 25)
                user3 = User("users/3-A", "Robert", 29)
                session.store(user1)
                session.store(user2)
                session.store(user3)
                session.save_changes()

            # region bulk_insert_attachment
            # Choose user profiles for which to attach a file
            with store.open_session() as session:
                user_ids = [
                    session.advanced.get_document_id(user)
                    for user in list(session.query(object_type=User).where_less_than("Age", 30))
                ]

            # Prepare content to attach
            bytes_to_attach = b"some contents here"

            # Create a BulkInsert instance
            with store.bulk_insert() as bulk_insert:
                for user_id in user_ids:
                    # Call 'attachments_for', pass the document ID for which to attach the file
                    attachments_bulk_insert = bulk_insert.attachments_for(user_id)

                    # Call 'store' to add the file to the BulkInsert instance
                    # The data stored in bulk_insert will be streamed to the server in batches
                    attachments_bulk_insert.store("AttachmentName", bytes_to_attach)
            # endregion
