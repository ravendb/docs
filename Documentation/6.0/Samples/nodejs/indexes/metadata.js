import { 
    DocumentStore,
    AbstractJavaScriptIndexCreationTask
} from "ravendb";

const store = new DocumentStore();

//region index_1
class Products_ByMetadata extends AbstractJavaScriptIndexCreationTask {
    constructor () {
        super();

        const { getMetadata } = this.mapUtils();

        this.map("Products", product => {
            // Call 'getMetadata' to access the metadata object
            var metadata = getMetadata(product);

            return {
                // Define the index fields
                LastModified: metadata['@last-modified'],
                HasCounters: !!metadata['@counters']
            };
        });
    }
}
//endregion

async function example() {
    
    const session = store.openSession();

    //region query_1
    const productsWithCounters = await session
        .query({ indexName: "Products/ByMetadata" })
        .whereEquals("HasCounters", true)
        .orderByDescending("LastModified")
        .all();
    //endregion
}

