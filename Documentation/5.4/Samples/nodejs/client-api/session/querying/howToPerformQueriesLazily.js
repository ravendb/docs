import { DocumentStore, AbstractJavaScriptIndexCreationTask, Facet, RangeFacet } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

async function lazyExamples() {
    {
        //region lazy_1
        // Define a lazy query
        const lazyEmployees = session
            .query({ collection: "Employees" })
            .whereEquals("FirstName", "Robert")
             // Add a call to 'lazily'
            .lazily();

        const employees = await lazyEmployees.getValue(); // Query is executed here
        //endregion
    }

    {
        //region lazy_2
        // Define a lazy count query
        const lazyCount = session
            .query({ collection: "Employees" })
            .whereEquals("FirstName", "Robert")
             // Add a call to 'countLazily'
            .countLazily();

        const count = await lazyCount.getValue(); // Query is executed here
        //endregion
    }

    {
        //region lazy_3
        // Define a lazy suggestion query
        const lazySuggestions = session
            .query({ collection: "Products" })
            .suggestUsing(builder => builder.byField("Name", "chaig"))
             // Add a call to 'executeLazy'
            .executeLazy();

        const suggestResult = await lazySuggestions.getValue(); // Query is executed here
        const suggestions = suggestResult["Name"].suggestions;
        //endregion
    }

    {
        //region lazy_4
        // The facets definition used in the facets query:
        // ===============================================
        const categoryNameFacet = new Facet();

        categoryNameFacet.fieldName = "categoryName";
        categoryNameFacet.displayFieldName = "Product Category";

        const rangeFacet = new RangeFacet();
        rangeFacet.ranges = [
            "pricePerUnit < " + 25,
            "pricePerUnit >= " + 25 + " and pricePerUnit < " + 50,
            "pricePerUnit >= " + 50 + " and pricePerUnit < " + 100,
            "pricePerUnit >= " + 100
        ];
        rangeFacet.displayFieldName = 'Price per Unit';

        const facetsDefinition = [categoryNameFacet, rangeFacet];
        
        // The lazy factes query:
        // ======================
        const lazyFacets = session
            .query({ indexName: "Products/ByCategoryAndPrice" })
            .aggregateBy(...facetsDefinition)
             // Add a call to 'executeLazy'
            .executeLazy();

        const facets = await lazyFacets.getValue(); // Query is executed here
        
        const categoryResults = facets["Product Category"];
        const priceResults = facets["Price per Unit"];
        //endregion
    }
}

{
    //region the_index
    // The index definition used in the facets query:
    class Products_ByCategoryAndPrice extends AbstractJavaScriptIndexCreationTask {
        constructor() {
            super();

            const { load } = this.mapUtils();

            this.map("Products", product => {
                return {
                    categoryName: load(product.Category, "Categories").Name,
                    pricePerUnit: product.PricePerUnit
                }
            });
        }
    }
    //endregion
}

{
    //region syntax
    lazily();
    lazily(onEval);
    
    countLazily();
    
    executeLazy();
    //endregion
}
