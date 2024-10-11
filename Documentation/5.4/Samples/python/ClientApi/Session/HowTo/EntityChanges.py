from enum import Enum
from typing import Dict, List

from ravendb import DocumentsChanges

from examples_base import ExampleBase, Employee


class EntityChanges(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_entity_changes(self):
        with self.embedded_server.get_document_store("EntityChanges") as store:
            # region changes_1
            with store.open_session() as session:
                # Store a new entity within the session
                # =====================================

                employee = Employee(first_name="John", last_name="Doe")
                session.store(employee, "employees/1-A")

                # 'has_changed' will be True
                self.assertTrue(session.advanced.has_changed(employee))

                # 'has_changed' will reset to False after saving changes
                session.save_changes()
                self.assertFalse(session.advanced.has_changed(employee))

                # Load & modify entity within the session
                # =======================================

                employee = session.load("employees/1-A", Employee)
                self.assertFalse(session.advanced.has_changed(employee))  # False

                employee.last_name = "Brown"
                self.assertTrue(session.advanced.has_changed(employee))  # True

                session.save_changes()
                self.assertFalse(session.advanced.has_changed(employee))  # False

            # endregion

            # region changes_2
            with store.open_session() as session:
                # Store (add) a new entity, it will be tracked by the session
                employee = Employee(first_name="John", last_name="Doe")
                session.store(employee, "employees/1-A")

                # Get the changes for the entity in the session
                # Call 'what_changed', pass the document id as a key to a resulting dict
                changes = session.advanced.what_changed()
                changes_for_employee = changes["employees/1-A"]

                self.assertEquals(1, len(changes_for_employee))  # a single change for this entity (adding)

                # Get the change type
                change_type = changes_for_employee[0].change
                self.assertEquals(DocumentsChanges.ChangeType.DOCUMENT_ADDED, change_type)

                session.save_changes()
            # endregion

            # region changes_3
            with store.open_session() as session:
                # Load the entity, it will be tracked by the session
                employee = session.load("employees/1-A", Employee)

                # Modify the entity
                employee.first_name = "Jim"
                employee.last_name = "Brown"

                # Get the changes for the entity in the session
                # Call 'what_changed', pass the document id as a key to a resulting dict
                changes = session.advanced.what_changed()
                changes_for_employee = changes["employees/1-A"]

                self.assertEquals("FirstName", changes_for_employee[0].field_name)  # Field name
                self.assertEquals("Jim", changes_for_employee[0].field_new_value)  # New value
                self.assertEquals(
                    changes_for_employee[0].change, DocumentsChanges.ChangeType.FIELD_CHANGED
                )  # Change type

                self.assertEquals("LastName", changes_for_employee[1].field_name)  # Field name
                self.assertEquals("Brown", changes_for_employee[1].field_new_value)  # New value
                self.assertEquals(
                    changes_for_employee[1].change, DocumentsChanges.ChangeType.FIELD_CHANGED
                )  # Change type

                session.save_changes()
            # endregion


class Foo:
    # region syntax_1
    # has_changed
    def has_changed(self, entity: object) -> bool: ...

    # endregion
    # region syntax_2
    # what_changed
    def what_changed(self) -> Dict[str, List[DocumentsChanges]]: ...

    # endregion

    # region syntax_3
    class DocumentsChanges:

        def __init__(
            self,
            field_old_value: object,
            field_new_value: object,
            change: DocumentsChanges.ChangeType,
            field_name: str = None,
            field_path: str = None,
        ): ...

        class ChangeType(Enum):
            DOCUMENT_DELETED = "DocumentDeleted"
            DOCUMENT_ADDED = "DocumentAdded"
            FIELD_CHANGED = "FieldChanged"
            NEW_FIELD = "NewField"
            REMOVED_FIELD = "RemovedField"
            ARRAY_VALUE_CHANGED = "ArrayValueChanged"
            ARRAY_VALUE_ADDED = "ArrayValueAdded"
            ARRAY_VALUE_REMOVED = "ArrayValueRemoved"

    # endregion
