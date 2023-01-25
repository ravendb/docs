import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function deleteIndex() {
    {
        //region delete_index
        // Define the delete index operation, specify the index name
        const deleteIndexOp = new DeleteIndexOperation("Orders/Totals");

        // Execute the operation by passing it to maintenance.send
        await store.maintenance.send(deleteIndexOp);
        //endregion
    }
}

{
    //region syntax
    const deleteIndexOp = new DeleteIndexOperation(indexName);
    //endregion
}
