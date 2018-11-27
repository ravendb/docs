import { DocumentStore, DeleteCommandData } from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

//region deleting_1
session.delete(entity);
session.delete(id);
session.delete(id, changeVector);
//endregion

class Employee {}

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
