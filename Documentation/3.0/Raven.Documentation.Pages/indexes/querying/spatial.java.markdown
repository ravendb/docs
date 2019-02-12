# Spatial

{INFO This article focuses only on **querying** side of spatial search. If you want to read how to index spatial data, click [here](../../indexes/indexing-spatial-data). /}

To perform a spatial search you can use `spatial` method:

{CODE-TABS}
{CODE-TAB:java:Query spatial_search_enhancements_9@Indexes\SpatialIndexes.java /}
{CODE-TAB:java:DocumentQuery spatial_search_enhancements_1_0@Indexes\SpatialIndexes.java /}
{CODE-TAB:java:Index spatial_search_enhancements_8@Indexes\SpatialIndexes.java /}
{CODE-TABS/}

Under `criteria` following methods are available:

{CODE:java spatial_search_enhancements_a@Indexes\SpatialIndexes.java /}

{WARNING:Obsolete method}
Since version 3.0.3699-Unstable `withinRadiusOf` method is marked as obsolete because of parameter order inconsistency. Use `withinRadius` instead.
{WARNING/}

## Radius search

The most basic usage and probably most common one is to search for all points or shapes within provided distance from the given center point. To perform this search we will use `withinRadiusOf` method that is a part of query customizations.

{CODE-TABS}
{CODE-TAB:java:Query spatial_search_3@Indexes\SpatialIndexes.java /}
{CODE-TAB:java:DocumentQuery spatial_search_8@Indexes\SpatialIndexes.java /}
{CODE-TAB:java:Index spatial_search_2@Indexes\SpatialIndexes.java /}
{CODE-TABS/}

## Advanced search

The `withinRadiusOf` method is a wrapper around `relatesToShape` method.

{CODE:java spatial_search_5@Indexes\SpatialIndexes.java /}

{CODE:java spatial_search_7@Indexes\SpatialIndexes.java /}

where first parameter is a name of the field containing the shape to use for filtering, next one is a shape in [WKT](https://en.wikipedia.org/wiki/Well-known_text_representation_of_geometry) format and the last one is a spatial relation type.

So to perform a radius search from the above example and use `relatesToShape` method, we do as follows

{CODE-TABS}
{CODE-TAB:java:Query spatial_search_4@Indexes\SpatialIndexes.java /}
{CODE-TAB:java:DocumentQuery spatial_search_9@Indexes\SpatialIndexes.java /}
{CODE-TAB:java:Index spatial_search_2@Indexes\SpatialIndexes.java /}
{CODE-TABS/}

{WARNING Distance in RavenDB by default is measured in **kilometers**. /}

## Related articles

- [Indexes : Indexing spatial data](../../indexes/indexing-spatial-data)
- [Client API : Session : How to query a spatial index?](../../client-api/session/querying/how-to-query-a-spatial-index)
