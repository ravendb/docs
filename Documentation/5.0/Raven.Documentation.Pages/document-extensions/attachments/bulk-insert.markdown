# Bulk Insert: How to Add Attachments

---

{NOTE: }

* `store.BulkInsert` is RavenDB's high-performance data insertion operation.  
  Use its `AttachmentsFor` interface's `Store` method to add attachments with great speed.  

* In this page:  
      * [Syntax](../../document-extensions/attachments/bulk-insert#syntax)  
      * [Usage Flow](../../document-extensions/attachments/bulk-insert#usage-flow)  
      * [Usage Sample](../../document-extensions/attachments/bulk-insert#usage-sample)  

{NOTE/}

{PANEL: Syntax}

*   `AttachmentsFor`
    {CODE AttachmentsFor-definition@DocumentExtensions\Attachments\BulkInsert.cs /}  

     | Parameters | Type | Description |
     |:-------------|:-------------|:-------------|
     | `id` | `string` | Document ID |

*   `AttachmentsFor.Store`
    {CODE AttachmentsFor.Store-definition@DocumentExtensions\Attachments\BulkInsert.cs /}  

     | Parameters | Type | Description |
     |:-------------|:-------------|:-------------|
     | `name` | `string` | Attachment Name |
     | `stream` | `Stream` | Attachment Stream |
     | `contentType` | `string` | Attachment Type (default: null)|

{PANEL/}

{PANEL: Usage Flow}

* Create a `store.BulkInsert` instance.  
* Pass the instance's `AttachmentsFor` interface -  
   * Document ID  
* Call `Store` as many times as you like. Pass it -  
   * The attachment Name, Stream, and Type  

{PANEL/}

{PANEL: Usage Sample}

* In this sample, we attach a file to all User documents.  
   {CODE bulk-insert-attachment@DocumentExtensions\Attachments\BulkInsert.cs /}  

{PANEL/}

## Related articles

### Attachments

- [Storing](../../document-extensions/attachments/storing)
- [Loading](../../document-extensions/attachments/loading)
- [Deleting](../../document-extensions/attachments/deleting)
