import { DocumentStore } from "ravendb";

const docStore = new DocumentStore();

//region open_session_1
// Open session for a 'default' database configured in the 'DocumentStore' instance
docStore.openSession();

// Open session for a specified database
docStore.openSession("Database1"); // database name string

docStore.openSession({
    database: "Db1",    // string (optional, defaults to 'default' database)
    requestExecutor	    // RequestExecutor instance (optional)
});
//endregion

const databaseName = "DB1";

//region open_session_2
store.openSession({});
//endregion

//region open_session_3
const sessionOptions = { database: databaseName };
store.openSession(sessionOptions);
//endregion

//region open_session_4
store.openSession();
// code here
//endregion
