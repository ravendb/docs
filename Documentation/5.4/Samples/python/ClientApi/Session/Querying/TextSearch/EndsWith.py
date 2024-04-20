from examples_base import ExampleBase, Product


class EndsWith(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_example(self):
        with self.embedded_server.get_document_store("EndsWith") as store:
            with store.open_session() as session:
                # region endsWith_1
                products = list(
                    session.query(object_type=Product)
                    # Call 'where_ends_with' on the field
                    # Pass the postfix to search by
                    .where_ends_with("Name", "Lager")
                )

                # Results will contain only Product documents having a 'Name' field
                # that ends with any case variation of 'lager'
                # endregion

            with store.open_session() as session:
                # region endsWith_4
                products = list(
                    session.query(object_type=Product)
                    # Pass 'exact=True' to search for an EXACT postfix match
                    .where_ends_with("Name", "Lager", exact=True)
                )

                # Results will contain only Product documents having a 'Name' field
                # that ends with 'Lager'
                # endregion

            with store.open_session() as session:
                # region endsWith_7
                products = list(
                    session.query(object_type=Product)
                    # Negate next statement
                    .not_()
                    # Call 'where_starts_with' on the field
                    # Pass the prefix to search by
                    .where_ends_with("Name", "Lager")
                )
                # Results will contain only Product documents having a 'Name' field
                # that does NOT end with 'lager' or any other case variations of it
                # endregion
