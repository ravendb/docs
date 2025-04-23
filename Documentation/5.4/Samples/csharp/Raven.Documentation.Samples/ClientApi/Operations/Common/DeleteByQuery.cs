using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Queries;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Operations.Common
{
    #region the_index
    // The index definition:
    // =====================
    
    public class Products_ByPrice : AbstractIndexCreationTask<Product>
    {
        public class IndexEntry
        {
            public decimal Price { get; set; }  
        }
        
        public Products_ByPrice()
        {
            Map = products => from product in products
                select new IndexEntry
                {
                    Price = product.PricePerUnit 
                };
        }
    }
    #endregion

    public class DeleteByQuery
    {
        public DeleteByQuery()
        {
            using (var store = new DocumentStore())
            {
                #region delete_by_query_0
                // Define the delete by query operation, pass an RQL querying a collection
                var deleteByQueryOp = new DeleteByQueryOperation("from 'Orders'");
                
                // Execute the operation by passing it to Operations.Send
                var operation = store.Operations.Send(deleteByQueryOp);
                
                // All documents in collection 'Orders' will be deleted from the server.
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region delete_by_query_1
                // Define the delete by query operation, pass an RQL querying a collection
                var deleteByQueryOp = new DeleteByQueryOperation("from 'Orders' where Freight > 30");
                
                // Execute the operation by passing it to Operations.Send
                var operation = store.Operations.Send(deleteByQueryOp);
                
                // * All documents matching the specified RQL will be deleted from the server.
                
                // * Since the dynamic query was made with a filtering condition,
                //   an auto-index is generated (if no other matching auto-index already exists).
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region delete_by_query_2
                // Define the delete by query operation, pass an RQL querying the index
                var deleteByQueryOp =
                    new DeleteByQueryOperation("from index 'Products/ByPrice' where Price > 10");
                
                // Execute the operation by passing it to Operations.Send
                var operation = store.Operations.Send(deleteByQueryOp);

                // All documents with document-field PricePerUnit > 10 will be deleted from the server.
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region delete_by_query_3
                // Define the delete by query operation
                var deleteByQueryOp = new DeleteByQueryOperation(new IndexQuery
                {
                    // Provide an RQL querying the index
                    Query = "from index 'Products/ByPrice' where Price > 10"
                });
                
                // Execute the operation by passing it to Operations.Send
                var operation = store.Operations.Send(deleteByQueryOp);

                // All documents with document-field PricePerUnit > 10 will be deleted from the server.
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region delete_by_query_4
                // Define the delete by query operation
                var deleteByQueryOp =
                    // Pass parameters:
                    // * The index name
                    // * A filtering expression on the index-field
                    new DeleteByQueryOperation<Products_ByPrice.IndexEntry>("Products/ByPrice",
                        x => x.Price > 10);
                
                // Execute the operation by passing it to Operations.Send
                var operation = store.Operations.Send(deleteByQueryOp);

                // All documents with document-field PricePerUnit > 10 will be deleted from the server.
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region delete_by_query_5
                // Define the delete by query operation
                var deleteByQueryOp =
                    // Pass param:
                    // * A filtering expression on the index-field
                    new DeleteByQueryOperation<Products_ByPrice.IndexEntry, Products_ByPrice>(
                        x => x.Price > 10);
                
                // Execute the operation by passing it to Operations.Send
                var operation = store.Operations.Send(deleteByQueryOp);

                // All documents with document-field PricePerUnit > 10 will be deleted from the server.
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region delete_by_query_6
                // Define the delete by query operation
                var deleteByQueryOp = new DeleteByQueryOperation(
                    // QUERY: Specify the query
                    new IndexQuery
                    {
                        Query = "from index 'Products/ByPrice' where Price > 10"
                    },
                    // OPTIONS: Specify the options for the operation
                    // (See all other available options in the Syntax section below)
                    new QueryOperationOptions
                    {
                        // Allow the operation to operate even if index is stale
                        AllowStale = true, 
                        // Get info in the operation result about documents that were deleted
                        RetrieveDetails = true 
                    });
                
                // Execute the operation by passing it to Operations.Send
                Operation operation = store.Operations.Send(deleteByQueryOp);
                    
                // Wait for operation to complete
                var result = operation.WaitForCompletion<BulkOperationResult>(TimeSpan.FromSeconds(15));
                
                // * All documents with document-field PricePerUnit > 10 will be deleted from the server.
                
                // * Details about deleted documents are available:
                var details = result.Details;
                var documentIdThatWasDeleted = details[0].ToJson()["Id"];
                #endregion
            }
        }

        public async Task DeleteByQueryAsync()
        {
            using (var store = new DocumentStore())
            {
                #region delete_by_query_0_async
                // Define the delete by query operation, pass an RQL querying a collection
                var deleteByQueryOp = new DeleteByQueryOperation("from 'Orders'");
                
                // Execute the operation by passing it to Operations.SendAsync
                var result = await store.Operations.SendAsync(deleteByQueryOp);
                
                // All documents in collection 'Orders' will be deleted from the server.
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region delete_by_query_1_async
                // Define the delete by query operation, pass an RQL querying a collection
                var deleteByQueryOp = new DeleteByQueryOperation("from 'Orders' where Freight > 30");
                
                // Execute the operation by passing it to Operations.SendAsync
                var result = await store.Operations.SendAsync(deleteByQueryOp);
                
                // * All documents matching the provided RQL will be deleted from the server.
                
                // * Since a dynamic query was made with a filtering condition,
                //   an auto-index is generated (if no other matching auto-index already exists).
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region delete_by_query_6_async
                // Define the delete by query operation
                var deleteByQueryOp = new DeleteByQueryOperation(
                    // QUERY: Specify the query
                    new IndexQuery
                    {
                        Query = "from index 'Products/ByPrice' where Price > 10"
                    },
                    // OPTIONS: Specify the options for the operation
                    // (See all other available options in the Syntax section below)
                    new QueryOperationOptions
                    {
                        // Allow the operation to operate even if index is stale
                        AllowStale = true, 
                        // Get info in the operation result about documents that were deleted
                        RetrieveDetails = true 
                    });
                
                // Execute the operation by passing it to Operations.Send
                Operation operation = await store.Operations.SendAsync(deleteByQueryOp);
                
                // Wait for operation to complete
                BulkOperationResult result = 
                    await operation.WaitForCompletionAsync<BulkOperationResult>(TimeSpan.FromSeconds(15))
                        .ConfigureAwait(false);
                
                // * All documents with document-field PricePerUnit > 10 will be deleted from the server.
                
                // * Details about deleted documents are available:
                var details = result.Details;
                var documentIdThatWasDeleted = details[0].ToJson()["Id"];
                #endregion
            }
        }
        
        private interface IFoo
        {
            #region syntax_1
            // Available overload:
            // ===================
            
            DeleteByQueryOperation DeleteByQueryOperation(
                string queryToDelete);
            
            DeleteByQueryOperation DeleteByQueryOperation(
                IndexQuery queryToDelete, 
                QueryOperationOptions options = null);

            DeleteByQueryOperation DeleteByQueryOperation<TEntity>(
                string indexName, 
                Expression<Func<TEntity, bool>> expression,
                QueryOperationOptions options = null);
            
            DeleteByQueryOperation DeleteByQueryOperation<TEntity, TIndexCreator>(
                Expression<Func<TEntity, bool>> expression,
                QueryOperationOptions options = null)
                where TIndexCreator : AbstractIndexCreationTask, new();
            #endregion
            
            /*
            #region syntax_2
            public class QueryOperationOptions
            {
                // Indicates whether operations are allowed on stale indexes.
                // DEFAULT: false
                public bool AllowStale { get; set; }
                
                // If AllowStale is set to false and index is stale, 
                // then this is the maximum timeout to wait for index to become non-stale. 
                // If timeout is exceeded then exception is thrown.
                // DEFAULT: null (if index is stale then exception is thrown immediately) 
                public TimeSpan? StaleTimeout { get; set; }
                
                // Limits the number of base operations per second allowed.
                // DEFAULT: no limit
                public int? MaxOpsPerSecond
                
                // Determines whether operation details about each document should be returned by server.
                // DEFAULT: false
                public bool RetrieveDetails { get; set; }
                
                // Ignore the maximum number of statements a script can execute.
                // Note: this is only relevant for the PatchByQueryOperation. 
                public bool IgnoreMaxStepsForScript { get; set; }
            }
            #endregion
            */
        }
    }
}
