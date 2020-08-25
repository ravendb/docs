# Session: Querying: How to Query a Spatial Index
---

{NOTE: }

* Spatial indexes can be queried using the `Spatial` method which contains a full spectrum of 
spatial methods. The following article will cover the methods `Spatial()`, `OrderByDistance()`, 
and `OrderByDistanceDescending`.  

* In this page:  
  * [Spatial](../../../client-api/session/querying/how-to-query-a-spatial-index#spatial)  
  * [OrderByDistance](../../../client-api/session/querying/how-to-query-a-spatial-index#orderbydistance)  
  * [OrderByDistanceDescending](../../../client-api/session/querying/how-to-query-a-spatial-index#orderbydistancedescending)  

{NOTE/}

---

{PANEL:Spatial}

{CODE spatial_1@ClientApi\Session\Querying\HowToQuerySpatialIndex.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **path** | `Expression<Func<T, object>>` | Path to spatial field in an index |
| **fieldName** | `string` | Path to spatial field in an index |
| **field** | `Func<DynamicSpatialFieldFactory<T>, DynamicSpatialField>` or `DynamicSpatialField` | Factory or field that points to a dynamic field (used with auto-indexes). Either `PointField` or `WktField` |
| **clause** | `Func<SpatialCriteriaFactory, SpatialCriteria>` | Spatial criteria that will be executed on a given spatial field from the `path` parameter. |

### DynamicSpatialFieldFactory

{CODE spatial_2@ClientApi\Session\Querying\HowToQuerySpatialIndex.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **latitudePath** or **longitudePath** or **wktPath** | `Expression<Func<T, object>>` | Path to the field in a document containing either longitude, latitude or WKT |

### SpatialCriteriaFactory

{CODE spatial_3@ClientApi\Session\Querying\HowToQuerySpatialIndex.cs /}

| Parameter | Type | Description |
| ------------- | ------------- | ----- |
| **shapeWkt** | `string` | [WKT](https://en.wikipedia.org/wiki/Well-known_text_representation_of_geometry)-based shape to be used in operation |
| **relation** | `SpatialRelation` | Shape relation. Can be `Within`, `Contains`, `Disjoint`, `Intersects` |
| **distErrorPercent** | `double` | Maximum distance error tolerance in percents. Default: 0.025 |
| **radius** or **latitude** or **longitude** | `double` | Used to define a radius circle |
| **radiusUnits** or **units** | `SpatialUnits` | Determines if circle or shape should be calculated in `Kilometers` or `Miles` |

{INFO: Polygons}
When using `spatial.wkt()` to define a **polygon**, the vertices (points that form the corners of the polygon) must be listed 
in a counter-clockwise order:  
<br/>
![](images/spatial_1.png)
{INFO/}

### Example I

{CODE-TABS}
{CODE-TAB:csharp:Sync spatial_4@ClientApi\Session\Querying\HowToQuerySpatialIndex.cs /}
{CODE-TAB:csharp:Async spatial_4_1@ClientApi\Session\Querying\HowToQuerySpatialIndex.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from Houses
where spatial.within(spatial.point(Latitude, Longitude), spatial.circle(10, 32.1234, 23.4321))
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example II

{CODE-TABS}
{CODE-TAB:csharp:Sync spatial_5@ClientApi\Session\Querying\HowToQuerySpatialIndex.cs /}
{CODE-TAB:csharp:Async spatial_5_1@ClientApi\Session\Querying\HowToQuerySpatialIndex.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from Houses
where spatial.within(spatial.point(Latitude, Longitude), spatial.wkt('Circle(32.1234 23.4321 d=10.0000)', 'Miles'))
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL:OrderByDistance}

To sort by distance from given point use the `OrderByDistance` method. The closest results will come first.

{CODE spatial_6@ClientApi\Session\Querying\HowToQuerySpatialIndex.cs /}

| Parameter | Type | Description |
| ------------- | ------------- | ----- |
| **path** | `Expression<Func<T, object>>` | Path to spatial field in index |
| **fieldName** | `string` | Path to spatial field in index |
| **field** | `Func<DynamicSpatialFieldFactory<T>, DynamicSpatialField>` or `DynamicSpatialField` | Factory or field that points to a dynamic field (used with auto-indexes). Either `PointField` or `WktField` |
| **shapeWkt** | `string` | [WKT](https://en.wikipedia.org/wiki/Well-known_text_representation_of_geometry)-based shape to be used as a point from which distance will be measured. If the shape is not a single point, then the center of the shape will be used as a reference. |
| **latitude** or **longitude** | `double` | Used to define a point from which distance will be measured |
| **roundFactor** | `double` | A distance interval in kilometers. The distance from the point is rounded up to the nearest interval. The results within the same interval are then sorted by another order. This other order is the next [chained ordering](../../../indexes/querying/sorting#chaining-orderings), or if no other order was specified, then by ascending order of document Id. See example [below](../../../client-api/session/querying/how-to-query-a-spatial-index#example-ii-1). |

### Example I

{CODE-TABS}
{CODE-TAB:csharp:Sync spatial_7@ClientApi\Session\Querying\HowToQuerySpatialIndex.cs /}
{CODE-TAB:csharp:Async spatial_7_1@ClientApi\Session\Querying\HowToQuerySpatialIndex.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from Houses
where spatial.within(spatial.point(Latitude, Longitude), spatial.circle(10, 32.1234, 23.4321))
order by spatial.distance(spatial.point(Latitude, Longitude), spatial.point(32.1234, 23.4321))
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example II

This example demonstrates rounding. The query sorts the results by distance from 
the specified point, but rounds the distance up to the nearest 100 km interval. 
All results within the same interval are sorted alphabetically by the field `Name`.

{CODE-TABS}
{CODE-TAB:csharp:Sync spatial_7_2@ClientApi\Session\Querying\HowToQuerySpatialIndex.cs /}
{CODE-TAB:csharp:Async spatial_7_3@ClientApi\Session\Querying\HowToQuerySpatialIndex.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from Houses as h
order by spatial.distance(
    spatial.point(h.Latitude, h.Longitude),
    spatial.point(32.1234, 23.4321),
    100), h.Name
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL:OrderByDistanceDescending}

To sort by distance from given point use the `OrderByDistanceDescending` method. The farthest results will come first.

{CODE spatial_8@ClientApi\Session\Querying\HowToQuerySpatialIndex.cs /}

| Parameter | Type | Description |
| ------------- | ------------- | ----- |
| **path** | `Expression<Func<T, object>>` | Path to spatial field in an index |
| **fieldName** | `string` | Path to spatial field in an index |
| **field** | `Func<DynamicSpatialFieldFactory<T>, DynamicSpatialField>` or `DynamicSpatialField` | Factory or field that points to a dynamic field (used with auto-indexes). Either `PointField` or `WktField` |
| **shapeWkt** | `string` | WKT-based shape to be used as a point from which distance will be measured. If the shape is not a single point, then the center of the shape will be used as a reference. |
| **latitude** or **longitude** | `double` | Used to define a point from which distance will be measured |
| **roundFactor** | `double` | A distance interval in kilometers. The measured distance is rounded up to the nearest interval. The results within the same distance interval are then sorted by another order. This other order is the next [chained ordering](../../../indexes/querying/sorting#chaining-orderings), or if no other order was specified, then by ascending order of document Id. See example [above](../../../client-api/session/querying/how-to-query-a-spatial-index#example-ii-1). |

### Example

{CODE-TABS}
{CODE-TAB:csharp:Sync spatial_9@ClientApi\Session\Querying\HowToQuerySpatialIndex.cs /}
{CODE-TAB:csharp:Async spatial_9_1@ClientApi\Session\Querying\HowToQuerySpatialIndex.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from Houses
where spatial.within(spatial.point(Latitude, Longitude), spatial.circle(10, 32.1234, 23.4321))
order by spatial.distance(spatial.point(Latitude, Longitude), spatial.point(32.1234, 23.4321)) desc
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

## Remarks

{NOTE By default, distances are measured in **kilometers**. /}

## Related Articles

### Session

- [How to Query](../../../client-api/session/querying/how-to-query)

### Querying

- [Spatial](../../../indexes/querying/spatial)

### Indexes

- [Indexing Spatial Data](../../../indexes/indexing-spatial-data) 
