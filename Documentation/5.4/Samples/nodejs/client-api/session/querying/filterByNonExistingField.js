import { DocumentStore, AbstractIndexCreationTask } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

//region the_index
// Define a static index on the 'orders' collection
// ================================================

class Orders_ByFreight extends AbstractJavaScriptIndexCreationTask<Order> {

    constructor() {
        super();

        // Define the index-fields 
        this.map("orders", o => ({
            // Index a field that might be missing in SOME documents
            freight: o.firstName,
            // Index a field that exists in ALL documents in the collection
            id: o.lastName
        }));
    }
}
//endregion

async function filterByNonExistingField() {
    {
        //region whereNotExists_1
        const ordersWithoutFreightField = await session
             // Define a query on 'orders' collection
            .query({ collection: "orders" })
             // Search for documents that do Not contain field 'freight'
            .not()
            .whereExists("freight")
             // Execute the query
            .all();
        
        // Results will be only the documents that do Not contain the 'freight' field in 'orders' collection 
        //endregion
    }

    {
        //region whereNotExists_2
        // Query the index
        // ===============

        const employees = await session
             // Define a query on the index
            .query({ indexName: "Orders/ByFreight" })
             // Search for documents that do Not contain field 'freight'
            .not()
            .whereExists("freight")
             // Execute the query
            .all();

        // Results will be only the documents that do Not contain the 'freight' field in 'orders' collection 
        //endregion
    }
}
