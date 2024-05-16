import { DocumentStore } from "ravendb";
import assert from "assert";

const documentStore = new DocumentStore();

async function includeWithRawQuery() {

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
        //region include_1
        const baseTime = new Date();
        const from = baseTime;
        const to = new Date(baseTime.getTime() + 60_000 * 5);
        
        // Define the Raw Query:
        const rawQuery = session.advanced
             // Use 'include timeseries' in the RQL
            .rawQuery("from users include timeseries('HeartRates', $from, $to)")
             // Pass optional parameters
            .addParameter("from", from)
            .addParameter("to", to);

        // Execute the query:
        // For each document in the query results,
        // the time series entries will be 'loaded' to the session along with the document
        const userDocuments = await rawQuery.all();

        const numberOfRequests1 = session.advanced.numberOfRequests;

        // The following call to 'get' will Not trigger a server request,
        // the entries will be retrieved from the session's cache.
        const entries = await session.timeSeriesFor(userDocuments[0], "HeartRates")
            .get(from, to);

        const entryValue = entries[0].value;

        const numberOfRequests2 = session.advanced.numberOfRequests;
        assert.equal(numberOfRequests1, numberOfRequests2);
        //endregion
    }
}

//region syntax
rawQuery(query, documentType?);
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
