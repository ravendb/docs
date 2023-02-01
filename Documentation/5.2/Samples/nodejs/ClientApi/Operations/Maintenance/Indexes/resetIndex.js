import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function getStats() {
    {
        //region reset
        // Define the reset index operation, pass index name
        const resetIndexOp = new ResetIndexOperation("Orders/Totals");

        // Execute the operation by passing it to maintenance.send
        // An exception will be thrown if index does not exist
        await store.maintenance.send(resetIndexOp);
        //endregion
    }
}

{
    //region syntax
    const resetIndexOp = new ResetIndexOperation(indexName);
    //endregion
}
