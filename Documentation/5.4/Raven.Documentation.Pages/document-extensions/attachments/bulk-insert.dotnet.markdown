# Bulk Insert Attachments

---

{NOTE: }

* [BulkInsert](../../client-api/bulk-insert/how-to-work-with-bulk-insert-operation) is RavenDB's high-performance data insertion operation.  
  Use its `AttachmentsFor` interface to add attachments to documents with great speed.  

* In this page:  
    * [Usage flow](../../document-extensions/attachments/bulk-insert#usage-flow)  
    * [Usage example](../../document-extensions/attachments/bulk-insert#usage-example)
    * [Syntax](../../document-extensions/attachments/bulk-insert#syntax)

{NOTE/}

{PANEL: Usage flow}

* Create a `BulkInsert` instance.  

* Pass the Document ID to the instance's `AttachmentsFor` method.

* To add an attachment, call `Store`.  
  Pass it the attachment's name, stream, and type (optional).  
  The `Store` function can be called repeatedly as necessary.

* Note:  
  If an attachment with the specified name already exists on the document,  
  the bulk insert operation will overwrite it.

{PANEL/}

{PANEL: Usage example}

In this example, we attach a file to all User documents that match a query.  
 
{CODE-TABS}
{CODE-TAB:csharp:BulkInsert_Attachments bulk_insert_attachment@DocumentExtensions\Attachments\BulkInsert.cs /}
{CODE-TAB:csharp:User_Class user_class@DocumentExtensions\Attachments\BulkInsert.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

{CODE AttachmentsFor-definition@DocumentExtensions\Attachments\BulkInsert.cs /}

| Parameter  | Type     | Description                                              |
|------------|----------|----------------------------------------------------------|
| `id`       | `string` | The document ID to which the attachment should be added. |
 
{CODE AttachmentsFor.Store-definition@DocumentExtensions\Attachments\BulkInsert.cs /}

| Parameter     | Type     | Description                        |
|---------------|----------|------------------------------------|
| `name`        | `string` | Name of attachment                 |
| `stream`      | `Stream` | The attachment's stream            |
| `contentType` | `string` | Type of attachment (default: null) |

{PANEL/}

## Related articles

### Attachments

- [Storing](../../document-extensions/attachments/storing)
- [Loading](../../document-extensions/attachments/loading)
- [Deleting](../../document-extensions/attachments/deleting)
