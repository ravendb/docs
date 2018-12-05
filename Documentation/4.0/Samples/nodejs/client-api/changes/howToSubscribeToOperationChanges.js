import { DocumentStore } from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

const operationId = 7;

{
    //region operation_changes_1
    store.changes().forOperationId(operationId);
    //endregion

    //region operation_changes_3
    store.changes().forAllOperations();
    //endregion
}

{
    {
        //region operation_changes_2
        store.changes().forOperationId(operationId)
            .on("data", change => {
                const operationState = change.state;

                // do something
            });
        //endregion
    }

    {
        //region operation_changes_4
        store.changes().forAllOperations()
            .on("data", change => {
                console.log("Operation #" + change.operationId);
            });
        //endregion
    }

}
