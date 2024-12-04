import { DocumentStore, AbstractJavaScriptIndexCreationTask } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

//region index_1
class Products_BySearchName extends AbstractJavaScriptIndexCreationTask {
    constructor() {
        super();

        this.map("Products", product => {
            return {
                name: product.Name
            };
        });

        // Configure the index-field 'Name' for FTS
        this.index("name", "Search");
    }
}
//endregion

//region index_2
class NumberOfUnitsOrdered_PerCategory extends AbstractJavaScriptIndexCreationTask {
    constructor() {
        super();

        const { load } = this.mapUtils();

        this.map("Products", product => {
            return {
                category: load(product.Category, "Categories").Name,
                numberOfUnitsOrdered: product.UnitsOnOrder
            };
        });

        this.reduce(results => results.groupBy(x => x.category).aggregate(g => {
            return {
                category: g.key,
                numberOfUnitsOrdered: g.values.reduce((a, b) => a + b.numberOfUnitsOrdered, 0)
            }
        }));
    }
}
//endregion

async function GetExplanations() {
    
    {
        //region inc_1
        // Define an object that will receive the explanations results
        let explanationsResults;
        
        const products = await session
             // Query the index
            .query({ indexName: "Products/BySearchName" })
             // Call 'includeExplanations', provide an object for the explanations results
            .includeExplanations(e => explanationsResults = e)
            .search("name", "Syrup Lager")
            .all();

        // When running the above query on the RavenDB sample data
        // the results contain 3 product documents.

        // Get the score details for a specific document from 'explanationsResults'
        const id = session.advanced.getDocumentId(products[0]);
        const scoreDetails = explanationsResults.explanations[products[0].id];
        //endregion
    }

    {
        //region inc_2
        // Define an object that will receive the explanations results
        let explanationsResults;

        // Define the group key by which to get explanations results
        const explanationsOptions = { groupKey: "category" }

        const items = await session
             // Query the Map-Reduce index
            .query({ indexName: "NumberOfUnitsOrdered/PerCategory" })
             // Call 'includeExplanations', provide:
             // * Options with the defined group key
             // * An object for the explanations results
            .includeExplanations(explanationsOptions, e => explanationsResults = e)
             // Query for categories that have a total of more than a 400 units ordered
            .whereGreaterThan("numberOfUnitsOrdered", 400)
            .all();

        // Get the score details for an item in the results
        // Pass the group key (category, in this case) to 'explanations'
        const scoreDetails = explanationsResults.explanations[items[0].category];
        //endregion
    }
}

{
    //region syntax_1
    // Use this overload when querying a Map index
    query.includeExplanations(explanationsCallback)

    // Use this overload when querying a Map-Reduce index
    query.includeExplanations(options, explanationsCallback)
    //endregion

    //region syntax_2
    // The Explanations object:
    // ========================

    class Explanations {
        get explanations(): {
            [key: string]: string[]; // An explanations list per key
        };
    }
    //endregion

    //region syntax_3
    // The Explanation Options object:
    // ===============================
    {
        // The group key that was used to group by the items in the Map-Reduce index 
        groupKey; // string;
    }
    //endregion
}
