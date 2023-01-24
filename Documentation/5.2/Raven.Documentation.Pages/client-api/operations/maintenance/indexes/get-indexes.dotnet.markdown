# Get Indexes Operation

---

{NOTE: }

* Use `GetIndexesOperation` to retrieve multiple index definitions from the database.

* In this page:
    * [Get indexes example](../../../../client-api/operations/maintenance/indexes/get-indexes#get-indexes-example)
    * [Syntax](../../../../client-api/operations/maintenance/indexes/get-indexes#syntax)

{NOTE/}

---

{PANEL: Get indexes example}

{CODE-TABS}
{CODE-TAB:csharp:Sync get_indexes@ClientApi\Operations\Maintenance\Indexes\GetIndex.cs /}
{CODE-TAB:csharp:Async get_indexes_async@ClientApi\Operations\Maintenance\Indexes\GetIndex.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

{CODE get_indexes_syntax@ClientApi\Operations\Maintenance\Indexes\GetIndex.cs /}

| Parameters | Type | Description |
| - | - | - |
| __start__ | int | Number of indexes to skip |
| __pageSize__ | int | Number of indexes to retrieve |

| Return value of `store.Maintenance.Send(getIndexesOp)` | |
| - | - |
| `IndexDefinition[]` | A list of [IndexDefinition](../../../../client-api/operations/maintenance/indexes/put-indexes#indexDefinition) <br> ordered alphabetically by index name |

{PANEL/}

## Related Articles

### Indexes

- [What are Indexes](../../../../indexes/what-are-indexes)
- [Creating and Deploying Indexes](../../../../indexes/creating-and-deploying)

### Server

- [Index Administration](../../../../server/administration/index-administration)

### Operations

- [How to Get Index](../../../../client-api/operations/maintenance/indexes/get-index)
- [How to Put Indexes](../../../../client-api/operations/maintenance/indexes/put-indexes)
- [How to Delete Index](../../../../client-api/operations/maintenance/indexes/delete-index)
