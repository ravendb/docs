# Operations: How to Check if an Index has Changed

**IndexHasChangedOperation** will let you check if the given index definition differs from the one on a server. This might be useful when you want to check the prior index deployment, if the index will be overwritten, and if indexing data will be lost.

## Syntax

{CODE:java index_has_changed_1@ClientApi\Operations\IndexHasChanged.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **indexDef** | `IndexDefinition` | index definition |

| Return Value | |
| ------------- | ----- |
| true | if an index **does not exist** on a server |
| true | if an index definition **does not match** the one from the **indexDef** parameter |
| false | if there are no differences between an index definition on the server and the one from the **indexDef** parameter |

## Example

{CODE:java index_has_changed_2@ClientApi\Operations\IndexHasChanged.java /}

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
