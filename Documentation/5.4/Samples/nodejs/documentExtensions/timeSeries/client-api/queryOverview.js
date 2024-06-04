import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

async function queryOverview() {
    {
        //region overview_1
        // Define the time series query text
        const tsQueryText = "from HeartRates";
        
        // Make a dynamic query over the "employees" collection
        const queryResults = await session.query({ collection: "employees" })
             // Query for employees hired after 1994
            .whereGreaterThan("HiredAt", "1994-01-01")
             // Call 'selectTimeSeries' to project the time series entries in the query results
             // Pass the defined time series query text
            .selectTimeSeries(b => b.raw(tsQueryText), TimeSeriesRawResult)
            .all();

        // Results:
        // ========
        
        // 1. Results will include all entries from time series "HeartRates" for matching employee documents.
        // 2. Since this is a dynamic query that filters documents,
        //    an auto-index (Auto/employees/ByHiredAt) will be created if it doesn't already exist.
        //    However, it is NOT a time series index !! 
        //    It is a regular documents auto-index that allows querying for documents based on their HiredAt field.
        
        // Access a time series entry value from the results:
        const entryValue = queryResults[0].results[0].values[0];
        //endregion
    }    
    {
        //region overview_2
        // Add 'scale <number>' to your time series query text 
        const tsQueryText = "from HeartRates scale 10";

        const queryResults = await session.query({ collection: "users" })
            .selectTimeSeries(b => b.raw(tsQueryText), TimeSeriesRawResult)
            .all();
        
        // The value in the query results is 10 times the value stored on the server
        const scaledValue = queryResults[0].results[0].values[0];
        //endregion
    }
    {
        //region overview_3
        const queryResults = await session.advanced
             // Provide RQL to rawQuery
            .rawQuery(`
                 // The time series function:
                 // =========================
                 declare timeseries tsQuery(user) {
                     from user.HeartRates
                     where (Values[0] > 100)
                 }
                 
                 // The custom JavaScript function:
                 // ===============================
                 declare function customFunc(user) {
                     var results = [];
                 
                     // Call the time series function to retrieve heart rate values for the user
                     var r = tsQuery(user);
                 
                     // Prepare the results
                     for(var i = 0 ; i < r.Results.length; i ++) {
                         results.push({
                             timestamp: r.Results[i].Timestamp, 
                             value: r.Results[i].Values.reduce((a, b) => Raven_Max(a, b)),
                             tag: r.Results[i].Tag  ?? "none"})
                     }
                     return results;
                 }
                 
                 // Query & project results:
                 // ========================
                 from "users" as user
                 select 
                     user.name,
                     customFunc(user) as timeSeriesEntries // Call the custom JavaScript function
             `)
             // Execute the query
            .all();
        //endregion
    }
}
