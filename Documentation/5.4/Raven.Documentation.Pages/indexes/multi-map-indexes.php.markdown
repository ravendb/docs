# Multi-Map Indexes
---

{NOTE: }

* Multi-Map indexes allow you to index data from multiple collections, 
  like polymorphic data or any data common to different types.  

* Learn how to [index polymorphic data](../indexes/indexing-polymorphic-data)  
  Learn how to [create Multi-Map-Reduce indexes](../indexes/map-reduce-indexes#creating-multi-map-reduce-indexes)  

* In this page:
  * [`addMap`](../indexes/multi-map-indexes#addmap)
  * [Searching across multiple collections](../indexes/multi-map-indexes#searching-across-multiple-collections)
  * [Remarks](../indexes/multi-map-indexes#remarks)

{NOTE/}

{PANEL: `addMap`}

The `addMap` method is used to map fields from a single collection, e.g. `Dogs`.  

Let's assume that we have `Dog` and `Cat` classes, both inheriting from the class `Animal`:

{CODE-TABS}
{CODE-TAB:php:Dog multi_map_1@Indexes/MultiMap.php /}
{CODE-TAB:php:Cat multi_map_2@Indexes/MultiMap.php /}
{CODE-TAB:php:Animal multi_map_3@Indexes/MultiMap.php /}
{CODE-TABS/}

We can define our index using `addMap` and query it as follows:

{CODE-TABS}
{CODE-TAB:php:_add_map multi_map_4@Indexes/MultiMap.php /}
{CODE-TAB:php:MultiMapJavaScript multi_map_5@Indexes/JavaScript.php /}
{CODE-TABS/}

{CODE-TABS}
{CODE-TAB:php:Query multi_map_7@Indexes\MultiMap.php /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Animals/ByName'
where Name = 'Mitzy'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Searching across multiple collections}

Another great usage of Multi-Map indexes is smart-search.  

To search for products, companies, or employees by their name, you need to define the following index:
{CODE:php multi_map_1_0@Indexes\MultiMap.php /}

and query it using:
{CODE:php multi_map_1_1@Indexes\MultiMap.php /}

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

- [Multi-Map-Index: Basic](https://demo.ravendb.net/demos/python/multi-map-indexes/multi-map-index-basic)
- [Multi-Map-Index: Customized Fields](https://demo.ravendb.net/demos/python/multi-map-indexes/multi-map-index-customized-fields)
- [Map Index](https://demo.ravendb.net/demos/python/static-indexes/map-index)
- [Multi-Map-Reduce Index](https://demo.ravendb.net/demos/python/multi-map-indexes/multi-map-reduce-index)
