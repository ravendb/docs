import * as assert from "assert";
import { DocumentStore } from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

{
    let instance;
    //region get_last_modified_1
    session.advanced.getLastModifiedFor(instance);
    //endregion
}

async function sample() {
    {
        //region get_last_modified_2
        const employee = await session.load("employees/1-A");
        const lastModified = session.advanced.getLastModifiedFor(employee);
        //endregion
    }
}
