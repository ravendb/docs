import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function patchOperations() {
    {
        //region patch_1
        const baseTime = new Date();
        
        const patchRequest = new PatchRequest();
        
        // Define the patch request using JavaScript:
        patchRequest.script = "timeseries(this, $timeseries).append($timestamp, $values, $tag);";
        
        // Provide values for the parameters in the script:
        patchRequest.values = {
            timeseries: "HeartRates",
            timestamp: baseTime.toISOString(),
            values: 59,
            tag: "watches/fitbit"
        };

        // Define the patch operation:
        const patchOp = new PatchOperation("users/john", null, patchRequest);

        // Execute the operation:
        await documentStore.operations.send(patchOp);
        //endregion
    }
    {
        //region patch_2
        const baseTime = new Date();

        // Create arrays of timestamps and random values to patch
        const values = [];
        const timeStamps = [];

        for (let i = 0; i < 100; i++) {
            const randomValue = 68 + Math.round(19 * Math.random());
            values.push(randomValue);

            const timeStamp = baseTime.getTime() + 60_000 * i;
            timeStamps.push(new Date(timeStamp).toISOString());
        }

        const patchRequest = new PatchRequest();

        patchRequest.script = `
               for (let i = 0; i < $values.length; i++) {
                   timeseries(id(this), $timeseries).append(
                       $timeStamps[i],
                       $values[i],
                       $tag);
               }`;

        patchRequest.values = {
            timeseries: "HeartRates",
            timeStamps: timeStamps,
            values: values,
            tag: "watches/fitbit"
        };
        
        const patchOp = new PatchOperation("users/john", null, patchRequest);
        await documentStore.operations.send(patchOp);
        //endregion
    }
    {
        //region patch_3
        const baseTime = new Date();

        const patchRequest = new PatchRequest();

        patchRequest.script = "timeseries(this, $timeseries).delete($from, $to);";

        patchRequest.values = {
            timeseries: "HeartRates",
            from: baseTime.toISOString(),
            to: new Date(baseTime.getTime() + 60_000 * 49).toISOString()
        };

        const patchOp = new PatchOperation("users/john", null, patchRequest);
        await documentStore.operations.send(patchOp);
        //endregion
    }
    {
        //region patch_4
        const indexQuery = new IndexQuery();
        
        // Define the query & patch string:
        indexQuery.query = `
            from users as u
            update {
                timeseries(u, $name).append($time, $values, $tag)
            }`;
        
        // Provide values for the parameters in the script:
        indexQuery.queryParameters = { 
                name: "HeartRates",
                time: new Date(baseTime.getTime() + 60_000).toISOString(),
                values: 59,
                tag: "watches/fitbit"
        }

        // Define the patch operation:
        const patchByQueryOp = new PatchByQueryOperation(indexQuery);

        // Execute the operation:
        await documentStore.operations.send(patchByQueryOp);
        //endregion
    }
    {
        //region patch_5
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
    }
    {
        //region patch_6
        const indexQuery = new IndexQuery();

        indexQuery.query = `
            declare function patchDocumentField(doc) {
                var differentTags = [];
                var entries = timeseries(doc, $name).get();
    
                for (var i = 0; i < entries.length; i++) {
                    var e = entries[i];
    
                    if (e.Tag !== null) {
                        if (!differentTags.includes(e.Tag)) {
                            differentTags.push(e.Tag);
                        }
                    }
                }
    
                doc.numberOfUniqueTagsInTS = differentTags.length;
                return doc;
            }
    
            from users as u
            update {
                put(id(u), patchDocumentField(u))
            }
        `;

        indexQuery.queryParameters = {
            name: "HeartRates"
        }

        const patchByQueryOp = new PatchByQueryOperation(indexQuery);
        
        // Execute the operation and wait for completion:
        const asyncOp = await documentStore.operations.send(patchByQueryOp);
        await asyncOp.waitForCompletion();
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
