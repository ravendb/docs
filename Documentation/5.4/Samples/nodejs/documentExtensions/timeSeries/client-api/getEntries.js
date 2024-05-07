import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function getTimeSeriesEntries() {

    const session = documentStore.openSession();
    await session.store(new User("John"), "users/john");

    const optionalTag = "watches/fitbit";
    const baseTime = new Date();
    baseTime.setUTCHours(0);

    const tsf = session.timeSeriesFor("users/john", "HeartRates");
    for (let i = 0; i < 10; i++)
    {
        const nextMinute = new Date(baseTime.getTime() + 60_000 * i);
        const nextMeasurement = 65 + i;
        tsf.append(nextMinute, nextMeasurement, optionalTag);
    }
    
    await session.saveChanges();

    {
        const session = documentStore.openSession();

        //region get_entries_1
        // Get all time series entries
        const allEntries = await session
            .timeSeriesFor("users/john", "HeartRates")
            .get();
        //endregion
    }
    {
        const session = documentStore.openSession();

        //region get_entries_2
        // Query for a document
        const user = await session.query({ collection: "users" })
            .whereEquals("name", "John")
            .first();
        
        const from = new Date();
        const to = new Date(baseTime.getTime() + 60_000 * 5);
        
        // Retrieve a range of 6 entries
        const tsEntries = await session
            .timeSeriesFor(user, "HeartRates")
            .get(from, to);
        //endregion
    }
    {
        // the below code was tested with entries entered from file: appendTimeSeries.js 
        const session = documentStore.openSession();
        
        //region get_entries_3
        let goingUp = false;

        const allEntries = await session
            .timeSeriesFor("users/john", "StockPrices")
            .get();
            
        const closePriceDay1 = allEntries[0].values[1];
        const closePriceDay2 = allEntries[1].values[1];
        const closePriceDay3 = allEntries[2].values[1];

        // Check if the stock's closing price is rising
        if ((closePriceDay2 > closePriceDay1) && (closePriceDay3 > closePriceDay2)) {
                goingUp = true;
        }
        //endregion
    }
    {
        // the below code was tested with entries entered from file: appendTimeSeries.js 
        const session = documentStore.openSession();

        //region get_entries_3_named
        let goingUp = false;

        const allEntries = await session
            .timeSeriesFor("users/john", "StockPrices")
            .get();

        // Call 'asTypedEntry' to be able to access the entry's values by their names
        // Pass the class type (StockPrice)
        const typedEntry1 = allEntries[0].asTypedEntry(StockPrice);
        
        // Access the entry value by its StockPrice class property name (close)
        const closePriceDay1 = typedEntry1.value.close;

        const typedEntry2 = allEntries[1].asTypedEntry(StockPrice);
        const closePriceDay2 = typedEntry2.value.close;

        const typedEntry3 = allEntries[2].asTypedEntry(StockPrice);
        const closePriceDay3 = typedEntry3.value.close;

        // Check if the stock's closing price is rising
        if ((closePriceDay2 > closePriceDay1) && (closePriceDay3 > closePriceDay2)) {
            goingUp = true;
        }
        //endregion
    }
    {
        //region get_entries_4
        const allEntries = await session
            .timeSeriesFor("users/john", "HeartRates")
             // Get all entries
            .get(null, null, builder => builder
                .includeDocument() // include the parent document
                .includeTags());   // include documents referenced in the entries tags
            
        // The following 'load' call will not trigger a server request
        const user = await session.load("users/john");
        //endregion
    }
}

//region syntax_1
// Available overloads:
// ====================

get(); // Get all entries

get(from, to);
get(from, to, start);

get(start, pageSize);
get(from, to, start, pageSize);

get(from, to, includes);
get(from, to, includes, start);
get(from, to, includes, start, pageSize);
//endregion

//region syntax_2
class TimeSeriesEntry {
    // The entry's time stamp
    timestamp; // Date
    
    // The entry's tag, can contain a related document ID
    tag; // string
    
    // List of up to 32 values for this entry
    values; // number[]
    
    // Is this an entry that belongs to a "rollup" time series
    isRollup; // boolean
    
    // Nodes info for incremental time series
    nodeValues; // Record<string, number[]>;
    
    // A method that returns the entry as a typed entry (TypedTimeSeriesEntry)
    asTypedEntry(clazz); // 'clazz' designates the type for the value  
}

class TypedTimeSeriesEntry {
    timestamp; // Date
    tag;       // string
    values;    // number[]
    isRollup;  // boolean
    
    // Access the value of a typed entry as an object
    value;     // object of type clazz
}
//endregion

class User {
    constructor(
        name = ''
    ) {
        Object.assign(this, {
            name
        });
    }
}
