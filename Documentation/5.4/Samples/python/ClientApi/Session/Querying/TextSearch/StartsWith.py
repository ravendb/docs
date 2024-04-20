from examples_base import ExampleBase, Product


class StartsWith(ExampleBase):

    def setUp(self):
        super().setUp()

    def test_example(self):
        with self.embedded_server.get_document_store("StartsWith") as store:
            with store.open_session() as session:
                # region startsWith_1
                products = list(
                    session.query(object_type=Product)
                    # Call 'where_starts_with' on the field
                    # Pass the prefix to search by
                    .where_starts_with("Name", "Ch")
                )

                # Results will contain only Product documents having a 'Name' field
                # that starts with any case variation of 'ch'
                # endregion

            with store.open_session() as session:
                # region startsWith_4
                products = list(
                    session.query(object_type=Product)
                    # Pass 'exact=True' to search for an EXACT prefix match
                    .where_starts_with("Name", "Ch", exact=True)
                )

                # Results will contain only Product documents having a 'Name' field
                # that starts with 'Ch'
                # endregion

            with store.open_session() as session:
                # region startsWith_7
                products = list(
                    session.query(object_type=Product)
                    # Negate next statement
                    .not_()
                    # Call 'where_starts_with' on the field
                    # Pass the prefix to search by
                    .where_starts_with("Name", "Ch")
                )
                # Results will contain only Product documents having a 'Name' field
                # that does NOT start with 'ch' or any other case variations of it
                # endregion
