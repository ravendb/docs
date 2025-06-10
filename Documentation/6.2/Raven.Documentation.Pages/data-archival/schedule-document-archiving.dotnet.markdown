# Schedule Document Archiving
---

{NOTE: }

* Documents cannot be archived directly - they must be scheduled for archival.
  To **schedule a document** for archival, add the `@archive-at` property to the document's metadata, and set its value to the desired archival time (in UTC).
  This can be done in several ways, as described in this article.

* When the archival feature is enabled, RavenDB runs a background archiving task that periodically scans the database for documents scheduled for archival.  
  The scan frequency is configurable when [Enabling the archival feature](../data-archival/enable-data-archiving) (default: 60 seconds).

* The archiving task will archive any document whose `@archive-at` time has passed at the time of the scan.  
  The `@archive-at` metadata property will be replaced with `@archived: true`.

* You can schedule documents for archival even if the archiving feature is not yet enabled.
  These documents will be archived once the feature is enabled and the task runs - provided the scheduled time has already passed.

---

* In this article:  
  * [Schedule a SINGLE document for archiving - from the Client API](../data-archival/schedule-document-archiving#schedule-a-single-document-for-archiving---from-the-client-api)
  * [Schedule a SINGLE document for archiving - from the Studio](../data-archival/schedule-document-archiving#schedule-a-single-document-for-archiving---from-the-studio)
  * [Schedule MULTIPLE documents for archiving - from the Client API](../data-archival/schedule-document-archiving#schedule-multiple-documents-for-archiving---from-the-client-api)
  * [Schedule MULTIPLE documents for archiving - from the Studio](../data-archival/schedule-document-archiving#schedule-multiple-documents-for-archiving---from-the-studio)

{NOTE/}

---

{PANEL: Schedule a SINGLE document for archiving - from the Client API}

To schedule a single document for archival from the Client API,  
add the `@archive-at` property directly to the document metadata as follows:

{CODE-TABS}
{CODE-TAB:csharp:Schedule_document schedule_document@DataArchival\ScheduleArchiving.cs /}
{CODE-TAB:csharp:Schedule_document_async schedule_document_async@DataArchival\ScheduleArchiving.cs /}
{CODE-TABS/}

Learn more about modifying the metadata of a document in [Modifying Document Metadata](../client-api/session/how-to/get-and-modify-entity-metadata).

{PANEL/}

{PANEL: Schedule a SINGLE document for archiving - from the Studio}

* To schedule a single document for archival from the Studio:  
  * Open the Document view. 
  * Add the `@archive-at` property to the document's metadata.
  * Set its value to the desired archive time in UTC format.
  * Save the document.

![Schedule a document for archiving](images/schedule-document-for-archiving.png "Schedule a document for archiving")

1. This is the `@archive-at` property that was added to the document's metadata.  
   E.g.: `"@archive-at": "2025-06-25T14:00:00.0000000Z"`
2. After saving the document, the Studio displays the time remaining until the document is archived.

{PANEL/}

{PANEL: Schedule MULTIPLE documents for archiving - from the Client API}

* Use the `PatchByQueryOperation` to schedule multiple documents for archiving.  

* In the **patch query**, you can apply any filtering condition to select only the documents you want to archive.  
  In the **patch script**, call the `archived.archiveAt` method to set the desired archival time (in UTC).

* When the patch operation is executed,  
  the server will add the `@archive-at` property to the metadata of all documents that match the query filter.

---

**Example:**  

The following example schedules all orders that were made at least a year ago for archival.  
The **patch query** filters for these older orders.  
Any document matching the query is then scheduled for archival by the **patch script**.

{CODE-TABS}
{CODE-TAB:csharp:Schedule_documents schedule_documents@DataArchival\ScheduleArchiving.cs /}
{CODE-TAB:csharp:Schedule_documents_async schedule_documents_async@DataArchival\ScheduleArchiving.cs /}
{CODE-TAB:csharp:Operation_overload schedule_documents_overload@DataArchival\ScheduleArchiving.cs /}
{CODE-TAB:csharp:Operation_overload_async schedule_documents_overload_async@DataArchival\ScheduleArchiving.cs /}
{CODE-TABS/}

---

**Syntax:**

{CODE syntax@DataArchival\ScheduleArchiving.cs /}

| Parameter             | Type        | Description                                                                          |
|-----------------------|-------------|--------------------------------------------------------------------------------------|
| **doc**               | `object`    | The document to schedule for archiving.                                              |
| **utcDateTimeString** | `string`    | The UTC timestamp (as a string) that specifies when the document should be archived. |

To learn more about the `PatchByQueryOperation`, see [Set-based patch operations](../client-api/operations/patching/set-based).

{PANEL/}

{PANEL: Schedule MULTIPLE documents for archiving - from the Studio}

* To schedule multiple documents for archiving from the Studio:  
  * Open the Patch view. 
  * Enter a patch script that uses the `archived.archiveAt` method, providing the desired archive date in UTC.
  * Execute the patch.

---

**Example**:

The following patch script, used directly in the Studio,  
performs the same operation as the [Client API example](../data-archival/schedule-document-archiving#schedule-multiple-documents-for-archiving---from-the-client-api) shown above.

![Schedule multiple documents for archiving](images/schedule-multiple-documents-for-archiving.png "Schedule multiple documents for archiving")

1. Open the Patch view.
2. Enter the patch script.
3. Click to execute the patch.
4. You can test the patch on a sample document before executing the whole operation.  
   Learn more in [Test patch](../studio/database/documents/patch-view#test-patch).

{PANEL/}

## Related Articles

### Document Archival
- [Overview](../data-archival/overview)
- [Enable data archiving](../data-archival/enable-document-archiving)
- [Archived documents and other features](../data-archival/archived-documents-and-other-features)
- [Unarchiving documents](../data-archival/unarchiving-documents)

### Configuration
- [Overview](../server/configuration/configuration-options#settings.json)  
- [Database Settings](../studio/database/settings/database-settings#view-database-settings)  

### Extensions
- [Document Expiration](../server/extensions/expiration)  

### Patching
- [Patch By Query](../client-api/rest-api/queries/patch-by-query)  
