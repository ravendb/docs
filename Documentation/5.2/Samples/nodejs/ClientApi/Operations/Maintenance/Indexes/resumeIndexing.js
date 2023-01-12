import { DocumentStore } from "ravendb";
import assert from "assert";

const documentStore = new DocumentStore();

async function resumeIndexing() {
    {
        //region resume_indexing
        // Define the resume indexing operation 
        const resumeIndexingOp = new StartIndexingOperation();

        // Execute the operation by passing it to maintenance.send
        await store.maintenance.send(resumeIndexingOp);

        // At this point,
        // you can be sure that all indexes on the preferred node are 'running'
        
        // Can verify indexing status on the preferred node by sending GetIndexingStatusOperation
        const indexingStatus = await store.maintenance.send(new GetIndexingStatusOperation());
        assert.strictEqual(indexingStatus.status, "Running");
        //endregion
    }
}

{
    //region syntax
    // class name has "Start", but this is ok, this is the "Resume" operation
    const resumeIndexingOp = new StartIndexingOperation();
    //endregion
}
