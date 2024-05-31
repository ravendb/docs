import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function appendWithBulkInsert() {
    {
        //region append_1
        const baseTime = new Date();

        // Create a BulkInsertOperation instance
        const bulkInsert = documentStore.bulkInsert();

        {
            // Call 'TimeSeriesFor', pass it:
            // * The document ID
            // * The time series name
            const timeSeriesBulkInsert = bulkInsert.timeSeriesFor("users/john", "HeartRates");

            // Call 'Append' to add an entry, pass it:
            // * The entry's Timestamp 
            // * The entry's Value or Values 
            // * The entry's Tag (optional) 
            const nextMinute = new Date(baseTime.getTime() + 60_000 * 1);
            await timeSeriesBulkInsert.append(nextMinute, 61, "watches/fitbit");

            timeSeriesBulkInsert.dispose();
        }

        // Call finish to send all data to the server
        await bulkInsert.finish();
        //endregion
    }    
    {
        //region append_2
        const baseTime = new Date();

        const bulkInsert = documentStore.bulkInsert();

        {
            const timeSeriesBulkInsert = bulkInsert.timeSeriesFor("users/john", "HeartRates");

            for (let i = 0; i < 100; i++) {
                let randomValue = Math.floor(Math.random() * (29)) + 60;
                let nextMinute = new Date(baseTime.getTime() + 60_000 * (i + 1));
                
                await timeSeriesBulkInsert.append(nextMinute, randomValue, "watches/fitbit");
            }

            timeSeriesBulkInsert.dispose();
        }

        await bulkInsert.finish();
        //endregion
    }
    {
        //region append_3
        const baseTime = new Date();

        const bulkInsert = documentStore.bulkInsert();

        {
            const timeSeriesBulkInsert = bulkInsert.timeSeriesFor("users/john", "HeartRates");

            const exerciseHeartRates = [89, 82, 85];
            await timeSeriesBulkInsert.append(new Date(baseTime.getTime() + 60_000),
                exerciseHeartRates, "watches/fitbit");

            const restingHeartRates = [59, 63, 61, 64, 65];
            await timeSeriesBulkInsert.append(new Date(baseTime.getTime() + 60_000 * 2),
                restingHeartRates, "watches/fitbit");

            timeSeriesBulkInsert.dispose();
        }

        await bulkInsert.finish();
        //endregion
    }
    {
        //region append_4
        const baseTime = new Date();

        const bulkInsert = documentStore.bulkInsert();

        {
            // Append first time series
            const timeSeriesBulkInsert = bulkInsert.timeSeriesFor("users/john", "HeartRates");
            await timeSeriesBulkInsert.append(new Date(baseTime.getTime() + 60_000), 61, "watches/fitbit");
            await timeSeriesBulkInsert.append(new Date(baseTime.getTime() + 60_000 * 2), 62, "watches/fitbit");
            timeSeriesBulkInsert.dispose();
        }
        {
            // Append another time series
            const timeSeriesBulkInsert = bulkInsert.timeSeriesFor("users/john", "ExerciseHeartRates");
            await timeSeriesBulkInsert.append(new Date(baseTime.getTime() + 60_000 * 3), 81, "watches/apple-watch");
            await timeSeriesBulkInsert.append(new Date(baseTime.getTime() + 60_000 * 4), 82, "watches/apple-watch");
            timeSeriesBulkInsert.dispose();
        }
        {
            // Append time series in another document
            const timeSeriesBulkInsert = bulkInsert.timeSeriesFor("users/jane", "HeartRates");
            await timeSeriesBulkInsert.append(new Date(baseTime.getTime() + 60_000), 59, "watches/fitbit");
            await timeSeriesBulkInsert.append(new Date(baseTime.getTime() + 60_000 * 2), 60, "watches/fitbit");
            timeSeriesBulkInsert.dispose();
        }

        await bulkInsert.finish();
        //endregion
    }
}

//region syntax_1
timeSeriesFor(id, name);
//endregion

//region syntax_2
append(timestamp, value);
append(timestamp, value, tag);
append(timestamp, values);
append(timestamp, values, tag);
//endregion


