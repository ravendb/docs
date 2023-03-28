# Storage Report
---

{NOTE: }

* The database storage report displays detailed information about the database's physical storage.  

* Go to __Stats > Storage Report__ in the Studio.  
  Refresh the page to the effect of [Documents Compression](../../../server/storage/documents-compression) and [Database Compaction](../../../client-api/operations/server-wide/compact-database).

* In this page:  
  * [Database storage report](../../../studio/database/stats/storage-report#database-storage-report)  

{NOTE/}

---

{PANEL: Database storage report}

![Storage Report](images/storage-report.png "Storage Report")

1. Go to __Stats > Storage Report__.  

2. This is the database location of on disk.  

3. __Database components__:  

   * Each rectangle is a database component.  
     The size of each rectangle is proportional to its storage size.  
   
   * To see storage details, hover over a section or see the detailed table below.

   * Click any rectangle to view its subdirectories' details.

4. __Set documents compression__:
   
   * Click to configure [documents compression](../../../studio/database/settings/documents-compression) from the Studio.  

5. __Compact database__:  

   * Click to compact the database on the local node the browser is opened on.  
     This will remove empty gaps on disk that still occupy space after deletes.  
     You can choose what should be compacted: documents and/or selected indexes.  

   * The compaction operation triggers documents compression on all existing documents in collections  
     that are configured for compression. See [compaction triggers compression](../../../client-api/operations/server-wide/compact-database#compaction-triggers-compression).  
   
   * Note: During this asynchronous operation __the database will be offline__.
 
   * The database can also be compacted from the Client API, see [compact database operation](../../../client-api/operations/server-wide/compact-database). 

{PANEL/}

## Related Articles

### Server

- [Documents compression](../../../server/storage/documents-compression)

### Client API 

- [Compact database operation](../../../client-api/operations/server-wide/compact-database)
