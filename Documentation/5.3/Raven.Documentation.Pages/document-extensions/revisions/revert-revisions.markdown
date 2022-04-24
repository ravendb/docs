# Revisions: Revert Revisions

---

{NOTE: }

* **Revert Revisions** is a Studio operation that allows you to return 
  the database to its state at a specified point in time.  
  To achieve this:  
   * Document revisions at the specified point in time are retrieved
     and saved, creating a new revision and effectively replacing the 
     live document version.  
     If a revision that matches the specified point in time is not found 
     for a document, its latest revision preceding that time is used.  
   * Documents that did not exist at the specified point in time are 
     deleted.  

     {WARNING: Enable Revisions before executing Revert Revisions.}
     If you want the operation to be revertible, you must enable Revisions 
     for collections in which documents may be reverted or deleted.  
     If revisions were created for all documents, you'll be able to 
     revert the whole operation simply by repeating it and specifying 
     a point in time just before you executed the operation.  
     If revisions were **not** created for documents, their revertion 
     or deletion will be final.  
     {WARNING/}

* Being able to bring the database back to any of its historic states 
  can, for example, ease database auditing, help understand changes made 
  in documents in their historical context, and instantly restore the 
  database to one of its past states without searching and restoring 
  a stored backup.  
  
* The accuracy in which the database can be reverted to one of its
  historic states depends upon the frequency in which documents have 
  been revisioned.  

* In this page:  
   * [Revert Revisions](../../document-extensions/revisions/revert-revisions#revert-revisions)  

{NOTE/}

---

{PANEL: Revert Revisions}

To Revert Revisions, reverting the database to a specified point in time, 
open the Studio Settings > **Document Revisions** View.  

![Document Revisions View](images/revert-revisions-1.png "Document Revisions View")

1. **Document Revisions View**  
   Click to configure and Revert revisions.  
2. **Revisions Configuration**  
   Make sure Revisions is Enabled before executing Revert Revisions, 
   by creating default and/or collection-specific configurations for 
   collections that may be affected when Revert Revisions is executed.  
3. **Revert Revisions**  
   Click to specify a point in time to revert the database to.  

---

![Revert Revisions](images/revert-revisions-2.png "Revert Revisions")

1. **Point in Time**  
   Click to specify the point in time that documents will roll back to.  
2. **Time Window**  
   The Time Window value is used for performance optimization.  
   By default, it is set to 96 hours.  

{PANEL/}

## Related Articles
[Revisions](../../server/extensions/revisions)
