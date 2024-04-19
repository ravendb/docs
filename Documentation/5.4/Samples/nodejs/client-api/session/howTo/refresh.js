import * as assert from "assert";
import { DocumentStore } from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

async function refresh() {
    {
        //region refresh_2
        const employee = await session.load("employees/1");
        assert.strictEqual("Doe", employee.lastName);

        // lastName changed to 'Shmoe'

        session.advanced.refresh(employee);

        assert.strictEqual("Shmoe", employee.lastName);
        //endregion
    }
}

{
    let entity;
    //region refresh_1
    session.advanced.refresh(entity);
    //endregion
}
