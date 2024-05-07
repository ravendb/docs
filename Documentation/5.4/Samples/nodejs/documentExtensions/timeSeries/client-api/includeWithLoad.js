import { DocumentStore } from "ravendb";
import assert from "assert";

const documentStore = new DocumentStore();

async function includeWithLoad() {

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
        const session = documentStore.openSession();

        const baseTime = new Date();
        const from = new Date(baseTime.getTime() + 60_000 * 3);
        const to = new Date(baseTime.getTime() + 60_000 * 8);
        
        // Load a document entity to the session
        const user = await session.load("users/john", {
            // Call 'includeTimeSeries' to include time series entries, pass:
            // * The time series name
            // * Start and end timestamps indicating the range of entries to include
            includes: builder => builder.includeTimeSeries("HeartRates", from, to)
        });

        const numberOfRequests1 = session.advanced.numberOfRequests;
        
        // The following call to 'get' will Not trigger a server request,  
        // the entries will be retrieved from the session's cache.  
        const entries = await session
            .timeSeriesFor("users/john", "HeartRates")
            .get(from, to);

        const numberOfRequests2 = session.advanced.numberOfRequests;
        assert.equal(numberOfRequests1, numberOfRequests2);
        //endregion
    }
}

//region syntax_1
load(id, options?);
//endregion

//region syntax_2
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
