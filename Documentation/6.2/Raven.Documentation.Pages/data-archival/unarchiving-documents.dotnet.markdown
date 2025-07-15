# Unarchiving Documents
---


  {NOTE: }

* Archived documents can be unarchived at any time.

* The archiving feature does Not need to be enabled to unarchive documents.  
  Any previously archived document can be unarchived, regardless of the feature's current state.

* Do **not** attempt to unarchive a document by manually removing the `@archived: true` metadata property from the document.
  This will not clear the internal archived status of the document.  
  To properly unarchive a document, use the `archived.unarchive()` method as described below.

* In this article:
  * [Unarchive documents - from the Client API](../data-archival/unarchiving-documents#unarchive-documents---from-the-client-api)
  * [Unarchive documents - from the Studio](../data-archival/unarchiving-documents#unarchive-documents---from-the-studio)
  * [Unarchiving documents with index-based patch queries](../data-archival/unarchiving-documents#unarchiving-documents-with-index-based-patch-queries)

{NOTE/}

---

{PANEL: Unarchive documents - from the Client API}

* To unarchive documents from the Client API, use the `PatchByQueryOperation` operation,  
  which targets one or more documents based on the patch query.
 
* You can apply any filtering condition within the query to target only the documents you want to unarchive.

* Within the **patch script**, call the `archived.unarchive()` method to unarchive all documents   
  that match the **patch query**.

---

**Example:**

The following operation will unarchive ALL archived documents in the _Orders_ collection.

{CODE-TABS}
{CODE-TAB:csharp:Unarchive_documents unarchiving@DataArchival\Unarchiving.cs /}
{CODE-TAB:csharp:Unarchive_documents_async unarchiving_async@DataArchival\Unarchiving.cs /}
{CODE-TAB:csharp:Operation_overload unarchiving_overload@DataArchival\Unarchiving.cs /}
{CODE-TAB:csharp:Operation_overload_async unarchiving_overload_async@DataArchival\Unarchiving.cs /}
{CODE-TABS/}

---

**Syntax:**

{CODE syntax@DataArchival\Unarchiving.cs /}

| Parameter  | Type     | Description                |
|------------|----------|----------------------------|
| **doc**    | `object` | The document to unarchive. |

{PANEL/}

{PANEL: Unarchive documents - from the Studio}

* To unarchive documents from the Studio:
  * Open the Patch view.
  * Enter a patch script that uses the `archived.unarchive()` method.
  * Execute the patch.

---

**Example**:

The following patch script, used directly in the Studio,
performs the same operation as the [Client API example](../data-archival/unarchiving-documents#unarchive-documents---from-the-client-api) shown above.
It will unarchive all archived documents in the _Orders_ collection.

![Unarchive documents](images/unarchive-documents.png "Unarchive documents")

1. Open the Patch view.
2. Enter the patch script.
3. Click to execute the patch.
4. You can test the patch on a sample document before executing the whole operation.  
   Learn more in [Test patch](../studio/database/documents/patch-view#test-patch).

{PANEL/}

{PANEL: Unarchiving documents with index-based patch queries}

* When running a patch query to unarchive documents over an index,  
  you need to consider the index configuration regarding archived documents.

* If the index is configured to exclude archived documents, the query portion of the patch operation will not match any archived documents -
  because those documents are not included in the index.  
  As a result, no documents will be unarchived by the patch operation.

* For example, the following patch query uses a dynamic query that creates an auto-index.  
  If the [Indexing.Auto.ArchivedDataProcessingBehavior](../server/configuration/indexing-configuration#indexing.auto.archiveddataprocessingbehavior) configuration key is set to its default `ExcludeArchived` value,
  then even if archived documents exist in the _Orders_ collection with `ShipTo.Country == 'USA'`, they will not be matched -  because the auto-index does not include them -  
  and the patch operation will not unarchive any documents.

    {CODE unarchive_using_auto_index@DataArchival\Unarchiving.cs /}

---

Two possible workarounds are:

1. **Configure the index to include archived documents**:  
   This ensures archived documents are included in the index and can be matched by the patch query.  
   Use this option only if including archived documents in the index aligns with your indexing strategy.
   
      **For auto-indexes**:  
      Set the [Indexing.Auto.ArchivedDataProcessingBehavior](../server/configuration/indexing-configuration#indexing.auto.archiveddataprocessingbehavior) configuration key to `IncludeArchived`.  
      **For static-indexes**:  
      Set the [Indexing.Static.ArchivedDataProcessingBehavior](../server/configuration/indexing-configuration#indexing.static.archiveddataprocessingbehavior) configuration key to `IncludeArchived`,  
      or - configure the definition of the specific static-index to include archived documents.  
      See [Archived documents and indexing](../data-archival/archived-documents-and-other-features#archived-documents-and-indexing).

2. **Use a collection query instead of an index query**:  
   Run a simple collection-based query that does not rely on an index.  
   Apply your filtering logic inside the patch script to unarchive only the relevant documents.  
   
      For example:
      {CODE unarchive_using_filter_in_script@DataArchival\Unarchiving.cs /}

{PANEL/}

## Related Articles

### Document Archival
- [Overview](../data-archival/overview)
- [Enable data archiving](../data-archival/enable-data-archiving)
- [Schedule document archiving](../data-archival/schedule-document-archiving)
- [Archived documents and other features](../data-archival/archived-documents-and-other-features)

### Configuration
- [Overview](../server/configuration/configuration-options#settings.json)  
- [Database Settings](../studio/database/settings/database-settings#view-database-settings)  

### Extensions
- [Document Expiration](../server/extensions/expiration)  

### Patching
- [Patch By Query](../client-api/rest-api/queries/patch-by-query)  
