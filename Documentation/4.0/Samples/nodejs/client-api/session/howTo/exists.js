import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

{
    let id;
    //region exists_1
    session.advanced.exists(id);
    //endregion
}

async function sample() {
    //region exists_2
    const exists = await session.advanced.exists("employees/1-A");
    if (exists) {
        // do something ...
    }
    //endregion
}
