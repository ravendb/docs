# Session : Querying : How to query a spatial index?

Spatial indexes can be queried using `Spatial` method which contains full spectrum of spatial methods. Following article will cover those methods:

- [Spatial](../../../client-api/session/querying/how-to-query-a-spatial-index#spatial)
- [OrderByDistance](../../../client-api/session/querying/how-to-query-a-spatial-index#orderbydistance)
- [OrderByDistanceDescending](../../../client-api/session/querying/how-to-query-a-spatial-index#orderbydistancedescending)

{PANEL:Spatial}

### Syntax

{CODE spatial_1@ClientApi\Session\Querying\HowToQuerySpatialIndex.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **path** | Expression<Func<T, object>> | Path to spatial field in index |
| **fieldName** | string | Path to spatial field in index |
| **field** | Func<DynamicSpatialFieldFactory<T>, DynamicSpatialField> or DynamicSpatialField | Factory or field that points to a dynamic field (used with auto-indexes). Either `PointField` or `WktField` |
| **clause** | Func<SpatialCriteriaFactory, SpatialCriteria> | Spatial criteria that will be executed on given spatial field from `path` parameter. |

### DynamicSpatialFieldFactory

{CODE spatial_2@ClientApi\Session\Querying\HowToQuerySpatialIndex.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **latitudePath** or **longitudePath** or **wktPath** | Expression<Func<T, object>> | Path to field in document containing either longitude, latitude or WKT |

### SpatialCriteriaFactory

{CODE spatial_3@ClientApi\Session\Querying\HowToQuerySpatialIndex.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **shapeWkt** | string | WKT-based shape to be used in operation |
| **relation** | SpatialRelation | Shape relation. Can be `Within`, `Contains`, `Disjoint`, `Intersects` |
| **distErrorPercent** | double | Maximum distance error tolerance in percents. Default: 0.025 |
| **radius** or **latitude** or **longitude** | double | Used to define a radius circle |
| **radiusUnits** | SpatialUnits | Determines if circle should be calculated to `Kilometers` or `Miles` units |

### Example I

{CODE-TABS}
{CODE-TAB:csharp:Sync spatial_4@ClientApi\Session\Querying\HowToQuerySpatialIndex.cs /}
{CODE-TAB:csharp:Async spatial_4_1@ClientApi\Session\Querying\HowToQuerySpatialIndex.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from Houses
where spatial.within(spatial.point(Latitude, Longitude), spatial.circle(10, 32.1234. 23.4321))
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example II

{CODE-TABS}
{CODE-TAB:csharp:Sync spatial_5@ClientApi\Session\Querying\HowToQuerySpatialIndex.cs /}
{CODE-TAB:csharp:Async spatial_5_1@ClientApi\Session\Querying\HowToQuerySpatialIndex.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from Houses
where spatial.within(spatial.point(Latitude, Longitude), spatial.wkt('Circle(32.1234 23.4321 d=10.0000)'))
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL:OrderByDistance}

To sort by distance from given point use `OrderByDistance` method. The closest results will come first.

{CODE spatial_6@ClientApi\Session\Querying\HowToQuerySpatialIndex.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **path** | Expression<Func<T, object>> | Path to spatial field in index |
| **fieldName** | string | Path to spatial field in index |
| **field** | Func<DynamicSpatialFieldFactory<T>, DynamicSpatialField> or DynamicSpatialField | Factory or field that points to a dynamic field (used with auto-indexes). Either `PointField` or `WktField` |
| **shapeWkt** | string | WKT-based shape to be used as a point from which distance will be measured. If shape is not a single point then center of the shape will be used as a reference. |
| **latitude** or **longitude** | double | Used to define a point from which distance will be measured |

### Example

{CODE-TABS}
{CODE-TAB:csharp:Sync spatial_7@ClientApi\Session\Querying\HowToQuerySpatialIndex.cs /}
{CODE-TAB:csharp:Async spatial_7_1@ClientApi\Session\Querying\HowToQuerySpatialIndex.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from Houses
where spatial.within(spatial.point(Latitude, Longitude), spatial.circle(10, 32.1234. 23.4321))
order by spatial.distance(spatial.point(Latitude, Longitude), spatial.point(32.1234, 23.4321))
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL:OrderByDistanceDescending}

To sort by distance from given point use `OrderByDistanceDescending` method. The farthest results will come first.

{CODE spatial_8@ClientApi\Session\Querying\HowToQuerySpatialIndex.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **path** | Expression<Func<T, object>> | Path to spatial field in index |
| **fieldName** | string | Path to spatial field in index |
| **field** | Func<DynamicSpatialFieldFactory<T>, DynamicSpatialField> or DynamicSpatialField | Factory or field that points to a dynamic field (used with auto-indexes). Either `PointField` or `WktField` |
| **shapeWkt** | string | WKT-based shape to be used as a point from which distance will be measured. If shape is not a single point then center of the shape will be used as a reference. |
| **latitude** or **longitude** | double | Used to define a point from which distance will be measured |

### Example

{CODE-TABS}
{CODE-TAB:csharp:Sync spatial_9@ClientApi\Session\Querying\HowToQuerySpatialIndex.cs /}
{CODE-TAB:csharp:Async spatial_9_1@ClientApi\Session\Querying\HowToQuerySpatialIndex.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from Houses
where spatial.within(spatial.point(Latitude, Longitude), spatial.circle(10, 32.1234. 23.4321))
order by spatial.distance(spatial.point(Latitude, Longitude), spatial.point(32.1234, 23.4321)) desc
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

## Remarks

{NOTE By default, distances are measured in **kilometers**. /}

## Related articles

- [Indexes : Indexing spatial data](../../../indexes/indexing-spatial-data)   
- [Indexes : Querying : Spatial](../../../indexes/querying/spatial)   
