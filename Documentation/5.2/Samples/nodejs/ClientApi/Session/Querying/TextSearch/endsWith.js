import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

async function endsWith() {

    {
        //region endsWith_1
        const products = await session
            .query({ collection: "Products" })
             // Call 'whereEndsWith'
             // Pass the document field and the postfix to search by
            .whereEndsWith("Name", "Lager")
            .all();

        // Results will contain only Product documents having a 'Name' field
        // that ends with 'Lager' OR 'lager'
        //endregion
    }

    {
        //region endsWith_2
        const products = await session
            .query({ collection: "Products" })
            // Call 'whereEndsWith'
            // Pass 'true' as the 3'rd parameter to search for an EXACT postfix match
            .whereEndsWith("Name", "Lager", true)
            .all();

        // Results will contain only Product documents having a 'Name' field
        // that ends with 'Lager'
        //endregion
    }

    {
        //region endsWith_3
        const products = await session
            .query({ collection: "Products" })
            // Call 'Not' to negate the next predicate
            .not()
            // Call 'whereEndWith'
            // Pass the document field and the postfix to search by
            .whereEndsWith("Name", "Lager")
            .all();

        // Results will contain only Product documents having a 'Name' field
        // that does NOT end with 'Lager' or 'lager'
        //endregion
    }
}

{
    //region syntax
    // Available overloads:
    whereEndsWith(fieldName, value);
    whereEndsWith(fieldName, value, exact);
    //endregion
}
