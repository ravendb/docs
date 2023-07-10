import { DocumentStore, AbstractIndexCreationTask } from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

class Employee { }

//region the_index
// The index definition:

class Employees_ByName extends AbstractJavaScriptIndexCreationTask {

    constructor() {
        super();

        // Define the INDEX-fields 
        this.map("Employees", e => ({
            
            // Content of INDEX-fields 'firstName' & 'lastName' 
            // is composed of the relevant DOCUMENT-fields
            firstName: e.firstName,
            lastName: e.lastName
        }));

        // * The index-fields can be queried on to fetch matching documents. 
        //   You can query and filter Employee documents based on their first or last names.

        // * Employee documents that do Not contain both 'firstName' and 'lastName' fields
        //   will Not be indexed.

        // * Note: the INDEX-field name does Not have to be exactly the same
        //   as the DOCUMENT-field name. 
    }
}
//endregion

async function queryIndex() {
    const session = store.openSession();

    {
        //region index_query_1_1
        // Query the 'Employees' collection using the index - without filtering
        // (Open the 'Index' tab to view the index class definition)
        
        const employees = await session
             // Pass the index name as a parameter
             // Use slash `/` in the index name, replacing the underscore `_` from the index class definition
            .query({ indexName: "Employees/ByName" })
             // Execute the query
            .all();
        
        // All 'Employee' documents that contain DOCUMENT-fields 'firstName' and\or 'lastName' will be returned
        //endregion
    }
    
    {
        //region index_query_1_2
        // Query the 'Employees' collection using the index - without filtering

        const employees = await session
             // Pass the queried collection as the first param
             // Pass the index class as the second param
            .query(Employee, Employees_ByName)
             // Execute the query
            .all();

        // All 'Employee' documents that contain DOCUMENT-fields 'firstName' and\or 'lastName' will be returned
        //endregion
    }
    
    {
        //region index_query_2_1
        // Query the 'Employees' collection using the index - filter by INDEX-field

        const employees = await session
             // Pass the index name as a parameter
             // Use slash `/` in the index name, replacing the underscore `_` from the index class definition
            .query({ indexName: "Employees/ByName" })
             // Filter the retrieved documents by some predicate on an INDEX-field
            .whereEquals("lastName", "King")
             // Execute the query
            .all();

        // Results will include all documents from 'Employees' collection whose 'lastName' equals to 'King'
        //endregion
    }
    
    {
        //region index_query_2_2
        // Query the 'Employees' collection using the index - filter by INDEX-field

        const employees = await session
             // Pass the queried collection as the first param
             // Pass the index class as the second param
            .query(Employee, Employees_ByName)
             // Filter the retrieved documents by some predicate on an INDEX-field
            .whereEquals("lastName", "King")
             // Execute the query
            .all();

        // Results will include all documents from 'Employees' collection whose 'lastName' equals to 'King'
        //endregion
    }
    
    {
        //region index_query_3_1
        // Query the 'Employees' collection using the index - page results

        // This example is based on the previous filtering example
        const employees = await session
            .query({ indexName: "Employees/ByName" })
            .whereEquals("lastName", "King")
            .skip(5)  // Skip first 5 results
            .take(10) // Retrieve up to 10 documents
            .all();

        // Results will include up to 10 matching documents
        //endregion
    }
    
    {
        //region index_query_3_2
        // Query the 'Employees' collection using the index - page results
        
        // This example is based on the previous filtering example
        const employees = await session
            .query(Employee, Employees_ByName)
            .whereEquals("lastName", "King")
            .skip(5)  // Skip first 5 results
            .take(10) // Retrieve up to 10 documents
            .all();

        // Results will include up to 10 matching documents
        //endregion
    }
    
    {
        //region index_query_4_1
        // Query with RawQuery - filter by INDEX-field
        
        const results = await session
             // Provide RQL to rawQuery
            .advanced.rawQuery("from index 'Employees/ByName' where lastName == 'King'")
             // Execute the query
            .all();

        // Results will include all documents from 'Employees' collection whose 'lastName' equals to 'King'.
        //endregion
    }
}

