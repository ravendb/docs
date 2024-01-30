import { DocumentStore, AbstractIndexCreationTask } from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

{
    //region the_index
    // The index definition:
    // =====================
    
    class Products_ByPrice extends AbstractJavaScriptIndexCreationTask {
        constructor () {
            super();

            this.map("products", product => {
                return {
                    Price: product.PricePerUnit
                };
            });
        }
    }
    //endregion

    async function deleteByQuery() {
        {
            //region delete_by_query_0
            // Define the delete by query operation, pass an RQL querying a collection
            const deleteByQueryOp = new DeleteByQueryOperation("from 'Orders'");

            // Execute the operation by passing it to operations.send
            const operation = await store.operations.send(deleteByQueryOp);
            
            // All documents in collection 'Orders' will be deleted from the server.
            //endregion
        }

        {
            //region delete_by_query_1
            // Define the delete by query operation, pass an RQL querying a collection
            const deleteByQueryOp = new DeleteByQueryOperation("from 'Orders' where Freight > 30");
            
            // Execute the operation by passing it to operations.send
            const operation = await store.operations.send(deleteByQueryOp);
            
            // * All documents matching the specified RQL will be deleted from the server.
            
            // * Since the dynamic query was made with a filtering condition,
            //   an auto-index is generated (if no other matching auto-index already exists).
            //endregion
        }

        {
            //region delete_by_query_2
            // Define the delete by query operation, pass an RQL querying the index
            const deleteByQueryOp = 
                new DeleteByQueryOperation("from index 'Products/ByPrice' where Price > 10");

            // Execute the operation by passing it to operations.send
            const operation = await store.operations.send(deleteByQueryOp);
            
            // All documents with document-field PricePerUnit > 10 will be deleted from the server.
            //endregion
        }

        {
            //region delete_by_query_3
            // Define the index query, provide an RQL querying the index
            const indexQuery = new IndexQuery();
            indexQuery.query = "from index 'Products/ByPrice' where Price > 10";

            // Define the delete by query operation
            const deleteByQueryOp = new DeleteByQueryOperation(indexQuery);

            // Execute the operation by passing it to operations.send
            const operation = await store.operations.send(deleteByQueryOp);

            // All documents with document-field PricePerUnit > 10 will be deleted from the server.
            //endregion
        }

        {
            //region delete_by_query_4
            // QUERY: Define the index query, provide an RQL querying the index
            const indexQuery = new IndexQuery();
            indexQuery.query = "from index 'Products/ByPrice' where Price > 10";
            
            // OPTIONS: Define the operations options
            // (See all available options in the Syntax section below)
            const options = {
                // Allow the operation to operate even if index is stale
                allowStale: true,
                // Limit the number of base operations per second allowed.
                maxOpsPerSecond: 500
            }
            
            // Define the delete by query operation
            const deleteByQueryOp = new DeleteByQueryOperation(indexQuery, options);
            
            // Execute the operation by passing it to operations.send
            const operation = await store.operations.send(deleteByQueryOp);

            // All documents with document-field PricePerUnit > 10 will be deleted from the server.
            //endregion
        }
    }
}

{
    //region syntax_1
    // Available overload:
    // ===================
    const deleteByQueryOp = new DeleteByQueryOperation(indexQuery);
    const deleteByQueryOp = new DeleteByQueryOperation(indexQuery, options);
    //endregion

    //region syntax_2
    // options object
    {
        // Indicates whether operations are allowed on stale indexes.
        allowStale, // boolean
            
        // If AllowStale is set to false and index is stale, 
        // then this is the maximum timeout to wait for index to become non-stale. 
        // If timeout is exceeded then exception is thrown.
        staleTimeout, // number
            
        // Limits the number of base operations per second allowed.  
        maxOpsPerSecond, // number
    }
    //endregion
}
