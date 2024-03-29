# Indexes: JavaScript Indexes

This feature was created for users who want to create an index and prefer JavaScript over C#. 
JavaScript indexes can be defined by a user with lower permissions than the C# indexes (admin not required). 
All other capabilities and features are the same as C# indexes.  

{SAFE:Experimental}
This is an experimental feature. 
To enable it, add the following to your [`settings.json` file](../server/configuration/configuration-options#settings.json):
{CODE-BLOCK:json}
"Features.Availability": "Experimental"
{CODE-BLOCK/}
{SAFE/}

## Creating  JavaScript Index

If we want to create JavaScript index we need to create an instance of our class that inherits 
from AbstractJavaScriptIndexCreationTask.   
AbstractJavaScriptIndexCreationTask inherits from AbstractIndexCreationTask 
(Read more about AbstractIndexCreationTask [here](../indexes/creating-and-deploying#Using-AbstractIndexCreationTask).)

{CODE javaScriptindexes_1@Indexes\JavaScript.cs /}

## Map Index

`Map` indexes, sometimes referred to as simple indexes, contain one (or more) mapping functions that indicate which fields from the documents should be indexed. 
They indicate which documents can be searched by which fields.

{CODE-BLOCK:csharp}
   map(<collection-name>, function (document){
        return {
            // indexed properties go here e.g:
            // Name: document.Name
        };
    })
{CODE-BLOCK/}

### Example I - Simple Map Index

{CODE javaScriptindexes_6@Indexes\JavaScript.cs /}

### Example II - Map Index With Additional Sources

{CODE indexes_2@Indexes\JavaScript.cs /}

Read more about map indexes [here](../indexes/map-indexes).

## Multi Map Index

Multi-Map indexes allow you to index data from multiple collections

### Example

{CODE multi_map_5@Indexes\JavaScript.cs /}

Read more about multi map indexes [here](../indexes/map-reduce-indexes).

## Map-Reduce Index
Map-Reduce indexes allow you to perform complex aggregations of data.
The first stage, called the map, runs over documents and extracts portions of data according to the defined mapping function(s).
Upon completion of the first phase, reduction is applied to the map results and the final outcome is produced.

{CODE-BLOCK:csharp}
   groupBy(x => {map properties})
        .aggregate(y => {
            return {
                // indexed properties go here e.g:
                // Name: y.Name
            };
        })
{CODE-BLOCK/}

{WARNING: `this` Keyword}
`this` is bound to the state of our JavaScript interpreter, which has no relevance to creating indexes. Using `this` is unsupported and may cause undefined behavior. 
{WARNING/}

### Example I

{CODE map_reduce_0_0@Indexes\JavaScript.cs /}

### Example II

{CODE map_reduce_3_0@Indexes\JavaScript.cs /}

Read more about map reduce indexes [here](../indexes/multi-map-indexes).

{INFO:Information}
Supported JavaScript version : ECMAScript 5.1
{INFO/}

## Related Articles

### Indexes

- [Indexing Related Documents](../indexes/indexing-related-documents)
- [Map Indexes](../indexes/map-indexes)
- [Map-Reduce Indexes](../indexes/map-reduce-indexes)
- [Creating and Deploying Indexes](../indexes/creating-and-deploying)

### Querying
- [Basics](../indexes/querying/basics)
