import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function namedValues() {
    {
        //region named_values_1
        const baseTime = new Date();
        const oneHour = 60 * 60 * 1000;
        let nextHour = new Date(baseTime.getTime() + oneHour);
        
        const tsf = session.timeSeriesFor("users/john", "RoutePoints", RoutePoint);
        
        const routePoint = new RoutePoint();
        routePoint.latitude = 40.712776;
        routePoint.longitude = -74.005974;

        // Append coordinates using the routePoint object
        tsf.append(nextHour, routePoint, "devices/Navigator");

        await session.saveChanges();
        //endregion
    }    
    {
        //region named_values_2
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

        // Call 'append' with the custom StockPrice class
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
        //endregion
    }
    {
        //region named_values_3
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
        //region named_values_4
        const oneDay = 24 * 60 * 60 * 1000;
        const startTime = new Date();
        const endTime = new Date(startTime.getTime() + 3 * oneDay);

        // Note: the 'where' clause must come after the 'between' clause
        const tsQueryText = `
            from StockPrices
            between $start and $end
            where Tag == "AppleTech"`;

        const query = session.query({ collection: "companies" })
            .whereEquals("address.city", "New York")
            .selectTimeSeries(b => b.raw(tsQueryText), TimeSeriesRawResult)
            .addParameter("start", startTime)
            .addParameter("end", endTime);

        // Execute the query:
        const results = await query.all();

        // Access entries results:
        const tsEntries = results[0].results;

        // Call 'asTypedEntry' to be able to access the entry's values by their names
        // Pass the class type (StockPrice)
        const volumeDay1 = tsEntries[0].asTypedEntry(StockPrice).value.volume;
        const volumeDay2 = tsEntries[1].asTypedEntry(StockPrice).value.volume;
        const volumeDay3 = tsEntries[2].asTypedEntry(StockPrice).value.volume;
        //endregion
    }
    {
        //region named_values_5
        // Register the named values for the 'StockPrices' series on the server
        await documentStore.timeSeries.register("Users",
            "StockPrices", ["open", "close", "high", "low", "volume"]);
        //endregion
    }
}

//region syntax
// Available overloads:
// ====================

register(collection, name, valueNames);
register(collectionClass, name, valueNames);
register(collectionClass, timeSeriesEntryClass);
register(collectionClass, timeSeriesEntryClass, name); 
//endregion

//region routePoint_class
class RoutePoint {
    
    // Add the following static param:
    static TIME_SERIES_VALUES = ["latitude", "longitude"];

    // The Latitude and Longitude properties will contain the time series entry values.
    // The names for these values will be "latitude" and "longitude" respectively.
    
    constructor(
        latitude = 0,
        longitude = 0
    ) {
        Object.assign(this, {
            latitude,
            longitude
        });
    }
}
//endregion

//region stockPrice_class
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
