import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function getIndexNames() {
    {
        //region get_index_names
        // Define the get index names operation
        // Pass number of indexes to skip & number of indexes to retrieve
        const getIndexNamesOp = new GetIndexNamesOperation(0, 10);

        // Execute the operation by passing it to maintenance.send
        const indexNames = await store.maintenance.send(getIndexNamesOp);

        // indexNames will contain the first 10 indexes, alphabetically ordered
        //endregion
    }
}

{
    //region get_index_names_syntax
    const getIndexNamesOp = new GetIndexNamesOperation(start, pageSize);
    //endregion
}
