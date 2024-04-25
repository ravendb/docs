from examples_base import ExampleBase, Product


class OptimisticConcurrency(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_optimistic_concurrency(self):
        with self.embedded_server.get_document_store("OptimisticConcurrency") as store:
            # region optimistic_concurrency_1
            with store.open_session() as session:
                # Enable optimistic concurrency for this session
                session.advanced.use_optimistic_concurrency = True

                # Save a document in this session
                product = Product(name="Some name")
                session.store(product, "products/999")
                session.save_changes()

                # Modify the document 'externally' by another session
                with store.open_session() as other_session:
                    other_product = other_session.load("products/999")
                    other_product.name = "Other name"
                    other_session.save_changes()

                # Trying to modify the document without reloading it first will throw
                product.name = "Better Name"
                session.save_changes()  # This will throw a ConcurrencyException

        # region optimistic_concurrency_2
        # Enable for all sessions that will be opened within this document store
        store.conventions.use_optimistic_concurrency = True
        with store.open_session() as session:
            is_session_using_optimistic_concurrency = session.advanced.use_optimistic_concurrency  # will return True
        # endregion

        # region optimistic_concurrency_3
        with store.open_session() as session:
            # Store document 'products/999'
            session.store(Product(name="Some name", Id="products/999"))
            session.save_changes()

        with store.open_session() as session:
            # Enable optimistic concurrency for the session
            session.advanced.use_optimistic_concurrency = True

            # Store the same document
            # Pass 'null' as the change_vector to turn OFF optimistic concurrency for this document
            session.store(Product(name="Some Other Name"), change_vector=None, key="products/999")

            # This will NOT throw a ConcurrencyException, and the document will be saved
            session.save_changes()
        # endregion

        # region optimistic_concurrency_4
        with store.open_session() as session:
            # Store document 'products/999'
            session.store(Product(name="Some name", Id="products/999"))
            session.save_changes()

        with store.open_session() as session:
            # Disable optimistic concurrency for the session
            session.advanced.use_optimistic_concurrency = False

            # Store the same document
            # Pass empty str as the change_vector to turn ON optimistic concurrency for this document
            session.store(Product(name="Some Other Name"), key="products/999", change_vector="")

            # This will throw a ConcurrencyException, and the document will NOT be saved
            session.save_changes()
        # endregion
