# Revisions Troubleshooting
---

{NOTE: }

* In this page:  
  * [`ThrowDocumentIdTooBig` exception](../../document-extensions/revisions/troubleshooting#throwdocumentidtoobig-exception)  

{NOTE/}

---

{PANEL: `ThrowDocumentIdTooBig` exception}

A `ThrowDocumentIdTooBig` exception is thrown when attempting to store a document revision 
whose ID length exceeds **512 bytes**.  

* **When does this happen?**  
  A document revision's *ID* is based upon the revision's [change vector](../../server/clustering/replication/change-vector), 
  which **is** allowed to grow [beyond 512 bytes](../../client-api/operations/maintenance/clean-change-vector).  
  The change vector includes IDs of used cluster instances, but may also include IDs of unused databases (like 
  the origin database of a document that was once imported via external replication). This way, a change vector 
  may lengthen beyond the limit allowed for revision IDs.  

* **What to do?**  
  Keep revision IDs short, by minimizing the length of the change vectors they are based upon.  
  Do this by adding IDs of unused databases to the **Unused Database IDs** list via Studio.  
  When RavenDB creates a change vector, it will scan this list and skip any database ID it 
  finds in it to keep change vector size at bay.  

     ![Unused Database IDs List](images\revisions_unused-database-IDs.png "Unused Database IDs List")

* **What to be aware of:**  
  When adding a database ID to the **Unused Database IDs** list via studio:  
   * **Do not** add IDs of databases that are currently in use.  
   * If external replication tasks are running:  
     **Do not** add database IDs used by the destination databases.  
     **Add** the unused IDs on the destination databases first.  
   * **Do not** use `DatabaseTopologyIdBase64` or `ClusterTransactionIdBase64` (required for cluster tx).  

---

#### When is this check Enabled:

* **New databases only**  
  Checking revisions ID length is enabled only for **new databases**.  
   * A database is regarded **new**, and its revisions ID length **will** be checked, if 
     its version is not defined in the database record or the version is **5.4.200** or newer.  
   * Revisions ID length will **not** be checked for databases older than **5.4.200**.  
* **Imported databases**  
  Importing a database is always regarded as the creation of a new database.  
  An exception **will** therefore be thrown if the ID of an imported revision 
  exceeds 512 bytes, regardless of the imported revision's database version.  
* **Restoring database from backup**  
   * Revisions ID length **will** be checked if the database version is not defined in its 
     restored database record or if the version is **5.4.200** or newer.  
   * Revision ID lengths will **not** be checked when restoring databases older than **5.4.200**.  
* **Restoring database from a snapshot**  
  Revisions ID length will not be checked while restoring a snapshot, since snapshots are 
  restored as an image. If revision IDs longer than 512 exist in the restored database, it 
  is because its version is older than **5.4.200** in any case and this check is not required.  

{PANEL/}

## Related Articles

### Document Extensions

* [Document Revisions Overview](../../document-extensions/revisions/overview)  
* [Revisions and Other Features](../../document-extensions/revisions/revisions-and-other-features)  

### Client API

* [Revisions: API Overview](../../document-extensions/revisions/client-api/overview)  
* [Operations: Configuring Revisions](../../document-extensions/revisions/client-api/operations/configure-revisions)  
* [Session: Loading Revisions](../../document-extensions/revisions/client-api/session/loading)  

### Studio

* [Settings: Document Revisions](../../studio/database/settings/document-revisions)  
* [Document Extensions: Revisions](../../studio/database/document-extensions/revisions)  
