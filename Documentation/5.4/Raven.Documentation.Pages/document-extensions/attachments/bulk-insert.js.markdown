# Bulk Insert Attachments

---

{NOTE: }

* [BulkInsert](../../client-api/bulk-insert/how-to-work-with-bulk-insert-operation) is RavenDB's high-performance data insertion operation.  
  Use its `attachmentsFor` interface to add attachments to documents with great speed.  

* In this page:  
    * [Usage flow](../../document-extensions/attachments/bulk-insert#usage-flow)  
    * [Usage example](../../document-extensions/attachments/bulk-insert#usage-example)
    * [Syntax](../../document-extensions/attachments/bulk-insert#syntax)

{NOTE/}

{PANEL: Usage flow}

* Create a `bulkInsert` instance.  

* Pass the Document ID to the instance's `attachmentsFor` method.

* To add an attachment, call `store`.  
  Pass it the attachment's name, content, and type (optional).  
  The `store` function can be called repeatedly as necessary.

{PANEL/}

{PANEL: Usage example}

In this example, we attach a file to all User documents that match a query.  
 
{CODE-TABS}
{CODE-TAB:nodejs:BulkInsert_Attachments bulk_insert_attachment@documentExtensions\attachments\bulkInsert.js /}
{CODE-TAB:nodejs:User_Class user_class@documentExtensions\attachments\bulkInsert.js /}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax_1@documentExtensions\attachments\bulkInsert.js /}

| Parameter  | Type     | Description                                              |
|------------|----------|----------------------------------------------------------|
| `id`       | `string` | The document ID to which the attachment should be added. |
 
{CODE:nodejs syntax_2@documentExtensions\attachments\bulkInsert.js /}

| Parameter     | Type     | Description                        |
|---------------|----------|------------------------------------|
| `name`        | `string` | Name of attachment                 |
| `bytes`       | `Buffer` | The attachment's content           |
| `contentType` | `string` | Type of attachment (default: null) |

{PANEL/}

## Related articles

### Attachments

- [Storing](../../document-extensions/attachments/storing)
- [Loading](../../document-extensions/attachments/loading)
- [Deleting](../../document-extensions/attachments/deleting)
