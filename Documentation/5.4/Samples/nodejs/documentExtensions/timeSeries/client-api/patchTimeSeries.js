import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function patchTimeSeries() {

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
        //region patch_1
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

        //region patch_2
        // Define the patch request
        // ========================
        
        const patchRequest = new PatchRequest();

        // Provide a JavaScript script, use the 'delete' method
        // Note: "args." can be replaced with "$". E.g.: "args.to" => "$to"
        patchRequest.script = `timeseries(this, args.timeseries)
                 .delete(
                     args.from,
                     args.to 
                 );`;
       
        // NOTE: the 'from' & 'to' params in the patch request script should be in UTC
        const utcDate = new Date(baseTime.getTime() + baseTime.getTimezoneOffset() * 60_000);

        // Provide values for the params used within the script
        patchRequest.values = {
            timeseries: "HeartRates",
            from: utcDate,
            to: new Date(utcDate.getTime() + 60_000 * 49)
        }

        // Define the patch command
        const patchCommand = new PatchCommandData("users/john", null, patchRequest, null)

        // Pass the patch command to 'defer'
        session.advanced.defer(patchCommand);

        // Call saveChanges for the patch request to execute on the server
        await session.saveChanges();
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
