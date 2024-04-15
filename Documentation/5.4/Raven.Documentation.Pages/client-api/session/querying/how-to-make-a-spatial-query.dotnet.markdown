# Make a Spatial Query

---

{NOTE: }

* Documents that contain spatial data can be queried by spatial queries that employ geographical criteria.  
  You can use either _Dynamic spatial query_ or _Spatial index query_.  
  
    * **Dynamic spatial query**  
      Make a dynamic spatial query on a collection (described below).  
      An auto-index will be created by the server.  

    * **Spatial index query**  
      Index your documents' spatial data in a static-index (see [indexing spatial data](../../../indexes/indexing-spatial-data)) 
      and then make a spatial query on this index (see [query a spatial index](../../../indexes/querying/spatial)).  

* To perform a spatial search,  
  use the `Spatial` method, which provides a wide range of spatial functionalities.

* When making a dynamic spatial query from Studio,  
  results are also displayed on the global map. See [spatial queries map view](../../../studio/database/queries/spatial-queries-map-view).

---

* In this page:

  * [Search by radius](../../../client-api/session/querying/how-to-make-a-spatial-query#search-by-radius)  
  * [Search by shape](../../../client-api/session/querying/how-to-make-a-spatial-query#search-by-shape)
      * [Circle](../../../client-api/session/querying/how-to-make-a-spatial-query#circle)
      * [Polygon](../../../client-api/session/querying/how-to-make-a-spatial-query#polygon)    
  * [Spatial sorting](../../../client-api/session/querying/how-to-make-a-spatial-query#spatial-sorting)
      * [Order by distance](../../../client-api/session/querying/how-to-make-a-spatial-query#order-by-distance)
      * [Order by distance descending](../../../client-api/session/querying/how-to-make-a-spatial-query#order-by-distance-descending)
      * [Sort results by rounded distance](../../../client-api/session/querying/how-to-make-a-spatial-query#sort-results-by-rounded-distance)
      * [Get resulting distance](../../../client-api/session/querying/how-to-make-a-spatial-query#get-resulting-distance)
  * [Spatial API](../../../client-api/session/querying/how-to-make-a-spatial-query#spatial-api)
      * [`Spatial`](../../../client-api/session/querying/how-to-make-a-spatial-query#section)
      * [`DynamicSpatialFieldFactory`](../../../client-api/session/querying/how-to-make-a-spatial-query#section-1)
      * [`SpatialCriteriaFactory`](../../../client-api/session/querying/how-to-make-a-spatial-query#section-2)
      * [`OrderByDistance`](../../../client-api/session/querying/how-to-make-a-spatial-query#section-3)
      * [`OrderByDistanceDescending`](../../../client-api/session/querying/how-to-make-a-spatial-query#section-4)

{NOTE/}

---

{PANEL: Search by radius}

Use the `WithinRadius` method to search for all documents containing spatial data that is located  
within the specified distance from the given center point.

{CODE-TABS}
{CODE-TAB:csharp:Query spatial_1@ClientApi\Session\Querying\MakeSpatialQuery.cs /}
{CODE-TAB:csharp:Query_async spatial_1_1@ClientApi\Session\Querying\MakeSpatialQuery.cs /}
{CODE-TAB:csharp:DocumentQuery spatial_1_2@ClientApi\Session\Querying\MakeSpatialQuery.cs /}
{CODE-TAB-BLOCK:sql:RQL}
// This query will return all matching employee entities
// that are located within 20 kilometers radius
// from point (47.623473 latitude, -122.3060097 longitude).

from Employees
where spatial.within(
    spatial.point(Address.Location.Latitude, Address.Location.Longitude),
    spatial.circle(20, 47.623473, -122.3060097)
)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Search by shape}

* Use the `RelatesToShape` method to search for all documents containing spatial data that is located  
  in the specified relation to the given shape.

* The shape is specified as either a **circle** or a **polygon** in a [WKT](https://en.wikipedia.org/wiki/Well-known_text_representation_of_geometry) format.

* The relation to the shape can be one of: `Within`, `Contains`, `Disjoint`, `Intersects`.

---

#### Circle

{CODE-TABS}
{CODE-TAB:csharp:Query spatial_2@ClientApi\Session\Querying\MakeSpatialQuery.cs /}
{CODE-TAB:csharp:Query_async spatial_2_1@ClientApi\Session\Querying\MakeSpatialQuery.cs /}
{CODE-TAB:csharp:DocumentQuery spatial_2_2@ClientApi\Session\Querying\MakeSpatialQuery.cs /}
{CODE-TAB-BLOCK:sql:RQL}
// This query will return all matching employee entities
// that are located within 20 kilometers radius
// from point (47.623473 latitude, -122.3060097 longitude).

from Employees
where spatial.within(
    spatial.point(Address.Location.Latitude, Address.Location.Longitude),
    spatial.wkt("CIRCLE(-122.3060097 47.623473 d=20)", "miles")
)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

---

#### Polygon

{CODE-TABS}
{CODE-TAB:csharp:Query spatial_3@ClientApi\Session\Querying\MakeSpatialQuery.cs /}
{CODE-TAB:csharp:Query_async spatial_3_1@ClientApi\Session\Querying\MakeSpatialQuery.cs /}
{CODE-TAB:csharp:DocumentQuery spatial_3_2@ClientApi\Session\Querying\MakeSpatialQuery.cs /}
{CODE-TAB-BLOCK:sql:RQL}
// This query will return all matching company entities
// that are located within the specified polygon.

from companies
where spatial.within(
    spatial.point(Address.Location.Latitude, Address.Location.Longitude),
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

{INFO: Polygon rules}

* The polygon's coordinates must be provided in counterclockwise order.

* The first and last coordinates must mark the same location to form a closed region.

![WKT polygon](images/spatial_1.png "WKT Polygon")

{INFO/}

{PANEL/}

{PANEL: Spatial sorting}

* Use `OrderByDistance` or `OrderByDistanceDescending` to sort the results by distance from a given point.

* By default, distance is measured by RavenDB in **kilometers**.  
  The distance can be rounded to a specific range.  

---

#### Order by distance

{CODE-TABS}
{CODE-TAB:csharp:Query spatial_4@ClientApi\Session\Querying\MakeSpatialQuery.cs /}
{CODE-TAB:csharp:Query_async spatial_4_1@ClientApi\Session\Querying\MakeSpatialQuery.cs /}
{CODE-TAB:csharp:DocumentQuery spatial_4_2@ClientApi\Session\Querying\MakeSpatialQuery.cs /}
{CODE-TAB-BLOCK:sql:RQL}
// Return all matching employee entities located within 20 kilometers radius
// from point (47.623473 latitude, -122.3060097 longitude).

// Sort the results by their distance from a specified point,
// the closest results will be listed first.       

from Employees
where spatial.within(
    spatial.point(Address.Location.Latitude, Address.Location.Longitude),
    spatial.circle(20, 47.623473, -122.3060097)
)
order by spatial.distance(
    spatial.point(Address.Location.Latitude, Address.Location.Longitude),
    spatial.point(47.623473, -122.3060097)
)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

---

#### Order by distance descending

{CODE-TABS}
{CODE-TAB:csharp:Query spatial_5@ClientApi\Session\Querying\MakeSpatialQuery.cs /}
{CODE-TAB:csharp:Query_async spatial_5_1@ClientApi\Session\Querying\MakeSpatialQuery.cs /}
{CODE-TAB:csharp:DocumentQuery spatial_5_2@ClientApi\Session\Querying\MakeSpatialQuery.cs /}
{CODE-TAB-BLOCK:sql:RQL}
// Return all employee entities sorted by their distance from a specified point.
// The farthest results will be listed first.

from Employees
order by spatial.distance(
    spatial.point(Address.Location.Latitude, Address.Location.Longitude),
    spatial.point(47.623473, -122.3060097)
) desc
{CODE-TAB-BLOCK/}
{CODE-TABS/}

---

#### Sort results by rounded distance

{CODE-TABS}
{CODE-TAB:csharp:Query spatial_6@ClientApi\Session\Querying\MakeSpatialQuery.cs /}
{CODE-TAB:csharp:Query_async spatial_6_1@ClientApi\Session\Querying\MakeSpatialQuery.cs /}
{CODE-TAB:csharp:DocumentQuery spatial_6_2@ClientApi\Session\Querying\MakeSpatialQuery.cs /}
{CODE-TAB-BLOCK:sql:RQL}
// Return all employee entities.
// Results are sorted by their distance to a specified point rounded to the nearest 100 km interval.
// A secondary sort can be applied within the 100 km range, e.g. by field LastName.

from Employees
order by spatial.distance(
    spatial.point(Address.Location.Latitude, Address.Location.Longitude),
    spatial.point(47.623473, -122.3060097),
    100
), LastName
{CODE-TAB-BLOCK/}
{CODE-TABS/}

---

#### Get resulting distance

The distance is available in the `@spatial` metadata property within each result.

{CODE spatial_4_getDistance@ClientApi\Session\Querying\MakeSpatialQuery.cs /}

{PANEL/}

{PANEL: Spatial API}

#### `Spatial`

{CODE spatial_7@ClientApi\Session\Querying\MakeSpatialQuery.cs /}

| Parameters    | Type                                                                                       | Description                                                                                                                |
|---------------|--------------------------------------------------------------------------------------------|----------------------------------------------------------------------------------------------------------------------------|
| **path**      | `Expression<Func<T, object>>`                                                              | Path to spatial field in an index<br>(when querying an index)                                                              | 
| **fieldName** | `string`                                                                                   | Path to spatial field in an index<br>(when querying an index)                                                              |
| **field**     | `Func<DynamicSpatialFieldFactory<T>, DynamicSpatialField>`<br>or<br>`DynamicSpatialField` | Factory or field that points to a document field<br>(when making a dynamic query).<br>Either `PointField` or `WktField`. |
| **clause**    | `Func<SpatialCriteriaFactory, SpatialCriteria>`                                            | Spatial criteria that will be executed on a given spatial field                                                            |

---

#### `DynamicSpatialFieldFactory`

{CODE spatial_8@ClientApi\Session\Querying\MakeSpatialQuery.cs /}

| Parameters                                         | Type                          | Description                                                                  |
|----------------------------------------------------|-------------------------------|------------------------------------------------------------------------------|
| **latitudePath** / **longitudePath** / **wktPath** | `Expression<Func<T, object>>` | Path to the field in a document containing either longitude, latitude or WKT |

---

#### `SpatialCriteriaFactory`

{CODE spatial_9@ClientApi\Session\Querying\MakeSpatialQuery.cs /}

| Parameter                                 | Type              | Description                                                                                                                         |
|-------------------------------------------|-------------------|-------------------------------------------------------------------------------------------------------------------------------------|
| **shapeWkt**                              | `string`          | [WKT](https://en.wikipedia.org/wiki/Well-known_text_representation_of_geometry)-based shape used in query criteria                  |
| **relation**                              | `SpatialRelation` | Relation of the shape to the spatial data in the document/index.<br>Can be `Within`, `Contains`, `Disjoint`, `Intersects`.          |
| **distErrorPercent**                      | `double`          | Maximum distance error tolerance in percents. Default: 0.025                                                                        |
| **radius** / **latitude** / **longitude** | `double`          | Used to define a radius of a circle                                                                                                 |
| **radiusUnits** / **units**               | `SpatialUnits`    | Determines if circle or shape should be calculated in `Kilometers` or `Miles`.<br>By default, distances are measured in kilometers. |

---

#### `OrderByDistance`

{CODE spatial_10@ClientApi\Session\Querying\MakeSpatialQuery.cs /}

---

#### `OrderByDistanceDescending`

{CODE spatial_11@ClientApi\Session\Querying\MakeSpatialQuery.cs /}

| Parameter                    | Type                                                                                       | Description                                                                                                                                                                                                                                                      |
|------------------------------|--------------------------------------------------------------------------------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **path**                     | `Expression<Func<T, object>>`                                                              | Path to spatial field in index<br>(when querying an index)                                                                                                                                                                                                       |
| **fieldName**                | `string`                                                                                   | Path to spatial field in index<br>(when querying an index)                                                                                                                                                                                                       |
| **field**                    | `Func<DynamicSpatialFieldFactory<T>, DynamicSpatialField>`<br>or<br>`DynamicSpatialField`  | Factory or field that points to a document field<br>(when making a dynamic query).<br>Either `PointField` or `WktField`.                                                                                                                                         |
| **shapeWkt**                 | `string`                                                                                   | [WKT](https://en.wikipedia.org/wiki/Well-known_text_representation_of_geometry)-based shape to be used as a point from which distance will be measured. If the shape is not a single point, then the center of the shape will be used as a reference.            |
| **latitude** / **longitude** | `double`                                                                                   | Used to define a point from which distance will be measured                                                                                                                                                                                                      |
| **roundFactor**              | `double`                                                                                   | A distance interval in kilometers.<br>The distance from the point is rounded up to the nearest interval.<br>The results within the same interval can be sorted by a secondary order.<br>If no other order was specified, then by ascending order of document Id. |

{PANEL/}

## Related Articles

### Session

- [How to Query](../../../client-api/session/querying/how-to-query)

### Indexes

- [Indexing Spatial Data](../../../indexes/indexing-spatial-data) 
- [Query spatial index](../../../indexes/querying/spatial) 

### Studio

- [Spatial query view](../../../studio/database/queries/spatial-queries-map-view) 
