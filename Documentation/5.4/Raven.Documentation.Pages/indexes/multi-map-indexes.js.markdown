# Multi-Map Indexes
---

{NOTE: }

* Multi-Map indexes allow you to index data from multiple collections, 
  like polymorphic data or any data common to different types.  

* Learn how to [index polymorphic data](../indexes/indexing-polymorphic-data)  
  Learn how to [create Multi-Map-Reduce indexes](../indexes/map-reduce-indexes#creating-multi-map-reduce-indexes)  

* In this page:
  * [Indexing multiple collections](../indexes/multi-map-indexes#indexing-multiple-collections)  
  * [Searching across multiple collections](../indexes/multi-map-indexes#searching-across-multiple-collections)  
  * [Remarks](../indexes/multi-map-indexes#remarks)  

{NOTE/}

{PANEL: Indexing multiple collections}

Let's assume that we have `Dog` and `Cat` classes, both inheriting from the class `Animal`:

{CODE-TABS}
{CODE-TAB:nodejs:Dog multiMapClass_1@indexes/multiMap.js /}
{CODE-TAB:nodejs:Cat multiMapClass_2@indexes/multiMap.js /}
{CODE-TAB:nodejs:Animal multiMapClass_3@indexes/multiMap.js /}
{CODE-TABS/}

Now we can define and query our index as follows:

{CODE:nodejs multiMapIndex_1@indexes/multiMap.js /}

{CODE-TABS}
{CODE-TAB:nodejs:Query multiMapQuery_1@indexes/multiMap.js /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Animals/ByName"
where Name == "Mitzy"
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Searching across multiple collections}

Another great usage of Multi-Map indexes is smart-search.  

To search for products, companies, or employees by their name, you need to define the following index:
{CODE:nodejs multiMapIndex_2@indexes/multiMap.js /}

and query it using:
{CODE-TABS}
{CODE-TAB:nodejs:Query multiMapQuery_2@indexes/multiMap.js /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Smart/Search" 
where search(content, "Lau*")
select id() as id, displayName, collection
{CODE-TAB-BLOCK/}
{CODE-TABS/}

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
