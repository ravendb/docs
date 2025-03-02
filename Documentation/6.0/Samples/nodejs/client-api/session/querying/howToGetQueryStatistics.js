import { DocumentStore, AbstractIndexCreationTask } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

{
    let statsCallback;
    const query = session.query({ collection: "test" });
    
    //region syntax
    query.statistics(statsCallback);
    //endregion
}

async function getStats() {
    //region stats
    // Define an output param for getting the query stats
    let stats;

    const employees = await session
        .query({ collection: "Employees" })
        .whereEquals("FirstName", "Anne")
         // Get query stats:
         // * Call 'statistics', pass a callback function
         // * Output param 'stats' will be filled with the stats when query returns 
        .statistics(s => stats = s)
        .all();

    const numberOfResults = stats.value.totalResults; // Get results count
    const queryDuration = stats.value.durationInMs;   // Get query duration
    const indexNameUsed = stats.value.indexName;      // Get index name used in query
    // ...
    //endregion
}

