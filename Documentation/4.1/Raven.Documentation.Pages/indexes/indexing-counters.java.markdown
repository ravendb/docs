# Indexes: Indexing Counters

Counter names can be indexed using `CounterNamesFor`. 

{WARNING:Indexing and Counters}

Please note that **re-indexation will only happen** when counter is **added** or **deleted** from the document. The counter increments will not trigger the process.

{WARNING/}

## Creating Indexes

The `CounterNamesFor` method returns all of the counter names for a document passed as the first argument.

{CODE-TABS}
{CODE-TAB:java:CounterNamesFor syntax@Indexes\IndexingCounters.java /}
{CODE-TABS/}

{CODE-TABS}
{CODE-TAB:java:Index index@Indexes\IndexingCounters.java /}
{CODE-TABS/}

## Example

{CODE:java query1@Indexes\IndexingCounters.java /}
