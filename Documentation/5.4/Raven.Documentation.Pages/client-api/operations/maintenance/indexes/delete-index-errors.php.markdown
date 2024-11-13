# Delete Index Errors Operation

---

{NOTE: }

* Use `DeleteIndexErrorsOperation` to delete indexing errors.  

* The operation will be executed only on the server node that is defined by the current [client-configuration](../../../../client-api/configuration/load-balance/overview#client-logic-for-choosing-a-node).

* Deleting the errors will only **clear the index errors**.  
  An index with an 'Error state' will Not be set back to 'Normal state'.  

* To just get index errors see [get index errors](../../../../client-api/operations/maintenance/indexes/get-index-errors). 

* In this page:
    * [Delete errors from all indexes](../../../../client-api/operations/maintenance/indexes/delete-index-errors#delete-errors-from-all-indexes)
    * [Delete errors from specific indexes](../../../../client-api/operations/maintenance/indexes/delete-index-errors#delete-errors-from-specific-indexes)
    * [Syntax](../../../../client-api/operations/maintenance/indexes/delete-index-errors#syntax)

{NOTE/}

---

{PANEL: Delete errors from all indexes}

{CODE:php delete_errors_all@ClientApi\Operations\Maintenance\Indexes\DeleteIndexErrors.php /}

{PANEL/}

{PANEL: Delete errors from specific indexes}

{CODE:php delete_errors_specific@ClientApi\Operations\Maintenance\Indexes\DeleteIndexErrors.php /}

{PANEL/}

{PANEL: Syntax}

{CODE:php syntax@ClientApi\Operations\Maintenance\Indexes\DeleteIndexErrors.php /}

| Parameters | Type | Description |
| - | - | - |
| **$indexNames** | `StringArray`<br>`array`<br>`string` | List of index names to delete errors from.<br>An exception is thrown if any of the specified indexes does not exist. |

{PANEL/}

## Related Articles

### Indexes

- [What are Indexes](../../../../indexes/what-are-indexes)
- [Creating and Deploying Indexes](../../../../indexes/creating-and-deploying)
- [Debugging Index Errors](../../../../indexes/troubleshooting/debugging-index-errors)
- [Index Administration](../../../../indexes/index-administration)

### Operations

- [How to Reset Index](../../../../client-api/operations/maintenance/indexes/reset-index)
- [Get index errors](../../../../client-api/operations/maintenance/indexes/get-index-errors)
