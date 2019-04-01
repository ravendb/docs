# Migration: Indexing Changes

{PANEL:Breaking changes in index definition}

Regardless of [the data migration strategy](../../migration/server/data-migration) you have chosen, you might get warnings complaining about an inability to import some indexes.

This is due to breaking changes to index definitions and requires a manual change.

### LoadDocument

`LoadDocument` requires a second argument which is a collection name of the loaded document.

| 3.x |
|:---:|
| {CODE indexes_1@Migration\Server\Indexes.cs /} |

| 4.0 |
|:---:|
| {CODE indexes_2@Migration\Server\Indexes.cs /} |

### Spatial Field Creation

`AbstractIndexCreationTask.SpatialGenerate` and `SpatialIndex.Generate` should be replaced with `CreateSpatialField`. In addition, you can define your own field name and use it when querying.   

There are also the following changes:

- No support for GeoJSON and other non-standard formats
- No support for spatial clustering

| 3.x |
|:---:|
| {CODE indexes_3@Migration\Server\Indexes.cs /} |

| 4.0 |
|:---:|
| {CODE indexes_4@Migration\Server\Indexes.cs /} |

### AsDocument 

`AsDocument` call should be replaced by [`AsJson`](../../indexes/converting-to-json-and-accessing-metadata#asjson---converting-to-json) method.

| 3.x |
|:---:|
| {CODE indexes_5@Migration\Server\Indexes.cs /} |

| 4.0 |
|:---:|
| {CODE indexes_6@Migration\Server\Indexes.cs /} |

### DynamicList

Any occurrence of `new Raven.Abstractions.Linq.DynamicList()` should be removed from an index definition.

{PANEL/}

{PANEL:Built-in Indexes}

`Raven/DocumentsByEntityName` and `Raven/ConflictDocuments` are no longer necessary and are intentionally skipped during the import.

{PANEL/}

{PANEL:Auto Indexes}

The query optimizer and auto indexing mechanisms have significantly changed in RavenDB 4.0. Auto indexes imported from 3.x will get `Legacy/` prefix added to their names. You should consider removing the legacy ones and let RavenDB create new auto indexes from scratch.

Another important breaking change is that dynamic queries (not specifying an index name) are only handled by auto indexes. The query optimizer doesn't take into account the static indexes when it determines what index should be used to handle a query.

The legacy auto indexes won't be used to satisfy dynamic queries because they will be imported as the static ones.

{PANEL/}


{PANEL: Plugins & Extensions}

### Compilation Extensions

As a replacement for Compilation Extensions you can use the [Additional Sources](../../indexes/extending-indexes) feature. It allows you to write your own C# code and use it during indexing. The code is attached to the index definition.

You can also deploy custom DLLs next to RavenDB binaries and reference them in your extensions.

### Custom Analyzers

Please read our dedicated article about [using non-default or custom analyzers](../../indexes/using-analyzers#using-non-default-analyzer).

### Analyzer Generators

Analyzer generators aren't supported.

{PANEL/}
