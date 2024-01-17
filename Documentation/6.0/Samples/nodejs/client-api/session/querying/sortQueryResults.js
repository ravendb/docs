import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

async function sortQueryResults() {
    {
        //region sort_1
        const products = await session
             // Make a dynamic query on the 'products' collection    
            .query({ collection: "products" })
             // Apply filtering (optional)
            .whereGreaterThan("UnitsInStock", 10)
             // Call 'orderBy'
             // Pass the document-field by which to order the results and the ordering type
            .orderBy("UnitsInStock", "Long")
            .all(); 

        // Results will be sorted by the 'UnitsInStock' value in ascending order,
        // with smaller values listed first.
        //endregion
    }

    {
        //region sort_2
        const products = await session
            .query({ collection: "products" })
             // Apply filtering
            .whereLessThan("UnitsInStock", 5)
            .orElse()
            .whereEquals("Discontinued", true)
             // Call 'orderByScore'
            .orderByScore()
            .all();

        // Results will be sorted by the score value
        // with best matching documents (higher score values) listed first.
        //endregion
    }

    {
        //region sort_3
        const products = await session
            .query({ collection: "products" })
            .whereGreaterThan("UnitsInStock", 10)
             // Call 'randomOrdering'
            .randomOrdering()
             // An optional seed can be passed, e.g.:
             // .randomOrdering("someSeed")
            .all();

        // Results will be randomly ordered.
        //endregion
    }

    {
        //region sort_4
        const numberOfProductsPerCategory = await session
            .query({ collection: "products" })
             // Group by category
            .groupBy("Category")
            .selectKey("Category")
             // Count the number of product documents per category
            .selectCount()
             // Order by the count value
            .orderBy("count", "Long")
            .all();

        // Results will contain the number of Product documents per category
        // ordered by that count in ascending order.
        //endregion
    }

    {
        //region sort_5
        const numberOfUnitsInStockPerCategory = await session
            .query({ collection: "products" })
             // Group by category
            .groupBy("Category")
            .selectKey("Category")
             // Sum the number of units in stock per category
            .selectSum(new GroupByField("UnitsInStock", "sum"))
             // Order by the sum value
            .orderBy("sum", "Long")
            .all();

        // Results will contain the total number of units in stock per category
        // ordered by that number in ascending order.
        //endregion
    }

    {
        //region sort_6
        const products = await session
            .query({ collection: "products" })
             // Call 'OrderBy', order by field 'QuantityPerUnit'
             // Pass a second param, requesting to order the text alphanumerically
            .orderBy("QuantityPerUnit", "AlphaNumeric")
            .all();
        //endregion

        //region sort_6_results
        // Running the above query on the NorthWind sample data,
        // would produce the following order for the QuantityPerUnit field:
        // ================================================================
        
        // "1 kg pkg."
        // "1k pkg."
        // "2 kg box."
        // "4 - 450 g glasses"
        // "5 kg pkg."
        // ...
        
        // While running with the default Lexicographical ordering would have produced:
        // ============================================================================
        
        // "1 kg pkg."
        // "10 - 200 g glasses"
        // "10 - 4 oz boxes"
        // "10 - 500 g pkgs."
        // "10 - 500 g pkgs."
        // ...
        //endregion
    }

    {
        //region sort_7
        const products = await session
            .query({ collection: "products" })
            .whereGreaterThan("UnitsInStock", 10)
             // Apply the primary sort by 'UnitsInStock'
            .orderByDescending("UnitsInStock", "Long")
             // Apply a secondary sort by the score
            .orderByScore()
             // Apply another sort by 'Name'
            .orderBy("Name")
            .all();

        // Results will be sorted by the 'UnitsInStock' value (descending),
        // then by score,
        // and then by 'Name' (ascending).
        //endregion
    }

    {
        //region sort_8
        const products = await session
            .query({ collection: "products" })
            .whereGreaterThan("UnitsInStock", 10)
             // Order by field 'UnitsInStock', pass the name of your custom sorter class
            .orderBy("UnitsInStock", { sorterName: "MySorter" })
            .all();

        // Results will be sorted by the 'UnitsInStock' value
        // according to the logic from 'MySorter' class
        //endregion
    }

    {
        //region get_score_from_metadata
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
        const score = metadata[CONSTANTS.Documents.Metadata.INDEX_SCORE];
        //endregion
    }
}

{
    //region syntax
    // orderBy overloads:
    orderBy(field);
    orderBy(field, ordering);
    orderBy(field, options);

    // orderByDescending overloads:
    orderByDescending(field);
    orderByDescending(field, ordering);
    orderByDescending(field, options);
    //endregion
}
