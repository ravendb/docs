import { DocumentStore, PointField, WktField } from "ravendb";

let fieldName, 
    clause, 
    field, 
    shapeWkt, 
    relation, 
    distErrorPercent,
    latitude,
    longitude,
    wkt,
    radius,
    radiusUnits;

class House {
    constructor(name, latitude, longitude) {
        this.name = name;
        this.latitude = latitude;
        this.longitude = longitude;
    }
}

const documentStore = new DocumentStore();
const session = documentStore.openSession();

{
    const query = session.query();
    //region spatial_1
    query.spatial(fieldName, clause);
    query.spatial(field, clause);
    //endregion
}

{
    /*
    //region spatial_2
    import { PointField, WktField } from "ravendb";

    new PointField(latitude, longitude);

    new WktField(wkt);
    //endregion
    */
}

{
    const query = session.query()
        .spatial("field", (spatialCriteriaFactory) => {
            //region spatial_3
            spatialCriteriaFactory.relatesToShape(shapeWkt, relation);
            spatialCriteriaFactory.relatesToShape(shapeWkt, relation, distErrorPercent);

            spatialCriteriaFactory.intersect(shapeWkt);
            spatialCriteriaFactory.intersect(shapeWkt, distErrorPercent) ;

            spatialCriteriaFactory.contains(shapeWkt);
            spatialCriteriaFactory.contains(shapeWkt, distErrorPercent);

            spatialCriteriaFactory.disjoint(shapeWkt);
            spatialCriteriaFactory.disjoint(shapeWkt, distErrorPercent);

            spatialCriteriaFactory.within(shapeWkt);

            spatialCriteriaFactory.within(shapeWkt, distErrorPercent);

            spatialCriteriaFactory.withinRadius(radius, latitude, longitude);
            spatialCriteriaFactory.withinRadius(radius, latitude, longitude, radiusUnits);
            spatialCriteriaFactory.withinRadius(radius, latitude, longitude, radiusUnits, distErrorPercent);
            //endregion
        });

    //region spatial_6
    query.orderByDistance(field, latitude, longitude);

    query.orderByDistance(field, shapeWkt);

    query.orderByDistance(fieldName, latitude, longitude);

    query.orderByDistance(fieldName, shapeWkt);
    //endregion

    //region spatial_8
    query.orderByDistanceDescending(field, latitude, longitude);

    query.orderByDistanceDescending(field, shapeWkt);

    query.orderByDistanceDescending(fieldName, latitude, longitude);

    query.orderByDistanceDescending(fieldName, shapeWkt);
    //endregion
}



async function sample() {
    {
        //region spatial_4
        // return all matching entities
        // within 10 kilometers radius
        // from 32.1234 latitude and 23.4321 longitude coordinates
        const results = await session
            .query({ collection: "Houses" })
            .spatial(
                new PointField("latitude", "longitude"),
                f => f.withinRadius(10, 32.1234, 23.4321))
            .all();
        //endregion
    }

    {
        //region spatial_5
        // return all matching entities
        // within 10 kilometers radius
        // from 32.1234 latitude and 23.4321 longitude coordinates
        // this equals to WithinRadius(10, 32.1234, 23.4321)
        const results = await session
            .query({ collection: "Houses" })
            .spatial(
                new PointField("latitude", "longitude"),
                f => f.relatesToShape("Circle(32.1234 23.4321 d=10.0000)", "Within")
            )
            .all();

        //endregion
    }

    {
        //region spatial_7
        // return all matching entities
        // within 10 kilometers radius
        // from 32.1234 latitude and 23.4321 longitude coordinates
        // sort results by distance from 32.1234 latitude and 23.4321 longitude point
        const results = await session
            .query({ collection: "Houses" })
            .spatial(
                new PointField("latitude", "longitude"),
                f => f.withinRadius(10, 32.1234, 23.4321)
            )
            .orderByDistance(
                new PointField("latitude", "longitude"),
                32.12324, 23.4321)
            .all();
        //endregion
    }

    {
        //region spatial_9
        // return all matching entities
        // within 10 kilometers radius
        // from 32.1234 latitude and 23.4321 longitude coordinates
        // sort results by distance from 32.1234 latitude and 23.4321 longitude point
        const results = await session
            .query({ collection: "Houses" })
            .spatial(
                new PointField("latitude", "longitude"),
                f => f.withinRadius(10, 32.1234, 23.4321)
            )
            .orderByDistanceDescending(
                new PointField("latitude", "longitude"),
                32.12324, 23.4321)
            .all();
        //endregion
    }
}

