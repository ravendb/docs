import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

async function proximitySearch() {
    {
        //region proximity_1
        const employees = await session
            .query({ collection: "Employees" })
             // Make a full-text search with search terms
            .search("Notes", "fluent french")
             // Call 'proximity' with 0 distance
            .proximity(0)
            .all();

        // Running the above query on the Northwind sample data returns the following Employee documents:
        // * employees/2-A
        // * employees/5-A
        // * employees/9-A
        
        // Each resulting document has the text 'fluent in French' in its 'Notes' field.
        //
        // The word "in" is not taken into account as it is Not part of the terms list generated
        // by the analyzer. (Search is case-insensitive in this case).
        //
        // Note:
        // A document containing text with the search terms appearing with no words in between them
        // (e.g. "fluent french") would have also been returned.
        //endregion
    }
    {
        //region proximity_2
        const employees = await session
            .query({ collection: "Employees" })
             // Make a full-text search with search terms
            .search("Notes, "fluent french")
             // Call 'proximity' with distance 5
            .proximity(5)
            .all();

        // Running the above query on the Northwind sample data returns the following Employee documents:
        // * employees/2-A
        // * employees/5-A
        // * employees/6-A
        // * employees/9-A
        
        // This time document 'employees/6-A' was added to the previous results since it contains the phrase:
        // "fluent in Japanese and can read and write French"
        // where the search terms are separated by a count of 4 terms.
        //
        // "in" & "and" are not taken into account as they are not part of the terms list generated
        // by the analyzer.(Search is case-insensitive in this case).
        //endregion
    }    
}

{
    //region syntax
    proximity(proximity);
    //endregion
}
