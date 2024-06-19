# Multi-Map Indexes
---

{NOTE: }

* Multi-Map indexes allow you to index data from multiple collections, 
  like polymorphic data or any data common to different types.  

* Learn how to [index polymorphic data](../indexes/indexing-polymorphic-data)  
  Learn how to [create Multi-Map-Reduce indexes](../indexes/map-reduce-indexes#creating-multi-map-reduce-indexes)  

* In this page:
  * [AddMap & AddMapForAll](../indexes/multi-map-indexes#addmap-&-addmapforall)
  * [Searching across multiple collections](../indexes/multi-map-indexes#searching-across-multiple-collections)
  * [Remarks](../indexes/multi-map-indexes#remarks)

{NOTE/}

{PANEL: AddMap & AddMapForAll}

The `AddMap` method is used to map fields from a single collection, e.g. `Dogs`.  
`AddMapForAll` gives you the ability to specify what fields will be indexed from a base class.  

Let's assume that we have `Dog` and `Cat` classes, both inheriting from the class `Animal`:

{CODE-TABS}
{CODE-TAB:csharp:Dog multi_map_1@Indexes/MultiMap.cs /}
{CODE-TAB:csharp:Cat multi_map_2@Indexes/MultiMap.cs /}
{CODE-TAB:csharp:Animal multi_map_3@Indexes/MultiMap.cs /}
{CODE-TAB:csharp:IAnimal multi_map_6@Indexes/MultiMap.cs /}
{CODE-TABS/}

We can define our index using `AddMap` or `AddMapForAll` and query it as follows:

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

{PANEL/}

{PANEL: Searching across multiple collections}

Another great usage of Multi-Map indexes is smart-search.  

To search for products, companies, or employees by their name, you need to define the following index:
{CODE multi_map_1_0@Indexes\MultiMap.cs /}

and query it using:
{CODE multi_map_1_1@Indexes\MultiMap.cs /}

{PANEL/}

{PANEL: Remarks}

{INFO: }
Remember that all map functions **must** output objects 
with an **identical** shape (the field names have to match).  
{INFO/}

{PANEL/}

## Related Articles

### Indexes
- [Map Indexes](../indexes/map-indexes)
- [Map-Reduce Indexes](../indexes/map-reduce-indexes)
- [Indexing Polymorphic Data](../indexes/indexing-polymorphic-data)

### Studio
- [Create Multi-Map Index](../studio/database/indexes/create-multi-map-index)

<br/>

## Code Walkthrough

- [Multi-Map-Index: Basic](https://demo.ravendb.net/demos/csharp/multi-map-indexes/multi-map-index-basic)
- [Multi-Map-Index: Customized Fields](https://demo.ravendb.net/demos/csharp/multi-map-indexes/multi-map-index-customized-fields)
- [Map Index](https://demo.ravendb.net/demos/csharp/static-indexes/map-index)
- [Multi-Map-Reduce Index](https://demo.ravendb.net/demos/csharp/multi-map-indexes/multi-map-reduce-index)
