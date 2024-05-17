import { DocumentStore } from "ravendb";
import assert from "assert";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

async function queryTimeSeries() {
    {
        //region create_data_for_testing_the_article_1
        const u1 = new User();
        u1.age = 25; u1.name = "j1";
        const u2 = new User();
        u2.age = 35; u2.name = "j2";
        const u3 = new User();
        u3.age = 73; u3.name = "j3";
        
        await session.store(u1, "users/j1");
        await session.store(u2, "users/j2");
        await session.store(u3, "users/j3");

        const timeSeriesName = "HeartRates";
        const tsf1 = session.timeSeriesFor("users/j1", timeSeriesName);
        const tsf2 = session.timeSeriesFor("users/j2", timeSeriesName);

        const optionalTag = "watches/fitbit";
        const baseTime = new Date();
        
        tsf1.append(baseTime, 65, optionalTag);
        tsf2.append(baseTime, 65, optionalTag);

        for (let i = 1; i < 10; i++)
        {
            const nextMinute = new Date(baseTime.getTime() + 60_000 * i);
            const nextMeasurement = 65 + i;
            tsf1.append(nextMinute, nextMeasurement, optionalTag);
            tsf2.append(nextMinute, nextMeasurement, optionalTag);
        }

        const tsf3 = session.timeSeriesFor("users/j3", timeSeriesName);        
        tsf3.append(baseTime, 65, optionalTag);
        const oneDay = 24 * 60 * 60 * 1000;
        
        for (let i = 1; i < 20; i++)
        {
            const nextDay = new Date(baseTime.getTime() + oneDay * i);
            const nextMeasurement = 65 + i;
            tsf3.append(nextDay, nextMeasurement, optionalTag);
        }
        
        await session.saveChanges();
        //endregion

        //region create_data_for_testing_the_article_2
        await documentStore.timeSeries.register("Companies",
            "StockPrices", ["open", "close", "high", "low", "volume"]);
        
        const c1 = new Company();
        c1.name = "Apple";
        const a1 = new Address();
        a1.city = "New York";
        c1.address = a1;
        await session.store(c1, "companies/c1");

        const tsf = session.timeSeriesFor("companies/c1", "StockPrices", StockPrice);
        
        const optionalTag = "AppleTech";
        const baseTime = new Date();
        const oneDay = 24 * 60 * 60 * 1000;
        
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
        //endregion

        //region query_1
        // Define the time series query part (expressed in RQL):
        const tsQueryText = `
            from HeartRates
            where Tag == "watches/fitbit"`;
        
        // Define the high-level query:
        const query = session.query({ collection: "users" })
            .whereLessThan("age", 30)
             // Call 'selectTimeSeries' and pass it:
             // * the time series query text
             // * the `TimeSeriesRawResult` return type
            .selectTimeSeries(b => b.raw(tsQueryText), TimeSeriesRawResult);
        
        // Execute the query:
        const results = await query.all();

        // Access entries results:
        rawResults = results[0];
        assert.equal((rawResults instanceof TimeSeriesRawResult), true);

        const tsEntry = rawResults.results[0];
        assert.equal((tsEntry instanceof TimeSeriesEntry), true);

        const tsValue = tsEntry.value;
        //endregion
        
        //region query_2
        const startTime = new Date();
        const endTime = new Date(startTime.getTime() + 5 * 60_000);
        
        // Define the time series query text:
        const tsQueryText = `
            from HeartRates
            between $start and $end`;

        // Define the query:
        const query = session.query({ collection: "users" })
             // Call 'selectTimeSeries' and pass it:
             // * the time series query text
             // * the `TimeSeriesRawResult` return type
            .selectTimeSeries(b => b.raw(tsQueryText), TimeSeriesRawResult)
             // Add the parameters content
            .addParameter("start", startTime)
            .addParameter("end", endTime);
        
        // Execute the query:
        const results = await query.all();
        //endregion
        
        //region query_3
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

        const volumeDay1 = tsEntries[0].values[4];
        const volumeDay2 = tsEntries[1].values[4];
        const volumeDay3 = tsEntries[2].values[4];
        //endregion
        
        //region query_4
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
        
        //region query_5
        const oneDay = 24 * 60 * 60 * 1000;
        const startTime = new Date();
        const endTime = new Date(startTime.getTime() + 10 * oneDay);
        
        const tsQueryText = `from HeartRates between $start and $end
            where Tag == "watches/fitbit"
            group by "1 day"
            select count(), min(), max(), avg()`;

        const query = session.query({ collection: "users" })
            .whereGreaterThan("age", 72)
             // Call 'selectTimeSeries' and pass it:
             // * the time series query text
             // * the `TimeSeriesAggregationResult` return type
            .selectTimeSeries(b => b.raw(tsQueryText), TimeSeriesAggregationResult)
            .addParameter("start", startTime)
            .addParameter("end", endTime);
        
        // Execute the query:
        const results = await query.all();
        const aggregatedResults = results[0].results;

        const averageForDay1 = aggregatedResults[0].average[0];
        const averageForDay2 = aggregatedResults[1].average[0];        
        //endregion
        
        //region query_6
        const rql = `from users where age < 30
             select timeseries(
                from HeartRates
             )`;
        
        const query = session.advanced.rawQuery(rql, TimeSeriesRawResult);

        const result = await query.all();
        //endregion
        
        //region query_7
        const rql = `
            declare timeseries getHeartRates(user) 
            {
                from user.HeartRates 
                    between $start and $end
                    offset "03:00"
            }
            
            from users as u where age < 30
            select getHeartRates(u)`;

        const startTime = new Date();
        const endTime = new Date(startTime.getTime() + 24 * 60 * 60 * 1000);
        
        const query = session.advanced.rawQuery(rql, TimeSeriesRawResult)
            .addParameter("start", startTime)
            .addParameter("end", endTime);

        const result = await query.all();
        //endregion
        
        //region query_8
        const rql = `
            from Users as u where Age < 30
            select timeseries (
                from HeartRates 
                    between $start and $end
                    offset "03:00"
            )`;

        const startTime = new Date();
        const endTime = new Date(startTime.getTime() + 24 * 60 * 60 * 1000);

        const query = session.advanced.rawQuery(rql, TimeSeriesRawResult)
            .addParameter("start", startTime)
            .addParameter("end", endTime);

        const result = await query.all();
        //endregion
        
        //region query_9
        const rql = `
            from users as u
            select timeseries(
                from HeartRates between $start and $end
                group by '1 day'
                select min(), max()
                offset "03:00"
            )`;

        const oneDay = 24 * 60 * 60 * 1000;
        const startTime = new Date();
        const endTime = new Date(startTime.getTime() + 7 * oneDay);

        const query = session.advanced.rawQuery(rql, TimeSeriesRawResult)
            .addParameter("start", startTime)
            .addParameter("end", endTime);

        const result = await query.all();
        //endregion
    }
}

//region syntax_1
selectTimeSeries(timeSeriesQuery, projectionClass);
//endregion

//region syntax_2
raw(queryText);
//endregion

//region syntax_3
class TimeSeriesRawResult {
    results; // TimeSeriesEntry[]
    asTypedResult>(clazz);
}

class TimeSeriesAggregationResult extends TimeSeriesQueryResult {
    results; // TimeSeriesRangeAggregation[];
    asTypedEntry(clazz);
}
//endregion

//region syntax_4
session.rawQuery(query);
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
        name = '',
        age = 0
    ) {
        Object.assign(this, {
            name,
            age
        });
    }
}
//endregion

//region company_class
class Company {
    constructor(
        name = '',
        address = null,
    ) {
        Object.assign(this, {
            name,
            address
        });
    }
}
//endregion

//region address_class
class Address {
    constructor(
        city = ''
    ) {
        Object.assign(this, {
            city
        });
    }
}
//endregion
