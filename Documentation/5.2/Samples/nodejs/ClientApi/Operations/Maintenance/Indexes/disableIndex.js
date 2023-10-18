import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function disableIndex() {
    {
        //region disable_1
        // Define the disable index operation
        // Use this overload to disable on a single node
        const disableIndexOp = new DisableIndexOperation("Orders/Totals");

        // Execute the operation by passing it to maintenance.send
        await documentStore.maintenance.send(disableIndexOp);

        // At this point, the index is disabled only on the 'preferred node'
        // New data will not be indexed on this node only
        //endregion
    }
    {
        //region disable_2
        // Define the disable index operation
        // Pass 'true' to disable the index on all nodes in the database-group
        const disableIndexOp = new DisableIndexOperation("Orders/Totals", true);

        // Execute the operation by passing it to maintenance.send
        await documentStore.maintenance.send(disableIndexOp);

        // At this point, the index is disabled on all nodes
        // New data will not be indexed
        //endregion
    }
}

{
    //region syntax
    const disableIndexOp = new DisableIndexOperation(indexName, clusterWide = false);
    //endregion

}
