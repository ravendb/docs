import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

async function fuzzySearch() {
    {
        //region fuzzy
        const employees = await session
            .query({ collection: "Companies" })
             // Query with a term that is misspelled
            .whereEquals("Name", "Ernts Hnadel")
             // Call 'fuzzy' 
             // Pass the required similarity, a number between 0.0 and 1.0
            .fuzzy(0.5)
            .all();

        // Running the above query on the Northwind sample data returns document: companies/20-A
        // which contains "Ernst Handel" in its Name field.
        //endregion
    }
}

{
    //region syntax
    fuzzy(fuzzy);
    //endregion
}
