import { DocumentStore } from "ravendb";
import assert from "assert";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

async function sendMultipleCommands() {
    {        
        //region batch_1
        // This patch request will be used in the following 'PatchCommandData' command
        let patchRequest = new PatchRequest();
        patchRequest.script = "this.HomePhone = 'New phone number'";
        
        // Define the list of batch commands to execute
        const commands = [
            new PutCommandDataBase("employees/999", null, null, {
                FirstName: "James",
                "@metadata": {
                    "@collection": "employees"
                }
            }),

            new PatchCommandData("employees/2-A", null, patchRequest),

            new DeleteCommandData("employees/3-A", null)
        ];

        // Define the 'SingleNodeBatchCommand' command
        const batchCommand = new SingleNodeBatchCommand(documentStore.conventions, commands);

        // Execute the batch command,
        // all the 3 commands defined in the list will be executed in a single transaction
        await documentStore.getRequestExecutor().execute(batchCommand);

        // Can access the batch command results
        const commandResults = batchCommand.result.results;
        assert.equal(commandResults.length, 3);
        assert.equal(commandResults[0].type, "PUT");
        assert.equal(commandResults[0]["@id"], "employees/999");
        //endregion
    }
    {
        //region batch_2
        const session = documentStore.openSession();

        // This patch request will be used in the following 'PatchCommandData' command
        let patchRequest = new PatchRequest();
        patchRequest.script = "this.HomePhone = 'New phone number'";

        // Define the list of batch commands to execute
        const commands = [
            new PutCommandDataBase("employees/999", null, null, {
                FirstName: "James",
                "@metadata": {
                    "@collection": "employees"
                }
            }),

            new PatchCommandData("employees/2-A", null, patchRequest),

            new DeleteCommandData("employees/3-A", null)
        ];

        // Define the 'SingleNodeBatchCommand' command
        const batchCommand = new SingleNodeBatchCommand(documentStore.conventions, commands);

        // Execute the batch command,
        // all the 3 commands defined in the list will be executed in a single transaction
        await session.advanced.requestExecutor.execute(batchCommand);

        // Can access the batch command results
        const commandResults = batchCommand.result.results;
        assert.equal(commandResults.length, 3);
        assert.equal(commandResults[0].type, "PUT");
        assert.equal(commandResults[0]["@id"], "employees/999");
        //endregion
    }
}

{
    //region syntax_1
    SingleNodeBatchCommand(conventions, commands);
    SingleNodeBatchCommand(conventions, commands, batchOptions);
    //endregion

    //region syntax_2
    // The batchOptions object:
    {
        replicationOptions; // ReplicationBatchOptions
        indexOptions;       // IndexBatchOptions
        shardedOptions;     // ShardedBatchOptions
    }
    
    // The ReplicationBatchOptions object:
    {
        timeout?;        // number
        throwOnTimeout?; // boolean
        replicas?;       // number
        majority?;       // boolean
    }
    
    // The IndexBatchOptions object:
    {
        timeout?;        // number
        throwOnTimeout?; // boolean
        indexes?;        // string[]
    }

    // The ShardedBatchOptions object:
    {
        batchBehavior; // ShardedBatchBehavior
    }
    //endregion

    //region syntax_3
    // Executing `SingleNodeBatchCommand` returns the following object:
    // ================================================================
    
    class BatchCommandResult {
        results; // any[]
        transactionIndex; // number
    }
    //endregion
}
