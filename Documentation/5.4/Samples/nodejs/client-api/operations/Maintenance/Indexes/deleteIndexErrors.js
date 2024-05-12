import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function deleteIndexErrors() {
    {
        //region delete_errors_all
        // Define the delete index errors operation
        const deleteIndexErrorsOp = new DeleteIndexErrorsOperation();

        // Execute the operation by passing it to maintenance.send
        await store.maintenance.send(deleteIndexErrorsOp);

        // All errors from ALL indexes are deleted
        //endregion

        //region delete_errors_specific
        // Define the delete index errors operation from specific indexes
        const deleteIndexErrorsOp = new DeleteIndexErrorsOperation(["Orders/Totals"]);

        // Execute the operation by passing it to maintenance.send
        // An exception will be thrown if any of the specified indexes do not exist
        await store.maintenance.send(deleteIndexErrorsOp);

        // Only errors from index "Orders/Totals" are deleted
        //endregion
    }
}

{
    //region syntax
    // Available overloads:
    const deleteIndexErrorsOp = new DeleteIndexErrorsOperation();           // Delete errors from all indexes 
    const deleteIndexErrorsOp = new DeleteIndexErrorsOperation(indexNames); // Delete errors from specific indexes
    //endregion
}
