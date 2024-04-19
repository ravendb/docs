from examples_base import ExampleBase, Employee


class Foo:
    # region evict_1
    def evict(self, entity: object) -> None: ...

    # endregion


class EvictExample(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_evict(self):
        with self.embedded_server.get_document_store("Evict") as store:
            with store.open_session() as session:
                # region evict_2
                employee_1 = Employee(first_name="John", last_name="Doe")
                employee_2 = Employee(first_name="Joe", last_name="Shmoe")

                session.store(employee_1)
                session.store(employee_2)

                session.advanced.evict(employee_1)

                session.save_changes()  # only 'Joe Shmoe' will be saved
                # endregion

            with store.open_session() as session:
                # region evict_3
                employee = session.load("employees/1-A")  # loading from server
                employee = session.load("employees/1-A")  # no server call
                session.advanced.evict(employee)
                employee = session.load("employees/1-A")  # loading form server
                # endregion
