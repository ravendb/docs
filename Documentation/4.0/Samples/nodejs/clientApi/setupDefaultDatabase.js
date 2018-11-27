import { DocumentStore } from "ravendb";

//region default_database_1
// without specifying `database`
// we will need to specify the database in each action
// if no database is passed explicitly we will get an error

const store = new DocumentStore([ "http://localhost:8080" ]);
store.initialize();

const session = store.openSession("Northwind")
// ...

const compactSettings = { databaseName: "Northwind" };
await store.maintenance.server.send(
    new CompactDatabaseOperation(compactSettings));
//endregion

//region default_database_2
// when `database` is set to `Northwind`
// created `operations` or opened `sessions`
// will work on `Northwind` database by default
// if no database is passed explicitly
const store =  new DocumentStore("http://localhost:8080", "Northwind");
store.initialize();

const northwindSession = store.openSession();
// ...

await store.maintenance.send(
    new DeleteIndexOperation("NorthwindIndex"));

const adventureWorksSession = store.openSession("AdventureWorks");
// ...

await store.maintenance.forDatabase("AdventureWorks")
    .send(new DeleteIndexOperation("AdventureWorksIndex"));

//endregion
