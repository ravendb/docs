import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

class Syntax {
    //region open_session_1
    openSession();

    openSession(database);

    openSession(sessionOptions);
    //endregion
}

async function openSessionUseAwait() {
    
    //region open_session_2
    // Open the session, pass the options object to the session
    const session = documentStore.openSession({
        database: "your_database_name",
        transactionMode: "ClusterWide"
    });
    
    //   Run your business logic:
    //
    //   Store entities
    //   Load and Modify entities
    //   Query indexes & collections
    //   Delete documents
    //   ... etc.

    // For example: load a document and modify it
    // Note: 'load' returns a Promise and must be awaited
    const entity = await session.load("companies/1-A");
    entity.name = "NewCompanyName";
    
    // Save you changes
    // Note: 'saveChanges' also returns a Promise and must be awaited
    await session.saveChanges();
    // When 'SaveChanges' returns successfully,
    // all changes made to the entities in the session are persisted to the documents in the database
    //endregion
}

function openSessionUseThen() {
    //region open_session_3
    // Open the session, pass the options object to the session
    const session = documentStore.openSession({
        database: "your_database_name",
        transactionMode: "ClusterWide"
    });
    
    //   Run your business logic:
    //
    //   Store entities
    //   Load and Modify entities
    //   Query indexes & collections
    //   Delete documents
    //   ... etc.

    // For example: load a document, modify it, and save
    // Note: 'load' & 'saveChanges' each return a Promise that is then handled by callback functions
    session.load("companies/1-A")
        .then((company) => {
            company.name = "NewCompanyName";
        })
        .then(() => session.saveChanges())
        .then(() => {
            // When 'SaveChanges' returns successfully,
            // all changes made to the entities in the session are persisted to the documents in the database
        });
    //endregion
}
