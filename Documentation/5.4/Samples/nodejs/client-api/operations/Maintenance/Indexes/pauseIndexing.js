import { DocumentStore } from "ravendb";
import assert from "assert";

const documentStore = new DocumentStore();

async function pauseIndexing() {
    {
        //region pause_indexing
        // Define the pause indexing operation 
        const pauseIndexingOp = new StopIndexingOperation();

        // Execute the operation by passing it to maintenance.send
        await store.maintenance.send(pauseIndexingOp);

        // At this point:
        // All indexes in the default database will be 'paused' on the preferred node
        
        // Can verify indexing status on the preferred node by sending GetIndexingStatusOperation
        const indexingStatus = await store.maintenance.send(new GetIndexingStatusOperation());
        assert.strictEqual(indexingStatus.status, "Paused");
        //endregion
    }
}

{
    //region syntax
    // class name has "Stop", but this is ok, this is the "Pause" operation
    const pauseIndexingOp = new StopIndexingOperation();
    //endregion
}
