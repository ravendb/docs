# Indexes: Indexing Counters

Counter names can be indexed using `CounterNamesFor`. 

{WARNING:Indexing and Counters}

Please note that **re-indexation will only happen** when counter is **added** or **deleted** from the document. The counter increments will not trigger the process.

{WARNING/}

## Creating Indexes

The `CounterNamesFor` method available in `AbstractIndexCreationTask` returns all of the counter names for a document passed as the first argument.

{CODE-TABS}
{CODE-TAB:csharp:CounterNamesFor syntax@Indexes\IndexingCounters.cs /}
{CODE-TABS/}

{CODE-TABS}
{CODE-TAB:csharp:Index index@Indexes\IndexingCounters.cs /}
{CODE-TABS/}

## Example

{CODE-TABS}
{CODE-TAB:csharp:Sync query1@Indexes\IndexingCounters.cs /}
{CODE-TAB:csharp:Async query2@Indexes\IndexingCounters.cs /}
{CODE-TAB:csharp:DocumentQuery query3@Indexes\IndexingCounters.cs /}
{CODE-TABS/}
