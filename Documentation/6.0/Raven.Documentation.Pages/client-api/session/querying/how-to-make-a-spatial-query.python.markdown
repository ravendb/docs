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
  use the `spatial` method, which provides a wide range of spatial functionalities.

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
      * [`spatial`](../../../client-api/session/querying/how-to-make-a-spatial-query#section)
      * [`DynamicSpatialField`](../../../client-api/session/querying/how-to-make-a-spatial-query#section-1)
      * [`SpatialCriteria`](../../../client-api/session/querying/how-to-make-a-spatial-query#section-2)
      * [`order_by_distance`, `order_by_distance_wkt`](../../../client-api/session/querying/how-to-make-a-spatial-query#section-3)
      * [`order_by_distance_descending`, `order_by_distance_descending_wkt`](../../../client-api/session/querying/how-to-make-a-spatial-query#section-4)

{NOTE/}

---

{PANEL: Search by radius}

Use the `within_radius` method to search for all documents containing spatial data that is located  
within the specified distance from the given center point.

{CODE-TABS}
{CODE-TAB:python:Query spatial_1@ClientApi\Session\Querying\MakeSpatialQuery.py /}
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

* Use the `relates_to_shape` method to search for all documents containing spatial data that is located  
  in the specified relation to the given shape.

* The shape is specified as either a **circle** or a **polygon** in a [WKT](https://en.wikipedia.org/wiki/Well-known_text_representation_of_geometry) format.

* The relation to the shape can be one of: `WITHIN`, `CONTAINS`, `DISJOINT`, `INTERSECTS`.

---

#### Circle

{CODE-TABS}
{CODE-TAB:python:Query spatial_2@ClientApi\Session\Querying\MakeSpatialQuery.py /}
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
{CODE-TAB:python:Query spatial_3@ClientApi\Session\Querying\MakeSpatialQuery.py /}
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

* Use `order_by_distance` or `order_by_distance_descending` to sort the results by distance from a given point.

* By default, distance is measured by RavenDB in **kilometers**.  
  The distance can be rounded to a specific range.  

---

#### Order by distance

{CODE-TABS}
{CODE-TAB:python:Query spatial_4@ClientApi\Session\Querying\MakeSpatialQuery.py /}
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
{CODE-TAB:python:Query spatial_5@ClientApi\Session\Querying\MakeSpatialQuery.py /}
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
{CODE-TAB:python:Query spatial_6@ClientApi\Session\Querying\MakeSpatialQuery.py /}
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

* The distance is available in the `@spatial` metadata property within each result.
* Note the following difference between the underlying search engines:
    * When using **Lucene**:  
      This metadata property is always available in the results.
    * When using **Corax**:  
      In order to enhance performance, this property is not included in the results by default.  
      To get this metadata property you must set the [Indexing.Corax.IncludeSpatialDistance](../../../server/configuration/indexing-configuration#indexing.corax.includespatialdistance) configuration value to _true_.  
      Learn about the available methods for setting an indexing configuration key in this [indexing-configuration](../../../server/configuration/indexing-configuration) article.

{CODE:python spatial_4_getDistance@ClientApi\Session\Querying\MakeSpatialQuery.py /}

{PANEL/}

{PANEL: Spatial API}

#### `spatial`

{CODE:python spatial_7@ClientApi\Session\Querying\MakeSpatialQuery.py /}

| Parameters    | Type                                                                                       | Description                                                                                                                |
|---------------|--------------------------------------------------------------------------------------------|----------------------------------------------------------------------------------------------------------------------------|
| **field_name_or_field** | `Union[str, DynamicSpatialField]` | `Str` - Path to spatial field in an index (when querying an index)<br><br>**-or-**<br><br>`DynamicSpatialField` - Object that contains the document's spatial fields, either `PointField` or `WktField`(when making a dynamic query). |
| **clause**    | `Callable[[SpatialCriteriaFactory], SpatialCriteria]` | Callback taking leverage of SpatialCriteriaFactory that comes as an argument, allowing to build SpatialCriteria. |

---

#### `DynamicSpatialField`

{CODE:python spatial_8@ClientApi\Session\Querying\MakeSpatialQuery.py /}

| Parameters    | Type  | Description                                               |
|---------------|-------|-----------------------------------------------------------|
| **latitude**  | `str` | Path to a document point field that contains the latitude |
| **longitude** | `str` | Path to a document point field that contains the longitude |
| **wkt** | `str` | Path to a document wkt field that contains the WKT string |

---

#### `SpatialCriteria`

{CODE:python spatial_9@ClientApi\Session\Querying\MakeSpatialQuery.py /}

| Parameter     | Type  | Description        |
|---------------|-------|--------------------|
| **shape_wkt** | `str` | [WKT](https://en.wikipedia.org/wiki/Well-known_text_representation_of_geometry)-based shape used in query criteria |
| **relation** | `SpatialRelation` | Relation of the shape to the spatial data in the document/index.<br>Can be `WITHIN`, `CONTAINS`, `DISJOINT`, `INTERSECTS` |
| **units** / **radius_units** | `SpatialUnits` | Determines if circle or shape should be calculated in `KILOMETERS` or `MILES`.<br>By default, distances are measured in kilometers. |
| **dist_error_percent** (Optional) | `float` | Maximum distance error tolerance in percents.<br>**Default: 0.025** |
| **radius** / **latitude** / **longitude** | `float` | Used to define a radius of a circle |

---

#### `order_by_distance`, `order_by_distance_wkt`

{CODE:python spatial_10@ClientApi\Session\Querying\MakeSpatialQuery.py /}

---

#### `order_by_distance_descending`, `order_by_distance_descending_wkt`

{CODE:python spatial_11@ClientApi\Session\Querying\MakeSpatialQuery.py /}

| Parameter                    | Type                                                                                       | Description                                                                                                                                                                                                                                                      |
|------------------------------|--------------------------------------------------------------------------------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **field_or_field_name** | `Union[str, DynamicSpatialField]` |  `Str` - Path to spatial field in an index (when querying an index)<br><br>**-or-**<br><br>`DynamicSpatialField` - Object that contains the document's spatial fields, either `PointField` or `WktField`(when making a dynamic query). |
| **latitude** | `float` | The latitude of the point from which the distance is measured |
| **longitude** | `float` | The longitude of the point from which the distance is measured |
| **round_factor** (Optional) | `float` | A distance interval in kilometers.<br>The distance from the point is rounded up to the nearest interval.<br>The results within the same interval can be sorted by a secondary order.<br>If no other order was specified, then by ascending order of document Id. |
| **shape_wkt** | `str` | [WKT](https://en.wikipedia.org/wiki/Well-known_text_representation_of_geometry)-based shape used in query criteria |

{PANEL/}

## Related Articles

### Session

- [How to Query](../../../client-api/session/querying/how-to-query)

### Indexes

- [Indexing Spatial Data](../../../indexes/indexing-spatial-data) 
- [Query spatial index](../../../indexes/querying/spatial) 

### Studio

- [Spatial query view](../../../studio/database/queries/spatial-queries-map-view) 
