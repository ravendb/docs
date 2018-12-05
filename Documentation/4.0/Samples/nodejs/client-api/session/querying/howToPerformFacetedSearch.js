import { DocumentStore, Facet, RangeFacet, RangeBuilder } from "ravendb";

let FacetSetup;

const store = new DocumentStore();
const session = store.openSession();
const query = session.query();

{
    let facetBuilder, facet, facets, facetSetupDocumentId;
    //region facet_1
    query.aggregateBy(facetBuilder);
    query.aggregateBy(facet);
    query.aggregateBy(...facets);
    query.aggregateUsing(facetSetupDocumentId);
    //endregion

    let range, ranges, fieldName, displayName, options, path;
    query.aggregateBy(facetBuilder => {
        //region facet_7_1
        facetBuilder.byRanges(range, ...ranges);
        facetBuilder.byField(fieldName);

        facetBuilder.withDisplayName(displayName);

        facetBuilder.withOptions(options);

        facetBuilder.sumOn(path);
        facetBuilder.minOn(path);
        facetBuilder.maxOn(path);
        facetBuilder.averageOn(path);
        //endregion
    });
}

async function sample() {
    {
        //region facet_2_1
        const facetOptions = {
            termSortMode: "CountDesc"
        };

        const facet1 = new Facet();
        facet1.fieldName = "manufacturer";
        facet1.options = facetOptions;

        const facet2 = new RangeFacet();
        facet2.ranges = [ 
            "cost < 200",
            "cost between 200 and 400",
            "cost between 400 and 600",
            "cost between 600 and 800",
            "cost >= 800"
        ];
        facet2.aggregations = new Map([["Average", "cost"]]);

        const facet3 = new RangeFacet();
        facet3.ranges = [ 
            "megapixels < 3",
            "megapixels between 3 and 7",
            "megapixels between 7 and 10",
            "megapixels >= 10"
        ];

        const facets = await session
            .query({ indexName: "Camera/Costs" })
            .aggregateBy(facet1)
            .andAggregateBy(facet2)
            .andAggregateBy(facet3)
            .execute();
        //endregion
    }

    {
        //region facet_3_1
        const options = { termSortMode: "CountDesc" };

        const costBuilder = RangeBuilder.forPath("cost");
        const megapixelsBuilder = RangeBuilder.forPath("megapixels");

        const facetResult = await session
            .query({ indexName: "Camera/Costs" })
            .aggregateBy(builder => builder
                .byField("manufacturer")
                .withOptions(options))
            .andAggregateBy(builder => builder
                .byRanges(
                    costBuilder.isLessThan(200),
                    costBuilder.isGreaterThanOrEqualTo(200).isLessThan(400),
                    costBuilder.isGreaterThanOrEqualTo(400).isLessThan(600),
                    costBuilder.isGreaterThanOrEqualTo(600).isLessThan(800),
                    costBuilder.isGreaterThanOrEqualTo(800))
                .averageOn("cost"))
            .andAggregateBy(builder => builder
                .byRanges(
                    megapixelsBuilder.isLessThan(3),
                    megapixelsBuilder.isGreaterThanOrEqualTo(3).isLessThan(7),
                    megapixelsBuilder.isGreaterThanOrEqualTo(7).isLessThan(10),
                    megapixelsBuilder.isGreaterThanOrEqualTo(10)
                ))
            .execute();
        //endregion
    }

    //region facet_4_1
    const facetSetup = new FacetSetup();

    const facetManufacturer = new Facet();
    facetManufacturer.fieldName = "manufacturer";
    facetSetup.facets = [ facetManufacturer ];

    const cameraFacet = new RangeFacet();
    cameraFacet.ranges = [
        "cost < 200",
        "cost between 200 and 400",
        "cost between 400 and 600",
        "cost between 600 and 800",
        "cost >= 800"
    ];

    const megapixelsFacet = new RangeFacet();
    megapixelsFacet.ranges = [
        "megapixels < 3",
        "megapixels between 3 and 7",
        "megapixels between 7 and 10",
        "megapixels >= 10"
    ];

    facetSetup.rangeFacets = [ cameraFacet, megapixelsFacet ];

    await session.store(facetSetup, "facets/CameraFacets");
    await session.saveChanges();

    const facets = await session
        .query({ indexName: "Camera/Costs" })
        .aggregateUsing("facets/CameraFacets")
        .execute();
    //endregion

    class Camera {
    }
}
