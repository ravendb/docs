# Documents Compression
---

{NOTE: }

* Go to **Settings** > **Documents Compression** to configure compression for a 
particular database.  

* Go to the **Storage Report** to see the contents and size of your database' 
storage.  

* In this page:  
  * [Configure Documents Compression](../../../studio/database/settings/documents-compression#configure-documents-compression)  
  * [Database Storage Report](../../../studio/database/settings/documents-compression#database-storage-report)  

{NOTE/}

---

{PANEL: Configure Documents Compression}

RavenDB will compress documents for selected collections, when storing those documents.  
The compression will be applied to:  

* Newly created documents  
* Existing documents that are modified and saved  

An existing document opened just for reading will **not** be compressed.  

![Document Compression Configuration](images/documents-compression.png "Document Compression Configuration")

1. Select the collections for which to activate compression.  
2. List of selected collections.  
3. Toggle whether to activate compression for all revisions of all collections.  
4. Save and apply configuration.  

{PANEL/}

{PANEL: Database Storage Report}

Each database has a page **Stats** > **Storage Report** that displays detailed information 
about the contents of its storage. Go here to see the effects of the compression. Refresh the 
page to see changes.  

![RavenDB Storage Report](images/storage-report.png "RavenDB Storage Report")

1. The size of each rectangle is proportional to its storage size.  
   Click any rectangle to view its subdirectories details.  

{PANEL/}

## Related Articles

### Server

- [Documents Compression](../../../server/storage/documents-compression)

### Studio

- [Database Record](../../../studio/database/settings/database-record)
