# Make a Spatial Query

---

{NOTE: }

* Documents that contain spatial data can be queried by spatial queries that employ geographical criteria.  
  You have two options:
  
    * __Dynamic spatial query__  
      Either make a dynamic spatial query on a collection ( __described in this article__ ).  
      An auto-index will be created by the server.

    * __Spatial index query__  
      Or, index your documents' spatial data in a static-index (see [indexing spatial data](../../../indexes/indexing-spatial-data)),  
      and then make a spatial query on this index (see [query a spatial index](../../../indexes/querying/spatial)).

* To perform a spatial search,  
  use the `spatial` method which provides a wide range of spatial functionalities.

* When making a dynamic spatial query from the Studio,  
  results are also displayed on the global map. See [spatial queries map view](../../../studio/database/queries/spatial-queries-map-view).

---

* In this page:

  * [Search by radius](../../../client-api/session/querying/how-to-make-a-spatial-query#search-by-radius)  
  * [Search by shape](../../../client-api/session/querying/how-to-make-a-spatial-query#search-by-shape)
      * [Circle](../../../client-api/session/querying/how-to-make-a-spatial-query#circle)
      * [Polygon](../../../client-api/session/querying/how-to-make-a-spatial-query#polygon)    
  * [Spatial sorting](../../../client-api/session/querying/how-to-make-a-spatial-query#spatial-sorting)
      * [Order by distance](../../../client-api/session/querying/how-to-make-a-spatial-query#orderByDistance)
      * [Order by distance desc](../../../client-api/session/querying/how-to-make-a-spatial-query#orderByDistanceDesc)
      * [Rounded distance](../../../client-api/session/querying/how-to-make-a-spatial-query#roundedDistance)
      * [Get resulting distance](../../../client-api/session/querying/how-to-make-a-spatial-query#getResultingDistance)
  * [Spatial API](../../../client-api/session/querying/how-to-make-a-spatial-query#spatial-api)

{NOTE/}

---

{PANEL: Search by radius}

Use the `withinRadius` method to search for all documents containing spatial data that is located  
within the specified distance from the given center point.

{CODE-TABS}
{CODE-TAB:nodejs:Query spatial_1@client-api\session\querying\makeSpatialQuery.js /}
{CODE-TAB-BLOCK:sql:RQL}
// This query will return all matching employee entities
// that are located within 20 kilometers radius
// from point (47.623473 latitude, -122.3060097 longitude).

from "employees"
where spatial.within(
    spatial.point(address.location.latitude, address.location.longitude),
    spatial.circle(20, 47.623473, -122.3060097)
)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Search by shape}

* Use the `relatesToShape` method to search for all documents containing spatial data that is located  
  in the specified relation to the given shape.

* The shape is specified as either a __circle__ or a __polygon__ in a [WKT](https://en.wikipedia.org/wiki/Well-known_text_representation_of_geometry) format.

* The relation to the shape can be one of: `Within`, `Contains`, `Disjoint`, `Intersects`.

{NOTE: }

<a id="circle" /> __Circle__:

{CODE-TABS}
{CODE-TAB:nodejs:Query spatial_2@client-api\session\querying\makeSpatialQuery.js /}
{CODE-TAB-BLOCK:sql:RQL}
// This query will return all matching employee entities
// that are located within 20 kilometers radius
// from point (47.623473 latitude, -122.3060097 longitude).

from "employees"
where spatial.within(
    spatial.point(address.location.latitude, address.location.longitude),
    spatial.wkt("CIRCLE(-122.3060097 47.623473 d=20)", "miles")
)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

<a id="polygon" /> __Polygon__:

{CODE-TABS}
{CODE-TAB:nodejs:Query spatial_3@client-api\session\querying\makeSpatialQuery.js /}
{CODE-TAB-BLOCK:sql:RQL}
// This query will return all matching company entities
// that are located within the specified polygon.

from "companies"
where spatial.within(
    spatial.point(address.location.latitude, address.location.longitude),
    spatial.wkt("POLYGON ((
        -118.6527948 32.7114894,
        -95.8040242 37.5929338,
        -102.8344151 53.3349629,
        -127.5286633 48.3485664,
        -129.4620208 38.0786067,
        -118.7406746 32.7853769,
        -118.6527948 32.7114894))")
)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{INFO:  }

<a id="polygonRules" /> __Polygon rules__:

* The polygon's coordinates must be provided in counterclockwise order.

* The first and last coordinates must mark the same location to form a closed region.

![WKT polygon](images/spatial_1.png "WKT Polygon")

{INFO/}

{NOTE/}

{PANEL/}

{PANEL: Spatial sorting}

* Use `orderByDistance` or `orderByDistanceDescending` to sort the results by distance from a given point.

* By default, distance in RavenDB measured in **kilometers**.  
  The distance can be rounded to a specific range.  

{NOTE: }

<a id="orderByDistance" /> __Order by distance__:

{CODE-TABS}
{CODE-TAB:nodejs:Query spatial_4@client-api\session\querying\makeSpatialQuery.js /}
{CODE-TAB-BLOCK:sql:RQL}
// Return all matching employee entities located within 20 kilometers radius
// from point (47.623473 latitude, -122.3060097 longitude).

// Sort the results by their distance from a specified point,
// the closest results will be listed first.

from "employees"
where spatial.within(
    spatial.point(address.location.latitude, address.location.longitude),
    spatial.circle(20, 47.623473, -122.3060097)
)
order by spatial.distance(
    spatial.point(address.location.latitude, address.location.longitude),
    spatial.point(47.623473, -122.3060097)
)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

<a id="orderByDistanceDesc" /> __Order by distance descending__:

{CODE-TABS}
{CODE-TAB:nodejs:Query spatial_5@client-api\session\querying\makeSpatialQuery.js /}
{CODE-TAB-BLOCK:sql:RQL}
// Return all employee entities sorted by their distance from a specified point.
// The farthest results will be listed first.

from "employees"
order by spatial.distance(
    spatial.point(address.location.latitude, address.location.longitude),
    spatial.point(47.623473, -122.3060097)
) desc
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

<a id="roundedDistance" /> __Sort results by rounded distance__:

{CODE-TABS}
{CODE-TAB:nodejs:Query spatial_6@client-api\session\querying\makeSpatialQuery.js /}
{CODE-TAB-BLOCK:sql:RQL}
// Return all employee entities.
// Results are sorted by their distance to a specified point rounded to the nearest 100 km interval.
// A secondary sort can be applied within the 100 km range, e.g. by field lastName.

from "employees"
order by spatial.distance(
    spatial.point(address.location.latitude, address.location.longitude),
    spatial.point(47.623473, -122.3060097),
    100
), lastName
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

<a id="getResultingDistance" /> __Get resulting distance__:

* The distance is available in the `@spatial` metadata property within each result.

* Note the following difference between the underlying search engines:

    * When using __Lucene__:  
      This metadata property is always available in the results.

    * When using __Corax__:  
      In order to enhance performance, this property is not included in the results by default.  
      To get this metadata property you must set the [Indexing.Corax.IncludeSpatialDistance](../../../server/configuration/indexing-configuration#indexing.corax.includespatialdistance) configuration value to _true_.
      Learn how to set configuration values in this [Configuration overview](../../../server/configuration/configuration-options).

{CODE:nodejs spatial_4_getDistance@client-api\session\querying\makeSpatialQuery.js /}

{NOTE/}

{PANEL/}

{PANEL: Spatial API}

---

{INFO: }
#### spatial
{INFO/}

{CODE:nodejs spatial_7@client-api\session\querying\makeSpatialQuery.js /}

| Parameters    | Type                                           | Description                                                                                                                |
|---------------|------------------------------------------------|----------------------------------------------------------------------------------------------------------------------------|
| __fieldName__ | `string`                                       | Path to spatial field in an index<br>(when querying an index).                                                             |
| __field__     | `DynamicSpatialField`                          | Object that contains the document's spatial fields,<br>either `PointField` or `WktField`<br>(when making a dynamic query). |
| __clause__    | `(SpatialCriteriaFactory) => SpatialCrieteria` | Spatial criteria that will be executed on a given spatial field.                                                           |

---

{INFO: }
#### DynamicSpatialField
{INFO/}

{CODE:nodejs spatial_8@client-api\session\querying\makeSpatialQuery.js /}

| Parameters    | Type     | Description                                             |
|---------------|----------|---------------------------------------------------------|
| __latitude__  | `string` | Path to the document field that contains the latitude   |
| __longitude__ | `string` | Path to the document field that contains the longitude  |
| __wktPath__   | `string` | Path to the document field that contains the WKT string |

---

{INFO: }
#### SpatialCriteriaFactory
{INFO/}

{CODE:nodejs spatial_9@client-api\session\querying\makeSpatialQuery.js /}

| Parameter                                 | Type     | Description                                                                                                                         |
|-------------------------------------------|----------|-------------------------------------------------------------------------------------------------------------------------------------|
| __shapeWkt__                              | `string` | [WKT](https://en.wikipedia.org/wiki/Well-known_text_representation_of_geometry)-based shape used in query criteria                  |
| __relation__                              | `string` | Relation of the shape to the spatial data in the document/index.<br>Can be `Within`, `Contains`, `Disjoint`, `Intersects`.          |
| __distErrorPercent__                      | `number` | Maximum distance error tolerance in percents. Default: 0.025                                                                        |
| __radius__ / __latitude__ / __longitude__ | `number` | Used to define a radius of a circle                                                                                                 |
| __radiusUnits__ / __units__               | `string` | Determines if circle or shape should be calculated in `Kilometers` or `Miles`.<br>By default, distances are measured in kilometers. |

---

{INFO: }
#### orderByDistance
{INFO/}

{CODE:nodejs spatial_10@client-api\session\querying\makeSpatialQuery.js /}

---

{INFO: }
#### orderByDistanceDescending
{INFO/}

{CODE:nodejs spatial_11@client-api\session\querying\makeSpatialQuery.js /}

| Parameter                    | Type                  | Description                                                                                                                                                                                                                                                      |
|------------------------------|-----------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| __fieldName__                | `string`              | Path to spatial field in index<br>(when querying an index).                                                                                                                                                                                                      |
| __field__                    | `DynamicSpatialField` | Object that contains the document's spatial fields,<br>either `PointField` or `WktField`<br>(when making a dynamic query).                                                                                                                                       |
| __shapeWkt__                 | `string`              | [WKT](https://en.wikipedia.org/wiki/Well-known_text_representation_of_geometry)-based shape to be used as a point from which distance will be measured. If the shape is not a single point, then the center of the shape will be used as a reference.            |
| __latitude__ / __longitude__ | `number`              | Used to define a point from which distance will be measured                                                                                                                                                                                                      |
| __roundFactor__              | `number`              | A distance interval in kilometers.<br>The distance from the point is rounded up to the nearest interval.<br>The results within the same interval can be sorted by a secondary order.<br>If no other order was specified, then by ascending order of document Id. |

{PANEL/}

## Related Articles

### Session

- [How to Query](../../../client-api/session/querying/how-to-query)

### Indexes

- [Indexing Spatial Data](../../../indexes/indexing-spatial-data) 
- [Query spatial index](../../../indexes/querying/spatial) 

### Studio

- [Spatial query view](../../../studio/database/queries/spatial-queries-map-view) 
