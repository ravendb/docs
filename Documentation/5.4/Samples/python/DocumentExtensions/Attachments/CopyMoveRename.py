from typing import Union

from examples_base import ExampleBase, Employee


class Foo:
    # region copy_0
    def copy(
        self,
        entity_or_document_id: Union[object, str],
        source_name: str,
        destination_entity_or_document_id: object,
        destination_name: str,
    ) -> None: ...

    # endregion

    # region rename_0
    def rename(self, entity_or_document_id: Union[str, object], name: str, new_name: str) -> None: ...

    # endregion
    # region move_0
    def move(
        self,
        source_entity_or_document_id: Union[str, object],
        source_name: str,
        destination_entity_or_document_id: Union[str, object],
        destination_name: str,
    ) -> None: ...

    # endregion


class CopyMoveRename(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_copy_move_rename(self):
        with self.embedded_server.get_document_store("CopyMoveRename") as store:
            with store.open_session() as session:
                # region copy_1
                employee_1 = session.load("employees/1-A")
                employee_2 = session.load("employees/2-A")

                session.advanced.attachments.copy(employee_1, "photo.jpg", employee_2, "photo-copy.jpg")

                session.save_changes()
                # endregion

                # region rename_1
                employee = session.load("employees/1-A", Employee)

                session.advanced.attachments.rename(employee, "photo.jpg", "photo-new.jpg")

                session.save_changes()
                # endregion

                # region move_1
                employee1 = session.load("employees/1-A")
                employee2 = session.load("employees/2-A")

                session.advanced.attachments.move(employee1, "photo.jpg", employee2, "photo.jpg")

                session.save_changes()
                # endregion
