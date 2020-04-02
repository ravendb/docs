# Session: Querying: How to Query a Spatial Index

Spatial indexes can be queried using the `spatial` method which contains a full spectrum of spatial methods. The following article will cover these methods:

- [Spatial](../../../client-api/session/querying/how-to-query-a-spatial-index#spatial)
- [OrderByDistance](../../../client-api/session/querying/how-to-query-a-spatial-index#orderbydistance)
- [OrderByDistanceDescending](../../../client-api/session/querying/how-to-query-a-spatial-index#orderbydistancedescending)

{PANEL:Spatial}

{CODE:java spatial_1@ClientApi\Session\Querying\HowToQuerySpatialIndex.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **fieldName** | String | Path to spatial field in an index |
| **field** | DynamicSpatialField  | Field that points to a dynamic field (used with auto-indexes). Either `PointField` or `WktField` |
| **clause** | Function<SpatialCriteriaFactory, SpatialCriteria> | Spatial criteria |

### DynamicSpatialField  

{CODE:java spatial_2@ClientApi\Session\Querying\HowToQuerySpatialIndex.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **latitude** or **longitude** or **wkt** | String | Path to the field in a document containing either longitude, latitude or WKT |

### SpatialCriteriaFactory

{CODE:java spatial_3@ClientApi\Session\Querying\HowToQuerySpatialIndex.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **shapeWkt** | String | [WKT](https://en.wikipedia.org/wiki/Well-known_text_representation_of_geometry)-based shape to be used in operation |
| **relation** | SpatialRelation | Shape relation. Can be `WITHIN`, `CONTAINS`, `DISJOINT`, `INTERSECTS` |
| **distErrorPercent** | double | Maximum distance error tolerance in percents. Default: 0.025 |
| **radius** or **latitude** or **longitude** | double | Used to define a radius circle |
| **radiusUnits** | SpatialUnits | Determines if circle should be calculated in `KILOMETERS` or `MILES` units |

{INFO: Polygons}
When using `spatial.wkt()` to define a **polygon**, the vertices (points that form the corners of the polygon) must be listed 
in a counter-clockwise order:  
<br/>
![](images/spatial_1.png)
{INFO/}

### Example I

{CODE-TABS}
{CODE-TAB:java:Java spatial_4@ClientApi\Session\Querying\HowToQuerySpatialIndex.java /}
{CODE-TAB-BLOCK:sql:RQL}
from Houses
where spatial.within(spatial.point(Latitude, Longitude), spatial.circle(10, 32.1234. 23.4321))
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example II

{CODE-TABS}
{CODE-TAB:java:Java spatial_5@ClientApi\Session\Querying\HowToQuerySpatialIndex.java /}
{CODE-TAB-BLOCK:sql:RQL}
from Houses
where spatial.within(spatial.point(Latitude, Longitude), spatial.wkt('Circle(32.1234 23.4321 d=10.0000)'))
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL:OrderByDistance}

To sort by distance from given point use the `orderByDistance` method. The closest results will come first.

{CODE:java spatial_6@ClientApi\Session\Querying\HowToQuerySpatialIndex.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **fieldName** | String | Path to spatial field in index |
| **field** | DynamicSpatialField | Field that points to a dynamic field (used with auto-indexes). Either `PointField` or `WktField` |
| **shapeWkt** | String | [WKT](https://en.wikipedia.org/wiki/Well-known_text_representation_of_geometry)-based shape to be used as a point from which distance will be measured. If the shape is not a single point, then the center of the shape will be used as a reference. |
| **latitude** or **longitude** | double | Used to define a point from which distance will be measured |

### Example

{CODE-TABS}
{CODE-TAB:java:Java spatial_7@ClientApi\Session\Querying\HowToQuerySpatialIndex.java /}
{CODE-TAB-BLOCK:sql:RQL}
from Houses
where spatial.within(spatial.point(Latitude, Longitude), spatial.circle(10, 32.1234. 23.4321))
order by spatial.distance(spatial.point(Latitude, Longitude), spatial.point(32.1234, 23.4321))
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL:OrderByDistanceDescending}

To sort by distance from given point use the `OrderByDistanceDescending` method. The farthest results will come first.

{CODE:java spatial_8@ClientApi\Session\Querying\HowToQuerySpatialIndex.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **fieldName** | String | Path to spatial field in index |
| **field** | DynamicSpatialField | Field that points to a dynamic field (used with auto-indexes). Either `PointField` or `WktField` |
| **shapeWkt** | String | WKT-based shape to be used as a point from which distance will be measured. If the shape is not a single point, then the center of the shape will be used as a reference. |
| **latitude** or **longitude** | double | Used to define a point from which distance will be measured |

### Example

{CODE-TABS}
{CODE-TAB:java:Java spatial_9@ClientApi\Session\Querying\HowToQuerySpatialIndex.java /}
{CODE-TAB-BLOCK:sql:RQL}
from Houses
where spatial.within(spatial.point(Latitude, Longitude), spatial.circle(10, 32.1234. 23.4321))
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
