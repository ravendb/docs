# Migration: Indexes defined using client API

Below there is list of changes to `AbstractIndexCreationTask`, `AbstractMultiMapIndexCreationTask` and `IndexDefinition`.

## Namespace

Change `Raven.Abstractions.Indexing` and `Raven.Client.Indexes` namespaces to `Raven.Client.Documents.Indexes`.   

<br />

{PANEL:AbstractIndexCreationTask and AbstractMultiMapIndexCreationTask}

### MaxIndexOutputsPerDocument

`MaxIndexOutputsPerDocument` property was removed. A warning is issued instead. Read dedicated about performance hints if you use when [fanout indexes](../../indexes/fanout-indexes#performance-hints)

### Sort

There is no need to explicitly define sorting in definitions of static indexes anymore. RavenDB automatically determines sorting capabilities based on types of indexed values. You can remove `Sort` method usages from your index classes, such as:

{CODE indexes_1@Migration\ClientApi\Indexes.cs /} 

### Constants.AllFields

`Constants.AllFields` constant has been moved to `Constants.Documents.Indexing.Fields.AllFields`

### FieldIndexing.Analyzed

`FieldIndexing.Analyzed` has been renamed to `FieldIndexing.Search`

### FieldIndexing.NotAnalyzed

`FieldIndexing.NotAnalyzed` has been renamed to `FieldIndexing.Exact`

{PANEL/}

{PANEL:IndexDefinition}

### Map

`Map` property has been removed. Use `Maps` property instead.

### Field options

Options as are now defined per field using `Fields` property.

| 3.x | 4.0 |
|:---:|:---:|
| {CODE indexes_2@Migration\ClientApi\Indexes.cs /} | {CODE indexes_3@Migration\ClientApi\Indexes.cs /} |

{PANEL/}
