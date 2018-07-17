# Indexes : Multi-Map Indexes

Multi-Map indexes allow you to index data from multiple collections e.g. polymorphic data (check the [example](../indexes/indexing-polymorphic-data)) or any common data between types.

## AddMap & AddMapForAll

`AddMap` method is used to map fields from a single collection e.g. `Dogs`. `AddMapForAll` gives you the ability to specify what fields will be indexed from a base class. 

Let's assume that we have `Dog` and `Cat` classes, and both of them inherit from the class `Animal`:

{CODE-TABS}
{CODE-TAB:csharp:Dog multi_map_1@Indexes/MultiMap.cs /}
{CODE-TAB:csharp:Cat multi_map_2@Indexes/MultiMap.cs /}
{CODE-TAB:csharp:Animal multi_map_3@Indexes/MultiMap.cs /}
{CODE-TAB:csharp:IAnimal multi_map_6@Indexes/MultiMap.cs /}
{CODE-TABS/}

Now we can define our index using `AddMap` or `AddMapForAll` in the following way:

{CODE-TABS}
{CODE-TAB:csharp:AddMap multi_map_4@Indexes/MultiMap.cs /}
{CODE-TAB:csharp:AddMapForAll multi_map_5@Indexes/MultiMap.cs /}
{CODE-TAB:csharp:MultiMapJavaScript multi_map_5@Indexes/JavaScript.cs /}
{CODE-TABS/}

{CODE-TABS}
{CODE-TAB:csharp:Query multi_map_7@Indexes\MultiMap.cs /}
{CODE-TAB:csharp:DocumentQuery multi_map_8@Indexes\MultiMap.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Animals/ByName'
where Name = 'Mitzy'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Indexing Polymorphic Data

Please read more in our dedicated article on indexing polymorphic data. This article can be found [here](../indexes/indexing-polymorphic-data).

## Searching Across Multiple Collections

Another great application of Multi-Map indexes is smart-search. To search for products, companies, or employees by their name, you need to define the following index:

{CODE multi_map_1_0@Indexes\MultiMap.cs /}

and query it using:

{CODE multi_map_1_1@Indexes\MultiMap.cs /}

## Remarks

{INFO Remember that all map functions **must** output objects with **identical** shape (field names have to match). /}

## Related Articles

### Indexes

- [Map Indexes](../indexes/map-indexes)
- [Map-Reduce Indexes](../indexes/map-reduce-indexes)
- [Indexing Polymorphic Data](../indexes/indexing-polymorphic-data)
