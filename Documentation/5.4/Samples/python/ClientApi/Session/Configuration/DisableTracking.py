from abc import ABC, abstractmethod

from ravendb import InMemoryDocumentSessionOperations, SessionOptions, DocumentStore, DocumentSession
from ravendb.documents.conventions import ShouldIgnoreEntityChanges

from examples_base import ExampleBase, Product, Employee


class DisableTracking(ExampleBase):
    def setUp(self):
        super().setUp()

    class IgnoreChangesEntity:
        # region syntax_1
        def ignore_changes_for(self, entity: object) -> None: ...

        # endregion

    class IgnoreChangesConvention:
        should_ignore_entity_changes = None

        # region syntax_2
        @should_ignore_entity_changes.setter
        def should_ignore_entity_changes(self, value: ShouldIgnoreEntityChanges) -> None: ...

        class ShouldIgnoreEntityChanges(ABC):
            @abstractmethod
            def check(
                self,
                session_operations: "InMemoryDocumentSessionOperations",
                entity: object,
                document_id: str,
            ) -> bool:
                pass

        # endregion

    def test_disable_tracking(self):
        with self.embedded_server.get_document_store("DisableTracking") as store:
            with store.open_session() as session:
                # region disable_tracking_1
                # Load a product entity, the session will track its changes
                product = session.load("products/1-A", Product)

                # Disable tracking for the loaded product entity
                session.ignore_changes_for(product)

                # The following change will be ignored for save_changes
                product.units_in_stock += 1
                session.save_changes()
                # endregion

            # region disable_tracking_2
            with store.open_session(
                SessionOptions(
                    # Disable tracking for all entities in the session's options
                    no_tracking=True
                )
            ):
                # Load any entity, it will Not be tracked by the session
                employee1 = session.load("employees/1-A", Employee)

                # Loading again from same document will result in a new entity instance
                employee2 = session.load("employees/1-A", Employee)

                # Entities instances are not the same
                self.assertNotEqual(employee1, employee2)

            # endregion

            # region disable_tracking_3
            with store.open_session() as session:
                # Define a query
                employees_results = list(
                    session.advanced.document_query(object_type=Employee)
                    # Set no_tracking, all resulting entities will not ne tracked
                    .no_tracking().where_equals("FirstName", "Robert")
                )

                # The following modification will not be tracked for save_changes
                first_employee = employees_results[0]
                first_employee.last_name = "NewName"

                # Change to 'first_employee' will not be persisted
                session.save_changes()
            # endregion

            # region disable_tracking_4
            with DocumentStore() as store:
                # Define the 'ignore' convention on your document store:

                # Create a class that implements 'ravendb.documents.conventions.ShouldIgnoreEntityChanges'
                # and implement 'check' method - it's going to be called to check if entity should be ignored

                class MyCustomShouldIgnoreEntityChanges(ShouldIgnoreEntityChanges):
                    def check(self, session_operations: DocumentSession, entity: object, document_id: str) -> bool:
                        # Define for which entities tracking should be disabled
                        # Tracking will be disabled ONLY for entities of type Employee whose FirstName is Bob
                        return isinstance(entity, Employee) and entity.first_name == "Bob"

                store.conventions.should_ignore_entity_changes = MyCustomShouldIgnoreEntityChanges

                store.initialize()

                with store.open_session() as session:
                    employee1 = Employee(first_name="Alice", Id="employees/1")
                    employee2 = Employee(first_name="Bob", Id="employees/2")

                    session.store(employee1)  # This entity will be tracked
                    session.store(employee2)  # Changes to this entity will be ignored

                    session.save_changes()

                    employee1.first_name = "Bob"  # Changes to this entity will now be ignored
                    employee2.first_name = "Alice"  # This entity will now be tracked

                    session.save_changes()

            # endregion
