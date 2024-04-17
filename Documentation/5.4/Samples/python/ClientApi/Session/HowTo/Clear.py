from examples_base import ExampleBase, Employee


class Foo:
    # region clear_1
    def clear(self) -> None: ...

    # endregion


class ClearExample(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_example1(self):
        with self.embedded_server.get_document_store("Clear") as store:
            with store.open_session() as session:
                # region clear_2
                session.store(Employee(first_name="John", last_name="Doe"))

                session.advanced.clear()

                session.save_changes()  # nothing will happen
                # endregion
