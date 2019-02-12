# Spatial

{INFO This article focuses only on **querying** side of spatial search. If you want to read how to index spatial data, click [here](../../indexes/indexing-spatial-data). /}

To perform a spatial search you can use `Spatial` method:

{CODE-TABS}
{CODE-TAB:csharp:Query spatial_search_enhancements_9@Indexes\SpatialIndexes.cs /}
{CODE-TAB:csharp:DocumentQuery spatial_search_enhancements_1_0@Indexes\SpatialIndexes.cs /}
{CODE-TAB:csharp:Index spatial_search_enhancements_8@Indexes\SpatialIndexes.cs /}
{CODE-TABS/}

Under `criteria` following methods are available:

{CODE spatial_search_enhancements_a@Indexes\SpatialIndexes.cs /}

{WARNING:Obsolete method}
Since version 3.0.3699-Unstable `WithinRadiusOf` method is marked as obsolete because of parameter order inconsistency. Use `WithinRadius` instead.
{WARNING/}

## Radius search

The most basic usage and probably most common one is to search for all points or shapes within provided distance from the given center point. To perform this search we will use `WithinRadiusOf` method that is a part of query customizations.

{CODE-TABS}
{CODE-TAB:csharp:Query spatial_search_3@Indexes\SpatialIndexes.cs /}
{CODE-TAB:csharp:DocumentQuery spatial_search_8@Indexes\SpatialIndexes.cs /}
{CODE-TAB:csharp:Index spatial_search_2@Indexes\SpatialIndexes.cs /}
{CODE-TABS/}

## Advanced search

The `WithinRadiusOf` method is a wrapper around `RelatesToShape` method.

{CODE spatial_search_5@Indexes\SpatialIndexes.cs /}

{CODE spatial_search_7@Indexes\SpatialIndexes.cs /}

where first parameter is a name of the field containing the shape to use for filtering, next one is a shape in [WKT](https://en.wikipedia.org/wiki/Well-known_text_representation_of_geometry) format and the last one is a spatial relation type.

So to perform a radius search from the above example and use `RelatesToShape` method, we do as follows

{CODE-TABS}
{CODE-TAB:csharp:Query spatial_search_4@Indexes\SpatialIndexes.cs /}
{CODE-TAB:csharp:DocumentQuery spatial_search_9@Indexes\SpatialIndexes.cs /}
{CODE-TAB:csharp:Index spatial_search_2@Indexes\SpatialIndexes.cs /}
{CODE-TABS/}

{WARNING Distance in RavenDB by default is measured in **kilometers**. /}

## Related articles

- [Indexes : Indexing spatial data](../../indexes/indexing-spatial-data)
- [Client API : Session : How to query a spatial index?](../../client-api/session/querying/how-to-query-a-spatial-index)
