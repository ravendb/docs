import * as assert from "assert";
import { DocumentStore } from "ravendb";

class Company {
    constructor(name) {
        this.name = name;
    }
}

function whatIsSession() {
    const store = new DocumentStore();
    //region session_usage_1
    let companyId;

    {
        const session = store.openSession();
        // store new object
        const entity = new Company("Company");
        await session.store(entity);
        await session.saveChanges();

        // after calling saveChanges(), an id field if exists
        // is filled by the entity's id
        companyId = entity.id;
    }

    {
        const session = store.openSession();
        // load by id
        const entity = await session.load(companyId);
        // do something with the loaded entity
    }
    //endregion

    //region session_usage_2
    {
        const session = store.openSession();
        const entity = await session.load(companyId);
        entity.name = "Another company";

        // when a document is loaded with load() by Id ( or with query() ),
        // its changes are being tracked (by default).
        // A call to saveChanges() sends all accumulated changes to the server
        await session.saveChanges();
    }
    //endregion

    {
        const session = store.openSession();
        //region session_usage_3
        assert.strictEqual(
            await session.load(companyId), 
            await session.load(companyId));
        //endregion
    }
}
