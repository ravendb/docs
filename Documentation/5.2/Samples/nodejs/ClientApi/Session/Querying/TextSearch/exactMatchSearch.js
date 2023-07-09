import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

{
    let fieldName, value, exact;
    const query = session.query();
    //region query_1_0
    query.whereEquals(fieldName, value, [exact]);
    query.whereNotEquals(fieldName, value, [exact]);

    // ... rest of where methods
    //endregion
}

async function examples() {
    {
        //region query_1_1
        // load all entities from 'Employees' collection
        // where firstName equals 'Robert' (case sensitive match)
        const employees = await session.query({ collection: "Employees" })
            .whereEquals("FirstName", "Robert", true)
            .all();
        //endregion
    }

    {
        //region query_2_1
        // return all entities from 'Orders' collection
        // which contain at least one order line with
        // 'Singaporean Hokkien Fried Mee' product
        // perform a case-sensitive match
        const orders = await session.query({ collection: "Orders" })
            .whereEquals("Lines[].ProductName", "Singaporean Hokkien Fried Mee", true)
            .all();
        //endregion
    }
}
