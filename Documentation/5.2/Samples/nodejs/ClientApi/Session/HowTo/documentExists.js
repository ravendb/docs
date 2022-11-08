import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

{
    //region syntax
    session.advanced.exists(id);
    //endregion
}

async function sample() {
    //region exists
    const exists = await session.advanced.exists("employees/1-A");
    if (exists) {
        // document 'employees/1-A exists
    }
    //endregion
}
