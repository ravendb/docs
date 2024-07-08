# Indexes: Indexing Counters
---

{NOTE: }

* To index counters, create a [static index](../../indexes/creating-and-deploying#static-indexes) 
that inherits from `AbstractCountersIndexCreationTask` or `AbstractJavaScriptCountersIndexCreationTask`.  

* Auto-indexes for counters are not available at this time.  

* In this page:  
  * [Syntax](../../document-extensions/counters/indexing#syntax)  
  * [AbstractJavaScriptCountersIndexCreationTask](../../document-extensions/counters/indexing#section)  
  * [CounterNamesFor](../../document-extensions/counters/indexing#section)  

{NOTE/}

---

{PANEL:Syntax}

To index counter values, create an index that inherits from `AbstractCountersIndexCreationTask` 
and use `map` as follows.  
{CODE:python index_1@DocumentExtensions\Counters\IndexingCounters.py /}

---

### AbstractJavaScriptCountersIndexCreationTask

Creating an index inheriting from `AbstractJavaScriptCountersIndexCreationTask` allows 
you to write your map and reduce functions in JavaScript.  
Learn more about JavaScript indexes [here](../../indexes/javascript-indexes).  

{CODE-BLOCK: csharp}
public class AbstractJavaScriptCountersIndexCreationTask : AbstractCountersIndexCreationTask
{
    public HashSet<string> Maps;
    protected string Reduce;
}
{CODE-BLOCK/}

| Property | Type | Description |
| - | - | - |
| **Maps** | `HashSet<string>` | The set of javascript map functions |
| **Reduce** | `string` | The javascript reduce function |

Example:  

{CODE:python index_3@DocumentExtensions\Counters\IndexingCounters.py /}

---

### `CounterNamesFor`

While indexes inheriting from `AbstractIndexCreationTask` cannot index counter _values_, the `CounterNamesFor` 
method is available which returns the names of all counters for a specified document:  

{CODE:python syntax@DocumentExtensions\Counters\IndexingCounters.py /}

Example of index using `CounterNamesFor`:  

{CODE:python index_0@DocumentExtensions\Counters\IndexingCounters.py /}

{PANEL/}

## Related Articles  

### Indexes  
- [Indexing Related Documents](../../indexes/indexing-related-documents)  
- [Map-Reduce Indexes](../../indexes/map-reduce-indexes)  
- [Creating and Deploying Indexes](../../indexes/creating-and-deploying)  

### Document Extensions  
- [Counters: Overview](../../document-extensions/counters/overview)  
