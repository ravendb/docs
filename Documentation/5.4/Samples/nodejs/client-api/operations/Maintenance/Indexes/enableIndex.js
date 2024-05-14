import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function enableIndex() {
    {
        //region enable_1
        // Define the enable index operation
        // Use this overload to enable on a single node
        const enableIndexOp = new EnableIndexOperation("Orders/Totals");

        // Execute the operation by passing it to maintenance.send
        await documentStore.maintenance.send(enableIndexOp);

        // At this point, the index is enabled on the 'preferred node'
        // New data will be indexed on this node
        //endregion
    }
    {
        //region enable_2
        // Define the enable index operation
        // Pass 'true' to enable the index on all nodes in the database-group
        const enableIndexOp = new EnableIndexOperation("Orders/Totals", true);

        // Execute the operation by passing it to maintenance.send
        await documentStore.maintenance.send(enableIndexOp);

        // At this point, the index is enabled on ALL nodes
        // New data will be indexed
        //endregion
    }
}

{
    //region syntax
    const enableIndexOp = new EnableIndexOperation(indexName, clusterWide = false);
    //endregion

}
