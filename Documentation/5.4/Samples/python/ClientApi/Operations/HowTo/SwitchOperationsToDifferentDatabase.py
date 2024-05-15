from ravendb import MaintenanceOperationExecutor, DocumentStore, GetStatisticsOperation
from ravendb.documents.operations.executor import OperationExecutor
from ravendb.documents.operations.revisions import GetRevisionsOperation

from examples_base import ExampleBase, Order, Company


class SwitchOperationsToDifferentDatabase(ExampleBase):
    def setUp(self):
        super().setUp()

    class Foo:
        # region syntax_1
        def for_database(self, database_name: str) -> OperationExecutor: ...

        # endregion
        # region syntax_2
        def for_database(self, database_name: str) -> MaintenanceOperationExecutor: ...

        # endregion

    def test_switch_operations_to_different_database(self):
        # region for_database_1
        # Define default database on the store
        document_store = DocumentStore(urls=["yourServerURL"], database="DefaultDB")
        document_store.initialize()

        with document_store:
            # Use 'for_database', get operation executor for another database
            op_executor = document_store.operations.for_database("AnotherDB")

            # Send the operation, e.g. 'GetRevisionsOperation' will be executed on "AnotherDB"
            revisions_in_another_db = op_executor.send(GetRevisionsOperation("Orders/1-A", Order))

            # Without 'for_database', the operation is executed "DefaultDB"
            revisions_in_default_db = document_store.operations.send(GetRevisionsOperation("Company/1-A", Company))
        # endregion

        # region for_database_2
        # Define default database on the store
        document_store = DocumentStore(urls=["yourServerURL"], database="DefaultDB")
        document_store.initialize()

        with DocumentStore() as document_store:
            # Use 'for_database', get maintenance operation executor for another database
            op_executor = document_store.maintenance.for_database("AnotherDB")
            # Send the maintenance operation, e.g. get database stats for "AnotherDB"
            stats_for_another_db = op_executor.send(GetStatisticsOperation())
            # Without 'for_database', the stats are retrieved for "DefaultDB"
            stats_for_default_db = document_store.maintenance.send(GetStatisticsOperation())
        # endregion
