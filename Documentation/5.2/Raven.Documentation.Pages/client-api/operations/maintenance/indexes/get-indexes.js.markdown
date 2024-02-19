# Get Indexes Operation

---

{NOTE: }

* Use `GetIndexesOperation` to retrieve multiple __index definitions__ from the database.

* The operation will execute on the node defined by the [client configuration](../../../../client-api/configuration/load-balance/overview#client-logic-for-choosing-a-node).  
  However, the index definitions returned are taken from the database record,  
  which is common to all the database-group nodes.  
  i.e., an index state change done only on a local node is not reflected.

* To get a specific index state on a local node use `GetIndexStatisticsOperation`.

* In this page:
    * [Get indexes example](../../../../client-api/operations/maintenance/indexes/get-indexes#get-indexes-example)
    * [Syntax](../../../../client-api/operations/maintenance/indexes/get-indexes#syntax)

{NOTE/}

---

{PANEL: Get indexes example}

{CODE:nodejs get_indexes@ClientApi\Operations\Maintenance\Indexes\getIndex.js /}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs get_indexes_syntax@ClientApi\Operations\Maintenance\Indexes\getIndex.js /}

| Parameters | Type | Description |
| - | - | - |
| __start__ | number | Number of indexes to skip |
| __pageSize__ | number | Number of indexes to retrieve |

| Return value of `store.maintenance.send(getIndexesOp)` | |
| - | - |
| `IndexDefinition[]` | A list of [IndexDefinition](../../../../client-api/operations/maintenance/indexes/put-indexes#indexDefinition), <br>ordered alphabetically by index name. |

{PANEL/}

## Related Articles

### Indexes

- [What are Indexes](../../../../indexes/what-are-indexes)
- [Creating and Deploying Indexes](../../../../indexes/creating-and-deploying)
- [Index Administration](../../../../indexes/index-administration)

### Operations

- [How to Get Index](../../../../client-api/operations/maintenance/indexes/get-index)
- [How to Put Indexes](../../../../client-api/operations/maintenance/indexes/put-indexes)
- [How to Delete Index](../../../../client-api/operations/maintenance/indexes/delete-index)
