# Indexing spatial data

To support the ability to retrieve the data based on spatial coordinates, the spatial search has been introduced.

## Creating indexes

To take an advantage of the spatial search, first we need to create an index with a spatial field. To mark field as the spatial field, we need to use `SpatialGenerate` method:

{CODE spatial_search_0@Indexes\SpatialIndexes.cs /}

{CODE spatial_search_6@Indexes\SpatialIndexes.cs /}

where:   

*	**fieldName** is a name of the field containing the shape to use for filtering (if the overload with no `fieldName` is used, then the name is set to default value: `__spatial`)          
*	**lat/lng** are latitude/longitude coordinates   
*	**shapeWKT** is a shape in the [WKT](https://en.wikipedia.org/wiki/Well-known_text_representation_of_geometry) format    
*	**strategy** is a spatial search strategy (default: `GeohashPrefixTree`)
*	**maxTreeLevel** is a integer that indicates the maximum number of levels to be used in the `PrefixTree` and controls the precision of shape representation (**9** for `GeohashPrefixTree` and **23** for `QuadPrefixTree`)      

In our example we will use `Event` class and a very simple index defined below.

{CODE spatial_search_1@Indexes\SpatialIndexes.cs /}

{CODE spatial_search_2@Indexes\SpatialIndexes.cs /}

If our `Event` contains the WKT property already:   

{CODE spatial_search_enhancements_1@Indexes\SpatialIndexes.cs /}

then can define our field using the `Spatial` method in the `AbstractIndexCreationTask`:   

{CODE spatial_search_enhancements_2@Indexes\SpatialIndexes.cs /}

where under `options` we got access to our geography and Cartesian factories:   

{CODE spatial_search_enhancements_3@Indexes\SpatialIndexes.cs /}

{CODE-TABS}
{CODE-TAB:csharp:GeographySpatialOptionsFactory spatial_search_enhancements_4@Indexes\SpatialIndexes.cs /}
{CODE-TAB:csharp:CartesianSpatialOptionsFactory spatial_search_enhancements_5@Indexes\SpatialIndexes.cs /}
{CODE-TABS/}

## Spatial search strategies

{PANEL:GeohashPrefixTree}
Geohash is a latitude/longitude representation system that describes earth as a grid with 32 cells, assigning an alphanumeric character to each grid cell. Each grid cell is further divided into 32 smaller chunks, and each chunk has an alphanumeric character assigned as well, and so on.

E.g. The location of 'New York' in the United States is represented by the following geohash: [DR5REGY6R](http://geohash.org/dr5regy6r) and it represents the `40.7144 -74.0060` coordinates. Removing characters from the end of geohash will decrease the precision level.

More information about geohash uses, decoding algorithm and limitations can be found [here](https://en.wikipedia.org/wiki/Geohash).
{PANEL/}

{PANEL:QuadPrefixTree}
QuadTree represents earth as a grid with exactly four cells and similarly to geohash, each grid cell (sometimes called bucket) has a letter assigned and is divided further into 4 more cells and so on.

More information about QuadTree can be found [here](https://en.wikipedia.org/wiki/Quadtree).
{PANEL/}

{PANEL:BoundingBox}
More information about BoundingBox can be found [here](https://en.wikipedia.org/wiki/Minimum_bounding_rectangle).
{PANEL/}

{WARNING `GeohashPrefixTree` is a default `SpatialSearchStrategy`. Doing any changes to the strategy after index has been created will trigger re-indexation process. /}

### Precision

By default the precision level (`maxTreeLevel`) for GeohashPrefixTree is set to **9** and for QuadPrefixTree the value is **23**, which means that the coordinates are represented by a 9 or 23 character string. The difference exists, because the `QuadTree` representation would be much less precise if the level would be the same.

{PANEL:Geohash precision values}
Source: unterbahn.com

| Level | E-W distance at equator | N-S distance at equator |
|:----- |:------------------------|:------------------------|
| 12    | ~3.7cm                  | ~1.8cm                  |
| 11    | ~14.9cm                 | ~14.9cm                 |
| 10    | ~1.19m                  | ~0.60m                  |
| **9** | **~4.78m**              | **~4.78m**              |
| 8     | ~38.2m                  | ~19.1m                  |
| 7     | ~152.8m                 | ~152.8m                 |
| 6     | ~1.2km                  | ~0.61km                 |
| 5     | ~4.9km                  | ~4.9km                  |
| 4     | ~39km                   | ~19.6km                 |
| 3     | ~157km                  | ~157km                  |
| 2     | ~1252km                 | ~626km                  |
| 1     | ~5018km                 | ~5018km                 |

{PANEL/}

{PANEL:Quadtree precision values}

| Level | Distance at equator |
|:-------|:-------------------|
| 30     | ~4cm               |
| 29     | ~7cm               |
| 28     | ~15cm              |
| 27     | ~30cm              |
| 26     | ~60cm              |
| 25     | ~1.19m             |
| 24     | ~2.39m             |
| **23** | **~4.78m**         |
| 22     | ~9.56m             |
| 21     | ~19.11m            |
| 20     | ~38.23m            |
| 19     | ~76.23m            |
| 18     | ~152.92m           |
| 17     | ~305.84m           |
| 16     | ~611.67m           |
| 15     | ~1.22km            |
| 14     | ~2.45km            |
| 13     | ~4.89km            |
| 12     | ~9.79km            |
| 11     | ~19.57km           |
| 10     | ~39.15km           |
| 9      | ~78.29km           |
| 8      | ~156.58km          |
| 7      | ~313.12km          |
| 6      | ~625.85km          |
| 5      | ~1249km            |
| 4      | ~2473km            |
| 3      | ~4755km            |
| 2      | ~7996km            |
| 1      | ~15992km           |

{PANEL/}

## Format support

From version 2.5 RavenDB also supports indexing of [GeoJSON](http://www.geojson.org/geojson-spec.html) objects.

{CODE spatial_search_enhancements_6@Indexes\SpatialIndexes.cs /}

Beside the WKT and GeoJSON following formats are also supported:   

{CODE spatial_search_enhancements_7@Indexes\SpatialIndexes.cs /}

## Third-party spatial library integration

To integrate with other spatial libraries, the document store must be configured to use a custom library-specific `JsonConverter` which reads/writes WKT or GeoJSON.

Examples of such converters can be found at [Simon Bartlett's github repository page](https://github.com/sibartlett/RavenDB.Client.Spatial).

## Example

Let's assume that we have a `SpatialDoc` with a corresponding index available:

{CODE spatial_search_enhancements_8@Indexes\SpatialIndexes.cs /}

To find all results that are **within radius of** or **intersect** specified shape query as follows:

{CODE spatial_search_enhancements_9@Indexes\SpatialIndexes.cs /}

{INFO You can read more about **spatial search** in a **dedicated querying article** available [here](../indexes/querying/spatial). /}

## Remarks

{WARNING From RavenDB 2.0 the distance by default is measured in **kilometers** in contrast to the miles used in previous versions. /}

## Related articles

- [Querying : Spatial](../indexes/querying/spatial)
