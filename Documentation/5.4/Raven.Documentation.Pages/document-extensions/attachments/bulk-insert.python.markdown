# Bulk Insert Attachments

---

{NOTE: }

* [bulk_insert](../../client-api/bulk-insert/how-to-work-with-bulk-insert-operation) is RavenDB's 
  high-performance data insertion operation.  
  Use its `attachments_for` interface to add attachments to multiple documents with great speed.  
* Use `store` 

* In this page:  
    * [Usage flow](../../document-extensions/attachments/bulk-insert#usage-flow)  
    * [Usage example](../../document-extensions/attachments/bulk-insert#usage-example)

{NOTE/}

{PANEL: Usage flow}

* Create a `bulk_insert` instance.  

* Pass the Document ID to the instance's `attachments_for` method.

* To add an attachment, call the `store` method.  
  Pass it the attachment's name, stream, and type (optional).  
  `store` can be called repeatedly, as many times as needed.  

* Note:  
  If an attachment with the specified name already exists on the document,  
  the bulk insert operation will overwrite it.

{PANEL/}

{PANEL: Usage example}

In this example, we attach a file to all User documents that match a query.  
{CODE:python bulk_insert_attachment@DocumentExtensions\Attachments\BulkInsert.py /}

{PANEL/}

## Related articles

### Attachments

- [Storing](../../document-extensions/attachments/storing)
- [Loading](../../document-extensions/attachments/loading)
- [Deleting](../../document-extensions/attachments/deleting)
