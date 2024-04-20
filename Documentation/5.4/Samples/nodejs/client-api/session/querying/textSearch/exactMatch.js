import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

async function exactMatch() {
    {
        //region exact_1
        const employees = await session
             // Make a dynamic query on 'Employees' collection    
            .query({ collection: "Employees" })
             // Query for all documents where 'FirstName' equals 'Robert'
             // Pass 'true' as the 3'rd param for a case-sensitive match
            .whereEquals("FirstName", "Robert", true)
            .all();
        //endregion
    }

    {
        //region exact_2
        const orders = await session
             // Make a dynamic query on 'Orders' collection
            .query({ collection: "Orders" })
             // Query for documents that contain at least one order line with 'Teatime Chocolate Biscuits'
             // Pass 'true' as the 3'rd param for a case-sensitive match
            .whereEquals("Lines.ProductName", "Teatime Chocolate Biscuits", true)
            .all();
        //endregion
    }
}

{
    //region syntax
    // Available overloads:
    
    whereEquals(fieldName, value);
    whereEquals(fieldName, value, exact);

    whereNotEquals(fieldName, value);
    whereNotEquals(fieldName, value, exact);
    //endregion
}
