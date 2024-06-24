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

async function spatialIndexQuery() {
    
    {
        //region spatial_query_1
        // Define a spatial query on index 'Events/ByNameAndCoordinates'
        const employeesWithinRadius = await session
            .query({ indexName: "Events/ByNameAndCoordinates"})
             // Call 'spatial' method
            .spatial(
                /// Pass the spatial index-field containing the spatial data
                "coordinates",
                // Set the geographical area in which to search for matching documents
                // Call 'withinRadius', pass the radius and the center points coordinates  
                criteria => criteria.withinRadius(20, 47.623473, -122.3060097))
            .all();

        // The query returns all matching Event entities
        // that are located within 20 kilometers radius
        // from point (47.623473 latitude, -122.3060097 longitude).
        //endregion
    }

    {
        //region spatial_query_2
        // Define a spatial query on index 'EventsWithWKT/ByNameAndWKT'
        const employeesWithinShape = await session
            .query({ indexName: "EventsWithWKT/ByNameAndWKT" })
             // Call 'spatial' method
            .spatial(
                // Pass the spatial index-field containing the spatial data
                "wkt",
                // Set the geographical search criteria, call 'relatesToShape'
                criteria => criteria.relatesToShape(
                    // Specify the WKT string
                    `POLYGON ((
                           -118.6527948 32.7114894,
                           -95.8040242 37.5929338,
                           -102.8344151 53.3349629,
                           -127.5286633 48.3485664,
                           -129.4620208 38.0786067,
                           -118.7406746 32.7853769,
                           -118.6527948 32.7114894
                    ))`,
                    // Specify the relation between the WKT shape and the documents spatial data
                    "Within"
                ))
            .all();

        // The query returns all matching Event entities
        // that are located within the specified polygon.
        //endregion
    }

    {
        //region spatial_query_3
        // Define a spatial query on index 'Events_ByNameAndCoordinates'
        const employeesSortedByDistance = await session
            .query({ indexName: "Events/ByNameAndCoordinates" })
             // Filter results by geographical criteria
            .spatial(
                "coordinates",
                criteria => criteria.withinRadius(20, 47.623473, -122.3060097))
             // Sort results, call 'orderByDistance'
            .orderByDistance(
                // Pass the spatial index-field containing the spatial data
                "coordinates",
                // Sort the results by their distance from this point: 
                47.623473, -122.3060097)
            .all();

        // Return all matching Event entities located within 20 kilometers radius
        // from point (47.623473 latitude, -122.3060097 longitude).

        // Sort the results by their distance from a specified point,
        // the closest results will be listed first.
        //endregion
    }
}

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
