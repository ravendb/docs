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

{CODE-TABS}
{CODE-TAB:csharp:Sync get_index_names@ClientApi\Operations\Maintenance\Indexes\GetIndexNames.cs /}
{CODE-TAB:csharp:Async get_index_names_async@ClientApi\Operations\Maintenance\Indexes\GetIndexNames.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

{CODE get_index_names_syntax@ClientApi\Operations\Maintenance\Indexes\GetIndexNames.cs /}

| Parameters | Type | Description |
| - |- | - |
| __start__ | int | Number of index names to skip |
| __pageSize__ | int   | Number of index names to retrieve |

| Return Value of `store.Maintenance.Send(getIndexNamesOp)` | |
| - | - |
| `string[]` | A list of index names alphabetically ordered |

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
