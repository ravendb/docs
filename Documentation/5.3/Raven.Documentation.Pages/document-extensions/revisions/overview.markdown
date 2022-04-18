# Document Revisions Overview
---

{NOTE: }

* **Document Revisions** are snapshots of documents and their document extensions.  
   * Revisions can be created for documents **automatically** when the documents 
     are created, modified, or deleted.  
   * Revisions can also be created for documents [manually](../../document-extensions/revisions/overview#force-revision-creation).  

    {INFO: }
    The trail of revisions created for a document can be inspected 
    to track the changes made in the document over time.  
    The document's live version can be reverted to any of its recorded revisions.  
  
    Tracking document revisions allows you, for example, to check how an employee's 
    contract has changed over time, restore a single corrupted document without requiring 
    a backup file, or conduct a full-scale audit of your data.  
    {INFO/}

* Revisions are automatically **created** and **purged** for a document 
  only if the Revisions feature is enabled for the document's collection.  
  To enable, disable, or limit the Revisions feature, 
  apply [Revisions Configurations](../../document-extensions/revisions/overview#revisions-configurations) 
  to all and/or specific collections.  
* Revisions and their configurations can be managed via API methods or using Studio.  

* In this page:  
  * [Revisions Configurations](../../document-extensions/revisions/overview#revisions-configurations)  
     * [Creating and Applying Configurations](../../document-extensions/revisions/overview#creating-and-applying-configurations)  
     * [When are Configurations Executed](../../document-extensions/revisions/overview#when-are-configurations-executed)  
     * [Enabling and Disabling Revisions for existing documents](../../document-extensions/revisions/overview#enabling-and-disabling-revisions-for-existing-documents)  
  * [How it Works](../../document-extensions/revisions/overview#how-it-works)  
  * [Revisions Storage](../../document-extensions/revisions/overview#revisions-storage)  
  * [Force Revision Creation](../../document-extensions/revisions/overview#force-revision-creation)  

{NOTE/}

---

{PANEL: Revisions Configurations}

By default, the Revisions feature is Disabled for all collections: no revisions 
are created or purged for any document.  

* You can change this behavior by defining a **default configuration** that 
  applies to all document collections.  

* You can also define **collection-specific** configurations that apply only 
  to the collections they are defined for.  
  {NOTE: }
  If you define both a default configuration and collection-specific configurations, 
  a collection-specific configuration will **override** the default configuration 
  for the collection it is defined for.  
  {NOTE/}

* A Revisions Configuration defines -  
   * Whether to Enable or Disable Revisions.  
     If the Revisions feature is Enabled for a collection, creating, modifying, 
     or deleting any document of this collection will trigger the automatic Creation 
     of a new document revision and optionally the Purging of existing revisions for 
     the document.  
     If the Revisions feature is Disabled for a collection, RavenDB will **not** 
     automatically create or purge revisions for documents of this collection.  
   * Whether to limit the number of revisions that can be kept per document.  
     RavenDB will only purge revisions if they exceed the limits you set here.  
   * See [below](../../document-extensions/revisions/overview#creating-and-applying-configurations) 
     where to learn more about configuration options and how to apply them.  

---

### Creating and Applying Configurations

Configurations can be created and applied using Studio or client API methods.  

#### Via Studio

* Use the Studio Settings [Document Revisions](../../studio/database/settings/document-revisions) 
  page to create and manage revision configurations.  
* Use the Documents View [Revisions tab](../../studio/database/document-extensions/revisions) 
  to inspect and manage the revisions created for each document.  

#### Via API methods

* Revisions Store Operations:  
  [Creating configurations](../../document-extensions/revisions/client-api/operations/configure-revisions) using
  [`ConfigureRevisionsOperation`](../../document-extensions/revisions/client-api/operations/configure-revisions#configurerevisionsoperation)  
  [Getting and Counting Revisions](../../document-extensions/revisions/client-api/operations/get-revisions) using 
  [`GetRevisionsOperation`](../../document-extensions/revisions/client-api/operations/get-revisions#getrevisionsoperation)  
* Revisions Session methods:  
  [Loading revisions and their metadata](../../document-extensions/revisions/client-api/session/loading) using 
  [`GetFor`](../../document-extensions/revisions/client-api/session/loading#getfor), 
  [`GetMetadataFor`](../../document-extensions/revisions/client-api/session/loading#getmetadatafor), and 
  [`Get`](../../document-extensions/revisions/client-api/session/loading#get)  
  [Counting Revisions](../../document-extensions/revisions/client-api/session/counting) using 
  [`GetCountFor`](../../document-extensions/revisions/client-api/session/counting#getcountfor)  
  [Including revisions](../../document-extensions/revisions/client-api/session/including) using 
  [`IncludeRevisions`](../../document-extensions/revisions/client-api/session/including#section) and 
  [`RawQuery`](../../document-extensions/revisions/client-api/session/including#including-revisions-with-session.advanced.rawquery)  

---

### When are Configurations Executed

Creating a Revisions configuration does **not** immediately trigger its execution.  
Configurations are executed:  

* **When documents are Created, Modified, or Deleted**  
  When a document is created, modified or deleted the Revisions configuration that applies 
  to the document's collection is examined.  
  If the Revisions feature is enabled for this collection:  
   * A revision of the document is created.  
   * Revisions are potentially purged by limits set in the configuration.  

* **When [Enforce Configuration]() is applied**  
  Enforcing Configuration triggers the immediate examination and execution 
  of the default configuration and all collection-specific configurations.  
  {WARNING: }
  Large databases and collections may contain numerous revisions pending 
  purging, that Enforcing Configuration will purge all at once. Be aware that 
  this operation may require substantial server resources, and time it accordingly.  
  {WARNING/}

---

### Enabling and Disabling Revisions for existing documents

* **Enabling Revisions for existing documents**  
  Creating a configuration that enables Revisions for populated collections 
  will cause the creation of revisions for documents of these collections 
  starting at the time the feature was enabled, but not retrospectively.  
   * If you enable Revisions and then **modify an existing document**:  
     A revision that records the modified document will be created.  
     No revision will be created for the document's original version.  
   * If you enable Revisions and then **create a document**:  
     As you create the document a revision will also be created, 
     recording the document at its initial state.  

* **Disabling Revisions for existing documents**  
  Disabling Revisions for existing documents will stop the creation 
  of new revisions and the purging of existing ones, **leaving all 
  existing document revisions intact**.  

{PANEL/}

{PANEL: How it Works}

Let's go through the process of Revisions creation for a taste of its advantages.  

2. **Enable Revisions** so we can experiment with the feature.  
   You can enable Revisions using [Studio](../../studio/database/settings/document-revisions) 
   or the [ConfigureRevisionsOperation](../../document-extensions/revisions/client-api/operations/configure-revisions#syntax) 
   Store operation.  
   ![Enable Revisions for the Users Collection](images\revisions_enable-revisions.png "Enable Revisions for the Users Collection")

2. **Create a new document in the `Users` collection**. We'll create revisions for this document.  
   You can create the document using [Studio](../../studio/database/documents/create-new-document#create-new-document) 
   or the [session.Store](../../client-api/session/storing-entities#example) method.  
   ![Create a Document](images\revisions_create-document.png "Create a Document")

3. Creating the document also created its first revision.  
   **Use Studio to inspect the new document's [Revisions tab](../../studio/database/document-extensions/revisions)**.  
   ![Revision when Creating a Document](images\revisions_document-created.png "Revision when Creating a Document")

4. Click the _See the current document_ button to return to the live document version.  
   **Modify and Save the document**.  
   This will create a second revision.  
   ![Revision when Modifying a Document](images\revisions_modify-document.png "Revision when Modifying a Document")
   
5. **Delete the document**.  
   Though you removed the document, its **audit trail** is **not lost**:  the revisions 
   remain, including a new one indicating that the document was deleted.  

    To see the "orphaned" revisions (whose parent document was deleted) open the Studio 
    `Documents > Revisions Bin` section.  
    Clicking the removed document's ID will show its revisions.  
    ![Revisions Bin](images\revisions_revisions-bin.png "Revisions Bin")

6. **Create a document with the same ID as the document you deleted**.  
   The revisions of the deleted document will be removed from the 
   revisions bin and added to the new document.  
   ![Revisions for a Re-created Document](images\revisions_recreated-document.png "Revisions for a Re-created Document")
{PANEL/}

{PANEL: Revisions Storage}

### Revisions Documents Storage  

The creation of a document revision stores a full version of the modified document, 
in the same **blittable JSON document** format as that of regular documents.  
The compression of individual fields is enabled as for regular documents: any text 
field of more than 128 bytes is compressed.  

### Revisions Document Extensions Storage

* **Time Series**  
  The revisions of a document that owns time series do **not** store the time series 
  data, but include in their metadata a **timeseries-snapshot** with some information 
  regarding the time series at the time of the revisions creation.  
  Read [here](../../document-extensions/timeseries/time-series-and-other-features#revisions) 
  more about time series and revisions.  

* **Counters**  
  The revisions of a document that owns counters contain the counters' **names and values** 
  at the time of the revisions creation.  
  Read [here](../../document-extensions/counters/counters-and-other-features#counters-and-revisions) 
  more about counters and revisions.  
  
* **Attachments**  
  The revisions of a document that owns **attachments** contain **references** 
  to the attachments in RavenDB's storage, without replicating the attachments.  
  An attachment will be removed from RavenDB's storage only when no live 
  documents or document revisions refer to it.  

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
![Create a revision manually](images\revisions_create-revision-manually.png "Create a revision manually")

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
