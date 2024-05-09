from ravendb import DeleteByQueryOperation, AbstractIndexCreationTask, IndexQuery, QueryOperationOptions

from examples_base import ExampleBase


# region the_index
# The index definition:
# =====================
class ProductsByPrice(AbstractIndexCreationTask):
    class IndexEntry:
        def __init__(self, price: int):
            self.price = price

    def __init__(self):
        super().__init__()
        self.map = "from product in products select new {price = product.PricePerUnit}"


# endregion


class DeleteByQuery(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_delete_by_query(self):
        with self.embedded_server.get_document_store() as store:
            # region delete_by_query_0
            # Define the delete by query operation, pass an RQL querying a collection
            delete_by_query_op = DeleteByQueryOperation("from 'Orders'")

            # Execute the operation by passing it to Operation.send_async
            operation = store.operations.send_async(delete_by_query_op)

            # All documents in collection 'Orders' will be deleted from the server
            # endregion

            # region delete_by_query_1
            # Define the delete by query operation, pass an RQL querying a collection
            delete_by_query_op = DeleteByQueryOperation("from 'Orders' where Freight > 30")

            # Execute the operation by passing it to Operation.send_async
            operation = store.operations.send_async(delete_by_query_op)

            # * All documents matching the specified RQL will be deleted from the server.
            #
            # * Since the dynamic query was made with a filtering condition,
            #   an auto-index is generated (if no other matching auto-index already exists).
            # endregion
            ProductsByPrice().execute(store)

            # region delete_by_query_2
            # Define the delete by query operation, pass an RQL querying the index
            delete_by_query_op = DeleteByQueryOperation("from index 'Products/ByPrice' where Price > 10")

            # Execute the operation by passing it to Operation.send_async
            operation = store.operations.send_async(delete_by_query_op)

            # All documents with document-field PricePerUnit > 10 will be deleted from the server.
            # endregion

            # region delete_by_query_3
            # Define the delete by query operation
            delete_by_query_op = DeleteByQueryOperation(
                IndexQuery(query="from index 'Products/ByPrice' where Price > 10")
            )

            # Execute the operation by passing it to Operation.send_async
            operation = store.operations.send_async(delete_by_query_op)

            # All documents with document-field PricePerUnit > 10 will be deleted from the server.
            # endregion
            # region delete_by_query_6
            # Define the delete by query operation
            delete_by_query_op = DeleteByQueryOperation(
                # QUERY: Specify the query
                IndexQuery(query="from index 'Products/ByPrice' where Price > 10"),
                # OPTIONS: Specify the options for the operations
                # (See all other available options in the Syntax section below)
                QueryOperationOptions(
                    # Allow the operation to operate even if index is stale
                    allow_stale=True,
                    # Get info in the operation result about documents that were deleted
                    retrieve_details=True,
                ),
            )

            # Execute the operation by passing it to Operations.send_async
            operation = store.operations.send_async(delete_by_query_op)

            # * All documents with document-field PricePerUnit > 10 will be deleted from the server

            # * Details about deleted documents are available:
            details = result.details
            document_id_that_was_deleted = details[0]["Id"]
            # endregion
