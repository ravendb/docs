import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function deleteTimeSeries() {
    
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
        //region delete_1
        // Get an instance of 'timeSeriesFor'
        const tsf = session.timeSeriesFor("users/john", "HeartRates");
        
        // Call 'deleteAt' to delete a specific entry
        timeStampOfEntry = new Date(baseTime.getTime() + 60_000);
        tsf.deleteAt(timeStampOfEntry);

        // Save changes
        await session.saveChanges();
        //endregion
    }
    {
        //region delete_2
        // Get an instance of 'timeSeriesFor'
        const tsf = session.timeSeriesFor("users/john", "HeartRates");

        // Delete a range of 5 entries
        FromTimeStamp = new Date(baseTime.getTime());
        ToTimeStamp = new Date(baseTime.getTime() + 60_000 * 5);
        tsf.delete(FromTimeStamp, ToTimeStamp);

        // Save changes
        await session.saveChanges();
        //endregion
    }
    {
        //region delete_3
        // Get an instance of 'timeSeriesFor'
        const tsf = session.timeSeriesFor("users/john", "HeartRates");

        // Delete ALL entries
        // The whole time series will be removed
        tsf.delete();

        // Save changes
        await session.saveChanges();
        //endregion
    }
}

//region syntax
// Available overloads:
delete();          // Delete all entries
deleteAt(at);      // Delete a specific entry
delete(from, to);  // Delete a range of enties
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
