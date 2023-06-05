import { DocumentStore, AbstractIndexCreationTask } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

async function spatialQuery() {
    {
        //region spatial_1
        // This query will return all matching employee entities
        // that are located within 20 kilometers radius
        // from point (47.623473 latitude, -122.3060097 longitude).

        // Define a dynamic query on 'employees' collection
        const employeesWithinRadius = await session
            .query({ collection: "employees" })
             // Call 'spatial' method
            .spatial(
                // Specify the  document fields containing the spatial data
                new PointField("address.location.latitude", "address.location.longitude"),
                // Set the geographical area in which to search for matching documents
                // Call 'withinRadius', pass the radius and the center points coordinates  
                criteria => criteria.withinRadius(20, 47.623473, -122.3060097))
            .all();
        //endregion
    }

    {
        //region spatial_2
        // This query will return all matching employee entities
        // that are located within 20 kilometers radius
        // from point (47.623473 latitude, -122.3060097 longitude).

        // Define a dynamic query on 'employees' collection
        const employeesWithinShape = await session
            .query({ collection: "employees" })
             // Call 'spatial' method
            .spatial(
                // Specify the  document fields containing the spatial data
                new PointField("address.location.latitude", "address.location.longitude"),
                // Set the geographical search criteria, call 'relatesToShape'
                criteria => criteria.relatesToShape(
                    // Specify the WKT string. Note: longitude is written FIRST
                    "CIRCLE(-122.3060097 47.623473 d=20)",
                    // Specify the relation between the WKT shape and the documents spatial data
                    "Within",
                    // Customize radius units (default is Kilometers) and error percentage (Optional)
                    "Miles",
                    0))
            .all();
        //endregion
    }

    {
        //region spatial_3
        // This query will return all matching company entities
        // that are located within the specified polygon.

        // Define a dynamic query on 'companies' collection
        const companiesWithinShape = await session
            .query({ collection: "employees" })
            // Call 'spatial' method
            .spatial(
                // Specify the  document fields containing the spatial data
                new PointField("address.location.latitude", "address.location.longitude"),
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
                    "Within"))
            .all(); 
        //endregion
    }

    {
        //region spatial_4
        // Return all matching employee entities located within 20 kilometers radius
        // from point (47.623473 latitude, -122.3060097 longitude).
        
        // Sort the results by their distance from a specified point,
        // the closest results will be listed first.

        const employeesSortedByDistance = await session
            .query({ collection: "employees" })
             // Provide the query criteria:
            .spatial(
                new PointField("address.location.latitude", "address.location.longitude"),
                criteria => criteria.withinRadius(20, 47.623473, -122.3060097))
             // Call 'orderByDistance'
            .orderByDistance(
                // Specify the document fields containing the spatial data
                new PointField("address.location.latitude", "address.location.longitude"),
                // Sort the results by their distance from this point: 
                47.623473, -122.3060097)
            .all();
        //endregion
    }

    {
        //region spatial_5
        // Return all employee entities sorted by their distance from a specified point.
        // The farthest results will be listed first.

        const employeesSortedByDistanceDesc = await session
            .query({ collection: "employees" })
             // Call 'orderByDistanceDescending'
            .orderByDistanceDescending(
                // Specify the document fields containing the spatial data
                new PointField("address.location.latitude", "address.location.longitude"),
                // Sort the results by their distance (descending) from this point: 
                47.623473, -122.3060097)
            .all();
        //endregion
    }

    {
        //region spatial_6
        // Return all employee entities.
        // Results are sorted by their distance to a specified point rounded to the nearest 100 km interval.
        // A secondary sort can be applied within the 100 km range, e.g. by field lastName.

        const employeesSortedByRoundedDistance = await session
            .query({ collection: "employees" })
             // Call 'orderByDistanceDescending'
            .orderByDistance(
                // Specify the document fields containing the spatial data
                new PointField("address.location.latitude", "address.location.longitude")
                     // Round up distance to 100 km 
                    .roundTo(100),
                // Sort the results by their distance (descending) from this point: 
                47.623473, -122.3060097)
             // A secondary sort can be applied
            .orderBy("lastName")
            .all();
        //endregion
    }
}

{
    //region syntax

    //region spatial_7
    spatial(fieldName, clause);
    spatial(field, clause);
    //endregion

    {
        //region spatial_8
        class PointField {
            latitude; 
            longitude;
        }

        class WktField {
            wkt;
        }
        //endregion
    }

    {
        //region spatial_9
        relatesToShape(shapeWkt, relation);
        relatesToShape(shapeWkt, relation, units, distErrorPercent);
        intersects(shapeWkt);
        intersects(shapeWkt, distErrorPercent);
        intersects(shapeWkt, distErrorPercent);
        intersects(shapeWkt, units, distErrorPercent);
        contains(shapeWkt);
        contains(shapeWkt, units);
        contains(shapeWkt, distErrorPercent);
        contains(shapeWkt, units, distErrorPercent);
        disjoint(shapeWkt);
        disjoint(shapeWkt, units);
        disjoint(shapeWkt, distErrorPercent);
        disjoint(shapeWkt, units, distErrorPercent);
        within(shapeWkt);
        within(shapeWkt, units);
        within(shapeWkt, distErrorPercent);
        within(shapeWkt, units, distErrorPercent);
        withinRadius(radius, latitude, longitude);
        withinRadius(radius, latitude, longitude, radiusUnits);
        withinRadius(radius, latitude, longitude, radiusUnits, distErrorPercent);
        //endregion
    }

    {
        //region spatial_10
        orderByDistance(field, latitude, longitude);
        orderByDistance(field, shapeWkt);
        orderByDistance(fieldName, latitude, longitude);
        orderByDistance(fieldName, latitude, longitude, roundFactor: number);
        orderByDistance(fieldName, shapeWkt);
        //endregion
    }

    {
        //region spatial_11
        orderByDistanceDescending(field, latitude, longitude);
        orderByDistanceDescending(field, shapeWkt);
        orderByDistanceDescending(fieldName, latitude, longitude);
        orderByDistanceDescending(fieldName, latitude, longitude, roundFactor);
        orderByDistanceDescending(fieldName, shapeWkt);
        //endregion
    }
    
    //endregion
}
