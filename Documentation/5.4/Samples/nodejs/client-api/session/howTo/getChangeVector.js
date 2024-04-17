import * as assert from "assert";
import { DocumentStore } from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

{
    let entity;
    //region get_change_vector_1
    session.advanced.getChangeVectorFor(entity);
    //endregion
}

async function sample() {
    //region get_change_vector_2
    const employee = await session.load("employees/1-A");
    const changeVector = session.advanced.getChangeVectorFor(employee);
    //endregion
}

