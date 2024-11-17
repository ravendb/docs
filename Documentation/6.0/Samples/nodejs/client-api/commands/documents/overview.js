import { DocumentStore } from "ravendb";
import assert from "assert";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

async function putDocumentsCommand() {
    {
        //region execute_1
        // Define a command
        const cmd = new CreateSubscriptionCommand({
            name: "Orders subscription",
            query: "from Orders"
        });

        // Call 'execute' on the store's request executor to run the command on the server
        // Pass the command
        await documentStore.getRequestExecutor().execute(cmd);
        //endregion
    }
    {
        //region execute_2
        const session = documentStore.openSession();
        
        // Define a command
        const cmd = new GetDocumentsCommand(
            { conventions: documentStore.conventions, id: "orders/1-A" });

        // Call 'execute' on the session's request executor to run the command on the server
        // Pass the command
        await session.advanced.requestExecutor.execute(cmd);

        // Access the results
        const order = command.result.results[0];
        const orderedAt = order.OrderedAt;
        //endregion
    }
}
