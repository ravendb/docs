import { DocumentStore, DeleteCommandData } from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

let entity, id, changeVector;

//region deleting_1
await session.delete(entity);

await session.delete(id);

await session.delete(id, [changeVector]);
//endregion

class Employee {}

async function examples() { 
    //region deleting_2
    const employee = await session.load("employees/1");

    session.delete(employee);
    await session.saveChanges();
    //endregion

    //region deleting_3
    session.delete("employees/1");
    await session.saveChanges();
    //endregion

    //region deleting_4
    session.delete("employees/1");
    //endregion

    //region deleting_5
    session.advanced.defer(new DeleteCommandData("employees/1", null));
    //endregion
}
