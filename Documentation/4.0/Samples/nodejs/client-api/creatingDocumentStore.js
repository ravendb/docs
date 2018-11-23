import { DocumentStore } from "ravendb";

//region document_store_creation
store = new DocumentStore(["http://localhost:8080"], "Northwind");
store.initialize();
//endregion

//region document_store_holder
// documentStoreHolder.js
const store = new DocumentStore("http://localhost:8080", "Northwind");
store.initialize();
export { store as documentStore }; 
//endregion
