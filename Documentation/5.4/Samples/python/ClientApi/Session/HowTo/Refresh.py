from examples_base import ExampleBase, Employee


class Foo:
    # region refresh_1
    def refresh(self, entity: object) -> object: ...

    # endregion


class HowToRefresh(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_refresh(self):
        with self.embedded_server.get_document_store("Refresh") as store:
            with store.open_session() as session:
                # region refresh_2
                employee = session.load("employees/1", Employee)
                self.assertEquals("Doe", employee.last_name)

                # LastName changed to "Shmoe"

                session.advanced.refresh(employee)
                self.assertEquals("Shmoe", employee.last_name)
                # endregion
