# Revisions Troubleshooting
---

{NOTE: }

* In this page:  
  * [`ThrowRevisionKeyTooBig` exception](../../document-extensions/revisions/troubleshooting#throwrevisionkeytoobig-exception)  

{NOTE/}

---

{PANEL: `ThrowRevisionKeyTooBig` exception}

RavenDB allows **document revisions** and their **tombstones** to have IDs of up to **1,536 bytes**.  
A `ThrowRevisionKeyTooBig` exception will be thrown when an attempt is made to store a revision or a revision tombstone 
whose ID length exceeds this limit.  

* **How does a revision ID become too long?**  
   * RavenDB servers base the IDs they give revisions (and their tombstones) 
     on the revisions' [change vectors](../../server/clustering/replication/change-vector).  
   * Change vectors, on their part, are not limited and may grow [beyond the 1,536 bytes limit](../../client-api/operations/maintenance/clean-change-vector).  
   * It may therefore happen that a server would try to give a revision an ID, find its 
     change vector longer than 1,536 bytes, and fail with a `ThrowRevisionKeyTooBig` exception.  
* **How are Change Vectors bloated?**  
   * A revision's change vector is comprised of the IDs of databases that have handled the revision 
     over time.  
   * In some cases, such as transferring a revision via [import](../../studio/database/tasks/import-data/import-from-ravendb) 
     or [external replication](../../server/ongoing-tasks/external-replication), the ID of the 
     revision's Source database is no longer needed but is still added to the revision's change 
     vector on the Destination database.  
   * Repeatedly transferring a revision this way may bloat its change vector beyond the 1,536 bytes 
     limit. When the destination database attempts to give such a revision an ID to store it by, 
     it will fail with a `ThrowRevisionKeyTooBig` exception.  
* **What to do?**  
   * Revision **IDs** can be shortened by minimizing revision **change vectors**.  
   * To shorten revision change vectors, register IDs of databases that are irrelevant to this 
     server via Studio's **Unused Database IDs** view, as shown below. IDs listed in this view 
     will be omitted from revision change vectors.  

        ![Unused Database IDs List](images\troubleshooting_rev_unused-db-IDs.png "Unused Database IDs List")

        1. **Unused Database IDs**  
           Click to open the **Unused Database IDs** view.  
        2. **Database ID**  
           The ID of this database on the current cluster node.  
        3. **Change Vector**  
           A list of IDs that may be added to the list.  
           To add an ID to the unused IDs list, click the **Add to unused** button to its right.  
        4. **Save**  
           Click to save the current list of unused IDs.  
        5. **Unused Database IDs**  
           A. Click the bar to enter an ID manually, and add it to the list using the **Add ID** button.  
           B. This is the list of unused database IDs. To remove an ID from the list, click the trash bin to its right.  

* **What to be aware of:**  
  When adding a database ID to the **Unused Database IDs** list via studio:  
   * **Do not** add IDs of databases that are currently in use.  
     The ID of a RavenDB database can be found in the Studio > **Stats** view.  
     
         ![Studio Stats: Database ID](images\troubleshooting_rev_stats-DB-ID.png "Studio Stats: Database ID")

   * If an external replication task is running:  
     **Do not** add the IDs of databases that are used by the destination database.  
     **Add** the unused IDs on the **destination** database first, to prevent conflicts.  

   * **Do not** use the IDs indicated by the database record `DatabaseTopologyIdBase64` and 
     `ClusterTransactionIdBase64` properties.  
     Find these IDs using the Studio > Settings > **Database Record** view.  

         ![Database Record](images\troubleshooting_rev_db-record.png "Database Record")

---

#### When is this check Enabled:

* **New databases only**  
  Checking revisions ID length is enabled only for **new databases**.  
   * A database is regarded as **new**, and its revisions ID length **will** be checked, if 
     its version is not defined in the database record or the version is `5.4.203` or newer.  
   * Revisions ID length will **not** be checked for databases older than `5.4.203`.  
* **Imported databases**  
  Importing a database is always regarded as the creation of a new database.  
  An exception **will** therefore be thrown if the ID of an imported revision 
  exceeds 1,536 bytes, regardless of the imported revision's database version.  
* **Restoring database from backup**  
   * Revisions ID length **will** be checked if the database version is not defined in its 
     restored database record or if the version is `5.4.203` or newer.  
   * Revision ID lengths will **not** be checked when restoring databases older than `5.4.203`.  
* **Restoring database from a snapshot**  
  Revisions ID length will not be checked while restoring a snapshot, since snapshots are 
  restored as an image. If revision IDs longer than 1,536 bytes exist in the restored database, 
  they are in it because the database is of an older version than `5.4.203` and doesn't perform 
  this check.  
* **Receiving a revision via replication**  
  The check is not performed when receiving a revision or a revision tombstone via replication.  

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
* [External Replication](../../studio/database/tasks/ongoing-tasks/external-replication-task)  
* [Import](../../studio/database/tasks/import-data/import-from-ravendb)  

### Server

* [External Replication](../../server/ongoing-tasks/external-replication)  
