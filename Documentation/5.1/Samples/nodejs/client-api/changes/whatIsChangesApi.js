import { DocumentStore } from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

{
    let database;
    //region changes_1
    store.changes([database]);
    //endregion
}

async function example() {
    {
        //region changes_2
        const changes = store.changes();

        await changes.ensureConnectedNow();

        const allDocsChanges = changes.forAllDocuments()
            .on("data", change => {
                console.log(change.type + " on document " + change.id);
            })
            .on("error", err => {
                // handle error
            });

        // ...

        try {
            // application code here
        } finally {
            // dispose changes after use
            if (changes != null) {
                changes.dispose();
            }
        }
        //endregion
    }
}
