import { 
    DocumentStore, 
    AbstractIndexCreationTask,
    QueryData
} from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

{
    //region suggestions_index_1
    class Products_ByName extends AbstractJavaScriptIndexCreationTask {
        constructor() {
            super();
            
            this.map("Products", p => {
                return {
                    ProductName: p.Name
                };
            });

            // Configure index-field 'ProductName' for suggestions
            this.suggestion("ProductName");

            // Optionally: set 'Search' on this field
            // This will split the field content into multiple terms allowing for a full-text search
            this.index("ProductName", "Search");
        }
    }
    //endregion
    
    //region suggestions_index_2
    class Companies_ByNameAndByContactName extends AbstractJavaScriptIndexCreationTask {
        constructor() {
            super();

            this.map("Companies", p => {
                return {
                    CompanyName: p.Name,
                    ContactName: p.Contact.Name
                };
            });

            // Configure the index-fields for suggestions
            this.suggestion("CompanyName");
            this.suggestion("ContactName");

            // Optionally: set 'Search' on the index-fields
            // This will split the fields' content into multiple terms allowing for a full-text search
            this.index("CompanyName", "Search");
            this.index("ContactName", "Search");
        }
    }
    //endregion

    async function suggestions() {
        
        {
            //region suggestions_2
            // This query on index 'Products/ByName' has NO resulting documents
            const products = await session
                .query({ indexName: "Products/ByName" })
                .search("ProductName", "chokolade")
                .all();
            //endregion
        }

        {
            //region suggestions_3
            // Query the index for suggested terms for single term:
            // ====================================================
            
            const suggestions = await session
                 // Query the index   
                .query({ indexName: "Products/ByName" })
                 // Call 'suggestUsing'
                .suggestUsing(x => x
                     // Request to get terms from index-field 'ProductName' that are similar to 'chokolade' 
                    .byField("ProductName", "chokolade"))
                .execute();
            //endregion
        }

        {
            //region suggestions_4
            // The resulting suggested terms:
            // ==============================
            
            console.log("Suggested terms in index-field 'ProductName' that are similar to 'chokolade':");
            suggestions["ProductName"].suggestions.forEach(suggestedTerm => {
                console.log("\t" + suggestedTerm);
            });

            // Suggested terms in index-field 'ProductName' that are similar to 'chokolade':
            //     schokolade
            //     chocolade
            //     chocolate
            //endregion
        }

        {
            //region suggestions_5
            // Query the index for suggested terms for multiple terms:
            // =======================================================
            
            const suggestions = await session
                 // Query the index   
                .query({ indexName: "Products/ByName" })
                 // Call 'suggestUsing'
                .suggestUsing(x => x
                     // Request to get terms from index-field 'ProductName' that are similar to 'chokolade' OR 'syrop'
                    .byField("ProductName", ["chokolade", "syrop"]))
                .execute();
            //endregion
        }

        {
            //region suggestions_6
            // The resulting suggested terms:
            // ==============================
            
            // Suggested terms in index-field 'ProductName' that are similar to 'chokolade' OR to 'syrop':
            //     schokolade
            //     chocolade
            //     chocolate
            //     sirop
            //     syrup
            //endregion
        }

        {
            //region suggestions_7
            // Query the index for suggested terms in multiple fields:
            // =======================================================

            const suggestions = await session
                 // Query the index   
                .query({ indexName: "Companies/ByNameAndByContactName" })
                 // Call 'suggestUsing' to get suggestions for terms that are 
                 // similar to 'chese' in first index-field (e.g. 'CompanyName') 
                .suggestUsing(x => x.byField("CompanyName", "chese"))
                 // Call 'andSuggestUsing' to get suggestions for terms that are 
                 // similar to 'frank' in an additional index-field (e.g. 'ContactName')
                .andSuggestUsing(x => x.byField("ContactName", "frank"))
                .execute();
            //endregion
        }

        {
            //region suggestions_8
            // The resulting suggested terms:
            // ==============================
            
            // Suggested terms in index-field 'CompanyName' that is similar to 'chese':
            //     cheese
            //     chinese
            
            // Suggested terms in index-field 'ContactName' that are similar to 'frank':
            //     fran
            //     franken
            //endregion
        }

        {
            //region suggestions_9
            // Query the index for suggested terms - customize options and display name:
            // =========================================================================

            const suggestions = await session
                 // Query the index   
                .query({ indexName: "Products/ByName" })
                 // Call 'suggestUsing'
                .suggestUsing(x => x
                    .byField("ProductName", "chokolade")
                     // Customize suggestions options
                    .withOptions({
                        accuracy: 0.3,
                        pageSize: 5,
                        distance: "NGram",
                        sortMode: "Popularity"
                    })
                    // Customize display name for results
                    .withDisplayName("SomeCustomName"))
                .execute();
            //endregion
        }

        {
            //region suggestions_10
            // The resulting suggested terms:
            // ==============================

            console.log("Suggested terms:");
            // Results are available under the custom name entry
            suggestions["SomeCustomName"].suggestions.forEach(suggestedTerm => {
                console.log("\t" + suggestedTerm);
            });
            
            // Suggested terms:
            //     chocolade
            //     schokolade
            //     chocolate
            //     chowder
            //     marmalade
            //endregion
        }
    }
}
