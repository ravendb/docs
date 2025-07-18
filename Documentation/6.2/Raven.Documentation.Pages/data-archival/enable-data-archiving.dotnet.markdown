# Enable Data Archiving
---

{NOTE: }

* By default, data archiving is disabled.  
  To use the archiving feature, you must first **enable** it.
 
* When configuring the feature,  
  you can also set the **frequency** at which RavenDB scans the database for documents scheduled for archiving.

* Once enabled, the archiving task runs periodically at the configured frequency,  
  scanning the database for documents that have been scheduled for archival.  
  Learn how to schedule documents for archival in [Schedule document archiving](../data-archival/schedule-document-archiving).

* In this article:
  * [Enable archiving - from the Client API](../data-archival/enable-data-archiving#enable-archiving---from-the-client-api)
  * [Enable archiving - from the Studio](../data-archival/enable-data-archiving#enable-archiving---from-the-studio)  

{NOTE/}

---

{PANEL: Enable archiving - from the Client API}
 
Use `ConfigureDataArchivalOperation` to enable archiving for the database and configure the archiving task.

---

**Example**:

{CODE-TABS}
{CODE-TAB:csharp:Enable_archiving enable_archiving@DataArchival\EnableArchiving.cs /}
{CODE-TAB:csharp:Enable_archiving_async enable_archiving_async@DataArchival\EnableArchiving.cs /}
{CODE-TABS/}

---

**Syntax**:

{CODE syntax_1@DataArchival\EnableArchiving.cs /}

{CODE syntax_2@DataArchival\EnableArchiving.cs /}

| Parameter                 | Type    | Description                                                                                                                                                            |
|---------------------------|---------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **Disabled**              | `bool`  | `true` - archiving is disabled for the entire database (default).<br>`false` - archiving is enabled for the database.                                                  |
| **ArchiveFrequencyInSec** | `long?` | Frequency (in seconds) at which the server scans for documents scheduled for archiving. Default: `60`                                                                  |
| **MaxItemsToProcess**     | `long?` | The maximum number of documents the archiving task will process in a single run (i.e., each time it is triggered by the configured frequency). Default: `int.MaxValue` |

{PANEL/}

{PANEL: Enable archiving - from the Studio}

![Enable archiving](images/enable-archiving.png "Enable archiving")

1. Go to **Settings > Data Archival**.
2. Toggle on to enable data archival.
3. Toggle on to customize the frequency at which the server scans for documents scheduled for archiving.  
   Default is 60 seconds.
4. Toggle on to customize the maximum number of documents the archiving task will process in a single run.
5. Click Save to apply your settings.

{PANEL/}

## Related Articles

### Document Archival
- [Overview](../data-archival/overview)
- [Schedule document archiving](../data-archival/schedule-document-archiving)
- [Archived documents and other features](../data-archival/archived-documents-and-other-features)
- [Unarchiving documents](../data-archival/unarchiving-documents)

### Configuration
- [Overview](../server/configuration/configuration-options#settings.json)  
- [Database Settings](../studio/database/settings/database-settings#view-database-settings)  

### Extensions
- [Document Expiration](../server/extensions/expiration)  

### Patching
- [Patch By Query](../client-api/rest-api/queries/patch-by-query)  
