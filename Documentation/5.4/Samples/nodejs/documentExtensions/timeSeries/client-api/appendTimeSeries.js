import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function appendTimeSeries() {
    {
        //region append_1
        // Open a session and store a new document
        const session = documentStore.openSession();
        await session.store(new User("John"), "users/john");

        // Get an instance of 'timeSeriesFor'
        // Pass the document ID and the time series name
        const timeSeriesName = "HeartRates";
        const tsf = session.timeSeriesFor("users/john", timeSeriesName);
  
        // Create time series and add entries:
        // ===================================

        // Define an optional tag and some base time for the first entry:
        const optionalTag = "watches/fitbit";
        const baseTime = new Date();
        baseTime.setUTCHours(0);
        
        // The first 'append' call will create the 'HeartRates' time series on the document
        // (since this series doesn't exist yet on the document) and insert the first entry
        tsf.append(baseTime, 65, optionalTag);
        
        // The next 'append' calls will add more entries to the 'HeartRates' time series
        for (let i = 1; i < 10; i++)
        {
            const nextMinute = new Date(baseTime.getTime() + 60_000 * i);
            const nextMeasurement = 65 + i;
            tsf.append(nextMinute, nextMeasurement, optionalTag);
        }

        // Modify an existing entry:
        // =========================

        // Modify the last entry that was added
        // The entry with the specified time stamp will be updated
        tsf.append(new Date(baseTime.getTime() + 60_000 * 9), 60, optionalTag);

        // Save changes
        await session.saveChanges();
        
        // Results:
        // ========
        // The document will contain a time series named "HeartRates" with 10 entries.
        //endregion

        //region append_2
        const session = documentStore.openSession();
        await session.store(new User("John"), "users/john");

        const tsf = session.timeSeriesFor("users/john", "StockPrices");

        const optionalTag = "companies/kitchenAppliances";
        const baseTime = new Date();
        baseTime.setUTCHours(0);

        const oneDay = 24 * 60 * 60 * 1000;
        let nextDay = new Date(baseTime.getTime() + oneDay);
        
        // Provide multiple values to the entity
        tsf.append(nextDay, [ 52, 54, 63.5, 51.4, 9824 ], optionalTag);

        nextDay = new Date(baseTime.getTime() + oneDay * 2);
        tsf.append(nextDay, [ 54, 55, 61.5, 49.4, 8400 ], optionalTag);

        nextDay = new Date(baseTime.getTime() + oneDay * 3);
        tsf.append(nextDay, [ 55, 57, 65.5, 50, 9020 ], optionalTag);

        await session.saveChanges();

        // Results:
        // ========
        // The document will contain a time series called "StockPrices" with 3 entries.
        // Each entry will have 5 values.
        //endregion

        //region append_3
        // Register the named values for the 'StockPrices' series on the server
        await documentStore.timeSeries.register("Users",
            "StockPrices", ["open", "close", "high", "low", "volume"]);
        
        const session = documentStore.openSession();
        await session.store(new User("John"), "users/john");

        // Get an instance of 'timeSeriesFor', pass:
        // * the document ID
        // * the time series name
        // * the class that will hold the entry's values
        const tsf = session.timeSeriesFor("users/john", "StockPrices", StockPrice);

        const optionalTag = "companies/kitchenAppliances";
        const baseTime = new Date();
        baseTime.setUTCHours(0);
        const oneDay = 24 * 60 * 60 * 1000;

        // Provide the multiple values via the StockPrice class
        const price1 = new StockPrice();
        price1.open = 52;
        price1.close = 54;
        price1.high = 63.5;
        price1.low = 51.4;
        price1.volume = 9824;

        let nextDay = new Date(baseTime.getTime() + oneDay);
        tsf.append(nextDay, price1, optionalTag);

        const price2 = new StockPrice();
        price2.open = 54;
        price2.close = 55;
        price2.high = 61.5;
        price2.low = 49.4;
        price2.volume = 8400;

        nextDay = new Date(baseTime.getTime() + oneDay * 2);
        tsf.append(nextDay, price2, optionalTag);

        const price3 = new StockPrice();
        price3.open = 55;
        price3.close = 57;
        price3.high = 65.5;
        price3.low = 50;
        price3.volume = 9020;

        nextDay = new Date(baseTime.getTime() + oneDay * 3);
        tsf.append(nextDay, price3, optionalTag);

        await session.saveChanges();

        // Results:
        // ========
        // The document will contain a time series called "StockPrices" with 3 entries.
        // Each entry will have 5 named values.
        //endregion
    }
}

//region syntax_1
append(timestamp, value);
append(timestamp, value, tag);
//endregion

//region syntax_2
append(timestamp, values);
append(timestamp, values, tag); 
//endregion

//region syntax_3
append(timestamp, entry);
append(timestamp, entry, tag);
append(entry);
//endregion

//region stockPrice_class
// This class is used in the "Named Values" example
class StockPrice {

    // Define the names for the entry values
    static TIME_SERIES_VALUES = ["open", "close", "high", "low", "volume"];
    
    constructor(
        open = 0,
        close = 0,
        high = 0,
        low = 0,
        volume = 0
    ) {
        Object.assign(this, {
            open,
            close,
            high,
            low,
            volume
        });
    }
}
//endregion

//region user_class
class User {
    constructor(
        name = ''
    ) {
        Object.assign(this, {
            name
        });
    }
}
//endregion
