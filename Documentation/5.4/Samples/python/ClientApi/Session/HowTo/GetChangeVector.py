from examples_base import ExampleBase


class Foo:
    # region get_change_vector_1
    def get_change_vector_for(self, entity: object) -> str: ...

    # endregion


class GetChangeVector(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_get_change_vector(self):
        with self.embedded_server.get_document_store("GetChangeVector") as store:
            with store.open_session() as session:
                # region get_change_vector_2
                employee = session.load("employees/1-A")
                change_vector = session.advanced.get_change_vector_for(employee)
                # endregion
