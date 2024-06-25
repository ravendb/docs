import { DocumentStore } from "ravendb";
import assert from "assert";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

async function queryTsIndex() {

    //region sample_ts_index
    class TsIndex extends AbstractRawJavaScriptTimeSeriesIndexCreationTask {
        constructor() {
            super();

            this.maps.add(`
                timeSeries.map("Employees", function (segment) {
                     let employee = load(segment.DocumentId, "Employees")
                     
                     // Return the index-entry:
                     return segment.Entries.map(entry => ({
                     
                         // Define the index-fields:
                         bpm: entry.Values[0],
                         date: new Date(entry.Timestamp),
                         tag: entry.Tag
                         employeeID: segment.DocumentId,
                         employeeName: employee.FirstName + " " + employee.LastName
                     }));
                })
            `;
        }
    }
    //endregion
    
    {
        //region query_index_1
        const results = await session
             // Query the index 
            .query({ indexName: "TsIndex" })
             // Query for all entries w/o any filtering
            .all();

        // Access results:
        const entryResult = results[0];
        const employeeName = entryResult.employeeName;
        const bpm = entryResult.bpm;
        //endregion

        //region query_index_2
        const results = await session
             // Provide RQL to rawQuery
            .advanced.rawQuery("from index 'TsIndex'")
             // Execute the query
            .all();
        //endregion

        //region query_index_3
        const results = await session
            .query({ indexName: "TsIndex" })
             // Retrieve only time series entries with high BPM values for a specific employee
            .whereEquals("employeeName", "Robert King")
            .whereGreaterThanOrEqual("bpm", 85)
            .all();
        //endregion

        //region query_index_4
        const results = await session
             // Retrieve only time series entries with high BPM values for a specific employee
            .advanced.rawQuery(`
                 from index "TsIndex"
                 where employeeName == "Robert King" and bpm > 85.0
            `)
            .all();
        //endregion

        //region query_index_5
        const results = await session
            .query({ indexName: "TsIndex" })
             // Retrieve time series entries where employees had a low BPM value
            .whereLessThan("bpm", 58)
             // Order by the 'date' index-field (descending order)
            .orderByDescending("date")
            .all();
        //endregion

        //region query_index_6
        const results = await session
             // Retrieve entries with low BPM value and order by 'date' descending
            .advanced.rawQuery(`
                  from index "TsIndex"
                  where bpm < 58
                  order by date desc
            `)
            .all();
        //endregion

        //region query_index_7
        const results = await session
            .query({ indexName: "TsIndex" })
            .whereGreaterThanOrEqual("bpm", 100)
             // Return only the employeeID index-field in the results
            .selectFields(["employeeID"])
             // Optionally: call 'distinct' to remove duplicates from results
            .distinct()
            .all();
        //endregion

        //region query_index_8
        const results = await session
             // Return only the employeeID index-field in the results
            .advanced.rawQuery(`
                 from index "TsIndex"
                 where bpm >= 100
                 select distinct employeeID
            `)
            .all();
        //endregion 
    }
}



