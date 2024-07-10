# Delete Index Operation

---

{NOTE: }

* Use `DeleteIndexOperation` to remove an index from the database.

* The index will be deleted from all the database-group nodes.

* In this page:
    * [Delete index example](../../../../client-api/operations/maintenance/indexes/delete-index#delete-index-example)
    * [Syntax](../../../../client-api/operations/maintenance/indexes/delete-index#syntax)

{NOTE/}

---

{PANEL: Delete index example}

{CODE-TABS}
{CODE-TAB:csharp:Sync delete_index@ClientApi\Operations\Maintenance\Indexes\DeleteIndex.cs /}
{CODE-TAB:csharp:Async delete_index_async@ClientApi\Operations\Maintenance\Indexes\DeleteIndex.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

{CODE syntax@ClientApi\Operations\Maintenance\Indexes\DeleteIndex.cs /}

| Parameter     | Type     | Description             |
|---------------|----------|-------------------------|
| **indexName** | `string` | Name of index to delete |

{PANEL/}

## Related Articles

### Indexes

- [What are Indexes](../../../../indexes/what-are-indexes)
- [Creating and Deploying Indexes](../../../../indexes/creating-and-deploying)
- [Index Administration](../../../../indexes/index-administration)

### Operations

- [How to Get Index](../../../../client-api/operations/maintenance/indexes/get-index)
- [How to Put Indexes](../../../../client-api/operations/maintenance/indexes/put-indexes)
