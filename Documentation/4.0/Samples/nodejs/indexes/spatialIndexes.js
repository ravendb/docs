import { 
    AbstractIndexCreationTask, 
    DocumentStore, 
    IndexDefinition, 
    PutIndexesOperation,
    IndexDefinitionBuilder,
    SpatialOptionsFactory
} from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

//region spatial_search_enhancements_3
const spatialOptsFactory = new SpatialOptionsFactory();

spatialOptsFactory.geography();

spatialOptsFactory.cartesian();
//endregion

let circleRadiusUnits, maxTreeLevel, bounds;

//region spatial_search_enhancements_4
spatialOptsFactory.geography().defaultOptions([circleRadiusUnits]);
spatialOptsFactory.geography().boundingBoxIndex([circleRadiusUnits]);
spatialOptsFactory.geography().geohashPrefixTreeIndex(maxTreeLevel, [circleRadiusUnits]);
spatialOptsFactory.geography().quadPrefixTreeIndex(maxTreeLevel, [circleRadiusUnits]);
//endregion

//region spatial_search_enhancements_5
spatialOptsFactory.cartesian().boundingBoxIndex();
spatialOptsFactory.cartesian().quadPrefixTreeIndex(maxTreeLevel, bounds);
//endregion

/*
//region spatial_search_0
object CreateSpatialField(double? lat, double? lng);

object CreateSpatialField(string shapeWkt);
//endregion
*/

//region spatial_search_2
class EventWithWKT {
    constructor(id, name, wkt) {
        this.id = id;
        this.name = name;
        this.wkt = wkt;
    }
}

class EventsWithWKT_ByNameAndWKT extends AbstractIndexCreationTask {
    constructor() {
        super();

        this.map = `docs.EventWithWKTs.Select(e => new {     
            name = e.name,     
            wkt = this.CreateSpatialField(e.wkt) 
        })`; 
    }
}
//endregion

//region spatial_search_1
class Event {
    constructor(id, name, latitude, longitude) {
        this.id = id;
        this.name = name;
        this.latitude = latitude;
        this.longitude = longitude;
    }
}

class Events_ByNameAndCoordinates extends AbstractIndexCreationTask {
    constructor() {
        super();
        this.map = `docs.Events.Select(e => new {     
            name = e.name,     
            coordinates = this.CreateSpatialField(((double?) e.latitude), ((double?) e.longitude)) 
        })`;
    }
}
//endregion

//region spatial_search_3
class Events_ByNameAndCoordinates_Custom extends AbstractIndexCreationTask {
    constructor() {
        super();

        this.map = "docs.Events.Select(e => new { " +
            "    name = e.name, " +
            "    coordinates = this.CreateSpatialField(((double?) e.latitude), ((double?) e.longitude)) " +
            "})";

        this.spatial("coordinates", f => f.cartesian().boundingBoxIndex());
    }
}
//endregion
