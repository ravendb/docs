# Indexing changes


{PANEL:Breaking changes in index definition}

Regardless [the data migration strategy](../../migration/server/data-migration) you have chosen you might get warnings complaining about inability to import some indexes.
That is due to breaking changes to index definitions and require manual change.

## LoadDocument

`LoadDocument` requires a second argument which is a collection name of the loaded document.

| 3.x | 4.0 |
|:---:|:---:|
| {CODE indexes_1@Migration\Server\Indexes.cs /} | {CODE indexes_2@Migration\Server\Indexes.cs /} |

## Spatial field creation

`AbstractIndexCreationTask.SpatialGenerate` and `SpatialIndex.Generate` should be replaced with `CreateSpatialField`. In addition to that you can define own field name and use it when querying.   

In addition to the above there are also the following changes:

- No support for GeoJSON and other non-standard formats
- No support for spatial clustering

| 3.x | 4.0 |
|:---:|:---:|
| {CODE indexes_3@Migration\Server\Indexes.cs /} | {CODE indexes_4@Migration\Server\Indexes.cs /} |

## DynamicList

Any occurrence of `new Raven.Abstractions.Linq.DynamicList()` should be removed from an index definition.

{PANEL/}

{PANEL:Built-in indexes}

`Raven/DocumentsByEntityName` and `Raven/ConflictDocuments` are no longer necessary and are intentionally skipped during the import.

{PANEL/}

{PANEL:Auto indexes}

The query optimizer and auto indexing mechanism have significantly changed in RavenDB 4.0. Auto indexes imported from 3.x will get `Legacy/` prefix to their names. You should consider removing the legacy ones and let RavenDB create
new auto indexes from scratch.

Another important breaking change is that dynamic queries (not specifying index name) are only handled by auto indexes. The query optimizer doesn't take into account the static indexes when it determines what index should be used to handle a query.
It means the legacy auto indexes won't be used to satisfy dynamic queries because they will be imported as the static ones.

{PANEL/}


{PANEL: Indexing extensions}

As the replacement for plugins you can use [Additional Sources]() feature. It allows you to write own C# code and use it during indexing. The code is attached to the index definition.
In addition, you can deploy custom DLLs next to RavenDB binaries and reference them in your extensions.

{PANEL/}
