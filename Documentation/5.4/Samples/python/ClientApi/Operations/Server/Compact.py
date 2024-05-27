from ravendb import GetIndexNamesOperation, GetDatabaseRecordOperation
from ravendb.documents.operations.compact import CompactDatabaseOperation
from ravendb.documents.operations.definitions import OperationIdResult
from ravendb.primitives.constants import int_max
from ravendb.serverwide.misc import CompactSettings
from ravendb.serverwide.operations.common import ServerOperation

from examples_base import ExampleBase


class Foo:
    class CompactDatabaseOperation(ServerOperation[OperationIdResult]):
        def __init__(self, compact_settings: CompactSettings) -> None: ...


class Compact(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_compact(self):
        with self.embedded_server.get_document_store("Compact") as store:
            # region compact_0
            # Define the compact settings
            settings = CompactSettings(
                # Database to compact
                "Northwind",
                # Set 'documents' to True to compact all documents in database
                # Indexes are not set and will not be compacted
                documents=True,
            )

            # Define the compact operation, pass the settings
            compact_op = CompactDatabaseOperation(settings)

            # Execute compaction by passing the operation to maintenance.server.send
            operation = store.maintenance.server.send_async(compact_op)

            # Wait for operation to complete, during compaction the database is offline
            operation.wait_for_completion()
            # endregion

            # region compact_1
            # Define the compact settings
            settings = CompactSettings(
                # Database to compact
                database_name="Northwind",
                # Setting 'documents' to False will compact only the specified indexes
                documents=False,
                # Specify which indexes to compact
                indexes=["Orders/Totals", "Orders/ByCompany"],
                # Optimize indexes is Lucene's feature to gain disk space and efficiency
                # Set whether to skip this optimization when compacting the indexes
                skip_optimize_indexes=False,
            )
            # Define the compact operation, pass the settings
            compact_op = CompactDatabaseOperation(settings)

            # Execute compaction by passing the operation to maintenance.server.send
            operation = store.maintenance.server.send_async(compact_op)
            # Wait for operation to complete
            operation.wait_for_completion()
            # endregion

            # region compact_2
            #  Get all indexes names in the database using the 'GetIndexNamesOperation' operation
            #  Use 'ForDatabase' if the target database is different from the default database defined on the store
            all_indexes_names = store.maintenance.for_database("Northwind").send(GetIndexNamesOperation(0, int_max))

            # Define the compact settings
            settings = CompactSettings(
                database_name="Northwind",  # Database to compact
                documents=True,  # Compact all documents
                indexes=all_indexes_names,  # All indexes will be compacted
                skip_optimize_indexes=True,  # Skip Lucene's indexes optimization
            )

            # Define the compact operation, pass the settings
            compact_op = CompactDatabaseOperation(settings)

            # Execute compaction by passing the operation to maintenance.server.send
            operation = store.maintenance.server.send(compact_op)
            # Wait for operation to complete
            operation.wait_for_completion()
            # endregion

            # region compact_3
            # Get all member nodes in the database-group using the 'GetDatabaseRecordOperation' operation
            all_member_nodes = store.maintenance.server.send(GetDatabaseRecordOperation("Northwind")).topology.members

            # Define the compact settings as needed
            settings = CompactSettings(
                # Database to compact
                database_name="Northwind",
                # Compact all documents in database
                documents=True,
            )

            # Execute the compact operation on each member node
            for node_tag in all_member_nodes:
                # Define the compact operation, pass the settings
                compact_op = CompactDatabaseOperation(settings)

                # Execute the operation on a specific node
                # Use 'for_node' to specify the node to operate on
                # todo: https://issues.hibernatingrhinos.com/issue/RDBC-847/Implement-documentStore.Maintenance.Server.ForNode
                operation = store.maintenance.server.for_node(node_tag).send_async(compact_op)

                # Wait for operation to complete
                operation.wait_for_completion()

            # endregion
