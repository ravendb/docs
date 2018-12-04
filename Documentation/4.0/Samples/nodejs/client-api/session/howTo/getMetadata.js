import * as assert from "assert";
import { DocumentStore } from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

{

    class Employee {}

    class User {
        constructor(name) {
            this.name = name;
        }
    }

async function sample() {
    {
        //region get_metadata_2
        const employee = await session.load("employees/1-A");
        const metadata = session.advanced.getMetadataFor(employee);
        //endregion
    }

    {
        //region modify_metadata_1
        const user = new User();
        user.name = "Idan";

        await session.store(user);

        const metadata = session.advanced.getMetadataFor(user);
        metadata["Permissions"] = "READ_ONLY";
        await session.saveChanges();
        //endregion
    }

    {
        //region modify_metadata_2
        const user = await session.load("users/1-A");
        const metadata = session.advanced.getMetadataFor(user);
        metadata["Permissions"] = "READ_AND_WRITE";
        await session.saveChanges();
        //endregion
    }
}

{
    //region get_metadata_1
    session.advanced.getMetadataFor(entity);
    //endregion
}
