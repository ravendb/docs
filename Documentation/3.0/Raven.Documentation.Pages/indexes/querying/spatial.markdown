# Spatial

Beside the `Event` class let us add `SpatialDoc` with a corresponding index to show how to do a strongly-typed spatial query using `Spatial` method.

{CODE spatial_search_enhancements_8@Indexes\SpatialIndexes.cs /}

{CODE spatial_search_enhancements_9@Indexes\SpatialIndexes.cs /}

The methods available under `criteria` are:   

{CODE spatial_search_enhancements_a@Indexes\SpatialIndexes.cs /}

## Radius search

The most basic usage and probably most common one is to search for all points or shapes within provided distance from the given center point. To perform this search we will use `WithinRadiusOf` method that is a part of query customizations.

{CODE-TABS}
{CODE-TAB:csharp:Query spatial_search_3@Indexes\SpatialIndexes.cs /}
{CODE-TAB:csharp:DocumentQuery spatial_search_8@Indexes\SpatialIndexes.cs /}
{CODE-TABS/}

## Advanced search

The `WithinRadiusOf` method is a wrapper around `RelatesToShape` method.

{CODE spatial_search_5@Indexes\SpatialIndexes.cs /}

{CODE spatial_search_7@Indexes\SpatialIndexes.cs /}

where first parameter is a name of the field containing the shape to use for filtering, next one is a shape in [WKT](http://en.wikipedia.org/wiki/Well-known_text) format and the last one is a spatial relation type.

So to perform a radius search from the above example and use `RelatesToShape` method, we do as follows

{CODE-TABS}
{CODE-TAB:csharp:Query spatial_search_4@Indexes\SpatialIndexes.cs /}
{CODE-TAB:csharp:DocumentQuery spatial_search_9@Indexes\SpatialIndexes.cs /}
{CODE-TABS/}

{WARNING From RavenDB 2.0 the distance by default is measured in **kilometers** in contrast to the miles used in previous versions. /}

## Related articles

TODO