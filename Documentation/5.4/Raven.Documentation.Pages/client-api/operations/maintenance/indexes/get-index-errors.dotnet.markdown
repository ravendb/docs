# Get Index Errors Operation

---

{NOTE: }

* Use `GetIndexErrorsOperation` to get errors encountered during indexing.

* The index errors will be retrieved only from the server node defined by the current [client-configuration](../../../../client-api/configuration/load-balance/overview#client-logic-for-choosing-a-node).

* To learn about clearing index errors, see [delete index errors](../../../../client-api/operations/maintenance/indexes/delete-index-errors). 

* In this page:
    * [Get errors for all indexes](../../../../client-api/operations/maintenance/indexes/get-index-errors#get-errors-for-all-indexes)
    * [Get errors for specific indexes](../../../../client-api/operations/maintenance/indexes/get-index-errors#get-errors-for-specific-indexes)
    * [Syntax](../../../../client-api/operations/maintenance/indexes/get-index-errors#syntax)

{NOTE/}

---

{PANEL: Get errors for all indexes}

{CODE-TABS}
{CODE-TAB:csharp:Sync get_errors_all@ClientApi\Operations\Maintenance\Indexes\GetIndexErrors.cs /}
{CODE-TAB:csharp:Async get_errors_all_async@ClientApi\Operations\Maintenance\Indexes\GetIndexErrors.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Get errors for specific indexes}

{CODE-TABS}
{CODE-TAB:csharp:Sync get_errors_specific@ClientApi\Operations\Maintenance\Indexes\GetIndexErrors.cs /}
{CODE-TAB:csharp:Async get_errors_specific_async@ClientApi\Operations\Maintenance\Indexes\GetIndexErrors.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

{CODE syntax_1@ClientApi\Operations\Maintenance\Indexes\GetIndexErrors.cs /}

| Parameters | Type | Description |
| - | - | - |
| **indexNames** | `string[]` | List of index names to get errors for |

| Return value of<br>`store.Maintenance.Send(getIndexErrorsOp)`| Description |
| - | - |
| `IndexErrors[]` | List of `IndexErrors` classes - see definition below.<br>An exception is thrown if any of the specified indexes doesn't exist. |

{CODE syntax_2@ClientApi\Operations\Maintenance\Indexes\GetIndexErrors.cs /}

{CODE syntax_3@ClientApi\Operations\Maintenance\Indexes\GetIndexErrors.cs /}

{PANEL/}

## Related Articles

### Indexes

- [What are Indexes](../../../../indexes/what-are-indexes)
- [Creating and Deploying Indexes](../../../../indexes/creating-and-deploying)
- [Debugging Index Errors](../../../../indexes/troubleshooting/debugging-index-errors)
- [Index Administration](../../../../indexes/index-administration)

### Operations

- [How to Reset Index](../../../../client-api/operations/maintenance/indexes/reset-index)
- [Delete index errors](../../../../client-api/operations/maintenance/indexes/delete-index-errors)
