# Index has Changed Operation

 ---

{NOTE: }

* **When deploying an index**:  
  * If the new index definition is **different** from the current index definition on the server,  
    the current index will be overwritten and data will be re-indexed according to the new index definition.
  * If the new index definition is the **same** as the one currently deployed on the server,  
    it will not be overwritten and re-indexing will not occur upon deploying the index.

* **Prior to deploying an index:**,  
  * Use `IndexHasChangedOperation` to check if the new index definition differs from the one  
    on the server to avoid any unwanted changes to the existing indexed data.  

* In this page:
    * [Check if index has changed](../../../../client-api/operations/maintenance/indexes/index-has-changed#check-if-index-has-changed)
    * [Syntax](../../../../client-api/operations/maintenance/indexes/index-has-changed#syntax)

{NOTE/}

---

{PANEL: Check if index has changed}

{CODE:php index_has_changed@ClientApi\Operations\Maintenance\Indexes\IndexHasChanged.php /}

{PANEL/}

{PANEL: Syntax}

{CODE:php syntax@ClientApi\Operations\Maintenance\Indexes\IndexHasChanged.php /}

| Parameters | Type | Description |
| - | - | - |
| **$definition** | [?IndexDefinition](../../../../client-api/operations/maintenance/indexes/put-indexes#indexDefinition) | The index definition to check |

| Return Value | Description |
| - | - |
| `true` | When the index **does not exist** on the server<br>or -<br>When the index definition **is different** from the one deployed on the server  |
| `false` | When the index definition is **the same** as the one deployed on the server |

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
