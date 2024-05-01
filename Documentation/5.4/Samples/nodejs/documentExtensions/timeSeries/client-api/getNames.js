import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function getTimeSeriesNames() {

    const session = documentStore.openSession();
    await session.store(new User("John"), "users/john");

    const optionalTag = "watches/fitbit";
    const baseTime = new Date();
    baseTime.setUTCHours(0);

    const tsf1 = session.timeSeriesFor("users/john", "HeartRates1");
    for (let i = 0; i < 10; i++)
    {
        const nextMinute = new Date(baseTime.getTime() + 60_000 * i);
        const nextMeasurement = 65 + i;
        tsf1.append(nextMinute, nextMeasurement, optionalTag);
    }

    const tsf2 = session.timeSeriesFor("users/john", "HeartRates2");
    tsf2.append(baseTime, 60, optionalTag);

    await session.saveChanges();

    {
        //region get_names
        // Open a session
        const session = documentStore.openSession();
        
        // Load a document entity to the session
        const user = await session.load("users/john");

        // Call getTimeSeriesFor, pass the entity
        const tsNames = session.advanced.getTimeSeriesFor(user);

        // Results will include the names of all time series associated with document 'users/john'
        //endregion
    }
}

//region syntax
getTimeSeriesFor(instance);
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
