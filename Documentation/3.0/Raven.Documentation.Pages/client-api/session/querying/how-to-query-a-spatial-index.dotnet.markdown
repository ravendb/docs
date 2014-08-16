# Session : Querying : How to query a spatial index?

Spatial indexes can be queried using `Spatial` method which contains full spectrum of spatial methods, certain shortcuts have been created for easier access and are available in `Customize` as a part of query customizations. Following article will cover those methods:

- [Spatial](../../../client-api/session/querying/how-to-query-a-spatial-index#spatial)
- [RelatesToShape](../../../client-api/session/querying/how-to-query-a-spatial-index#relatestoshape)
- [SortByDistance](../../../client-api/session/querying/how-to-query-a-spatial-index#sortbydistance)
- [WithinRadiusOf](../../../client-api/session/querying/how-to-query-a-spatial-index#withinradiusof)

## Spatial

{CODE spatial_1@ClientApi\Session\Querying\HowToQuerySpatialIndex.cs /}

**Parameters**   

path
:   Type: Expression<Func&lt;TResult, object&gt;>   
Path to spatial field.

clause
:   Type: Func<[SpatialCriteriaFactory](../../../glossary/client-api/querying/spatial-criteria-factory), SpatialCriteria>   
Spatial criteria that will be executed on given spatial field from `path` parameter.

**Return value**

Type: IRavenQueryable   
Object instance implementing IRavenQueryable interface containing additional query methods and extensions.

### Example

{CODE spatial_2@ClientApi\Session\Querying\HowToQuerySpatialIndex.cs /}

## RelatesToShape

`RelatesToShape` is low-level method available for spatial searches. You can pass any valid [WKT](http://en.wikipedia.org/wiki/Well-known_text) shape with any relation.  All other methods sooner or later are using it to define a shape.

{CODE spatial_3@ClientApi\Session\Querying\HowToQuerySpatialIndex.cs /}

**Parameters**   

fieldName
:   Type: string   
Spatial field name.

shapeWKT
:   Type: string   
[WKT](http://en.wikipedia.org/wiki/Well-known_text) formatted shape.

rel
:   Type: [SpatialRelation](../../../glossary/client-api/querying/spatial-relation)   
Spatial relation to check.

**Return value**

Type: IDocumentQueryCustomization   
Returns self for easier method chaining.

### Example

{CODE spatial_4@ClientApi\Session\Querying\HowToQuerySpatialIndex.cs /}

## SortByDistance

To sort by distance from origin point use `SortByDistance` method.

{CODE spatial_5@ClientApi\Session\Querying\HowToQuerySpatialIndex.cs /}

**Return value**

Type: IDocumentQueryCustomization   
Returns self for easier method chaining.

### Example

{CODE spatial_6@ClientApi\Session\Querying\HowToQuerySpatialIndex.cs /}

## WithinRadiusOf

`WithinRadiusOf` filter matches to be inside specified radius. Internally it creates a circle with passed latitude and longitude as a center point with given distance (radius) and `SpatialRelation` set to `Within`.

{CODE spatial_7@ClientApi\Session\Querying\HowToQuerySpatialIndex.cs /}

**Parameters**   

fieldName
:   Type: string   
Spatial field name. In overloads without this parameter default field name is assumed (`__spatial`).

radius
:   Type: double   
Circle radius.

latitude
:   Type: double   
Latitude pointing to circle center.

longitude
:   Type: double   
Longitude pointing to circle center.

spatialUnits
:   Type: SpatialUnits   
Units that will be used to measure distance (`Kilometers` or `Miles`).

**Return value**

Type: IDocumentQueryCustomization   
Returns self for easier method chaining.

### Example

{CODE spatial_8@ClientApi\Session\Querying\HowToQuerySpatialIndex.cs /}

## Remarks

{NOTE By default, distances are measured in **kilometers**. /}

#### Related articles

TODO