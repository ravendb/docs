# Indexes : JavaScript Indexes

The JavaScript indexes feature was created for users who are not proficient in the C# language and want to create an index.   
JavaScript indexes can be defined by a user with lower permissions than the C# indexes (admin not required).   
All other capabilities and features are the same as C# indexes.   

{INFO:Information}
This is an experimental feature.
You need to enable the featured by adding the following key to your settings.json file:

{CODE-BLOCK:json}
{
    ...
    "Features.Availability": "Experimental"
    ...
}
{CODE-BLOCK/}
{INFO/}

### Example I - Map index

{CODE javaScriptindexes_6@Indexes\JavaScript.cs /}

Read more about map indexes [here](../indexes/map-indexes).

### Example II - Multi map index

{CODE multi_map_5@Indexes\JavaScript.cs /}

Read more about multi map indexes [here](../indexes/map-reduce-indexes).

### Example III - Map-Reduce index

{CODE map_reduce_0_0@Indexes\JavaScript.cs /}
Read more about map reduce indexes [here](../indexes/multi-map-indexes).

## Related Articles

### Indexes

- [Indexing Related Documents](../indexes/indexing-related-documents)
- [Map Indexes](../indexes/map-indexes)
- [Map-Reduce Indexes](../indexes/map-reduce-indexes)
- [Creating and Deploying Indexes](../indexes/creating-and-deploying)

### Querying
- [Basics](../indexes/querying/basics)
