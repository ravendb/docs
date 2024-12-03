import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

{
    //region syntax
    query.includeExplanations(explanationsCallback)
    //endregion

    //region syntax_2
    // The Explanations object:
    // ========================
    
    class Explanations {
        get explanations(): {
            [key: string]: string[]; // An explanations list per document ID key
        };
    }
    //endregion
}

async function explain() {
    //region explain
    // Define an object that will receive the explanations results
    let explanationsResults;

    const results = await session.query({ collection: "Products" })
         // Call includeExplanations, pass a callback function
         // Output param 'explanationsResults' will be filled with explenations results when query returns 
        .includeExplanations(e => explanationsResults = e)
         // Define query criteria
         // i.e. search for docs containing Syrup -or- Lager in their Name field
        .search("Name", "Syrup Lager")
         // Execute the query
        .all();

    // Get the score details for a specific document from 'explanationsResults'
    const scoreDetails = explanationsResults.explanations[productResults[0].id];
    //endregion
}
