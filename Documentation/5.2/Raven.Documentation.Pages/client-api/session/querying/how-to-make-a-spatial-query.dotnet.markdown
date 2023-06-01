# How to Make a Spatial Query

---

{NOTE: }

* Documents that contain spatial data can be queried by spatial queries that employ geographical criteria.  
  You have two options:
  
    * Either make a dynamic spatial query on a collection ( __described in this article__ ).  
      An auto-index will be created by the server.

    * Or, index your documents' spatial data in a static-index (see [indexing spatial data](../../../indexes/indexing-spatial-data)),  
      and then make a spatial query on this index (see [query a spatial index](../../../indexes/querying/spatial)).

* To perform a spatial search,  
  use the `Spatial` method which provides a wide range of spatial functionalities.

* When making a __dynamic spatial query__ from the Studio,  
  results are also displayed on the global map. See [spatial queries map view](../../../studio/database/queries/spatial-queries-map-view#circular-region-example).

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
  * [Spatial API](../../../client-api/session/querying/how-to-make-a-spatial-query#spatial-api)

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

* The shape is specified as either a __circle__ or a __polygon__ in a [WKT](https://en.wikipedia.org/wiki/Well-known_text_representation_of_geometry) format.

* The relation to the shape can be one of: `Within`, `Contains`, `Disjoint`, `Intersects`.

{NOTE: }

<a id="circle" /> __Circle__:

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
    spatial.wkt('CIRCLE(-122.3060097 47.623473 d=20)', 'miles')
)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

<a id="polygon" /> __Polygon__:

{CODE-TABS}
{CODE-TAB:csharp:Query spatial_3@ClientApi\Session\Querying\MakeSpatialQuery.cs /}
{CODE-TAB:csharp:Query_async spatial_3_1@ClientApi\Session\Querying\MakeSpatialQuery.cs /}
{CODE-TAB:csharp:DocumentQuery spatial_3_2@ClientApi\Session\Querying\MakeSpatialQuery.cs /}
{CODE-TAB-BLOCK:sql:RQL}
// This query will return all matching employee entities
// that are located within the specified polygon.

from companies
where spatial.within(
    spatial.point(Address.Location.Latitude, Address.Location.Longitude),
    spatial.wkt('POLYGON ((
        -118.6527948 32.7114894,
        -95.8040242 37.5929338,
        -102.8344151 53.3349629,
        -127.5286633 48.3485664,
        -129.4620208 38.0786067,
        -118.7406746 32.7853769,
        -118.6527948 32.7114894))')
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

* Use `OrderByDistance` or `OrderByDistanceDescending` to sort the results by distance from a given point.

* By default, distance in RavenDB measured in **kilometers**.  
  The distance can be rounded to a specific range.  

{NOTE: }

<a id="orderByDistance" /> __Order by distance__:

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

{NOTE/}

{NOTE: }

<a id="orderByDistanceDesc" /> __Order by distance descending__:

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

{NOTE/}

{NOTE: }

<a id="roundedDistance" /> __Sort results by rounded distance__:

{CODE-TABS}
{CODE-TAB:csharp:Query spatial_6@ClientApi\Session\Querying\MakeSpatialQuery.cs /}
{CODE-TAB:csharp:Query_async spatial_6_1@ClientApi\Session\Querying\MakeSpatialQuery.cs /}
{CODE-TAB:csharp:DocumentQuery spatial_6_2@ClientApi\Session\Querying\MakeSpatialQuery.cs /}
{CODE-TAB-BLOCK:sql:RQL}
// Return all employee entities.
// Results are sorted by their distance to a specified point rounded to the nearest 100 km interval.
// A secondary sort can be applied within the 100 km range, e.g. by field LastName.

from Employees as e
order by spatial.distance(
    spatial.point(Address.Location.Latitude, Address.Location.Longitude),
    spatial.point(47.623473, -122.3060097),
    100
), e.LastName
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{PANEL/}

{PANEL: Spatial API}

---

{INFO: }
#### Spatial
{INFO/}

{CODE spatial_7@ClientApi\Session\Querying\MakeSpatialQuery.cs /}

| Parameters    | Type                                                                                       | Description                                                                                                                |
|---------------|--------------------------------------------------------------------------------------------|----------------------------------------------------------------------------------------------------------------------------|
| __path__      | `Expression<Func<T, object>>`                                                              | Path to spatial field in an index<br>(when querying an index)                                                              | 
| __fieldName__ | `string`                                                                                   | Path to spatial field in an index<br>(when querying an index)                                                              |
| __field__     | `Func<DynamicSpatialFieldFactory<T>, DynamicSpatialField>`<br>or<br>`DynamicSpatialField` | Factory or field that points to a document field<br>(when making a dynamic query).<br>Either `PointField` or `WktField`. |
| __clause__    | `Func<SpatialCriteriaFactory, SpatialCriteria>`                                            | Spatial criteria that will be executed on a given spatial field                                                            |

---

{INFO: }
#### DynamicSpatialFieldFactory
{INFO/}

{CODE spatial_8@ClientApi\Session\Querying\MakeSpatialQuery.cs /}

| Parameters                                         | Type                          | Description                                                                  |
|----------------------------------------------------|-------------------------------|------------------------------------------------------------------------------|
| __latitudePath__ / __longitudePath__ / __wktPath__ | `Expression<Func<T, object>>` | Path to the field in a document containing either longitude, latitude or WKT |

---

{INFO: }
#### SpatialCriteriaFactory
{INFO/}

{CODE spatial_9@ClientApi\Session\Querying\MakeSpatialQuery.cs /}

| Parameter                                 | Type              | Description                                                                                                                         |
|-------------------------------------------|-------------------|-------------------------------------------------------------------------------------------------------------------------------------|
| __shapeWkt__                              | `string`          | [WKT](https://en.wikipedia.org/wiki/Well-known_text_representation_of_geometry)-based shape used in query criteria                  |
| __relation__                              | `SpatialRelation` | Relation of the shape to the spatial data in the document/index.<br>Can be `Within`, `Contains`, `Disjoint`, `Intersects`           |
| __distErrorPercent__                      | `double`          | Maximum distance error tolerance in percents. Default: 0.025                                                                        |
| __radius__ / __latitude__ / __longitude__ | `double`          | Used to define a radius of a circle                                                                                                 |
| __radiusUnits__ / __units__               | `SpatialUnits`    | Determines if circle or shape should be calculated in `Kilometers` or `Miles`.<br>By default, distances are measured in kilometers. |

---

{INFO: }
#### OrderByDistance
{INFO/}

{CODE spatial_10@ClientApi\Session\Querying\MakeSpatialQuery.cs /}

---

{INFO: }
#### OrderByDistanceDescending
{INFO/}

{CODE spatial_11@ClientApi\Session\Querying\MakeSpatialQuery.cs /}

| Parameter                    | Type                                                                                       | Description                                                                                                                                                                                                                                                                                                                                                         |
|------------------------------|--------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| __path__                     | `Expression<Func<T, object>>`                                                              | Path to spatial field in index<br>(when querying an index)                                                                                                                                                                                                                                                                                                          |
| __fieldName__                | `string`                                                                                   | Path to spatial field in index<br>(when querying an index)                                                                                                                                                                                                                                                                                                          |
| __field__                    | `Func<DynamicSpatialFieldFactory<T>, DynamicSpatialField>`<br>or<br>`DynamicSpatialField`  | Factory or field that points to a document field<br>(when making a dynamic query).<br>Either `PointField` or `WktField`.                                                                                                                                                                                                                                          |
| __shapeWkt__                 | `string`                                                                                   | [WKT](https://en.wikipedia.org/wiki/Well-known_text_representation_of_geometry)-based shape to be used as a point from which distance will be measured. If the shape is not a single point, then the center of the shape will be used as a reference.                                                                                                               |
| __latitude__ / __longitude__ | `double`                                                                                   | Used to define a point from which distance will be measured                                                                                                                                                                                                                                                                                                         |
| __roundFactor__              | `double`                                                                                   | A distance interval in kilometers.<br>The distance from the point is rounded up to the nearest interval.<br>The results within the same interval can be sorted by another order. This other order is the next [chained ordering](../../../indexes/querying/sorting#chaining-orderings), or if no other order was specified, then by ascending order of document Id. |

{PANEL/}

## Related Articles

### Session

- [How to Query](../../../client-api/session/querying/how-to-query)

### Querying

- [Spatial](../../../indexes/querying/spatial)

### Indexes

- [Indexing Spatial Data](../../../indexes/indexing-spatial-data) 

### Studio

- [Spatial query view](../../../studio/database/queries/spatial-queries-map-view) 
