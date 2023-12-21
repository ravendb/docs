import { 
    DocumentStore,
    AbstractJavaScriptIndexCreationTask
} from "ravendb";
import assert from "assert";

const store = new DocumentStore();
const session = store.openSession();

{    
    //region camera_class
    class Camera {
        constructor(
            manufacturer = '',
            cost = 0,
            megaPixels = 0,
            maxFocalLength = 0,
            unitsInStock = 0
        ) {
            Object.assign(this, {
                manufacturer,
                cost,
                megaPixels,
                maxFocalLength,
                unitsInStock
            });
        }
    }
    //endregion
    
    //region camera_index
    class Cameras_ByFeatures extends AbstractJavaScriptIndexCreationTask {
        constructor () {
            super();

            this.map("Cameras", camera => {
                return {
                    brand: camera.manufacturer,
                    price: camera.cost,
                    megaPixels: camera.megaPixels,
                    maxFocalLength: camera.maxFocalLength,
                    unitsInStock: camera.unitsInStock
                };
            });
        }
    }
    //endregion

    async function facetedSearch() {

        //region camera_sample_data
        // Creating sample data for the examples in this article:
        // ======================================================
        const bulkInsert = store.bulkInsert();

        const cameras = [
            new Camera("Sony", 100, 20.1, 200, 10),
            new Camera("Sony", 200, 29, 250, 15),
            new Camera("Nikon", 120, 22.3, 300, 2),
            new Camera("Nikon", 180, 32, 300, 5),
            new Camera("Nikon", 220, 40, 300, 20),
            new Camera("Canon", 200, 30.4, 400, 30),
            new Camera("Olympus", 250, 32.5, 600, 4),
            new Camera("Olympus", 390, 40, 600, 6),
            new Camera("Fuji", 410, 45, 700, 1),
            new Camera("Fuji", 590, 45, 700, 5),
            new Camera("Fuji", 650, 61, 800, 17),
            new Camera("Fuji", 850, 102, 800, 19)
        ];

        for (const camera of cameras) {
            await bulkInsert.store(camera);
        }

        await bulkInsert.finish();
        //endregion

        //region facets_1
        // Define a Facet:
        // ===============
        const brandFacet = new Facet();
        // Specify the index-field for which to get count of documents per unique ITEM
        // e.g. get the number of Camera documents for each unique brand
        brandFacet.fieldName = "brand";
        // Set a display name for this field in the results (optional) 
        brandFacet.displayFieldName = "Camera Brand";

        // Define a RangeFacet:
        // ====================
        const priceRangeFacet = new RangeFacet();
        // Specify ranges within an index-field in order to get count per RANGE
        // e.g. get the number of Camera documents that cost below 200, between 200 & 400, etc...
        priceRangeFacet.ranges = [
            "price < 200",
            "price >= 200 and price < 400",
            "price >= 400 and price < 600",
            "price >= 600 and price < 800",
            "price >= 800"
        ];
        // Set a display name for this field in the results (optional) 
        priceRangeFacet.displayFieldName = "Camera Price";

        const facets = [brandFacet, priceRangeFacet];
        //endregion

        //region facets_2
        const results = await session
             // Query the index
            .query({ indexName: "Cameras/ByFeatures" })
             // Call 'aggregateBy' to aggregate the data by facets
             // Pass the defined facets from above
            .aggregateBy(...facets)
            .execute();
        //endregion

        //region facets_2_rawQuery
        const results = await session.advanced
             // Query the index
             // Provide the RQL string to the rawQuery method
            .rawQuery(`from index "Cameras/ByFeatures"
                       select
                           facet(brand) as "Camera Brand",
                           facet(price < 200,
                                 price >= 200 and price < 400,
                                 price >= 400 and price < 600,
                                 price >= 600 and price < 800,
                                 price >= 800) as "Camera Price"`)
             // Execute the query
            .executeAggregation();
        //endregion

        //region facets_3
        // Define the index-field (e.g. 'price') that will be used by the range-facet in the query below 
        const range = RangeBuilder.forPath("price");
        
        const results = await session
            .query({ indexName: "Cameras/ByFeatures" })
             // Call 'aggregateBy' to aggregate the data by facets
             // Use a builder as follows:
            .aggregateBy(builder => builder
                 // Specify the index-field (e.g. 'brand') for which to get count per unique ITEM
                .byField("brand"))
                 // Set a display name for the field in the results (optional) 
                .withDisplayName("Camera Brand"))
             // Call 'andAggregateBy' to aggregate the data by also by range-facets
             // Use a builder as follows:
            .andAggregateBy(builder => builder                
                .byRanges(
                    // Specify the ranges within index field 'price' in order to get count per RANGE
                    range.isLessThan(200),
                    range.isGreaterThanOrEqualTo(200).isLessThan(400),
                    range.isGreaterThanOrEqualTo(400).isLessThan(600),
                    range.isGreaterThanOrEqualTo(600).isLessThan(800),
                    range.isGreaterThanOrEqualTo(800))
                 // Set a display name for the field in the results (optional) 
                .withDisplayName("Camera Brand"))
            .execute();
        //endregion

        //region facets_4
        // The resulting aggregations per display name will contain:
        // =========================================================

        // For the "Camera Brand" Facet:
        //     "canon"   - Count: 1
        //     "fuji"    - Count: 4
        //     "nikon"   - Count: 3
        //     "olympus" - Count: 2
        //     "sony"    - Count: 2

        // For the "Camera Price" Ranges:
        //     "Price < 200"                      - Count: 3
        //     "Price >= 200.0 and Price < 400.0" - Count: 5
        //     "Price >= 400.0 and Price < 600.0" - Count: 2
        //     "Price >= 600.0 and Price < 800.0" - Count: 1
        //     "Price >= 800.0"                   - Count: 1
        //endregion
        
        //region facets_5
        // Get facets results for index-field 'brand' using the display name specified:
        // ============================================================================
        const brandFacets = results["Camera Brand"];
        const numberOfBrands = brandFacets.values.length; // 5 unique brands

        // Get the aggregated facet value for a specific Brand:
        let facetValue = brandFacets.values[0];
        // The brand name is available in the 'range' property
        // Note: value is lower-case since the default RavenDB analyzer was used by the index
        assert.equal(facetValue.range, "canon");
        // Number of documents for 'Canon' is available in the 'count' property
        assert.equal(facetValue.count, 1);
        
        // Get facets results for index-field 'price' using the display name specified:
        // ============================================================================
        const priceFacets = results["Camera Price"];
        const numberOfRanges = priceFacets.values.length; // 5 different ranges

        // Get the aggregated facet value for a specific Range:
        facetValue = priceFacets.values[0];
        assert.equal(facetValue.range, "price < 200"); // The range string
        assert.equalfacetValue.count, 3); // Number of documents in this range
        //endregion
        
        //region facets_6
        const filteredResults = await session
            .query({ indexName: "Cameras/ByFeatures" })
             // Limit query results to the selected brands: 
            .whereIn("brand", ["Fuji", "Nikon"])
            .aggregateBy(...facets)
            .execute();
        //endregion
        
        //region facets_7
        // Define a Facet:
        // ===============
        const facet = new Facet();
        
        // Specify the index-field for which to get count of documents per unique ITEM
        facet.fieldName = "brand";

        // Set some facet options 
        // E.g.: Return top 3 brands with most items count
        const facetOptions = new FacetOptions();
        facetOptions.pageSize = 3;
        facetOptions.termSortMode = "CountDesc";

        facet.options = facetOptions;
        //endregion
        
        //region facets_8
        const results = await session
             // Query the index
            .query({ indexName: "Cameras/ByFeatures" })
             // Call 'aggregateBy' to aggregate the data by facets
             // Pass the defined facet from above
            .aggregateBy(facet)
            .execute();
        //endregion

        //region facets_8_rawQuery
        const results = await session.advanced
             // Query the index
             // Provide the RQL string to the rawQuery method
            .rawQuery(`from index "Cameras/ByFeatures"
                       select facet(brand, $p0)`)
             // Add the facet options to the "p0" parameter
            .addParameter("p0", { "termSortMode": "CountDesc", "pageSize": 3 })
             // Execute the query
            .executeAggregation();
        //endregion
        
        //region facets_9
        // Set facet options to use in the following query 
        // E.g.: Return top 3 brands with most items count
        const facetOptions = new FacetOptions();
        facetOptions.pageSize = 3;
        facetOptions.termSortMode = "CountDesc";
        
        const results = await session
             //Query the index
            .query({ indexName: "Cameras/ByFeatures" })
             // Call 'aggregateBy' to aggregate the data by facets
             // Use a builder as follows:
            .aggregateBy(builder => builder
                 // Specify the index-field (e.g. 'brand') for which to get count per unique ITEM
                .byField("brand")
                 // Call 'withOptions', pass the facets options
                .withOptions(facetOptions))
            .execute();
        //endregion
        
        //region facets_10
        // The resulting items will contain:
        // =================================
        
        // For the "brand" Facet:
        //     "fuji"    - Count: 4
        //     "nikon"   - Count: 3
        //     "olympus" - Count: 2
        
        // As requested, only 3 unique items are returned, ordered by documents count descending:
        //endregion
        
        //region facets_11
        // Get facets results for index-field 'brand':
        // ===========================================
        const brandFacets = results["brand"];
        const numberOfBrands = brandFacets.values.length; // 3 brands

        // Get the aggregated facet value for a specific Brand:
        const facetValue = brandFacets.values[0];
        // The brand name is available in the 'range' property
        // Note: value is lower-case since the default RavenDB analyzer was used by the index 
        assert.equal(facetValue.range, "fuji");
        // Number of documents for 'Fuji' is available in the 'count' property
        assert.equal(facetValue.count, 4);
        //endregion
        
        //region facets_12
        // Define a Facet:
        // ===============
        const facet = new Facet();
        facet.fieldName = "brand";

        // Define the index-fields to aggregate:
        const unitsInStockAggregationField = new FacetAggregationField();
        unitsInStockAggregationField.name = "unitsInStock";

        const priceAggregationField = new FacetAggregationField();
        priceAggregationField.name = "price";

        const megaPixelsAggregationField = new FacetAggregationField();
        megaPixelsAggregationField.name = "megaPixels";

        const maxFocalLengthAggregationField = new FacetAggregationField();
        maxFocalLengthAggregationField.name = "maxFocalLength";

        // Define the aggregation operations:
        facet.aggregations.set("Sum", [unitsInStockAggregationField]);
        facet.aggregations.set("Average", [priceAggregationField]);
        facet.aggregations.set("Min", [priceAggregationField]);
        facet.aggregations.set("Max", [megaPixelsAggregationField, maxFocalLengthAggregationField]);

        // Define a RangeFacet:
        // ====================
        const rangeFacet = new RangeFacet();
        rangeFacet.ranges = [
            "price < 200",
            "price >= 200 and price < 400",
            "price >= 400 and price < 600",
            "price >= 600 and price < 800",
            "price >= 800"
        ];

        // Define the aggregation operations:
        rangeFacet.aggregations.set("Sum", [unitsInStockAggregationField]);
        rangeFacet.aggregations.set("Average", [priceAggregationField]);
        rangeFacet.aggregations.set("Min", [priceAggregationField]);
        rangeFacet.aggregations.set("Max", [megaPixelsAggregationField, maxFocalLengthAggregationField]);
        
        const facetsWithAggregations = [facet, rangeFacet];
        //endregion
        
        //region facets_13
        const results = await session
             // Query the index
            .query({ indexName: "Cameras/ByFeatures" })
             // Call 'aggregateBy' to aggregate the data by facets
             // Pass the defined facet from above
            .aggregateBy(...facetsWithAggregations)
            .execute();
        //endregion

        //region facets_13_rawQuery
        const results = await session.advanced
             // Query the index
             // Provide the RQL string to the rawQuery method
            .rawQuery(`from index "Cameras/ByFeatures"
                       select
                           facet(brand,
                                 sum(unitsInStock),
                                 avg(price),
                                 min(price),
                                 max(megaPixels),
                                 max(maxFocalLength)),
                           facet(price < $p0,
                                 price >= $p1 and price < $p2,
                                 price >= $p3 and price < $p4,
                                 price >= $p5 and price < $p6,
                                 price >= $p7,
                                 sum(unitsInStock),
                                 avg(price),
                                 min(price),
                                 max(megaPixels),
                                 max(maxFocalLength))`)
             // Add the parameters' values
            .addParameter("p0", 200)
            .addParameter("p1", 200)
            .addParameter("p2", 400)
            .addParameter("p3", 400)
            .addParameter("p4", 600)
            .addParameter("p5", 600)
            .addParameter("p6", 800)
            .addParameter("p7", 800)
             // Execute the query
            .executeAggregation();
        //endregion
        
        //region facets_14
        // Define the index-field (e.g. 'price') that will be used by the range-facet in the query below 
        const range = RangeBuilder.forPath("price");

        const results = await session
            .query({ indexName: "Cameras/ByFeatures" })
             // Call 'aggregateBy' to aggregate the data by facets
             // Use a builder as follows:
            .aggregateBy(builder => builder
                 // Specify the index-field (e.g. 'brand') for which to get count per unique ITEM
                .byField("brand")
                 // Specify the aggregations per the brand facet:
                .sumOn("unitsInStock")
                .averageOn("price")
                .minOn("price")
                .maxOn("megaPixesls")
                .maxOn("maxFocalLength"))
             // Call 'andAggregateBy' to aggregate the data by also by range-facets
             // Use a builder as follows:
            .andAggregateBy(builder => builder
                .byRanges(
                    // Specify the ranges within index field 'price' in order to get count per RANGE
                    range.isLessThan(200),
                    range.isGreaterThanOrEqualTo(200).isLessThan(400),
                    range.isGreaterThanOrEqualTo(400).isLessThan(600),
                    range.isGreaterThanOrEqualTo(600).isLessThan(800),
                    range.isGreaterThanOrEqualTo(800))
                 // Specify the aggregations per the price range:
                .sumOn("unitsInStock")
                .averageOn("price")
                .minOn("price")
                .maxOn("megaPixesls")
                .maxOn("maxFocalLength"))
            .execute();
        //endregion
        
        //region facets_15
        // The resulting items will contain (Showing partial results):
        // ===========================================================
        
        // For the "brand" Facet:
        //     "canon" Count:1, Sum: 30, Name: unitsInStock
        //     "canon" Count:1, Min: 200, Average: 200, Name: price
        //     "canon" Count:1, Max: 30.4, Name: megaPixels
        //     "canon" Count:1, Max: 400, Name: maxFocalLength
        //
        //     "fuji" Count:4, Sum: 42, Name: unitsInStock
        //     "fuji" Count:4, Min: 410, Name: price
        //     "fuji" Count:4, Max: 102, Name: megaPixels
        //     "fuji" Count:4, Max: 800, Name: maxFocalLength
        //     
        //     etc.....
        
        // For the "price" Ranges:
        //     "Price < 200" Count:3, Sum: 17, Name: unitsInStock
        //     "Price < 200" Count:3, Min: 100, Average: 133.33, Name: price
        //     "Price < 200" Count:3, Max: 32, Name: megaPixels
        //     "Price < 200" Count:3, Max: 300, Name: maxFocalLength
        //
        //     "Price < 200 and Price > 400" Count:5, Sum: 75, Name: unitsInStock
        //     "Price < 200 and Price > 400" Count:5, Min: 200, Average: 252, Name: price
        //     "Price < 200 and Price > 400" Count:5, Max: 40, Name: megaPixels
        //     "Price < 200 and Price > 400" Count:5, Max: 600, Name: maxFocalLength
        //     
        //     etc.....
        //endregion
        
        //region facets_16
        // Get results for the 'brand' Facets:
        // ==========================================
        const brandFacets = results["brand"];

        // Get the aggregated facet value for a specific brand:
        let facetValue = brandFacets.values[0];
        // The brand name is available in the 'range' property:
        assert.equal(facetValue.range, "canon");
        // The index-field on which aggregation was done is in the 'name' property:
        assert.equal(facetValue.name, "unitsInStock");
        // The requested aggregation result:
        assert.equal(facetValue.sum, 30);

        // Get results for the 'price' RangeFacets:
        // =======================================
        const priceRangeFacets = results["price"];

        // Get the aggregated facet value for a specific Brand:
        facetValue = priceRangeFacets.values[0];
        // The range string is available in the 'range' property:
        assert.equal(facetValue.range, "price < 200");
        // The index-field on which aggregation was done is in the 'name' property:
        assert.equal(facetValue.name, "unitsInStock");
        // The requested aggregation result:
        assert.equal(facetValue.sum, 17);
        //endregion
        
        //region facets_17
        // Create a FacetSetup object:
        // ===========================
        const facetSetup = new FacetSetup();

        // Provide the ID of the document in which the facet setup will be stored.
        // This is optional -
        // if not provided then the session will assign an ID for the stored document.
        facetSetup.id = "customDocumentID";

        // Define Facets and RangeFacets to query by:
        const facet = new Facet();
        facet.fieldName = 'brand';

        facetSetup.facets = [facet];

        const rangeFacet = new RangeFacet();
        rangeFacet.ranges = [
            "megaPixels < 20",
            "megaPixels >= 20 and megaPixels < 30",
            "megaPixels >= 30 and megaPixels < 50",
            "megaPixels >= 50"
        ];

        facetSetup.rangeFacets = [rangeFacet];

        // Store the facet setup document and save changes:
        // ================================================
        await session.store(facetSetup);
        await session.saveChanges();

        // The document will be stored under the 'FacetSetups' collection
        //endregion
        
        //region facets_18
        const results = await session
            // Query the index
            .query({ indexName: "Cameras/ByFeatures" })
            // Call 'aggregateUsing'
            // Pass the ID of the document that contains your facets setup
            .aggregateUsing("customDocumentID")
            .execute();
        //endregion

        //region facets_18_rawQuery
        const results = await session.advanced
             // Query the index
             // Provide the RQL string to the rawQuery method
            .rawQuery(`from index "Cameras/ByFeatures"
                       select facet(id("customDocumentID"))`)
             // Execute the query
            .executeAggregation();
        //endregion
        
        //region syntax_1
        // Aggregate data by Facets:
        aggregateBy(facet);
        aggregateBy(...facet);
        aggregateBy(action);

        // Aditional aggregation for another Facet/RangeFacet (use with fluent API) 
        andAggregateBy(facet);
        andAggregateBy(builder);

        // Aggregate data by Facets stored in a document 
        aggregateUsing(facetSetupDocumentId);
        //endregion
        
        
        
        {
            //region syntax_2
            class Facet {
                fieldName;
            }
            //endregion

            //region syntax_3
            class RangeFacet {
                ranges;
            }
            //endregion

            //region syntax_4
            class FacetBase {
                displayFieldName;
                options;
                aggregations; // "None" | "Max" | "Min" | "Average" | "Sum"
            }
            //endregion
        }
        
        //region syntax_5
        byField(fieldName);
        byRanges(range, ...ranges);
        
        withDisplayName(displayName);
        withOptions(options);
        
        sumOn(path);
        sumOn(path, displayName);
        
        minOn(path);
        minOn(path, displayName);
        
        maxOn(path);
        maxOn(path, displayName);
        
        averageOn(path);
        averageOn(path, displayName);
        //endregion
        
        //region syntax_6
        class FacetOptions {
            termSortMode;
            includeRemainingTerms; 
            start;
            pageSize;
        }
        //endregion
    }    
}
