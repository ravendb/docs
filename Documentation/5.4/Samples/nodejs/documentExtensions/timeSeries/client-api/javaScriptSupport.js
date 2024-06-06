import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function javascriptSupport() {

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
        //region js_support_1
        const baseTime = new Date();

        // Prepare random values and timestamps to patch
        const values = [];
        const timeStamps = [];

        for (let i = 0; i < 100; i++) {
            const randomValue = 65 + Math.round(20 * Math.random());
            values.push(randomValue);

            // NOTE: the timestamp passed in the patch request script should be in UTC
            const timeStamp = new Date(baseTime.getTime() + 60_000 * i);
            const utcDate = new Date(timeStamp.getTime() + timeStamp.getTimezoneOffset() * 60_000);
            timeStamps.push(utcDate);
        }

        // Define the patch request
        // ========================

        const patchRequest = new PatchRequest();

        // Provide a JavaScript script, use the 'append' method
        // Note: "args." can be replaced with "$". E.g.: "args.tag" => "$tag"
        patchRequest.script = `
            for(var i = 0; i < args.values.length; i++)
            {
                timeseries(id(this), args.timeseries)
                .append (
                    new Date(args.timeStamps[i]),
                    args.values[i],
                    args.tag);
            }`;

        // Provide values for the params used within the script
        patchRequest.values = {
            timeseries: "HeartRates",
            timeStamps: timeStamps,
            values: values,
            tag: "watches/fitbit"
        }

        // Define the patch command
        const patchCommand = new PatchCommandData("users/john", null, patchRequest, null)

        // Pass the patch command to 'defer'
        session.advanced.defer(patchCommand);

        // Call saveChanges for the patch request to execute on the server
        await session.saveChanges();
        //endregion

        //region js_support_2
        const indexQuery = new IndexQuery();

        indexQuery.query = `
            from users as u
            where u.age < 30
            update
            {
                timeseries(u, "HeartRates").delete()
            }`;

        const deleteByQueryOp = new PatchByQueryOperation(indexQuery);

        // Execute the operation: 
        // Time series "HeartRates" will be deleted for all users with age < 30
        await documentStore.operations.send(deleteByQueryOp);
        //endregion
        //endregion
    }
}

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
