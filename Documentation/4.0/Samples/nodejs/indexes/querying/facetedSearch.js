import { 
    DocumentStore, 
    AbstractIndexCreationTask,
    Facet,
    RangeFacet,
    FacetSetup
} from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

class Camera { }

//region step_2
class Cameras_ByManufacturerModelCostDateOfListingAndMegapixels extends AbstractIndexCreationTask {
    constructor() {
        super();

        this.map = `from camera in docs.Cameras select new {   
            camera.manufacturer,   
            camera.model,   
            camera.cost,   
            camera.dateOfListing,   
            camera.megapixels
        }`;
    }
}
//endregion

async function step1() {
    {
        //region step_1
        const facet1 = new Facet();
        facet1.fieldName = "manufacturer";

        const facet2 = new RangeFacet();
        facet2.ranges = [ 
            "cost <= 200",
            "cost between 200 and 400",
            "cost between 400 and 600",
            "cost between 600 and 800",
            "cost >= 800"
        ];

        const facet3 = new RangeFacet();
        facet3.ranges = [ 
            "megapixels < 3",
            "megapixels between 3 and 7",
            "megapixels between 7 and 10",
            "megapixels >= 10"
        ];

        const facets = [ facet1 ];
        const rangeFacets = [ facet2, facet3 ];

        //endregion

        //region step_4_0
        const facetSetup = new FacetSetup();
        facetSetup.facets = facets;
        facetSetup.rangeFacets = rangeFacets;

        await session.store(facetSetup, "facets/CameraFacets");
        //endregion
    }
}

async function step3() {
    const facets = [];

    //region step_3_0
    const facetResults = await session
        .query({ indexName: "Cameras/ByManufacturerModelCostDateOfListingAndMegapixels" })
        .whereBetween("cost", 100, 300)
        .aggregateBy(facets)
        .execute();
    //endregion
}

async function step4() {
    {
        //region step_4_1
        const facetResults = await session
            .query({ indexName: "Cameras/ByManufacturerModelCostDateOfListingAndMegapixels" })
            .whereBetween("cost", 100, 300)
            .aggregateUsing("facets/CameraFacets")
            .execute();
        //endregion
    }
}

