# Indexes: Indexing Counters
---

{NOTE: }

* To index counters, create a [static index](../../indexes/creating-and-deploying#static-indexes) 
that inherits from `AbstractCountersIndexCreationTask` or `AbstractJavaScriptCountersIndexCreationTask`.  

* Auto-indexes for counters are not available at this time.  

* In this page:  
  * [Syntax](../../document-extensions/counters/indexing#syntax)  
  * [AbstractJavaScriptCountersIndexCreationTask](../../document-extensions/counters/indexing#section)  
  * [CounterNamesFor](../../document-extensions/counters/indexing#section-1)  
  * [Querying the Index](../../document-extensions/counters/indexing#querying-the-index)  

{NOTE/}

---

{PANEL:Syntax}

In order to index counter values, create an index that inherits from `AbstractCountersIndexCreationTask`. 
Next, use these method which take the index expression:  

{CODE-BLOCK:java }
 protected void addMap(String map)
{CODE-BLOCK/}

`addMap` only indexes the counters with 
the specified name.  

Examples of indexes using each method:  

{CODE-TABS}
{CODE-TAB:java:addMap index_1@Indexes\IndexingCounters.java /}
{CODE-TABS/}  
<br/>

---

### `AbstractJavaScriptCountersIndexCreationTask`

Creating an index inheriting from `AbstractJavaScriptCountersIndexCreationTask` allows 
you to write your map and reduce functions in JavaScript.  
Learn more about JavaScript indexes [here](../../indexes/javascript-indexes).  

{CODE-BLOCK: java}
public class AbstractJavaScriptCountersIndexCreationTask extends AbstractIndexCreationTaskBase<CountersIndexDefinition> 
{
    private final CountersIndexDefinition _definition = new CountersIndexDefinition();

    public Set<String> getMaps() {
        return _definition.getMaps();
    }

    public void setMaps(Set<String> maps) {
        _definition.setMaps(maps);
    }

    public Map<String, IndexFieldOptions> getFields() {
        return _definition.getFields();
    }

    public void setFields(Map<String, IndexFieldOptions> fields) {
        _definition.setFields(fields);
    }

    protected String getReduce() {
        return _definition.getReduce();
    }

    protected void setReduce(String reduce) {
        _definition.setReduce(reduce);
    }

    @Override
    public boolean isMapReduce() {
        return getReduce() != null;
    }

    /**
     * @return If not null than each reduce result will be created as a document in the specified collection name.
     */
    protected String getOutputReduceToCollection() {
        return _definition.getOutputReduceToCollection();
    }
}


{CODE-BLOCK/}

| Property | Type | Description |
| - | - | - |
| **Maps** | `Set<String>` | The set of javascript map functions |
| **Reduce** | `String` | The javascript reduce function |

Example:  

{CODE:java index_3@DocumentExtensions\Counters\IndexingCounters.java /}

---

### `CounterNamesFor`

While indexes inheriting from `AbstractIndexCreationTask` cannot index counter _values_, the `counterNamesFor` 
method is available which returns the names of all counters for a specified document:  

{CODE:java syntax@Indexes\IndexingCounters.java /}

Example of index using `counterNamesFor`:  

{CODE:java index_0@Indexes\IndexingCounters.java /}

{PANEL/}

{PANEL: Querying the Index}  

{CODE:java query1@Indexes\IndexingCounters.java /}

{PANEL/}

## Related Articles  

### Indexes  
- [Indexing Related Documents](../../indexes/indexing-related-documents)  
- [Map-Reduce Indexes](../../indexes/map-reduce-indexes)  
- [Creating and Deploying Indexes](../../indexes/creating-and-deploying)  

### Document Extensions  
- [Counters: Overview](../../document-extensions/counters/overview)  
