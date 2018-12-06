import { DocumentStore } from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

class User {
    constructor(name) {
        this.name = name;
    }
}

{
    //region InitializationSample
    const store = new DocumentStore([
        "http://[node A url]",
        "http://[node B url]",
        "http://[node C url]"
    ], "TestDB");

    store.initialize();

    // the rest of ClientAPI code
    //endregion
}

async function writeAssuranceSample() {
    //region WriteAssuranceSample
    const session = store.openSession();
    const user = new User("John Doe");

    await session.store(user);

    //make sure that the comitted data is replicated to 2 nodes
    //before returning from the saveChanges() call.
    session
        .advanced
        .waitForReplicationAfterSaveChanges();
    //endregion

}
