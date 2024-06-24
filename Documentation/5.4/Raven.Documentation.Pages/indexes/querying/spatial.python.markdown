# Query a Spatial Index

---

{NOTE: }

* Documents that contain spatial data can be queried by spatial queries that employ geographical criteria.  
  You have two options:

    * **Dynamic spatial query**  
      Either make a dynamic spatial query on a collection (see [how to make a spatial query](../../client-api/session/querying/how-to-make-a-spatial-query)).  
      An auto-index will be created by the server.

    * **Spatial index query**  
      Or, index your documents' spatial data in a static-index (see [indexing spatial data](../../indexes/indexing-spatial-data))  
      and then make a spatial query on this index ( **described in this article** ).

* A few examples of querying a spatial index are provided below.  
  **A spatial query performed on a static-index is similar to the** [dynamic spatial query](../../client-api/session/querying/how-to-make-a-spatial-query).  
  Find all spatial API methods listed [here](../../client-api/session/querying/how-to-make-a-spatial-query#spatial-api).  

* Examples in this page:
    * [Search by radius](../../indexes/querying/spatial#search-by-radius)
    * [Search by shape](../../indexes/querying/spatial#search-by-shape)
    * [Sort results](../../indexes/querying/spatial#sort-results)

{NOTE/}

---

{PANEL: Search by radius}

* Query the spatial index:

* Use the `within_radius` method to search for all documents containing spatial data that is located  
  within the specified distance from the given center point.

{CODE-TABS}
{CODE-TAB:python:Query spatial_query_1@Indexes\SpatialIndexes.py /}
{CODE-TAB:python:Index spatial_index_1@Indexes\SpatialIndexes.py /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Events/ByNameAndCoordinates"
where spatial.within(
    Coordinates,
    spatial.circle(20, 47.623473, -122.3060097)
)

// The query returns all matching Event entities
// that are located within 20 kilometers radius
// from point (47.623473 latitude, -122.3060097 longitude).
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Search by shape}

* Query the spatial index:  
  Use the `relates_to_shape` method to search for all documents containing spatial data that is located  
  in the specified relation to the given shape.

* The shape in the query is specified as either a **circle** or a **polygon** in a WKT format.  
  See polygon rules [here](../../client-api/session/querying/how-to-make-a-spatial-query#polygonRules).

* The relation to the shape can be one of: `WITHIN`, `CONTAINS`, `DISJOINT`, `INTERSECTS`.

* See more usage examples in the [dynamic search by shape](../../client-api/session/querying/how-to-make-a-spatial-query#search-by-shape) query.

{CODE-TABS}
{CODE-TAB:python:Query spatial_query_3@Indexes\SpatialIndexes.py /}
{CODE-TAB:python:Index spatial_index_2@Indexes\SpatialIndexes.py /}
{CODE-TAB-BLOCK:sql:RQL}
from index "EventsWithWKT/ByNameAndWKT"
where spatial.within(
    WKT,
    spatial.wkt("POLYGON ((
        -118.6527948 32.7114894,
        -95.8040242 37.5929338,
        -102.8344151 53.3349629,
        -127.5286633 48.3485664,
        -129.4620208 38.0786067,
        -118.7406746 32.7853769,
        -118.6527948 32.7114894))")
)

// The query returns all matching Event entities
// that are located within the specified polygon.
{CODE-TAB-BLOCK/}
{CODE-TABS/}

* Note:  
  The index in the above example indexes a WKT string in the spatial index-field.  
  However, you can query by shape also on spatial data that is indexed as lat/lng coordinates.  

{PANEL/}

{PANEL: Sort results}

* Query the spatial index:  
  Use `order_by_distance` or `order_by_distance_descending` to sort the results by distance from a given point.

* By default, distance in RavenDB measured in **kilometers**.  
  The distance can be rounded to a specific range.  

{CODE-TABS}
{CODE-TAB:python:Query spatial_query_5@Indexes\SpatialIndexes.py /}
{CODE-TAB:python:Index spatial_index_1@Indexes\SpatialIndexes.py /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Events/ByNameAndCoordinates"
where spatial.within(
    Coordinates,
    spatial.circle(20, 47.623473, -122.3060097)
)
order by spatial.distance(
    Coordinates,
    spatial.point(47.623473, -122.3060097)
)

// The query returns all matching Event entities located within 20 kilometers radius
// from point (47.623473 latitude, -122.3060097 longitude).

// Sort the results by their distance from a specified point,
// the closest results will be listed first.
{CODE-TAB-BLOCK/}
{CODE-TABS/}

* More sorting examples are available in the [dynamic spatial query](../../client-api/session/querying/how-to-make-a-spatial-query#spatial-sorting) article.

* To get the **distance** for each resulting entity see [get resulting distance](../../client-api/session/querying/how-to-make-a-spatial-query#getResultingDistance).

{PANEL/}

## Related Articles

### Indexes

- [Indexing Spatial Data](../../indexes/indexing-spatial-data)

### Client API

- [How to make a spatial query](../../client-api/session/querying/how-to-make-a-spatial-query)

### Studio

- [Spatial query view](../../studio/database/queries/spatial-queries-map-view) 
