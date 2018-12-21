import { 
    DocumentStore, 
    AbstractIndexCreationTask,
    MoreLikeThisStopWords,
    QueryData,
    PointField
} from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

class Event { }

//region spatial_3_2
class Events_ByCoordinates extends AbstractIndexCreationTask {
    constructor() {
        super();

        this.map = `docs.Events.Select(e => new {    
            Coordinates = this.CreateSpatialField(((double?) e.latitude), ((double?) e.longitude))
        })`;
    }
}
//endregion

async function spatial() {
    
        {
            //region spatial_1_0
            const results = await session
                .query(Event)
                .spatial(new PointField("latitude", "longitude"),
                    criteria => criteria.withinRadius(500, 30, 30))
                .all();
            //endregion
        }

        {
            //region spatial_2_0
            const results = await session
                .query(Event)
                .spatial(new PointField("latitude", "longitude"),
                    criteria => criteria.relatesToShape(
                        "Circle(30 30 d=500.0000)",
                        "Within"
                    ))
                .all();
            //endregion
        }

        {
            //region spatial_3_0
            const results = await session
                .query({ indexName: "Events/ByCoordinates" })
                .spatial("coordinates",
                    criteria => criteria.withinRadius(500, 30, 30))
                .all();
            //endregion
        }
 
}

