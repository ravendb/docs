import { DocumentStore } from "ravendb";
import assert from "assert";

const documentStore = new DocumentStore();

async function includeWithQueyr() {

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
        // Query for a document and include a whole time-series
        const user = await session.query({ collection: "users" })
            .whereEquals("name", "John")
             // Call 'includeTimeSeries' to include the time series entries in the response
             // Pass the time series name
             // (find more include builder overloads under the Syntax section)
            .include(includeBuilder => includeBuilder.includeTimeSeries("HeartRates"))
            .first();

        const numberOfRequests1 = session.advanced.numberOfRequests;

        // The following call to 'get' will Not trigger a server request,
        // the entries will be retrieved from the session's cache.
        const entries = await session.timeSeriesFor(user, "HeartRates")
            .get();

        const entryValue = entries[0].value;

        const numberOfRequests2 = session.advanced.numberOfRequests;
        assert.equal(numberOfRequests1, numberOfRequests2);
        //endregion
    }
}

//region syntax
includeTimeSeries(name);
includeTimeSeries(name, from, to);
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
