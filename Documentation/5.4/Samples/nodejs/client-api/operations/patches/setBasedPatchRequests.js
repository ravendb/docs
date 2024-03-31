import { DocumentStore, PatchByQueryOperation } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

//region index
class Products_BySupplier extends AbstractJavaScriptIndexCreationTask {
    constructor() {
        super();

        // Define the index-fields 
        this.map("Products", p => ({
            Supplier : e.Supplier
        }));
    }
}
//endregion

async function setBasedPatchRequests() {
    {
        //region update_whole_collection
        // Update all documents in a collection
        // ====================================
        
        // Define the Patch by Query Operation, pass the "query & update" string:
        const patchByQueryOp = new PatchByQueryOperation(
            `from Orders as o
             update
             {
                 // Increase the Freight in ALL documents in the Orders collection:
                 o.Freight += 10;
             }`);

        // Execute the operation by passing it to operations.send:
        const operation = await documentStore.operations.send(patchByQueryOp);
        //endregion
    }
    {
        //region update_collection_name
        // Update the collection name for all documents in the collection
        // ==============================================================

        // Delete the document before recreating it with a different collection name:
        const patchByQueryOp = new PatchByQueryOperation(
            `from Orders as c
             update
             {
                 del(id(c));
                 this["@metadata"]["@collection"] = "New_Orders";
                 put(id(c), this);
             }`);

        const operation = await documentStore.operations.send(patchByQueryOp);
        //endregion
    }
    {
        //region update_by_dynamic_query
        // Update all documents matching a dynamic query
        // =============================================
        
        // Update the Discount in all orders that match the dynamic query predicate:
        const patchByQueryOp = new PatchByQueryOperation(`from Orders as o
                                                          where o.Employee = 'employees/4-A'
                                                          update
                                                          {
                                                              o.Lines.forEach(line=> line.Discount = 0.3);
                                                          }`);

        const operation = await documentStore.operations.send(patchByQueryOp);
        
        // Note: An AUTO-INDEX will be created when the dynamic query is executed on the server.
        //endregion
    }
    {
        //region update_by_index_query
        // Update all documents matching a static index query
        // ==================================================
        
        // Modify the Supplier to 'suppliers/13-A' for all products that have 'suppliers/12-A': 
        const patchByQueryOp = new PatchByQueryOperation(`from index 'Products/BySupplier' as p
                                                          where p.Supplier = 'suppliers/12-A'
                                                          update
                                                          {
                                                              p.Supplier = 'suppliers/13-A'
                                                          }`);

        const operation = await documentStore.operations.send(patchByQueryOp);
        //endregion
    }
    {
        //region update_all_documents
        // Update all documents matching an @all_docs query
        // ================================================

        // Patch the 'Updated' field to ALL documents (query is using the @all_docs keyword):
        const patchByQueryOp = new PatchByQueryOperation(`from @all_docs
                                                          update
                                                          {
                                                              this.Updated = true;
                                                          }`);

        const operation = await documentStore.operations.send(patchByQueryOp);
        //endregion
    }
    {
        //region update_by_id
        // Update all documents matching a query by ID
        // ===========================================

        // Patch the 'Updated' field to all documents that have the specified IDs:
        const patchByQueryOp = new PatchByQueryOperation(`from @all_docs as d
                                                          where id() in ('orders/1-A', 'companies/1-A')
                                                          update
                                                          {
                                                              d.Updated = true;
                                                          }`);

        const operation = await documentStore.operations.send(patchByQueryOp);
        //endregion
    }
    {
        //region update_by_id_using_parameters
        // Update all documents matching a query by ID using query parmeters
        // =================================================================
        
        // Define an IndexQuery object:
        const indexQuery = new IndexQuery();
        
        // Define the "query & update" string
        // Patch the 'Updated' field to all documents that have the specified IDs
        // Parameter ($ids) contains the listed IDs:
        indexQuery.query = `from @all_docs as d 
                            where id() in ($ids)
                            update {
                                d.Updated = true
                            }`;
        
        // Define the parameters for the script:
        indexQuery.queryParameters = {
            ids: ["orders/830-A", "companies/91-A"]
        };
        
        // Pass the indexQuery to the operation definition
        const patchByQueryOp = new PatchByQueryOperation(indexQuery);
        
        // Execute the operation
        const operation = await documentStore.operations.send(patchByQueryOp);
        //endregion
    }
    {
        //region update_stale_results
        // Update documents matching a dynamic query even if auot-index is stale
        // =====================================================================
        
        // Define an IndexQuery object:
        const indexQuery = new IndexQuery();

        // Define the "query & update" string
        // Modify company to 'companies/13-A' for all orders that have 'companies/12-A':
        indexQuery.query = `from Orders as o
                            where o.Company = 'companies/12-A'
                            update
                            {
                                o.Company = 'companies/13-A'
                            }`;

        // Define query options:
        const queryOptions = {
            // The query uses an auto-index (index is created if it doesn't exist yet).
            // Allow patching on all matching documents even if the auto-index is still stale.
            allowStale: true
        };

        // Pass indexQuery & queryOptions to the operation definition
        const patchByQueryOp = new PatchByQueryOperation(indexQuery, queryOptions);

        // Execute the operation
        const operation = await documentStore.operations.send(patchByQueryOp);
        //endregion
    }

//region syntax
    
    {
        //region syntax_1
        await send(operation);
        //endregion
    }
    {
        //region syntax_2
        // Available overload:
        // ===================
        patchByQueryOp = new PatchByQueryOperation(queryToUpdate);
        patchByQueryOp = new PatchByQueryOperation(queryToUpdate, options);
        //endregion
    }
    {
        //region syntax_3
        class IndexQuery {
            query;           // string
            queryParameters; // Record<string, object>
        }        
        //endregion
    }
    {
        //region syntax_4
        // Options for 'PatchByQueryOperation'
        {
            // Limit the amount of base operation per second allowed.
            maxOpsPerSecond; // number

            // Indicate whether operations are allowed on stale indexes.
            allowStale;      // boolean

            // If AllowStale is set to false and index is stale, 
            // then this is the maximum timeout to wait for index to become non-stale. 
            // If timeout is exceeded then exception is thrown.
            staleTimeout;    // number

            // Set whether operation details about each document should be returned by server.
            retrieveDetails; // boolean
        }
        //endregion
    }

//endregion
