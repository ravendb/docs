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
        //   EITHER 'English' OR 'Italian' in their 'Notes' field (case-insensitive).
        //
        // * Matching documents that contain 'Italian' will get a HIGHER score
        //   than those that contain 'English'.
        //
        // * Unless configured otherwise, the resulting documents will be ordered by their score.  
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
        //   (start-with 'O') OR (start-with 'P') OR (end-with 'OP') in their 'Name' field (case-insensitive). 
        //
        // * Matching documents that end-with 'OP' will get the HIGHEST scores.
        //   Matching documents that start-with 'O' will get the LOWEST scores. 
        //
        // * Unless configured otherwise, the resulting documents will be ordered by their score.
        //endregion
    }
}
