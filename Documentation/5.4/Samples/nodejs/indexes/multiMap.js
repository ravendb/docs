import { DocumentStore, AbstractJavaScriptMultiMapIndexCreationTask } from "ravendb";

const documentStore = new DocumentStore();

{
    //region multiMapClass_1
    class Dog extends Animal { }
    //endregion

    //region multiMapClass_2
    class Cat extends Animal { }
    //endregion

    //region multiMapClass_3
    class Animal {
        constructor(name) {
            this.name = name;
        }
    }
    //endregion
}

//region multiMapIndex_1
class Animals_ByName extends AbstractJavaScriptMultiMapIndexCreationTask  {
    constructor() {
        super();

        // Index field 'name' from the Cats collection
        this.map('Cats', cat => {
            return {
                name: cat.name
            };
        });

        // Index field 'name' from the Dogs collection
        this.map('Dogs', dog => {
            return {
                name: dog.name
            };
        });
    }
}
//endregion

//region multiMapIndex_2
class Smart_Search extends AbstractJavaScriptMultiMapIndexCreationTask  {
    constructor() {
        super();
       
        this.map('Companies', company => {
            return {
                id: id(company),
                content: company.Name,
                displayName: company.Name,
                collection: this.getMetadata(company)["@collection"]
            };
        });

        this.map('Products', product => {
            return {
                id: id(product),
                content: product.Name,
                displayName: product.Name,
                collection: this.getMetadata(product)["@collection"]
            };
        });

        this.map('Employees', employee => {
            return {
                id: id(employee),
                content: [employee.FirstName, employee.LastName],
                displayName: employee.FirstName + " " +  employee.LastName,
                collection: this.getMetadata(employee)["@collection"]
            };
        });

        // Mark the 'content' field with 'Search' to enable full-text search queries
        this.index("content", "Search");

        // Store fields in index so that when projecting these fields from the query
        // the data will come from the index, and not from the storage.
        this.store("id", "Yes");
        this.store("collection", "Yes");
        this.store("displayName", "Yes");
    }
}
//endregion

async function multiMapQueries() { 
        const session = documentStore.openSession();

        //region multiMapQuery_1
        const results = await session
             // Query the index
            .query({ indexName: "Animals/ByName" })
             // Look for all animals (either a cat or a dog) that are named 'Mitzy' :)
            .whereEquals("name", "Mitzy")
            .all();
        //endregion

        //region multiMapQuery_2
        const results = await session
            .query({ indexName: "Smart/Search" })
             // Search across all indexed collections
            .search("content", "Lau*")
             // Project results
            .selectFields([ "id", "displayName", "collection" ])
            .all();
    
        // Results:
        // ========
    
        for (const result of results) {
            console.log(result.collection + ": " + result.displayName);
            
            // Companies: Laughing Bacchus Wine Cellars
            // Products:  Laughing Lumberjack Lager
            // Employees: Laura Callahan
        }
        //endregion 
}
