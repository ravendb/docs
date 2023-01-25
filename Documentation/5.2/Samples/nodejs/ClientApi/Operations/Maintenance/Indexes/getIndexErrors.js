import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function getIndexErrors() {
    {
        //region get_errors_all
        // Define the get index errors operation
        const getIndexErrorsOp = new GetIndexErrorsOperation();

        // Execute the operation by passing it to maintenance.send
        const indexErrors = await store.maintenance.send(getIndexErrorsOp);

        // indexErrors will contain errors for ALL indexes
        //endregion

        //region get_errors_specific
        // Define the get index errors operation for specific indexes
        const getIndexErrorsOp = new GetIndexErrorsOperation(["Orders/Totals"]);

        // Execute the operation by passing it to maintenance.send
        // An exception will be thrown if any of the specified indexes do not exist
        const indexErrors = await store.maintenance.send(getIndexErrorsOp);

        // indexErrors will contain errors only for index "Orders/Totals"
        //endregion
    }
}

{
    //region syntax_1
    // Available overloads:
    const getIndexErrorsOp = new GetIndexErrorsOperation();           // Get errors for all indexes 
    const getIndexErrorsOp = new GetIndexErrorsOperation(indexNames); // Get errors for specific indexes
    //endregion

    //region syntax_2
    // An 'index errors' object:
    {
        name,  // Index name
        errors // List of 'error objects' for this index
    }
    //endregion
    
    //region syntax_3
    // An 'error object':
    {
        // The error message
        error,
            
        // Time of error
        timestamp,

        // If Action is 'Map'    - field will contain the document ID
        // If Action is 'Reduce' - field will contain the Reduce key value
        // For all other Actions - field will be null    
        document,

        // Area where error has occurred, e.g. Map/Reduce/Analyzer/Memory/etc.
        action
    }
    //endregion
}
