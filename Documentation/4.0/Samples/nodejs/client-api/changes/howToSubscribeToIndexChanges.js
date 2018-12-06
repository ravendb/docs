import { DocumentStore } from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

{
    let indexName;
    //region index_changes_1
    store.changes().forIndex(indexName);
    //endregion

    //region index_changes_3
    store.changes().forAllIndexes();
    //endregion
}

async function examples() {
    {
        //region index_changes_2
        store.changes().forIndex("Orders/All")
            .on("data", change => {
                switch (change.type) {
                    case "None":
                        // do something
                        break;
                    case "BatchCompleted":
                        // do something
                        break;
                    case "IndexAdded":
                        // do something
                        break;
                    case "IndexRemoved":
                        // do something
                        break;
                    case "IndexDemotedToIdle":
                        // do something
                        break;
                    case "IndexPromotedFromIdle":
                        // do something
                        break;
                    case "IndexDemotedToDisabled":
                        // do something
                        break;
                    case "IndexMarkedAsErrored":
                        // do something
                        break;
                    case "SideBySideReplace":
                        // do something
                        break;
                    case "Renamed":
                        // do something
                        break;
                    case "IndexPaused":
                        // do something
                        break;
                    case "LockModeChanged":
                        // do something
                        break;
                    case "PriorityChanged":
                        // do something
                        break;
                    default:
                        throw new Error("Not supported.");
                }
            });
        //endregion
    }

    {
        //region index_changes_4
        store.changes().forAllIndexes()
            .on("data", change => {
                console.log(change.type + " on index " + change.name);
            });
        //endregion
    }
}
