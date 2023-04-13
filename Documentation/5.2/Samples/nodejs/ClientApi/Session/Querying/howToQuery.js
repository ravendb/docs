import { DocumentStore, AbstractIndexCreationTask } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

async function howToQuery() {
    {
        //region query_1
        // This is a Full Collection Query
        // No auto-index is created since no filtering is applied

        // Query for all documents from 'employees' collection
        const employees = await session.query({ collection: "employees" })
            // Execute the query
            .all();
        
        // All 'employee' entities are loaded and will be tracked by the session
        //endregion
    }

    {
        //region query_2
        // Query collection by document ID
        // No auto-index is created when querying only by ID
        
        const employee = await session.query({ collection: "employees" })
            .whereEquals("id()", "employees/1-A") // Query for specific document from 'employees' collection 
            .first();                             // Execute the query
        
        // The resulting 'employee' entity is loaded and will be tracked by the session 
        //endregion
    }

    {
        //region query_3
        // Query collection - filter by document field

        // An auto-index will be created if there isn't already an existing auto-index
        // that indexes this document field

        const employees = await session.query({ collection: "employees" })
            .whereEquals("firstName", "Robert") // Query for all 'employee' documents that match this predicate 
            .all();                             // Execute the query

        // The resulting 'employee' entities are loaded and will be tracked by the session 
        //endregion
    }

    {
        //region query_4
        // Query collection - page results
        // No auto-index is created since no filtering is applied

        const products = await session.query({ collection: "products" })
            .skip(5)  // Skip first 5 results
            .take(10) // Load up to 10 entities from 'products' collection
            .all();   // Execute the query
        
        // The resulting 'product' entities are loaded and will be tracked by the session 
        //endregion
    }

    {
        //region query_5
        // Query with rawQuery - filter by document field

        // An auto-index will be created if there isn't already an existing auto-index
        // that indexes this document field

        const employees = await session.advanced
             // Provide RQL to rawQuery
            .rawQuery("from employees where firstName = 'Robert'")
             // Execute the query
            .all();

        // The resulting 'employee' entities are loaded and will be tracked by the session 
        //endregion
    }
}

{
    //region syntax
    session.query(opt);
    //endregion
}
