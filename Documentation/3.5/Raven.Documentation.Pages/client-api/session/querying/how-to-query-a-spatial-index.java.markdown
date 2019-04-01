# Session: Querying: How to query a spatial index?

Spatial indexes can be queried using `spatial` method which contains full spectrum of spatial methods, certain shortcuts have been created for easier access and are available in `customize` as a part of query customizations. Following article will cover those methods:

- [spatial](../../../client-api/session/querying/how-to-query-a-spatial-index#spatial)
- [relatesToShape](../../../client-api/session/querying/how-to-query-a-spatial-index#relatestoshape)
- [sortByDistance](../../../client-api/session/querying/how-to-query-a-spatial-index#sortbydistance)
- [withinRadiusOf](../../../client-api/session/querying/how-to-query-a-spatial-index#withinradiusof)

{PANEL:Spatial}

{CODE:java spatial_1@ClientApi\Session\Querying\HowToQuerySpatialIndex.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **path** | Path<?> | Path to spatial field. |
| **clause** | [SpatialCriteria](../../../glossary/spatial-criteria-factory) | Spatial criteria that will be executed on given spatial field from `path` parameter. |

| Return Value | |
| ------------- | ----- |
| IRavenQueryable | Object instance implementing IRavenQueryable interface containing additional query methods and extensions. |

### Example

{CODE:java spatial_2@ClientApi\Session\Querying\HowToQuerySpatialIndex.java /}

{PANEL/}

{PANEL:RelatesToShape}

`relatesToShape` is low-level method available for spatial searches. You can pass any valid [WKT](https://en.wikipedia.org/wiki/Well-known_text_representation_of_geometry) shape with any relation.  All other methods sooner or later are using it to define a shape.

{CODE:java spatial_3@ClientApi\Session\Querying\HowToQuerySpatialIndex.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **fieldName** | String | Spatial field name. |
| **shapeWKT** | String | [WKT](https://en.wikipedia.org/wiki/Well-known_text_representation_of_geometry) formatted shape. |
| **rel** | [SpatialRelation](../../../glossary/spatial-relation) | Spatial relation to check. |

| Return Value | |
| ------------- | ----- |
| IDocumentQueryCustomization | Returns self for easier method chaining. |

### Example

{CODE:java spatial_4@ClientApi\Session\Querying\HowToQuerySpatialIndex.java /}

{PANEL/}

{PANEL:SortByDistance}

To sort by distance from origin point use `sortByDistance` method.

{CODE:java spatial_5@ClientApi\Session\Querying\HowToQuerySpatialIndex.java /}

| Return Value | |
| ------------- | ----- |
| IDocumentQueryCustomization | Returns self for easier method chaining. |

### Example

{CODE:java spatial_6@ClientApi\Session\Querying\HowToQuerySpatialIndex.java /}

{PANEL/}

{PANEL:WithinRadiusOf}

`WithinRadiusOf` filter matches to be inside specified radius. Internally it creates a circle with passed latitude and longitude as a center point with given distance (radius) and `SpatialRelation` set to `Within`.

{CODE:java spatial_7@ClientApi\Session\Querying\HowToQuerySpatialIndex.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **fieldName** | String | Spatial field name. In overloads without this parameter default field name is assumed (`__spatial`). |
| **radius** | double | Circle radius. | 
| **latitude** | double | Latitude pointing to circle center. |
| **longitude** | double | Longitude pointing to circle center. |
| **spatialUnits** | SpatialUnits | Units that will be used to measure distance (`Kilometers` or `Miles`). |

| Return Value | |
| ------------- | ----- |
| IDocumentQueryCustomization | Returns self for easier method chaining. |

### Example

{CODE:java spatial_8@ClientApi\Session\Querying\HowToQuerySpatialIndex.java /}

{PANEL/}

## Remarks

{NOTE By default, distances are measured in **kilometers**. /}

## Related articles

- [Indexes : Indexing spatial data](../../../indexes/indexing-spatial-data)   
- [Indexes : Querying : Spatial](../../../indexes/querying/spatial)   
