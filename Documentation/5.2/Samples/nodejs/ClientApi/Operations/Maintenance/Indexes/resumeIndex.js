import { DocumentStore } from "ravendb";
import assert from "assert";

const documentStore = new DocumentStore();

async function resumeIndex() {
    {
        //region resume_index
        // Define the resume index operation, pass the index name 
        const resumeIndexOp = new StartIndexOperation("Orders/Totals");

        // Execute the operation by passing it to maintenance.send
        await store.maintenance.send(resumeIndexOp);

        // At this point:
        // Index 'Orders/Totals' is resumed on the preferred node

        // Can verify the index status on the preferred node by sending GetIndexingStatusOperation
        const indexingStatus = await store.maintenance.send(new GetIndexingStatusOperation());
        
        const index = indexingStatus.indexes.find(x => x.name === "Orders/Totals")
        assert.strictEqual(index.status, "Running");
        //endregion
    }
}

{
    //region syntax
    // class name has "Start", but this is ok, this is the "Resume" operation
    const resumeIndexOp = new StartIndexOperation(indexName);
    //endregion
}
