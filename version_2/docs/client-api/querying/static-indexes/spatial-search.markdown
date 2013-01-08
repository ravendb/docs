#Spatial Search

To support the ability to retrieve the data based on spatial coordinates, the spatial search have been introduced.

##Creating Indexes

To take an advantage of spatial search, first we need to create an index with spatial field. To mark field as a spatial field, we need to use `SpatialGenerate` method

{CODE spatial_search_0@ClientApi/Querying/StaticIndexes/SpatialSearch.cs /}

{CODE spatial_search_6@ClientApi/Querying/StaticIndexes/SpatialSearch.cs /}

where:   

*	**fieldName** is a name of the field containing the shape to use for filtering (if the overload with no `fieldName` is used, then the name is set to default value: `__spatial`)          
*	**lat/lng** are latitude/longitude coordinates   
*	**shapeWKT** is a shape in [WKT](http://en.wikipedia.org/wiki/Well-known_text) format    
*	**strategy** is a spatial search strategy
*	**maxTreeLevel** is a integer that indicates the maximum number of levels to be used in `PrefixTree` and controls the precision of shape representation   

In our example we will use `Event` class and very simple index defined below.

{CODE spatial_search_1@ClientApi/Querying/StaticIndexes/SpatialSearch.cs /}

{CODE spatial_search_2@ClientApi/Querying/StaticIndexes/SpatialSearch.cs /}

{NOTE `GeohashPrefixTree` is a default `SpatialSearchStrategy`. Doing any changes to the strategy after index has been created will trigger re-indexation process. /}

##Radius search

The most basic usage and probably most common one is to search for all points or shapes within provided distance from the given center point. To perform this search we will use `WithinRadiusOf` method that is a part of query customizations.

{CODE spatial_search_3@ClientApi/Querying/StaticIndexes/SpatialSearch.cs /}

##Advanced search

The `WithinRadiusOf` method is a wrapper around `RelatesToShape` method.

{CODE spatial_search_5@ClientApi/Querying/StaticIndexes/SpatialSearch.cs /}

{CODE spatial_search_7@ClientApi/Querying/StaticIndexes/SpatialSearch.cs /}

where first parameter is a name of the field containing the shape to use for filtering, next one is a shape in [WKT](http://en.wikipedia.org/wiki/Well-known_text) format and the last one is a spatial relation type.

So to perform a radius search from the above example and use `RelatesToShape` method, we do as follows

{CODE spatial_search_4@ClientApi/Querying/StaticIndexes/SpatialSearch.cs /}

{WARNING From RavenDB 2.0 the distance is measured in **kilometers** in contrast to the miles used in previous versions. /}





