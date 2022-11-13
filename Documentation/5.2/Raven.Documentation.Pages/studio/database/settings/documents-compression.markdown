# Documents Compression
---

{NOTE: }

* The **Documents Compression** feature uses the [Zstd compression algorithm](https://github.com/facebook/zstd) to 
  continuously create a more efficient data model with better compression ratios.  

* Go to **Settings** > **Documents Compression** to configure compression for a 
particular database.  
  * Revisions are compressed by default.  
  * **Compress all collections** or specific collections can be selected for compression in this view.  

* Compression will be applied to:  
  * Newly created documents  
  * Existing documents when they are modified and saved  

{INFO: Compressing all existing documents}

Only new or modified documents from selected collections will be compressed.  
To compress all existing documents without modifying them,  
use [CompactDatabaseOperation](../../../client-api/operations/server-wide/compact-database) 
after configuring which collections to compress.

{INFO/}

* Go to **Stats** > **Storage Report** to see the contents and size of your database 
storage, and to compact the database.  

* In this page:  
  * [Configure Documents Compression](../../../studio/database/settings/documents-compression#configure-documents-compression)  
  * [Database Storage Report](../../../studio/database/settings/documents-compression#database-storage-report)  


{NOTE/}

---

{PANEL: Configure Documents Compression}

RavenDB will compress documents in selected collections or in all collections when storing those documents.  
The compression will be applied to:  

* Newly created documents  
* Existing documents that are modified and saved  

* If opened just for reading, **existing documents will not be compressed** .  
  * If you also want to compress existing documents without editing them, 
    after configuring the collections to compress, you can use the [CompactDatabaseOperation](../../../client-api/operations/server-wide/compact-database).
    While removing empty gaps which occupy space in your database, 
    this operation will also trigger compression on collections that were configured.  
     * Compaction can also be done via the studio [Database Storage Report](../../../studio/database/settings/documents-compression#database-storage-report) view.
     * Note: Compression and compaction are two different methods.  
       Compression reduces the amount of storage that data uses,  
       while compaction removes empty gaps that still occupy space after deletes.  

* Compression is configured in the [Database Record](../../../studio/database/settings/database-record).  

![Document Compression Configuration](images/documents-compression.png "Document Compression Configuration")

1. **Select Collection(s)**  
   Select the collections on which to activate compression or select  
    * **Compress all collections**  
      ![Compress All Collections](images/documents-compression-all-collections.png "Compress All Collections")  
      Toggle to compress new or modified documents in all collections in this particular database.  
2. **Collections Selected**  
   List of selected collections (seen if "Compress all collections" is toggled off).  
3. **Compress Revisions**  
   Toggle whether to activate compression for all revisions of all collections.  
4. **Save**  
   Save and apply configuration.  



{PANEL/}

{PANEL: Database Storage Report}

The database storage report displays detailed information about the database's physical storage.  
Go to this page (Stats > Storage Report) to see the effect of compression and compaction.  
(Refresh the page to see the changes.)  

![sampleDB Storage Report](images/storage-report.png "sampleDB Storage Report")

1. **Database Component**  
   * The size of each rectangle is proportional to its storage size.  
   * Click any rectangle to view its subdirectories' details.  
   * To see storage details, hover over a section or see the detailed list below.  
2. **Compact Database**  
   * Click to [compact the database](../../../client-api/operations/server-wide/compact-database) 
     which removes empty gaps that occupy space in your database.
   * All documents will be compressed in any collection where compression is configured when 
     using the `CompactDatabaseOperation`.  
   * Note: During this asynchronous operation **the database will be offline**.

{PANEL/}

## Related Articles

### Server

- [Documents Compression](../../../server/storage/documents-compression)
- [Database Configuration - Compress Revisions Default](../../../server/configuration/database-configuration#databases.compression.compressrevisionsdefault)

### Studio

- [Database Record](../../../studio/database/settings/database-record)

### API 

- [To Compact a Database](../../../client-api/operations/server-wide/compact-database)
