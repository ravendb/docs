# Revisions Bin Cleaner
---

{NOTE: }

* The [Revisions Bin](../../studio/database/document-extensions/revisions/revisions-bin) stores revisions of deleted documents, ensuring they remain accessible.

* While you can manually delete these revisions directly from the Revisions Bin,  
  you can also set up a cleaner task to remove them automatically, as described below.

* For a complete list of deletion methods, see [All ways to delete revisions](../../studio/database/document-extensions/revisions/all-revisions#all-ways-to-delete-revisions).
  
* In this page:
    * [The revisions bin cleaner](../../document-extensions/revisions/revisions-bin-cleaner#the-revisions-bin-cleaner)
    * [Setting the revisions bin cleaner - from the Studio](../../document-extensions/revisions/revisions-bin-cleaner#setting-the-revisions-bin-cleaner---from-the-studio)
    * [Setting the revisions bin cleaner - from the Client API](../../document-extensions/revisions/revisions-bin-cleaner#setting-the-revisions-bin-cleaner---from-the-client-api)
    * [Syntax](../../document-extensions/revisions/revisions-bin-cleaner#syntax)

{NOTE/}

---

{PANEL: The revisions bin cleaner}
    
Each entry in the [Revisions Bin](../../studio/database/document-extensions/revisions/revisions-bin) represents a "Delete Revision",
which is a revision that marks a document as deleted and provides access to the revisions that were created for the document before it was deleted.

{WARNING: }
When the cleaner removes a "Delete Revision" entry,  
ALL the revisions associated with the deleted document are **permanently deleted**.
{WARNING/}

---

* The Revisions Bin Cleaner is configured with the following parameters:
    * **Frequency** - How often the cleaner runs.
    * **Entries age to keep** - The cleaner deletes revision entries older than this value.

* The cleaner task can be managed from:  
  * The [Revisions bin cleaner view](../../document-extensions/revisions/revisions-bin-cleaner#setting-the-revisions-bin-cleaner---from-the-studio) in the Studio
  * The [Client API](../../document-extensions/revisions/revisions-bin-cleaner#setting-the-revisions-bin-cleaner---from-the-client-api).

* When working with a secure server:
   * Configuring the Revisions Bin Cleaner is logged in the [audit log](../../server/security/audit-log/audit-log).
   * Deleting revisions is only available to a client certificate with a security clearance of [Database Admin](../../server/security/authorization/security-clearance-and-permissions#section) or higher.

{PANEL/}

{PANEL: Setting the revisions bin cleaner - from the Studio}

![Revisions bin cleaner view](images/revisions-bin-cleaner.png "Revisions Bin Cleaner View")

1. Go to **Settings > Revisions Bin Cleaner**   
2. Toggle ON to enable the cleaner task.  
3. Set the minimum entries age to keep:  
   * When toggled ON:  
       * Revisions Bin entries older than this value will be deleted.  
       * Default: `30` days.  
   * When toggled OFF:  
       * ALL Revisions Bin entries will be deleted.  
4. Set the custom cleaner frequency:  
   * Define how often (in seconds) the Revisions Bin Cleaner runs.
   * Default: `300` seconds (5 minutes).

{PANEL/}

{PANEL: Setting the revisions bin cleaner - from the Client API}

* Use `ConfigureRevisionsBinCleanerOperation` to configure the Revisions Bin Cleaner from the Client API.

* By default, the operation will be applied to the [default database](../../client-api/setting-up-default-database).  
  To operate on a different database see [switch operations to different database](../../client-api/operations/how-to/switch-operations-to-a-different-database).

* In this example, we enable the cleaner and configure its execution frequency and retention policy.

    {CODE-TABS}
    {CODE-TAB:csharp:Sync configure_cleaner@DocumentExtensions\Revisions\ClientAPI\Operations\RevisionsBinCleaner.cs /}
    {CODE-TAB:csharp:Async configure_cleaner_async@DocumentExtensions\Revisions\ClientAPI\Operations\RevisionsBinCleaner.cs /}
    {CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

{CODE:csharp syntax_1@DocumentExtensions\Revisions\ClientAPI\Operations\RevisionsBinCleaner.cs /}
{CODE:csharp syntax_2@DocumentExtensions\Revisions\ClientAPI\Operations\RevisionsBinCleaner.cs /}

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
* [Revisions Bin](../../studio/database/document-extensions/revisions/revisions-bin)
* [All Revisions](../../studio/database/document-extensions/revisions/all-revisions)
