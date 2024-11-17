import { DocumentStore } from "ravendb";

let urls, database, authOptions;

{
    //region document_store_ctor
    new DocumentStore(urls, [database], [authOptions]);
    //endregion
}

{
    //region document_store_creation
    const store = new DocumentStore(["http://localhost:8080"], "Northwind");
    store.initialize();
    //endregion
}

//region document_store_holder
// documentStoreHolder.js
const store = new DocumentStore("http://localhost:8080", "Northwind");
store.initialize();
export { store as documentStore }; 
//endregion
