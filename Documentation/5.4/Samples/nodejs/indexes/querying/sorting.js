import { DocumentStore, AbstractJavaScriptIndexCreationTask } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

//region index_1
class Products_ByUnitsInStock extends AbstractJavaScriptIndexCreationTask {
    constructor() {
        super();

        this.map("Products", p => {
            return {
                unitsInStock: p.UnitsInStock
            };
        });
    }
}
//endregion

//region index_2
class Products_BySearchName extends AbstractJavaScriptIndexCreationTask {
    constructor() {
        super();

        this.map("Products", p => {
            return {
                // Index-field 'name' will be configured below for full-text search
                name: p.Name,
                
                // Index-field 'nameForSorting' will be used for ordering query results 
                nameForSorting: p.Name

                // Note:
                // Both index-fields are assigned the same content (the 'Name' from the document)
            };
        });

        // Configure only the 'name' index-field for FTS
        this.index("name", "Search");
    }
}
//endregion

async function SortIndexQueryResults() {
    
    {
        //region sort_1
        const products = await session
             // Query the index
            .query({ indexName: "Products/ByUnitsInStock" })
             // Apply filtering (optional)
            .whereGreaterThan("unitsInStock", 10)
             // Call 'orderByDescending'
             // Pass the index-field by which to order the results and the ordering type
            .orderByDescending("unitsInStock", "Long")
            .all();

        // Results will be sorted by the 'unitsInStock' value in descending order,
        // with higher values listed first.
        //endregion
    }

    {
        //region sort_2
        const products = await session
            // Query the index
            .query({ indexName: "Products/BySearchName"})
            // Call 'search':
            // Pass the index-field that was configured for FTS and the term to search for.
            // Here we search for terms that start with "ch" within index-field 'name'.
            .search("name", "ch*")
            // Call 'orderBy':
            // Pass the other index-field by which to order the results.
            .orderBy("nameForSorting")
            .all();
        

        // Running the above query on the NorthWind sample data, ordering by 'NameForSorting' field,
        // we get the following order:
        // =========================================================================================
        
        // "Chai"
        // "Chang"
        // "Chartreuse verte"
        // "Chef Anton's Cajun Seasoning"
        // "Chef Anton's Gumbo Mix"
        // "Chocolade"
        // "Jack's New England Clam Chowder"
        // "Pâté chinois"
        // "Teatime Chocolate Biscuits"
        
        // While ordering by the searchable 'Name' field would have produced the following order:
        // ======================================================================================
        
        // "Chai"
        // "Chang"
        // "Chartreuse verte"
        // "Chef Anton's Cajun Seasoning"
        // "Pâté chinois"
        // "Chocolade"
        // "Teatime Chocolate Biscuits"
        // "Chef Anton's Gumbo Mix"
        // "Jack's New England Clam Chowder"
        //endregion
    }
}
