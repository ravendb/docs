# Indexes: Multi-Map Indexes

Multi-Map indexes allow you to index data from multiple collections e.g. polymorphic data (check the [example](../indexes/indexing-polymorphic-data)) or any common data between types.

## AddMap

`AddMap` method is used to map fields from a single collection e.g. `Dogs`.

Let's assume that we have `Dog` and `Cat` classes, and both of them inherit from the class `Animal`:

{CODE-TABS}
{CODE-TAB:java:Dog multi_map_1@Indexes/MultiMap.java /}
{CODE-TAB:java:Cat multi_map_2@Indexes/MultiMap.java /}
{CODE-TAB:java:Animal multi_map_3@Indexes/MultiMap.java /}
{CODE-TAB:java:IAnimal multi_map_6@Indexes/MultiMap.java /}
{CODE-TABS/}

Now we can define our index using `addMap` in the following way:

{CODE-TABS}
{CODE-TAB:java:AddMap multi_map_4@Indexes/MultiMap.java /}
{CODE-TAB:java:MultiMapJavaScript multi_map_5@Indexes/JavaScript.java /}
{CODE-TABS/}

{CODE-TABS}
{CODE-TAB:java:Query multi_map_7@Indexes\MultiMap.java /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Animals/ByName'
where Name = 'Mitzy'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Indexing Polymorphic Data

Please read more in our dedicated article on indexing polymorphic data. This article can be found [here](../indexes/indexing-polymorphic-data).

## Searching Across Multiple Collections

Another great application of Multi-Map indexes is smart-search. To search for products, companies, or employees by their name, you need to define the following index:

{CODE:java multi_map_1_0@Indexes\MultiMap.java /}

and query it using:

{CODE:java multi_map_1_1@Indexes\MultiMap.java /}

## Remarks

{INFO Remember that all map functions **must** output objects with **identical** shape (field names have to match). /}

## Related Articles

### Indexes
- [Map Indexes](../indexes/map-indexes)
- [Map-Reduce Indexes](../indexes/map-reduce-indexes)
- [Indexing Polymorphic Data](../indexes/indexing-polymorphic-data)

### Studio
- [Create Multi Map Index](../studio/database/indexes/create-multi-map-index)
