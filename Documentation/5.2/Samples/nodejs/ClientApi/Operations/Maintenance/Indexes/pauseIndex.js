import { DocumentStore } from "ravendb";
import assert from "assert";

const documentStore = new DocumentStore();

async function pauseIndex() {
    {
        //region pause_index
        // Define the pause index operation, pass the index name 
        const pauseIndexOp = new StopIndexOperation("Orders/Totals");

        // Execute the operation by passing it to maintenance.send
        await store.maintenance.send(pauseIndexOp);

        // At this point:
        // Index 'Orders/Totals' is paused on the preferred node

        // Can verify the index status on the preferred node by sending GetIndexingStatusOperation
        const indexingStatus = await store.maintenance.send(new GetIndexingStatusOperation());
        
        const index = indexingStatus.indexes.find(x => x.name === "Orders/Totals")
        assert.strictEqual(index.status, "Paused");
        //endregion
    }
}

{
    //region syntax
    // class name has "Stop", but this is ok, this is the "Pause" operation
    const pauseIndexOp = new StopIndexOperation(indexName);
    //endregion
}
