# Session: Querying: How to Query a Spatial Index

Spatial indexes can be queried using the `spatial()` method which contains a full spectrum of spatial methods. The following article will cover these methods:

- [spatial()](../../../client-api/session/querying/how-to-query-a-spatial-index#spatial)
- [orderByDistance()](../../../client-api/session/querying/how-to-query-a-spatial-index#orderbydistance)
- [orderByDistanceDescending()](../../../client-api/session/querying/how-to-query-a-spatial-index#orderbydistancedescending)

{PANEL:Spatial}

{CODE:nodejs spatial_1@client-api\session\querying\howToQuerySpatialIndex.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **fieldName** | string | Path to spatial field in an index |
| **field** | `DynamicSpatialField`  | Field that points to a dynamic field (used with auto-indexes). Either `PointField` or `WktField` |
| **clause** | `(spatialCriteriaFactory) => spatialCriteria` | Spatial criteria builder |

### DynamicSpatialField  

{CODE:nodejs spatial_2@client-api\session\querying\howToQuerySpatialIndex.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **latitude** or **longitude** or **wkt** | string | Path to the field in a document containing either longitude, latitude or WKT |

### SpatialCriteriaFactory

{CODE:nodejs spatial_3@client-api\session\querying\howToQuerySpatialIndex.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **shapeWkt** | string | WKT-based shape to be used in operation |
| **relation** | `SpatialRelation` | Shape relation. Can be `Within`, `Contains`, `Disjoint`, `Intersects` |
| **distErrorPercent** | number | Maximum distance error tolerance in percents. Default: *0.025* |
| **radius** or **latitude** or **longitude** | number | Used to define a radius circle |
| **radiusUnits** | `SpatialUnits` | Determines if circle should be calculated in `Kilometers` or `Miles` units |

### Example I

{CODE-TABS}
{CODE-TAB:nodejs:Node.js spatial_4@client-api\session\querying\howToQuerySpatialIndex.js /}
{CODE-TAB-BLOCK:sql:RQL}
from Houses
where spatial.within(spatial.point(latitude, longitude), spatial.circle(10, 32.1234. 23.4321))
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example II

{CODE-TABS}
{CODE-TAB:nodejs:Node.js spatial_5@client-api\session\querying\howToQuerySpatialIndex.js /}
{CODE-TAB-BLOCK:sql:RQL}
from Houses
where spatial.within(spatial.point(latitude, longitude), spatial.wkt('Circle(32.1234 23.4321 d=10.0000)'))
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL:OrderByDistance}

To sort by distance from given point use the `orderByDistance()` method. The closest results will come first.

{CODE:nodejs spatial_6@client-api\session\querying\howToQuerySpatialIndex.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **fieldName** | string | Path to spatial field in index |
| **field** | `DynamicSpatialField` | Field that points to a dynamic field (used with auto-indexes). Either `PointField` or `WktField` |
| **shapeWkt** | string | WKT-based shape to be used as a point from which distance will be measured. If the shape is not a single point, then the center of the shape will be used as a reference. |
| **latitude** or **longitude** | number | Used to define a point from which distance will be measured |

### Example

{CODE-TABS}
{CODE-TAB:nodejs:Node.js spatial_7@client-api\session\querying\howToQuerySpatialIndex.js /}
{CODE-TAB-BLOCK:sql:RQL}
from Houses
where spatial.within(spatial.point(latitude, longitude), spatial.circle(10, 32.1234. 23.4321))
order by spatial.distance(spatial.point(latitude, longitude), spatial.point(32.1234, 23.4321))
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL:OrderByDistanceDescending}

To sort by distance from given point use the `orderByDistanceDescending()` method. The farthest results will come first.

{CODE:nodejs spatial_8@client-api\session\querying\howToQuerySpatialIndex.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **fieldName** | string | Path to spatial field in index |
| **field** | `DynamicSpatialField` | Field that points to a dynamic field (used with auto-indexes). Either `PointField` or `WktField` |
| **shapeWkt** | string | WKT-based shape to be used as a point from which distance will be measured. If the shape is not a single point, then the center of the shape will be used as a reference. |
| **latitude** or **longitude** | number | Used to define a point from which distance will be measured |

### Example

{CODE-TABS}
{CODE-TAB:nodejs:Node.js spatial_9@client-api\session\querying\howToQuerySpatialIndex.js /}
{CODE-TAB-BLOCK:sql:RQL}
from Houses
where spatial.within(spatial.point(latitude, longitude), spatial.circle(10, 32.1234. 23.4321))
order by spatial.distance(spatial.point(latitude, longitude), spatial.point(32.1234, 23.4321)) desc
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
