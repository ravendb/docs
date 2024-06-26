# Get Index Names Operation

---

{NOTE: }

* Use `GetIndexNamesOperation` to retrieve multiple **index names** from the database.

* In this page:
    * [Get index names example](../../../../client-api/operations/maintenance/indexes/get-index-names#get-index-names-example)
    * [Syntax](../../../../client-api/operations/maintenance/indexes/get-index-names#syntax)

{NOTE/}

---

{PANEL: Get index names example}

{CODE:python get_index_names@ClientApi\Operations\Maintenance\Indexes\GetIndexNames.py /}

{PANEL/}

{PANEL: Syntax}

{CODE:python get_index_names_syntax@ClientApi\Operations\Maintenance\Indexes\GetIndexNames.py /}

| Parameters | Type | Description |
| - |- | - |
| **start** | `int` | Number of index names to skip |
| **page_size** | `int`   | Number of index names to retrieve |

| Return Value of<br>`store.maintenance.send(GetIndexNamesOperation)` | Description |
| - | - |
| `str[]` | A list of index names.<br> Alphabetically ordered. |

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
