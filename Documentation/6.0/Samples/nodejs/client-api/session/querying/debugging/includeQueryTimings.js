import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

{
    //region syntax
    query.timings(timingsCallback)
    //endregion

    //region syntax_2
    class QueryTimings {
        public durationInMs;
        public timings;
    }
    //endregion
}

async function timings() {
    //region timing
    // Define an object that will receive the timings results
    let timingsResults;

    const results = await session.query({ collection: "Products" })
        // Call timings, pass a callback function
        // Output param 'timingsResults' will be filled with the timings details when query returns 
        .timings(t => timingsResults = t)
        // Define query criteria
        // i.e. search for docs containing Syrup -or- Lager in their Name field
        .search("Name", "Syrup Lager")
        // Execute the query
        .all();

    // Get total query duration:
    // =========================    
    const totalQueryDuration = timingsResults.durationInMs;

    // Get specific parts duration:
    // ============================
    const optimizerDuration = timingsResults.timings.optimizer.durationInMs;
    // or: timingsResults.timings["optimizer"].durationInMs;    
    const luceneDuration = timingsResults.timings.query.timings.lucene.durationInMs;
    // or: timingsResults.timings["query"].timings.["lucene"].durationInMs;
    //endregion
}
