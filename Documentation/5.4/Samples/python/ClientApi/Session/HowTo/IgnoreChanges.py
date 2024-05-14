from examples_base import ExampleBase, Product


class Foo:
    # region ignore_changes_1

    def ignore_changes_for(self, entity: object) -> None: ...

    # endregion


class IgnoreChanges(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_ignore_changes(self):
        with self.embedded_server.get_document_store("IgnoreChanges") as store:
            with store.open_session() as session:
                self.add_products(session)
                # region ignore_changes_2
                product = session.load("products/1-A", Product)
                session.ignore_changes_for(product)
                product.units_in_stock += 1  # this will be ignored for save_changes
                session.save_changes()
                # endregion
