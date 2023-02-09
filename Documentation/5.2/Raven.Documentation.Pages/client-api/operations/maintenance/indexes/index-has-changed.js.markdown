# Index has Changed Operation

 ---

{NOTE: }

* __When deploying an index__:  
  * If the new index definition is __different__ than the current index definition on the server,  
    the current index will be overwritten and data will be re-indexed according to the new index definition.
  * If the new index definition is the __same__ as the one on the server,  
    it will not be overwritten and re-indexing will not occur upon deploying the index.

* __Prior to deploying an index:__,  
  * Use `IndexHasChangedOperation` to check if the new index definition differs from the one  
    on the server to avoid any unwanted changes to the existing indexed data.  

* In this page:
    * [Check if index has changed](../../../../client-api/operations/maintenance/indexes/index-has-changed#check-if-index-has-changed)
    * [Syntax](../../../../client-api/operations/maintenance/indexes/index-has-changed#syntax)

{NOTE/}

---

{PANEL: Check if index has changed}

{CODE:nodejs index_has_changed@ClientApi\Operations\Maintenance\Indexes\indexHasChanged.js /}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax@ClientApi\Operations\Maintenance\Indexes\indexHasChanged.js /}

| Parameters | Type | Description |
| - | - | - |
| __definition__ | [IndexDefinition](../../../../client-api/operations/maintenance/indexes/put-indexes#indexDefinition) | The index definition to check |

| Return Value | |
| - | - |
| `true` | When the index __does not exist__ on the server<br>or - <br>When the index definition __is different__ than the one deployed on the server  |
| `false` | When the index definition is __the same__ as the one deployed on the server |

{PANEL/}

## Related Articles

### Indexes

- [What are Indexes](../../../../indexes/what-are-indexes)
- [Creating and Deploying Indexes](../../../../indexes/creating-and-deploying)
- [Index Administration](../../../../indexes/index-administration)

### Operations

- [How to Get Indexes](../../../../client-api/operations/maintenance/indexes/get-indexes)
- [How to Put Indexes](../../../../client-api/operations/maintenance/indexes/put-indexes)
- [How to Delete Index](../../../../client-api/operations/maintenance/indexes/delete-index)
