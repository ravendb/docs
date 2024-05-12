import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function setLockMode() {
    {
        //region set_lock_single
        // Define the set lock mode operation
        // Pass index name & lock mode
        const setLockModeOp = new SetIndexesLockOperation("Orders/Totals", "LockedIgnore");

        // Execute the operation by passing it to maintenance.send
        // An exception will be thrown if index does not exist
        await store.maintenance.send(setLockModeOp);

        // Lock mode is now set to 'LockedIgnore'
        // Any modifications done now to the index will Not be applied, and will Not throw
        //endregion
    }
    {
        //region set_lock_multiple
        // Define the index list and the new lock mode:
        const parameters = {
            indexNames: ["Orders/Totals", "Orders/ByCompany"],
            mode: "LockedError"
        }

        // Define the set lock mode operation, pass the parameters
        const setLockModeOp = new SetIndexesLockOperation(parameters);

        // Execute the operation by passing it to maintenance.send
        // An exception will be thrown if any of the specified indexes do not exist
        await store.maintenance.send(setLockModeOp);

        // Lock mode is now set to 'LockedError' on both indexes
        // Any modifications done now to either index will throw
        //endregion
    }
}

{
    //region syntax_1
    // Available overloads:
    const setLockModeOp = new SetIndexesLockOperation(indexName, mode);
    const setLockModeOp = new SetIndexesLockOperation(parameters);
    //endregion

    //region syntax_2
    // parameters object
    {
        indexNames, // string[], list of index names
        mode        // Lock mode to set
    }
    //endregion
}
