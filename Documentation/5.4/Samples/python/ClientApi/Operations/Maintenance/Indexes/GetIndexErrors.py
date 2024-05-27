import datetime
from typing import Optional, List

from ravendb import GetIndexErrorsOperation
from ravendb.documents.indexes.definitions import IndexingError, IndexErrors
from ravendb.documents.operations.definitions import MaintenanceOperation

from examples_base import ExampleBase


class GetIndexErrors(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_get_index_errors(self):
        with self.embedded_server.get_document_store("GetIndexErrors") as store:
            # region get_errors_all
            # Define the get index errors operation
            get_index_errors_op = GetIndexErrorsOperation()

            # Execute the operation by passing it to maintenance.send
            index_errors = store.maintenance.send(get_index_errors_op)

            # index_errors will contain errors for ALL indexes
            # endregion

            # region get_errors_specific
            # Define the get index errors operation for specific indexes
            get_index_errors_op = GetIndexErrorsOperation("Orders/Totals")

            # Execute the operation by passing it to maintenance.send
            # An exception will be thrown if any of the specified indexes do not exist
            index_errors = store.maintenance.send(get_index_errors_op)

            # index_errors will contain errors only for index "Orders/Totals"
            # endregion

    class Foo:
        # region syntax_1
        class GetIndexErrorsOperation(MaintenanceOperation[List[IndexErrors]]):
            def __init__(self, *index_names: str):  # If no index_names provided, get errors for all indexes
                ...

        # endregion
        # region syntax_2
        class IndexErrors:
            def __init__(self, name: Optional[str] = None, errors: Optional[List[IndexingError]] = None):
                self.name = name  # Index name
                self.errors = errors  # List of errors for this index

        # endregion

        # region syntax_3
        class IndexingError:
            def __init__(
                self,
                error: Optional[str] = None,
                timestamp: Optional[datetime.datetime] = None,
                document: Optional[str] = None,
                action: Optional[str] = None,
            ):
                # Error message
                self.error = error

                # Time of error
                self.timestamp = timestamp

                # If action is 'Map'    - field will contain the document ID
                # If action is 'Reduce' - field will contain the Reduce key value
                # For all other actions - field will be None
                self.document = document

                # Area where error has occurred, e.g. Map/Reduce/Analyzer/Memory/etc.
                self.action = action

        # endregion
