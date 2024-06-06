import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function getOperation() {
    {
        //region get_1
        // Define the get operation
        var getTimeSeriesOp = new GetTimeSeriesOperation(
            "employees/1-A",  // The document ID    
            "HeartRates");    // The time series name

        // Execute the operation by passing it to 'operations.send'
        const timeSeriesEntries = await documentStore.operations.send(getTimeSeriesOp);
        
        // Access entries
        const firstEntryReturned = timeSeriesEntries.entries[0];
        //endregion

        //region get_2
        const baseTime = new Date();
        
        const startTime1 = new Date(baseTime.getTime() + 60_000 * 5);
        const endTime1 = new Date(baseTime.getTime() + 60_000 * 10);

        const startTime2 = new Date(baseTime.getTime() + 60_000 * 15);
        const endTime2 = new Date(baseTime.getTime() + 60_000 * 20);
        
        // Define the get operation
        const getMultipleTimeSeriesOp = new GetMultipleTimeSeriesOperation(
            "employees/1-A",
            [
                {
                    name: "ExerciseHeartRates",
                    from: startTime1,
                    to: endTime1
                },
                {
                    name: "RestHeartRates",
                    from: startTime2,
                    to: endTime2
                }
            ]));

        // Execute the operation by passing it to 'operations.send'
        const timesSeriesEntries = await documentStore.operations.send(getMultipleTimeSeriesOp);
        
        // Access entries
        const timeSeriesEntry = timesSeriesEntries.values.get("ExerciseHeartRates")[0].entries[0];
        //endregion
    }
}

//region syntax_1
// Available overloads:
// ====================
const getTimeSeriesOp = new GetTimeSeriesOperation(docId, timeseries);
const getTimeSeriesOp = new GetTimeSeriesOperation(docId, timeseries, from, to);
const getTimeSeriesOp = new GetTimeSeriesOperation(docId, timeseries, from, to, start);
const getTimeSeriesOp = new GetTimeSeriesOperation(docId, timeseries, from, to, start, pageSize);
//endregion

//region syntax_2
class TimeSeriesRangeResult {
    // Timestamp of first entry returned
    from; // Date;

    // Timestamp of last entry returned
    to; // Date;

    // The resulting entries
    // Will be empty if requesting an entries range that does Not exist
    entries; // TimeSeriesEntry[];
    
    // The number of entries returned
    // Will be undefined if not all entries of this time series were returned
    totalResults; // number
}
//endregion

//region syntax_3
// Available overloads:
// ====================
const getMultipleTimeSeriesOp = new GetMultipleTimeSeriesOperation(docId, ranges);
const getMultipleTimeSeriesOp = new GetMultipleTimeSeriesOperation(docId, ranges, start, pageSize);
//endregion

//region syntax_4
// The TimeSeriesRange object:
// ===========================
{
    // Name of time series
    name, // string
    
    // Get time series entries starting from this timestamp (inclusive).    
    from, // Date

    // Get time series entries ending at this timestamp (inclusive).
    to  // Date
}
//endregion

//region syntax_5
class TimeSeriesDetails {
    // The document ID
    id; // string
    
    // Dictionary of time series name to the time series results 
    values; // Map<string, TimeSeriesRangeResult[]>
}
//endregion
