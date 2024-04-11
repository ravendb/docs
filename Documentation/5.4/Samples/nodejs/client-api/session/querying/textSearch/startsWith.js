import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

async function startsWith() {

    {
        //region startsWith_1
        const products = await session
            .query({ collection: "Products" })
             // Call 'whereStartsWith'
             // Pass the document field and the prefix to search by
            .whereStartsWith("Name", "Ch")
            .all();

        // Results will contain only Product documents having a 'Name' field
        // that starts with any case variation of 'ch'
        //endregion
    }

    {
        //region startsWith_2
        const products = await session
            .query({ collection: "Products" })
            // Call 'whereStartsWith'
            // Pass 'true' as the 3'rd parameter to search for an EXACT prefix match
            .whereStartsWith("Name", "Ch", true)
            .all();

        // Results will contain only Product documents having a 'Name' field
        // that starts with 'Ch'
        //endregion
    }
    
    {
        //region startsWith_3
        const products = await session
            .query({ collection: "Products" })
            // Call 'Not' to negate the next predicate
            .not()
            // Call 'whereStartsWith'
            // Pass the document field and the prefix to search by
            .whereStartsWith("Name", "Ch")
            .all();

        // Results will contain only Product documents having a 'Name' field
        // that does NOT start with 'ch' or any other case variations of it
        //endregion
    }
}

{
    //region syntax
    // Available overloads:
    whereStartsWith(fieldName, value);
    whereStartsWith(fieldName, value, exact);
    //endregion
}