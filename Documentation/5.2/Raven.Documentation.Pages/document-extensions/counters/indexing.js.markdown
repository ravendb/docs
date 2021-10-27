# indexes: Indexing Counters
---

{NOTE: }

* To index counters, create a [static index](../../indexes/creating-and-deploying#static-indexes) 
that inherits from `AbstractCountersIndexCreationTask` or `AbstractJavaScriptCountersIndexCreationTask`.  

* Auto-indexes for counters are not available at this time.  

* In this page:  
  * [Usage](../../document-extensions/counters/indexing#usage)  
  * [AbstractJavaScriptCountersIndexCreationTask](../../document-extensions/counters/indexing#section)  
  * [CounterNamesFor](../../document-extensions/counters/indexing#section-1)  
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

<!---TODO: uncomment and delete the code Tabs up after src 5.2 is out
{CODE-TABS}
{CODE-TAB:nodejs:map index_1@documentExtensions\counters\indexingCounters.js /}
{CODE-TAB:nodejs:maps index_2@documentExtensions\counters\indexingCounters.js /}
{CODE-TABS/}  
---->
<br/>

---

### `AbstractJavaScriptCountersIndexCreationTask`

Creating an index inheriting from `AbstractJavaScriptCountersIndexCreationTask` allows 
you to write your map and reduce functions in JavaScript.  
Learn more about JavaScript indexes [here](../../indexes/javascript-indexes).  
<!--- TODO: uncomment after src 5.2 is out
{CODE:nodejs javaScriptIndexCreationTask@documentExtensions\counters\indexingCounters.js /}

| Property | Type | Description |
| - | - | - |
| **Maps** | `HashSet<string>` | The set of javascript map functions |
| **Reduce** | `string` | The javascript reduce function |


Example:  

{CODE:nodejs index_3@documentExtensions\counters\indexingCounters.js /}
---->

---

### `CounterNamesFor`

While indexes inheriting from `AbstractIndexCreationTask` cannot index counter _values_, the `counterNamesFor()` 
method is available which returns the names of all counters for a specified document:  

{CODE:nodejs syntax@documentExtensions\counters\indexingCounters.js /}

Example of index using `CounterNamesFor`:  

{CODE:nodejs index_0@documentExtensions\counters\indexingCounters.js /}

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
