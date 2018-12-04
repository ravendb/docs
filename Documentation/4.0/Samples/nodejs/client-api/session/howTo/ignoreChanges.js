import * as assert from "assert";
import { DocumentStore } from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

async function example() {
    {
        //region ignore_changes_2
        const product = await session.load("products/1-A");
        session.advanced.ignoreChangesFor(product);
        product.unitsInStock++; // this will be ignored by saveChanges()
        await session.saveChanges();
        //endregion
    }
}

{
    let entity;
    //region ignore_changes_1
    session.advanced.ignoreChangesFor(entity);
    //endregion
}
