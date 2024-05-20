import { DocumentStore,  AbstractJavaScriptIndexCreationTask  } from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

//region index_0
class Products_ByUnitsInStock extends AbstractJavaScriptIndexCreationTask  {
    constructor() {
        super();

        this.map("Products", p => ({
            UnitsInStock: p.UnitsInStock
        }));
    }
}
//endregion

//region index_1
// A fanout index - creating MULTIPLE index-entries per document:
// ==============================================================

class Orders_ByProductName extends AbstractJavaScriptIndexCreationTask {
    constructor() {
        super();

        this.map("Orders", order => {
            return order.Lines.map(line => {
                return {
                    ProductName: line.ProductName
                };
            });
        });
    }
}
//endregion

async function paging() {

    {
        //region paging_0
        // A simple query without paging:
        // ==============================
        
        const allResults = await session
            .query({ indexName: "Products/ByUnitsInStock" })
            .whereGreaterThan("UnitsInStock", 10)
            .all();

        // Executing the query on the Northwind sample data
        // will result in all 47 Product documents that match the query predicate.
        //endregion
    }
    {
        //region paging_1
        // Retrieve only the 3'rd page - when page size is 10:
        // ===================================================

        // Define an output param for getting the query stats
        let stats;
        
        const thirdPageResults = await session
            .query({ indexName: "Products/ByUnitsInStock" })
             // Get the query stats if you wish to know the TOTAL number of results
            .statistics(s => stats = s)
             // Apply some filtering condition as needed
            .whereGreaterThan("UnitsInStock", 10)
             // Call 'Skip', pass the number of items to skip from the beginning of the result set
             // Skip the first 20 resulting documents
            .skip(20)
             // Call 'Take' to define the number of documents to return
             // Take up to 10 products => so 10 is the "Page Size"
            .take(10)
            .all();

        // When executing this query on the Northwind sample data,
        // results will include only 10 Product documents ("products/45-A" to "products/54-A")

        const totalResults = stats.totalResults;
        
        // While the query returns only 10 results,
        // `totalResults` will hold the total number of matching documents (47).
        //endregion
    }
    {
        //region paging_2
        // Query for all results - page by page:
        // =====================================
        
        const PAGE_SIZE = 10;
        let pageNumber = 0;
        let pagedResults;

        do {
            pagedResults = await session
                .query({ indexName: "Products/ByUnitsInStock" })
                 // Apply some filtering condition as needed
                .whereGreaterThan("UnitsInStock", 10)
                 // Skip the number of results that were already fetched
                .skip(pageNumber * PAGE_SIZE)
                 // Request to get 'pageSize' results
                .take(PAGE_SIZE)
                .all();

            pageNumber++;

            // Make any processing needed with the current paged results here
            // ... 
        }
        while (pagedResults.length > 0); // Fetch next results
        //endregion
    }
    {
        //region paging_3
        let pagedResults;
        let stats;
        
        let totalResults = 0;
        let totalUniqueResults = 0;
        let skippedResults = 0;
        
        let pageNumber = 0;
        const PAGE_SIZE = 10;

        do {
            pagedResults = await session
                .query({ indexName: "Products/ByUnitsInStock" })
                .statistics(s => stats = s)
                .whereGreaterThan("UnitsInStock", 10)
                 // Define a projection
                .selectFields(["Category", "Supplier"])
                 // Call Distinct to remove duplicate projected results
                .distinct()
                 // Add the number of skipped results to the "start location"  
                .skip((pageNumber * PAGE_SIZE) + skippedResults)
                 // Define how many items to return
                .take(PAGE_SIZE)
                .all();

            totalResults = stats.totalResults;         // Number of total matching documents (includes duplicates)
            skippedResults += stats.skippedResults;    // Number of duplicate results that were skipped
            totalUniqueResults += pagedResults.length; // Number of unique results returned in this server call 
            
            pageNumber++;
        }
        while (pagedResults.length > 0); // Fetch next results

        // When executing the query on the Northwind sample data:
        // ======================================================
        
        // The total matching results reported in the stats is 47 (totalResults),
        // but the total unique objects returned while paging the results is only 29 (totalUniqueResults)
        // due to the 'distinct' usage which removes duplicates.
        
        // This is solved by adding the skipped results count to skip().
        //endregion
    }
    {
        //region paging_4
        let pagedResults;
        let stats;

        let totalResults = 0;
        let totalUniqueResults = 0;
        let skippedResults = 0;

        let pageNumber = 0;
        const PAGE_SIZE = 50;

        do {
            pagedResults = await session
                .query({ indexName: "Orders/ByProductName" })
                .statistics(s => stats = s)
                 // Add the number of skipped results to the "start location"  
                .skip((pageNumber * PAGE_SIZE) + skippedResults)
                .take(PAGE_SIZE)
                .all();

            totalResults = stats.totalResults;
            skippedResults += stats.skippedResults;
            totalUniqueResults += pagedResults.length;
            
            pageNumber++;
        }
        while (pagedResults.length > 0); // Fetch next results

        // When executing the query on the Northwind sample data:
        // ======================================================
        
        // The total results reported in the stats is 2155 (totalResults),
        // which represent the multiple index-entries generated as defined by the fanout index.
        
        // By adding the skipped results count to the Skip() method,
        // we get the correct total unique results which is 830 Order documents.
        //endregion
    }
}

