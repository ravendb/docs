import { DocumentStore, AbstractJavaScriptIndexCreationTask } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();



//region spatial_index_1
// Define an index with a spatial field
class Events_ByNameAndCoordinates extends AbstractJavaScriptIndexCreationTask {
    constructor() {
        super();
        const { createSpatialField } = this.mapUtils();

        this.map('events', e => {
            return {
                name: e.Name,
                // Call 'createSpatialField' to create a spatial index-field
                // Field 'coordinates' will be composed of lat & lng supplied from the document
                coordinates: createSpatialField(
                    e.latitude,
                    e.longitude
                )
                
                // Documents can be retrieved
                // by making a spatial query on the 'coordinates' index-field
            };
        });
    }
}

class Event {
    constructor(id, name, latitude, longitude) {
        this.id = id;
        this.name = name;
        this.latitude = latitude
        this.longitude = longitude;
    }
}

//endregion

//region spatial_index_2
// Define an index with a spatial field
class EventsWithWKT_ByNameAndWKT extends AbstractJavaScriptIndexCreationTask {
    constructor() {
        super();
        const { createSpatialField } = this.mapUtils();

        this.map('events', e => {
            return {
                name: e.Name,
                // Call 'createSpatialField' to create a spatial index-field
                // Field 'wkt' will be composed of the WKT string supplied from the document
                wkt: createSpatialField(e.wkt)

                // Documents can be retrieved by
                // making a spatial query on the 'wkt' index-field
            };
        });
    }
}

class EventWithWKT {
    constructor(id, name, wkt) {
        this.id = id;
        this.name = name;
        this.wkt = wkt;
    }
}
//endregion

//region spatial_index_3
class Events_ByNameAndCoordinates_Custom extends AbstractJavaScriptIndexCreationTask {
    constructor() {
        super();
        const { createSpatialField } = this.mapUtils();

        this.map('events', e => {
            return {
                name: e.Name,
                // Define a spatial index-field
                coordinates: createSpatialField(
                    e.latitude,
                    e.longitude
                )

                // Documents can be retrieved
                // by making a spatial query on the 'coordinates' index-field
            };
        });

        // Set the spatial indexing strategy for the spatial field 'coordinates' 
        this.spatial("coordinates", factory => factory.cartesian().boundingBoxIndex());
    }
}
//endregion

{
    //region syntax

    //region spatial_syntax_1
    createSpatialField(lat, lng);
    createSpatialField(wkt);
    //endregion
    
    //region spatial_syntax_2
    class SpatialOptionsFactory {
        geography(): GeographySpatialOptionsFactory;
        cartesian(): CartesianSpatialOptionsFactory;
    }
    //endregion
    
    //region spatial_syntax_3
    defaultOptions(circleRadiusUnits);
    boundingBoxIndex(circleRadiusUnits);
    geohashPrefixTreeIndex(maxTreeLevel, circleRadiusUnits);
    quadPrefixTreeIndex(maxTreeLevel, circleRadiusUnits);
    //endregion
    
    //region spatial_syntax_4
    boundingBoxIndex(): SpatialOptions;
    quadPrefixTreeIndex(maxTreeLevel, bounds);

    class SpatialBounds {
        minX; // number
        maxX; // number
        minY; // number
        maxY; // number
    }
    //endregion
    
    //endregion
}
