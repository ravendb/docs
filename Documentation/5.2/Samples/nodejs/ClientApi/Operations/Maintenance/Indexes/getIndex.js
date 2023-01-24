import { DocumentStore } from "ravendb";
import assert from "assert";

const documentStore = new DocumentStore();

async function getIndex() {
    {
        //region get_index
        // Define the get index operation, pass the index name
        const getIndexOp = new GetIndexOperation("Orders/Totals");

        // Execute the operation by passing it to maintenance.send
        const indexDefinition = await store.maintenance.send(getIndexOp);

        // Access the index definition
        const state = indexDefinition.state;
        const lockMode = indexDefinition.lockMode;
        const deploymentMode = indexDefinition.deploymentMode;
        // etc.
        //endregion

        //region get_indexes
        // Define the get indexes operation
        // Pass number of indexes to skip & number of indexes to retrieve
        const getIndexesOp = new GetIndexesOperation(0, 10);

        // Execute the operation by passing it to maintenance.send
        const indexes = await store.maintenance.send(getIndexesOp);

        // indexes will contain the first 10 indexes, alphabetically ordered by index name
        // Access an index definition from the resulting list
        const name = indexes[0].name;
        const state = indexes[0].state;
        const lockMode = indexes[0].lockMode;
        const deploymentMode = indexes[0].deploymentMode;
        // etc.
        //endregion
    }
}

{
    //region get_index_syntax
    const getIndexOp = new GetIndexOperation(indexName);
    //endregion

    //region get_indexes_syntax
    const getIndexesOp = new GetIndexesOperation(start, pageSize);
    //endregion
}
