# Revisions: Revert Revisions

---

{NOTE: }

* Use **Revert Revisions** to revert your database to its state at 
  a specified point in time (or as close to that state as we can get).  

* Being able to bring the database back to any of its historical states 
  can, for example, ease database auditing, help understand changes made 
  in documents in their historical context, and instantly restore the 
  database to one of its past states without searching and restoring 
  a stored backup.  
  
* In this page:  
   * [Revert Revisions](../../document-extensions/revisions/revert-revisions#revert-revisions)  

{NOTE/}

---

{PANEL: Revert Revisions}

**Revert Revisions** is used to revert **all the documents** in the database 
to their state at a historical point in time.  

When the process is executed:  

* Documents created **before** the point in time will be **kept**.  
   * Any of these documents that own revisions will be **reverted** 
     to the revision created at the specified point in time or to 
     the nearest revision preceding this time.  
     {INFO: }
     When the number of revisions that a document may keep is 
     [limited](../../document-extensions/revisions/overview#revisions-configuration-properties)
     (by number or age), only the most recent revisions are kept 
     for it.  
     Such documents, that were created **before** the specified point 
     in time but may hold revisions that were created **after** that 
     time, may be reverted to a revision newer than the specified time.  
     By doing so we make sure that all the documents that existed 
     at the time you specified still exist after the reversion.  
     {INFO/}
   * To revert a document to one of its revisions, RavenDB will create 
     a new revision for the document that copieschanged to "copies" the historical 
     revision, effectively replacing the live version of the document.  
* Documents created **after** the specified point in time will be **deleted**.  
* Database entities other than documents, such as ongoing tasks, will **not** 
  be reverted by this process.  

---

To Revert Revisions, open the Studio Settings > **Document Revisions** view.  

![Document Revisions View](images/revert-revisions-1.png "Document Revisions View")

1. **Document Revisions View**  
   Open the view to configure and revert revisions.  
2. **Revisions Configuration**  
   Our ability to revert database documents to their past revisions, 
   and the accuracy of the reversion (how close we can get to the database 
   state at the specified time), depend upon continuous creation 
   of revisions.  
   Make sure that a [Revisions configuratio](../../document-extensions/revisions/overview#revisions-configuration) 
   that suits your needs is defined.  
3. **Revert Revisions**  
   Click to specify a point in time to revert the database to.  

---

![Revert Revisions](images/revert-revisions-2.png "Revert Revisions")

1. **Point in Time**  
   Specify the point in time to revert documents to.  
   Documents will be reverted to a revision that was created for them 
   at the specified point in time, or to the nearest revision preceding 
   this time.  

2. **Time Window**  
   Set a Time Window value to limit the search by.  
   Restricting the search to the set time window prevents RavenDB from 
   conducting unnecessarily long searches and reversion to revisions 
   that are too old.  
    * To revert each document to its state at the specified point in time, 
      RavenDB goes through the document's revisions in the revisions storage, 
      where revisions are ordered by change vector, not by creation time.  
    * The **Time Window** value sets a limit to the search.  
      If the search reaches a revision whose creation time exceeds the time 
      window limit, the search will **end** and the document will **not be reverted**.  
    * the search limit is: `Point in Time` **+** `Time Window`  
      {INFO: E.g.}
      If `Point in Time` is **2 days ago**  
      and `Time Window`is **4 days**  
      the search will end if a revision that was created more than **6 days ago** is reached.  
      {INFO/}

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
