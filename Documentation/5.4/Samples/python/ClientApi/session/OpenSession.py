from typing import Optional

from ravendb import (
    RequestExecutor,
    SessionOptions,
    TransactionMode,
    DocumentSession,
    CreateDatabaseOperation,
)
from ravendb import DocumentStore as DocumentStoreReal
from ravendb.documents.conventions import DocumentConventions, ShouldIgnoreEntityChanges
from ravendb.infrastructure.orders import Employee
from ravendb.serverwide.database_record import DatabaseRecord

from examples_base import ExamplesBase


class DocumentStoreFake(DocumentStoreReal):
    def __init__(self):
        super().__init__("http://127.0.0.1:8080", "OpeningSession")
        self.initialize()
        try:
            self.maintenance.server.send(CreateDatabaseOperation(DatabaseRecord("OpeningSession")))
        except RuntimeError as e:
            if "ConcurrencyException" in e.args[0]:
                pass

        with self.open_session() as session:
            session.store(Employee(first_name="MC Ride"))
            session.save_changes()


class DocumentStoreFakeUninitialized(DocumentStoreReal):
    def __init__(self):
        super().__init__("http://127.0.0.1:8080", "OpeningSession")


class OpeningSession(ExamplesBase):
    def setUp(self):
        super().setUp()

    class IFoo2:
        # region Session_options
        def __init__(
            self,
            database: Optional[str] = None,
            no_tracking: Optional[bool] = None,
            no_caching: Optional[bool] = None,
            request_executor: Optional[RequestExecutor] = None,
            transaction_mode: Optional[TransactionMode] = None,
            disable_atomic_document_writes_in_cluster_wide_transaction: Optional[bool] = None,
        ):
            self.database = database
            self.no_tracking = no_tracking
            self.no_caching = no_caching
            self.request_executor = request_executor
            self.transaction_mode = transaction_mode
            self.disable_atomic_document_writes_in_cluster_wide_transaction = (
                disable_atomic_document_writes_in_cluster_wide_transaction
            )

        # endregion

    class IFoo:
        # region open_Session_1
        # Open a Session, you may pass either specified database, or preconfigured SessionOptions object
        # Passing no optional arguments opens a session for the default database configured in DocumentStore.database
        def open_session(
            self, database: Optional[str] = None, session_options: Optional[SessionOptions] = None
        ) -> DocumentSession:
            ...

        # endregion

    def test_sample(self):
        with self.embedded_server.get_document_store("your_database_here") as store1:
            DocumentStore = DocumentStoreFake
            # region open_session_2
            with DocumentStore() as store:
                store.open_session()
                # - is equivalent to:
                store.open_session(session_options=SessionOptions())

                # The second overload -
                store.open_session("your_database_name")
                # - is equivalent to:
                store.open_session(session_options=SessionOptions(database="your_database_name"))
            # endregion

            # region open_Session_3
            with DocumentStore() as store:
                # Open a Session in synchronous operation mode for cluster-wide transactions
                options = SessionOptions(database="your_database_name", transaction_mode=TransactionMode.CLUSTER_WIDE)

                with store.open_session(session_options=options) as session:
                    #   Run your business logic:
                    #
                    #   Store documents
                    #   Load and Modify documents
                    #   Query indexes & collections
                    #   Delete documents
                    #   ... etc.
                    session.save_changes()
            # endregion

            with DocumentStore() as store:
                # region open_Session_4
                with store.open_session() as session:
                    # code here
                    # endregion
                    ...

                # region open_Session_tracking_1
                with store.open_session(session_options=SessionOptions(no_tracking=True)) as session:
                    # note: providing expected result type turns on IntelliSense suggestions on the 'employee1'
                    employee1 = session.load("employees/1-A", Employee)
                    employee2 = session.load("employees/1-A")

                    # because NoTracking is set to 'true'
                    # each load will create a new Employee instance
                    self.assertNotEqual(employee1, employee2)
                # endregion

                # region open_Session_caching_1
                with store.open_session(session_options=SessionOptions(no_caching=True)) as session:
                    # code here
                    # endregion
                    ...
            DocumentStore = DocumentStoreFakeUninitialized
            # region ignore_entity_function
            with DocumentStore() as store:
                # Create new DocumentConventions object
                conventions = DocumentConventions()

                # Create a class that implements 'ravendb.documents.conventions.ShouldIgnoreEntityChanges'
                # and implement 'check' method - it's going to be called to check if entity should be ignored
                class MyCustomShouldIgnoreEntityChanges(ShouldIgnoreEntityChanges):
                    def check(self, session_operations: DocumentSession, entity: object, document_id: str) -> bool:
                        return isinstance(entity, Employee) and entity.first_name == "Bob"

                # Bind it to the conventions
                conventions.should_ignore_entity_changes = MyCustomShouldIgnoreEntityChanges

                # Update store conventions before initializing store
                store.conventions = conventions
                store.initialize()

                with store.open_session() as session:
                    employee1 = Employee(Id="employees/1", first_name="Alice")  # Entity is tracked
                    employee2 = Employee(Id="employees/2", first_name="Bob")  # Entity is ignored

                    session.save_changes()  # only employee1 is persisted

                    employee1.first_name = "Bob"  # Entity is now ignored
                    employee2.first_name = "Alice"  # Entity is now tracked

                    session.save_changes()  # Only employee2 is persisted
            # endregion
