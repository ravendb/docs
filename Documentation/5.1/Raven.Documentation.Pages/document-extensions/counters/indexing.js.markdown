# indexes: Indexing Counters
---

{NOTE: }

* To index counters, create a [static index](../../indexes/creating-and-deploying#static-indexes) 
that inherits from `AbstractCountersIndexCreationTask`.  

* Auto-indexes for counters are not available at this time.  

* In this page:  
  * [Usage](../../document-extensions/counters/indexing#usage)  
  * [Querying the Index](../../document-extensions/counters/indexing#querying-the-index)  

{NOTE/}

---

{PANEL:Usage}

In order to index counter values, create an index that inherits from `AbstractCountersIndexCreationTask`. 
Next, choose one of these two methods which take the index expression:  

{CODE-BLOCK:javascript }
this.maps = new Set(["map"]);

this.map("map");
{CODE-BLOCK/}

`maps` indexes all the counters in the indexed documents. `addMap` only indexes the counters with 
the specified name.  

Examples of indexes using each method:  

{CODE-TABS}
{CODE-TAB:nodejs:map index_1@documentExtensions\counters\indexingCounters.js /}
{CODE-TABS/}  

<br/>

---

{PANEL/}

{PANEL: Querying the Index}  

{CODE-TABS}
{CODE-TAB:nodejs:Query query1@documentExtensions\counters\indexingCounters.js /}
{CODE-TABS/}  

{PANEL/}

## Related Articles  

### indexes  
- [Indexing Related Documents](../../indexes/indexing-related-documents)  
- [Map-Reduce indexes](../../indexes/map-reduce-indexes)  
- [Creating and Deploying indexes](../../indexes/creating-and-deploying)  

### Document Extensions  
- [Counters: Overview](../../document-extensions/counters/overview)  
