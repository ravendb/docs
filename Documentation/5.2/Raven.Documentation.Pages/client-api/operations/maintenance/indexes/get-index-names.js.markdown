# Get Index Names Operation

---

{NOTE: }

* Use `GetIndexNamesOperation` to retrieve multiple __index names__ from the database.

* In this page:
    * [Get index names example](../../../../client-api/operations/maintenance/indexes/get-index-names#get-index-names-example)
    * [Syntax](../../../../client-api/operations/maintenance/indexes/get-index-names#syntax)

{NOTE/}

---

{PANEL: Get index names example}

{CODE:nodejs get_index_names@ClientApi\Operations\Maintenance\Indexes\getIndexNames.js /}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs get_index_names_syntax@ClientApi\Operations\Maintenance\Indexes\getIndexNames.js /}

| Parameters | Type | Description |
| - |- | - |
| __start__ | number | Number of index names to skip |
| __pageSize__ | number   | Number of index names to retrieve |

| Return Value of `store.maintenance.send(getIndexNamesOp)` | |
| - | - |
| `string[]` | A list of index names alphabetically ordered |

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
