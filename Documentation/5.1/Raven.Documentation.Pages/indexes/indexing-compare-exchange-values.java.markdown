# Indexes: Indexing Compare Exchange Values

---

{NOTE: }

* Compare exchange values can be loaded within an index using the value's key.  

* The index will update when the compare exchange value is updated, as well 
as when documents are modified in the indexed collection(s).  

* In this page:  
  * [How to use](../indexes/indexing-compare-exchange-values#how-to-use)  
  * [Examples](../indexes/indexing-compare-exchange-values#examples)  
  * [Querying the Index](../indexes/indexing-compare-exchange-values#querying-the-index)  

{NOTE/}

---

{PANEL: How to use }

When creating an index using `AbstractIndexCreationTask`, use javaScript 
to load a compare exchange value by its key.  

<br/>
### Examples

These indexes map the rooms in a hotel, as well as compare exchange values 
representing the guests in those rooms.  


{CODE-TABS}
{CODE-TAB:java:JavaScript-syntax index_1@Indexes/IndexingCompareExchange.java /}
{CODE-TABS/}

{PANEL/}

{PANEL: Querying the Index}

{CODE-TABS}
{CODE-TAB:java:RawQuery query_0@Indexes/IndexingCompareExchange.java /}
{CODE-TABS/}

{PANEL/}

## Related Articles

### Client-API

- [Compare Exchange Overview](../client-api/operations/compare-exchange/overview)

### Indexes

- [What Are Indexes](../indexes/what-are-indexes)
- [Creating and Deploying Indexes](../indexes/creating-and-deploying)
