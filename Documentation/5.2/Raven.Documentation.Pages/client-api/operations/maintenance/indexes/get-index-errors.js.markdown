# Get Index Errors Operation

---

{NOTE: }

* Use `GetIndexErrorsOperation` to get errors encountered during indexing.

* The index errors will be retrieved only from the server node defined by the current [client-configuration](../../../../client-api/configuration/load-balance/overview#client-logic-for-choosing-a-node).

* To clear index errors see [delete index errors](../../../../client-api/operations/maintenance/indexes/delete-index-errors).

* In this page:
    * [Get errors for all indexes](../../../../client-api/operations/maintenance/indexes/get-index-errors#get-errors-for-all-indexes)
    * [Get errors for specific indexes](../../../../client-api/operations/maintenance/indexes/get-index-errors#get-errors-for-specific-indexes)
    * [Syntax](../../../../client-api/operations/maintenance/indexes/get-index-errors#syntax)

{NOTE/}

---

{PANEL: Get errors for all indexes}

{CODE:nodejs get_errors_all@ClientApi\Operations\Maintenance\Indexes\getIndexErrors.js /}

{PANEL/}

{PANEL: Get errors for specific indexes}

{CODE:nodejs get_errors_specific@ClientApi\Operations\Maintenance\Indexes\getIndexErrors.js /}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax_1@ClientApi\Operations\Maintenance\Indexes\getIndexErrors.js /}

| Parameters | Type | Description |
| - | - | - |
| __indexNames__ | string[] | List of index names for which to get errors |

| Return value of `store.maintenance.send(getIndexErrorsOp)`| |
| - | - |
| __object[]__ |  List of 'index errors' objects - see definition below.<br>An exception is thrown if any of the specified indexes do not exist. |

{NOTE: }
{CODE:nodejs syntax_2@ClientApi\Operations\Maintenance\Indexes\getIndexErrors.js /}
{CODE:nodejs syntax_3@ClientApi\Operations\Maintenance\Indexes\getIndexErrors.js /}
{NOTE/}

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
