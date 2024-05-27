import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function appendAndDeleteOperations() {
    {
        //region operation_1
        const baseTime = new Date();

        // Define the Append operations:
        let nextMinute = new Date(baseTime.getTime() + 60_000);
        const appendOp1 = new AppendOperation(nextMinute, [79], "watches/fitbit");

        nextMinute = new Date(baseTime.getTime() + 60_000 * 2);
        const appendOp2 = new AppendOperation(nextMinute, [82], "watches/fitbit");

        nextMinute = new Date(baseTime.getTime() + 60_000 * 3);
        const appendOp3 = new AppendOperation(nextMinute, [80], "watches/fitbit");

        nextMinute = new Date(baseTime.getTime() + 60_000 * 4);
        const appendOp4 = new AppendOperation(nextMinute, [78], "watches/fitbit");

        // Define the 'TimeSeriesOperation':
        const timeSeriesOp = new TimeSeriesOperation("HeartRates");
        
        // Add the Append operations by calling 'append':
        timeSeriesOp.append(appendOp1);
        timeSeriesOp.append(appendOp2);
        timeSeriesOp.append(appendOp3);
        timeSeriesOp.append(appendOp4);

        // Define 'TimeSeriesBatchOperation':
        const timeSeriesBatchOp = new TimeSeriesBatchOperation("users/john", timeSeriesOp);

        // Execute the batch operation:
        await documentStore.operations.send(timeSeriesBatchOp);
        //endregion
    }    
    {
        //region operation_2
        const baseTime = new Date();
        
        const from = new Date(baseTime.getTime() + 60_000 * 2);
        const to = new Date(baseTime.getTime() + 60_000 * 3);

        // Define the Delete operation:
        const deleteOp = new DeleteOperation(from, to);

        // Define the 'TimeSeriesOperation':
        const timeSeriesOp = new TimeSeriesOperation("HeartRates");

        // Add the Delete operation by calling 'delete':
        timeSeriesOp.delete(deleteOp);

        // Define the 'TimeSeriesBatchOperation':
        const timeSeriesBatchOp = new TimeSeriesBatchOperation("users/john", timeSeriesOp);

        // Execute the batch operation:
        await documentStore.operations.send(timeSeriesBatchOp);
        //endregion
    }
    {
        //region operation_3
        const baseTime = new Date();

        // Define some Append operations:
        let nextMinute = new Date(baseTime.getTime() + 60_000);
        const appendOp1 = new AppendOperation(nextMinute, [79], "watches/fitbit");

        nextMinute = new Date(baseTime.getTime() + 60_000 * 2);
        const appendOp2 = new AppendOperation(nextMinute, [82], "watches/fitbit");

        nextMinute = new Date(baseTime.getTime() + 60_000 * 3);
        const appendOp3 = new AppendOperation(nextMinute, [80], "watches/fitbit");

        const from = new Date(baseTime.getTime() + 60_000 * 2);
        const to = new Date(baseTime.getTime() + 60_000 * 3);

        // Define a Delete operation:
        const deleteOp = new DeleteOperation(from, to);

        // Define the 'TimeSeriesOperation':
        const timeSeriesOp = new TimeSeriesOperation("HeartRates");

        // Add the Append & Delete operations to the list of actions
        // Note: the Delete action will be executed BEFORE all the Append actions
         //      even though it is added last
        timeSeriesOp.append(appendOp1);
        timeSeriesOp.append(appendOp2);
        timeSeriesOp.append(appendOp3);
        timeSeriesOp.delete(deleteOp);

        // Define the 'TimeSeriesBatchOperation':
        const timeSeriesBatchOp = new TimeSeriesBatchOperation("users/john", timeSeriesOp);

        // Execute the batch operation:
        await documentStore.operations.send(timeSeriesBatchOp);

        // Results:
        // All 3 entries that were appended will exist and are not deleted.
        // This is because the Delete action occurs first, before all Append actions.
        //endregion
    }
}

//region syntax_1
TimeSeriesBatchOperation(documentId, operation)
//endregion

//region syntax_2
class TimeSeriesOperation {
    name;
    append;
    delete;
}
//endregion

//region syntax_3
class AppendOperation {
    timestamp;
    values;
    tag;
}
//endregion

//region syntax_4
class DeleteOperation {
    from;
    to;
}
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
