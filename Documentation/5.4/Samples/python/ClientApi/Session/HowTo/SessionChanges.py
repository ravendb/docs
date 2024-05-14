from enum import Enum
from typing import Dict, List

from ravendb import DocumentsChanges

from examples_base import ExampleBase, Employee


class SessionChanges(ExampleBase):
    def test_session_changes(self):
        with self.embedded_server.get_document_store("SessionChanges") as store:
            # region changes_1
            with store.open_session() as session:
                # No changes made yet - 'has_changes' will be False
                self.assertFalse(session.has_changes())

                # Store a new entity within the session
                session.store(Employee(first_name="John", last_name="Doe"))

                # 'has_changes' will now be True
                self.assertTrue(session.has_changes())

                # 'has_changes' will reset to False after saving changes
                session.save_changes()
                self.assertFalse(session.has_changes())
            # endregion

            # region changes_2
            with store.open_session() as session:
                # Store (add) new entities, they will be tracked by the session
                session.store(Employee(first_name="John", last_name="Doe"), "employees/1-A")
                session.store(Employee(first_name="Jane", last_name="Doe"), "employees/2-A")

                # Call 'what_changed' to get all changes in the session
                changes = session.advanced.what_changed()
                self.assertEquals(2, len(changes))  # 2 entites were added

                # Get the change details for an entity, specify the entity ID
                changes_for_employee = changes["employees/1-A"]
                self.assertEquals(1, len(changes_for_employee))  # a single change for this entity (adding)

                # Get the change type
                change_type = changes_for_employee[0].change
                self.assertEquals(DocumentsChanges.ChangeType.DOCUMENT_ADDED, change_type)

                session.save_changes()
            # endregion

            # region changes_3
            with store.open_session() as session:
                # Load the entities, they will be tracked by the session
                employee_1 = session.load("employees/1-A", Employee)
                employee_2 = session.load("employees/2-A", Employee)

                # Modify entities
                employee_1.first_name = "Jim"
                employee_1.last_name = "Brown"
                employee_2.last_name = "Smith"

                # Delete an entity
                session.delete(employee_2)

                # Call 'what_changed' to get all changes in the session
                changes = session.advanced.what_changed()

                # Get the change details for an entity, specify the entity ID
                changes_for_employee = changes["employees/1-A"]

                self.assertEquals("FirstName", changes_for_employee[0].field_name)  # Field name
                self.assertEquals("Jim", changes_for_employee[0].field_new_value)  # New value
                self.assertEquals(
                    DocumentsChanges.ChangeType.FIELD_CHANGED, changes_for_employee[0].change
                )  # Change type

                self.assertEquals("LastName", changes_for_employee[1].field_name)  # Field name
                self.assertEquals("Brown", changes_for_employee[1].field_new_value)  # New value
                self.assertEquals(
                    DocumentsChanges.ChangeType.FIELD_CHANGED, changes_for_employee[1].change
                )  # Change type

                # Note: for employee2 - even though the LastName was changed to 'Smith',
                # the only reported change is the latest modification, which is the delete action.
                changes_for_employee = changes["employees/2-A"]
                self.assertEquals(DocumentsChanges.ChangeType.DOCUMENT_DELETED, changes_for_employee[0].change)

                session.save_changes()
            # endregion

    class Foo:
        # region syntax_1
        # has_changes
        def has_changes(self) -> bool: ...

        # endregion
        # region syntax_2
        # what_changed
        def what_changed(self) -> Dict[str, List[DocumentsChanges]]: ...

        # endregion

    class Foo:
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
