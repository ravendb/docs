# Documents Compression
---

{NOTE: }

* The **Documents Compression** feature employs the `Zstd` compression algorithm  
  to achieve more efficient data storage with constantly improving compression ratios.  
  Learn more in this [overview](../../../server/storage/documents-compression#overview).

* Documents compression can be set for all collections, selected collections, and revisions.  
  Default compression settings are [configurable](../../../server/configuration/database-configuration#databases.compression.compressallcollectionsdefault). 

* When turned on, compression will be applied to:  
    * **New documents**:  
        * A new document that is saved will be compressed.  
    * **Existing documents**:  
        * Existing documents that are modified and saved will be compressed.  
        * Existing documents that are Not modified will only be compressed when executing the    
          [compact database operation](../../../client-api/operations/server-wide/compact-database#compaction-triggers-compression).

* From the Studio, go to **Settings** > **Documents Compression** to set compression for a particular database.  
  Compression can also be set from the [Client API](../../../server/storage/documents-compression).

* To see the contents and size details of your database storage go to the [Storage Report](../../../studio/database/stats/storage-report) view in the Studio.  

* In this page:  
  * [Set documents compression from Studio](../../../studio/database/settings/documents-compression#set-documents-compression-from-studio)  

{NOTE/}

---

{PANEL: Set documents compression from Studio}

![Document Compression Configuration](images/documents-compression.png "Set Document Compression")

1. Go to **Settings > Document Compression**

2. Toggle on to compress documents from ALL collections.

3. Or, select specific collections to be compressed.

4. Toggle on to compress revisions of all collections.

5. Save the configuration.  

{PANEL/}

## Related Articles

### Server

- [Documents Compression](../../../server/storage/documents-compression)
- [Database Configuration - Compress Revisions Default](../../../server/configuration/database-configuration#databases.compression.compressrevisionsdefault)

### Studio

- [Database Record](../../../studio/database/settings/database-record)

### API 

- [To Compact a Database](../../../client-api/operations/server-wide/compact-database)
