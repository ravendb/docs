# Highlight Index Search Results
---

{NOTE: }

* When making a [Full-Text Search query](../../indexes/querying/searching), 
  in addition to retrieving documents that contain the searched terms, you can 
  also request to get a list of text fragments that highlight the searched terms.  

* This article provides examples of highlighting search results when querying a static-index.  
  Prior to reading this article, please refer to [Highlight search results](../../client-api/session/querying/text-search/highlight-query-results) 
  for general knowledge about Highlighting and for dynamic-queries examples.

* To search and get fragments with highlighted terms when querying a static-index,  
  the index field on which you search must be configured for highlighting. See examples below.  

* In this page:
  * [Highlight results - Map index](../../indexes/querying/highlighting#highlight-results---map-index)
  * [Highlight results - Map-Reduce index](../../indexes/querying/highlighting#highlight-results---map-reduce-index)
  * [Customize highlight tags](../../indexes/querying/highlighting#customize-highlight-tags)

{NOTE/}

---

{PANEL: Highlight results - Map index}

{NOTE: }

**Configure a Map index for highlighting**:

---

To search and get fragments with highlighted terms,  
the index-field on which you search **must be configured as follows**:

   * Store the index-field in the index
   * Configure the index-field for Full-Text search  
   * Store the index-field term vector with position and offsets  

{CODE:nodejs index_1@Indexes\Querying\highlights.js /}

{NOTE/}

{NOTE: }

**Query the index with `search`**:

---

{CODE-TABS}
{CODE-TAB:nodejs:Query highlight_1@Indexes\Querying\highlights.js /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Employees/ByNotes"
where search(EmployeeNotes, "manager")
include highlight(EmployeeNotes, 35, 2)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

**Query the index with `whereEquals`**:

---

{CODE-TABS}
{CODE-TAB:nodejs:Query highlight_2@Indexes\Querying\highlights.js /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Employees/ByNotes"
where EmployeeNotes == "manager"
include highlight(EmployeeNotes, 35, 2)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

**Process results**:

---

{CODE:nodejs highlight_3@Indexes\Querying\highlights.js /}

{NOTE/}

{PANEL/}

{PANEL: Highlight results - Map-Reduce index}

{NOTE: }

**Configure a Map-Reduce index for highlighting**:

---

In order to search and get fragments with highlighted terms in a Map-Reduce index:  

  * The index-field on which you **search** must be configured with:

      * Store the index-field in the index
     
      * Configure the index-field for Full-Text search

      * Store the index-field term vector with position and offsets

  * The index-field by which you **group-by** must be stored in the index.

{CODE:nodejs index_2@Indexes\Querying\highlights.js /}

{NOTE/}

{NOTE: }

**Query the index**:

---

{CODE-TABS}
{CODE-TAB:nodejs:Query highlight_4@Indexes\Querying\highlights.js /}
{CODE-TAB-BLOCK:sql:RQL}
from index "ContactDetailsPerCountry"
where search(ContactDetails, "agent")
include highlight(ContactDetails, 35, 2, $p0)
{"p0":{"groupKey":"Country"}}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

**Process results**:

---

{CODE:nodejs highlight_5@Indexes\Querying\highlights.js /}

{NOTE/}

{PANEL/}

{PANEL: Customize highlight tags}

* **Default tags**:  

  * Please refer to [Highlight tags](../../client-api/session/querying/text-search/highlight-query-results#highlight-tags) to learn about the default html tags used to wrap the highlighted terms.

* **Customizing tags**:  

  * The default html tags that wrap the highlighted terms can be customized to any other tags.  
  
  * Customizing the wrapping tags when querying an index is done exactly the same as when making  
    a dynamic query where the `preTags` and `postTags` parameters are passed to the `highlight` method.
  
  * Follow the example in [Highlight - customize tags](../../client-api/session/querying/text-search/highlight-query-results#highlight---customize-tags).

{PANEL/}

## Related articles

### Client API

- [Highlight search results](../../client-api/session/querying/text-search/highlight-query-results)
