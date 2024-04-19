import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

{
    //region syntax
    getCountersFor(entity);
    //endregion
}

async function getCountersFor() {
    //region example
    // Load a document
    const employee = await session.load("employees/1-A");
    
    // Get counter names from the loaded entity
    const counterNames = session.advanced.getCountersFor(employee);    
    //endregion
}
