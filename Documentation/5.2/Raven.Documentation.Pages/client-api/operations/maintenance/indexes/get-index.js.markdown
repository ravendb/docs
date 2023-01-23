# Get Index Operation

---

{NOTE: }

* Use `GetIndexOperation` to retrieve an index definition from the database.

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

### Server

- [Index Administration](../../../../server/administration/index-administration)

### Operations

- [How to Get Indexes](../../../../client-api/operations/maintenance/indexes/get-indexes)
- [How to Put Indexes](../../../../client-api/operations/maintenance/indexes/put-indexes)
- [How to Delete Index](../../../../client-api/operations/maintenance/indexes/delete-index)
