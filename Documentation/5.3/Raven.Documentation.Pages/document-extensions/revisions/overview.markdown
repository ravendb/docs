# Revisions Overview
---

{NOTE: }

* The **Revisions** feature creates a revision (snapshot) of a document every 
  time the document is updated and upon its deletion.  
  The trail of revisions created for a document can be observed to track 
  the document's history, and the currently live version of the document 
  can be reverted to any of its past revisions.  
* Tracking document revisions allows you, for example, to check how an employee's 
  contract has changed over time, revert a single corrupted document without restoring 
  a backup, or conduct a full-scale audit.  
* You can create default and collection-specific Revisions **configurations** 
  to determine whether revisions will be created and whether to limit their 
  number per document.  
  RavenDB will check your configurations when documents are modified, 
  and create and purge revisions by your settings.  
* Revisions can be configured and managed using API methods or via Studio.  

* In this page:  
  * [Revisions Configurations](../../document-extensions/revisions/overview#revisions-configurations)  
     * [Creating and Applying Configurations](../../document-extensions/revisions/overview#creating-and-applying-configurations)  
     * [When are Configurations Executed](../../document-extensions/revisions/overview#when-are-configurations-executed)  
  * [How it Works](../../document-extensions/revisions/overview#how-it-works)  
  * [Enabling or Disabling on an Existing Database](../../document-extensions/revisions/overview#enabling-or-disabling-on-an-existing-database)  
  * [Storage Concerns](../../document-extensions/revisions/overview#storage-concerns)  
  * [Force Revision Creation](../../document-extensions/revisions/overview#force-revision-creation)  

{NOTE/}

---

{PANEL: Revisions Configurations}

By default, revisions are created for **all documents** and existing revisions 
are **never purged**.  

* You can change this default behavior by defining a **default configuration** 
  of your own.  

* You can also define **collection-specific configurations** that will override 
  the default configuration for the collections they are defined for.  

* **Configuration properties** determine:  
   * **Whether to Enable or Disable Revisions**.  
     Enabling Revisions instructs RavenDB to Create a new revision and Purge existing 
     ones by your purging settings when documents are modified.  
     Disabling Revisions instructs RavenDB **not** to create or purge revisions.  
   * **Whether and by what limits revisions will be purged**.  
   * Learn [here](../../document-extensions/revisions/client-api/operations/configure-revisions#revisionscollectionconfiguration) 
     about the available configuration options and how to apply them.  

---

### Creating and Applying Configurations

Configurations can be created and applied using Studio or client API methods.  

#### Via Studio

* Use the Studio Settings [Document Revisions page](../../studio/database/settings/document-revisions) 
  to create and manage revision configurations.  
* Use the Documents View [Revisions tab](../../studio/database/document-extensions/revisions) 
  to observe and manage the revisions created for each document.  

#### Via API methods
Follow the links below to learn how to manage revisions using API methods.  

* Revisions Store Operations:  
  [Creating configurations](../../document-extensions/revisions/client-api/operations/configure-revisions)  
  [Getting and Counting Revisions](../../document-extensions/revisions/client-api/operations/get-revisions)  
* Revisions Session methods:  
  [Loading revisions](../../document-extensions/revisions/client-api/session/loading)  
  [Counting Revisions](../../document-extensions/revisions/client-api/session/counting)  
  [Including revisions](../../document-extensions/revisions/client-api/session/including)  

---

### When are Configurations Executed

Creating a Revisions configuration does **not** immediately trigger its execution.  
Configurations are executed:  

* **When documents are modified or deleted**.  
  When a document is modified or deleted the Revisions configuration that applies 
  to its collection is examined.  
  If the Revisions feature is enabled for this collection:  
   * A revision of the document is created.  
   * Revisions are purged by limits set in the configuration.  

* **When [Enforce Configuration]() is applied**.  
  Applying this operation triggers the immediate examination and execution 
  of the default configuration and all collection-specific configurations.  
  {WARNING: }
  Sizeable databases and collections may contain numerous revisions pending 
  purging, that Enforcing Configuration will purge all at once. Be aware that 
  this operation may require substantial server resources, and time it accordingly.  
  {WARNING/}

{PANEL/}

{PANEL: How it Works}

With the revisions feature enabled (learn [here](../../document-extensions/revisions/client-api/operations/configure-revisions#syntax 
how to enable it), let's execute this code:

{CODE store@DocumentExtensions\Revisions\Revisions.cs /}

This will create the document, and also add its first revision.  
If we inspect the document we will see this revision:

![Figure 1: Revisions](images\revisions1.png "Figure 1: Revisions")


This is a revision of the document (you can navigate to the document by clicking on `See the current document`) which is stored on the revisions storage.
Now, let's modify the original document. This would create another revision:

![Figure 2: Revisions, Modified](images\revisions2.png "Figure 2: Revisions, Modified")

As you can see, we have a full audit record of all the changes that were made to the document.

You can access the revisions of a specific document by the document's ID ("users/1").
Or you can access a specific revision by its change vector or by a specific date.
Accessing a revision by a change vector would return a specific revision, 
while accessing a revision by a date would return the revision on this specific date if exists,
and if not it would return the revision right before this date.

{CODE get_revisions@DocumentExtensions\Revisions\Revisions.cs /}

Now, let's delete the document. 
The document would be removed but a revision will be created, so you aren't going to lose the audit trail if the document is deleted.

In order to see orphaned revisions (revisions of deleted documents), you can go to the `Documents > Revisions Bin` section in the studio, 
which would list all revisions without existing document:

![Figure 3: Revisions, Deleted](images\revisions3.png "Figure 3: Revisions, Deleted")

If you'll go and create another document with this ID (users/1), then the revision won't be shown anymore in the Revision Bin section, 
but you can navigate to the document and see it's revisions, including the deleted ones.

Clicking on the revision we can also navigate to the other revisions of this document:

![Figure 4: Revisions, Deleted - other revisions](images\revisions4.png "Figure 4: Revisions, Deleted - other revisions")

The revisions feature attempts to make things as simple as possible. Once it is enabled, you'll automatically get the appropriate audit trail.

{PANEL/}

{PANEL: Enabling or Disabling on an Existing Database}

The revisions feature can be enabled on a existing database with (or without) data with some restrictions. 
You need to bear in mind that new revision will be created for any new save or delete operation, but this will not affect any existing data that was created prior turning that feature on.
If you create a document, then turn on revisions, and then overwrite the document, there won't be a revision for the original document. However, you would have a revision of the put operation after the revisions feature was enabled.

It's possible also to disable the revisions feature on an existing database.
In this case all existing revisions would still be stored and not deleted but we won't create any new revisions on any put or delete operations.

{PANEL/}

{PANEL: Storage Concerns}

Enabling the revisions will affect the usage of storage space. Each revision of a document is stored in full. The revisions of documents use the same blittable JSON format as regular 
documents so the compression of individual fields is enabled (any text field that is greater than 128 bytes will be compressed).

{PANEL/}

{PANEL: Force Revision Creation}

So far we've discussed the automatic creation of revisions when the feature is enabled. 
But you can also **force the creation** of a document revision, whether the feature is 
enabled or not.  
This is useful when, for example, you choose to disable Revisions but 
still want to create a revision for a specific document, e.g. to take a snapshot of the 
document as a precaution before editing it.  

* You can force the creation of a revision via Studio or using the `ForceRevisionCreationFor` API method.  
* A revision will be created Even If the Revisions feature is disabled for the document's collection.  
* A revision will be created Even If The document was not modified.  

---

#### Force Revision Creation via Studio

To create a revision manually via Studio, use the **Create Revision** button in the 
document view's Revisions tab.  
![Figure 5: Create a revision manually](images\revisions5.png "Figure 5: Create a revision manually")

---

#### Force Revision Creation via API

To create a revision manually via the API, use the session `ForceRevisionCreationFor` method.  

`ForceRevisionCreationFor` overloads:  
{CODE-BLOCK: csharp}
// Force revision creation by entity.
// Can be used with tracked entities only.
void ForceRevisionCreationFor<T>(T entity, 
              ForceRevisionStrategy strategy = ForceRevisionStrategy.Before);

// Force revision creation by document ID.
void ForceRevisionCreationFor(string id, 
              ForceRevisionStrategy strategy = ForceRevisionStrategy.Before);
{CODE-BLOCK/}

* **Parameters**:

    | Parameter | Type | Description |
    | - | - | - |
    | **entity** | `T` | The tracked entity you want to create a revision for |
    | **id** | string | ID of the document you want to create a revision for |
    | **strategy** | `ForceRevisionStrategy` | Defines the revision creation strategy (see below). <br> Default: `ForceRevisionStrategy.Before` |

    `ForceRevisionStrategy`:
    {CODE-BLOCK: csharp}
public enum ForceRevisionStrategy
{
    // Do not force a revision
    None,
        
    // Create a forced revision from the document that is currently in store, 
    // BEFORE applying any changes made by the user.  
    // The only exception is a new document, for which a revision will be 
    // created AFTER the update.
    Before
}
    {CODE-BLOCK/}

* **Sample**:
    {CODE-TABS}
    {CODE-TAB:csharp:By_ID ForceRevisionCreationByID@DocumentExtensions\Revisions\Revisions.cs /}
    {CODE-TAB:csharp:By_Entity ForceRevisionCreationByEntity@DocumentExtensions\Revisions\Revisions.cs /}
    {CODE-TABS/}

{PANEL/}

## Related Articles

### Client API

- [Session: What are Revisions](../../document-extensions/revisions/client-api/session/what-are-revisions)  
- [Session: Loading Revisions](../../document-extensions/revisions/client-api/session/loading)  
- [Operations: How to Configure Revisions](../../document-extensions/revisions/client-api/operations/configure-revisions)  
- [Revisions in Data Subscriptions](../../client-api/data-subscriptions/advanced-topics/subscription-with-revisioning)  
