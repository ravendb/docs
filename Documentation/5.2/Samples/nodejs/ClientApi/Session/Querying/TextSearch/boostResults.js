import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

async function boostResults() {
    {
        //region boost_1
        const employees = await session
             // Make a dynamic full-text search Query on 'Employees' collection
            .query({ collection: "Employees"})
             // This search predicate will use the default boost value of 1
            .search('Notes', 'English')
             // This search predicate will use a boost value of 10 
            .search('Notes', 'Italian')
             // Call 'boost' to set the boost value of the previous 'search' call
            .boost(10)
            .all();

        // * Results will contain all Employee documents that have
        //   EITHER 'English' OR 'Italian' in their 'Notes' field.
        //
        // * Matching documents with 'Italian' will be listed FIRST in the results,
        //   before those with 'English'.
        // 
        // * Search is case-insensitive.
        //endregion
    }

    {
        //region boost_2
        const companies = await session
             // Make a dynamic DocumentQuery on 'Companies' collection
            .query({ collection: "Companies"})
             // Define a 'where' condition
            .whereStartsWith("Name", "O")
             // Call 'boost' to set the boost value of the previous 'where' predicate
            .boost(10)
             // Call 'orElse' so that OR operator will be used between statements
            .orElse()
            .whereStartsWith("Name", "P")
            .boost(50)
            .orElse()
            .whereEndsWith("Name", "OP")
            .boost(90)
            .all();

        // * Results will contain all Company documents that either
        //   (start-with 'O') OR (start-with 'P') OR (end-with 'OP') in their 'Name' field.
        //
        // * Matching documents the end-with 'OP' will be listed FIRST.
        //   Matching documents that start-with 'P' will then be listed.
        //   Matching documents that start-with 'O' will be listed LAST.
        // 
        // * Search is case-insensitive.
        //endregion
    }

    {
        //region boost_3
        // Make a query:
        // =============
        
        const employees = await session
            .query({ collection: "Employees"})
            .search('Notes', 'English')
            .search('Notes', 'Italian')
            .boost(10)
            .all();

        // Get the score:
        // ==============

        // Call 'getMetadataFor', pass an entity from the resulting employees list
        const metadata = session.advanced.getMetadataFor(employees[0]);

        // Score is available in the '@index-score' metadata property
        const score = metadata[const score = metadata["@index-score"]];
        // const score = metadata[CONSTANTS.Documents.Metadata.INDEX_SCORE] // todo ???
        //endregion
    }
}
