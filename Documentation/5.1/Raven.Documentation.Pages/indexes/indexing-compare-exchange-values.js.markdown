# Indexes: Indexing Compare Exchange Values

---

{NOTE: }

* Compare exchange values can be loaded within an index using the value's key.  

* The index will update when the compare exchange value is updated, as well 
as when documents are modified in the indexed collection(s).  

* In this page:  
  * [Syntax](../indexes/indexing-compare-exchange-values#syntax)  
  * [Examples](../indexes/indexing-compare-exchange-values#examples)  
  * [Querying the Index](../indexes/indexing-compare-exchange-values#querying-the-index)  

{NOTE/}

---

{PANEL: Syntax}
For javascript indexes, use the method `cmpxchg(<key>)`.

| Parameter | Type | Description |
| - | - | - |
| **key** | `string` | The key of a particular compare exchange value. |
<br/>
### Examples

These indexes map the rooms in a hotel, as well as compare exchange values 
representing the guests in those rooms.  

{CODE:nodejs index_1@indexes/indexingCompareExchange.js /}

{PANEL/}

{PANEL: Querying the Index}

{CODE:nodejs query_2@indexes/indexingCompareExchange.js /}

{PANEL/}

## Related Articles

### Client-API

- [Compare Exchange Overview](../client-api/operations/compare-exchange/overview)

### Indexes

- [What Are Indexes](../indexes/what-are-indexes)
- [Creating and Deploying Indexes](../indexes/creating-and-deploying)
