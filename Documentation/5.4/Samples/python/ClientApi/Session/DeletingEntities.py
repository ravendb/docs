from typing import Optional, Union

from ravendb.documents.commands.batches import DeleteCommandData
from ravendb.infrastructure.orders import Employee

from examples_base import ExamplesBase


class DeletingEntities(ExamplesBase):
    def setUp(self):
        super().setUp()
        with self.embedded_server.get_document_store("DeletingEntities") as store:
            with store.open_session() as session:
                session.store(Employee(), "employees/1")
                session.save_changes()

    class Foo:
        # region deleting_1
        def delete(self, key_or_entity: Union[str, object], expected_change_vector: Optional[str] = None) -> None:
            ...

        # endregion

    def test_deleting_entities(self):
        with self.embedded_server.get_document_store("DeletingEntities") as store:
            with store.open_session() as session:
                # region deleting_2

                employee = session.load("employees/1")

                session.delete(employee)
                session.save_changes()

                # endregion

            with store.open_session() as session:
                # region deleting_3

                session.delete("employees/1")
                session.save_changes()

                # endregion

            with store.open_session() as session:
                # region deleting_4

                session.delete("employees/1")

                # endregion

            with store.open_session() as session:
                # region deleting_5

                session.advanced.defer(DeleteCommandData("employees/1", change_vector=None))

                # endregion
