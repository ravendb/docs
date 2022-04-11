# Revisions 
---

{NOTE: }

* The **Revisions** feature will create a revision (snapshot) of a document 
  every time the document is updated and upon its deletion.  
  Once revisions are created for a document, you can observe them and revert 
  the document's live version to any of its past revisions.  
* Tracking document revisions allows you, for example, to check how an employee's 
  contract has changed over time, revert a single corrupted document without restoring 
  a backup, or conduct a full-scale audit.  
* Revisions can be enabled for **all collections** or for **specific collections**.  
* Old revisions can be **automatically purged** to free storage space.  
* Revisions can be configured using API methods or via Studio.  

* In this page:  
  * [Configuration](../../document-extensions/revisions/overview#configuration)  
     * [Via Studio](../../document-extensions/revisions/overview#configuring-revisions-using-studio)  
     * [Via API](../../document-extensions/revisions/overview#configuring-revisions-using-the-client-api)  
  * [How it Works](../../document-extensions/revisions/overview#how-it-works)  
  * [Enabling or Disabling on an Existing Database](../../document-extensions/revisions/overview#enabling-or-disabling-on-an-existing-database)  
  * [Storage Concerns](../../document-extensions/revisions/overview#storage-concerns)  
  * [Force Revision Creation](../../document-extensions/revisions/overview#force-revision-creation)  

{NOTE/}

---

{PANEL: How it Works}

![Figure 1: Revisions](images\revisions\revisions1.png "Figure 1: Revisions")


This is a revision of the document (you can navigate to the document by clicking on `See the current document`) which is stored on the revisions storage.
Now, let's modify the original document. This would create another revision:

![Figure 2: Revisions, Modified](images\revisions\revisions2.png "Figure 2: Revisions, Modified")

As you can see, we have a full audit record of all the changes that were made to the document.

You can access the revisions of a specific document by the document's ID ("users/1").
Or you can access a specific revision by its change vector or by a specific date.
Accessing a revision by a change vector would return a specific revision, 
while accessing a revision by a date would return the revision on this specific date if exists,
and if not it would return the revision right before this date.

Now, let's delete the document. 
The document would be removed but a revision will be created, so you aren't going to lose the audit trail if the document is deleted.

In order to see orphaned revisions (revisions of deleted documents), you can go to the `Documents > Revisions Bin` section in the studio, 
which would list all revisions without existing document:

![Figure 3: Revisions, Deleted](images\revisions\revisions3.png "Figure 3: Revisions, Deleted")

If you'll go and create another document with this ID (users/1), then the revision won't be shown anymore in the Revision Bin section, 
but you can navigate to the document and see it's revisions, including the deleted ones.

Clicking on the revision we can also navigate to the other revisions of this document:

![Figure 4: Revisions, Deleted - other revisions](images\revisions\revisions4.png "Figure 4: Revisions, Deleted - other revisions")

The revisions feature attempts to make things as simple as possible. Once it is enabled, you'll automatically get the appropriate audit trail.

{PANEL/}

## Related Articles

### Client API

- [Session: What are Revisions](../../document-extensions/revisions/client-api/session/what-are-revisions)  
- [Session: Loading Revisions](../../document-extensions/revisions/client-api/session/loading)  
- [Operations: How to Configure Revisions](../../document-extensions/revisions/client-api/operations/configure-revisions)  
- [Revisions in Data Subscriptions](../../client-api/data-subscriptions/advanced-topics/subscription-with-revisioning)  
