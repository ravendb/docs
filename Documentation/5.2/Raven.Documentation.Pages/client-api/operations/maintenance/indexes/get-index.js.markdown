# Get Index Operation

---

{NOTE: }

* Use `GetIndexOperation` to retrieve the __index definition__ from the database.

* The operation will execute on the node defined by the [client configuration](../../../../client-api/configuration/load-balance/overview#client-logic-for-choosing-a-node).  
  However, the index definition returned is taken from the database record,  
  which is common to all the database-group nodes.  
  i.e., an index state change done only on a local node is not reflected.  

* To get the index state on the local node use [get index stats](../../../../client-api/operations/maintenance/indexes/get-index-stats).

* In this page:
    * [Get index example](../../../../client-api/operations/maintenance/indexes/get-index#get-index-example)
    * [Syntax](../../../../client-api/operations/maintenance/indexes/get-index#syntax)

{NOTE/}

---

{PANEL: Get index example}

{CODE:nodejs get_index@ClientApi\Operations\Maintenance\Indexes\getIndex.js /}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs get_index_syntax@ClientApi\Operations\Maintenance\Indexes\getIndex.js /}

| Parameters | Type | Description |
| - | - | - |
| __indexName__ | string | Name of index to get |

| Return value of `store.maintenance.send(getIndexOp)` | |
|- | - |
| `IndexDefinition` | An instance of class [IndexDefinition](../../../../client-api/operations/maintenance/indexes/put-indexes#indexDefinition) |

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
- [Get index stats](../../../../client-api/operations/maintenance/indexes/get-index-stats)
