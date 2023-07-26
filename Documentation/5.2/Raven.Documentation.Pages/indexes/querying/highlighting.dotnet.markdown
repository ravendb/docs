# Highlight Index Search Results
---

{NOTE: }

* When making a [Full-Text Search query](../../indexes/querying/searching),  
  in addition to retrieving documents that contain the searched terms in the results,  
  you can also request to get a list of text fragments that highlight the searched terms.  

* This article provides examples of highlighting search results when querying a static-index.  
  __Prior to this article__, please refer to [Highlight search results](../../client-api/session/querying/text-search/highlight-query-results) for general knowledge about Highlighting,  
  and for dynamic-queries examples.

* In order to search and get fragments with highlighted terms when querying a static-index,  
  the index field on which you search must be configured for highlighting,  
  see examples below. 

* In this page:
  * [Highlight results - Map index](../../indexes/querying/highlighting#highlight-results---map-index)
  * [Highlight results - Map-Reduce index](../../indexes/querying/highlighting#highlight-results---map-reduce-index)
  * [Customize highlight tags](../../indexes/querying/highlighting#customize-highlight-tags)

{NOTE/}

---

{PANEL: Highlight results - Map index}

{NOTE: }

__Configure a Map index for highlighting__:

---

In order to search and get fragments with highlighted terms, the index-field on which you search  
must be configured with:  

  * __`FieldStorage.Yes`__ - store the field in the index  
  * __`FieldIndexing.Search`__ - allow Full-Text search  
  * __`FieldTermVector.WithPositionsAndOffsets`__ - store the term's position and offsets

{CODE index_1@Indexes\Querying\Highlights.cs /}

{NOTE/}

{NOTE: }

__Query the index with `Search`__:

---

{CODE-TABS}
{CODE-TAB:csharp:Query highlight_1@Indexes\Querying\Highlights.cs /}
{CODE-TAB:csharp:Query_async highlight_2@Indexes\Querying\Highlights.cs /}
{CODE-TAB:csharp:DocumentQuery highlight_3@Indexes\Querying\Highlights.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Employees/ByNotes"
where search(EmployeeNotes, "manager")
include highlight(EmployeeNotes, 35, 2)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

__Query the index with `Where`__:

---

{CODE-TABS}
{CODE-TAB:csharp:Query highlight_4@Indexes\Querying\Highlights.cs /}
{CODE-TAB:csharp:Query_async highlight_5@Indexes\Querying\Highlights.cs /}
{CODE-TAB:csharp:DocumentQuery highlight_6@Indexes\Querying\Highlights.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Employees/ByNotes"
where EmployeeNotes == "manager"
include highlight(EmployeeNotes, 35, 2)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

__Process results__:

---

{CODE highlight_7@Indexes\Querying\Highlights.cs /}

{NOTE/}

{PANEL/}

{PANEL: Highlight results - Map-Reduce index}

{NOTE: }

__Configure a Map-Reduce index for highlighting__:

---

In order to search and get fragments with highlighted terms in a Map-Reduce index:

  * The index-field on which you __search__ must be configured with:

    * __`FieldStorage.Yes`__ - store the field in the index
    * __`FieldIndexing.Search`__ - allow Full-Text search
    * __`FieldTermVector.WithPositionsAndOffsets`__ - store the term's position and offsets

  * The index-field by which you __group-by__ must configured with:

    * __`FieldStorage.Yes`__ - store the field in the index

{CODE index_2@Indexes\Querying\Highlights.cs /}

{NOTE/}

{NOTE: }

__Query the index__:

---

{CODE-TABS}
{CODE-TAB:csharp:Query highlight_8@Indexes\Querying\Highlights.cs /}
{CODE-TAB:csharp:Query_async highlight_9@Indexes\Querying\Highlights.cs /}
{CODE-TAB:csharp:DocumentQuery highlight_10@Indexes\Querying\Highlights.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index "ContactDetailsPerCountry"
where search(ContactDetails, "agent")
include highlight(ContactDetails, 35, 2, $p0)
{"p0":{"GroupKey":"Country"}}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

__Process results__:

---

{CODE highlight_11@Indexes\Querying\Highlights.cs /}

{NOTE/}

{PANEL/}

{PANEL: Customize highlight tags}

* __Default tags__:  

  * Please refer to [Highlight tags](../../client-api/session/querying/text-search/highlight-query-results#highlight-tags) to learn about the default html tags used to wrap the highlighted terms.

* __Customizing tags__:  

  * The default html tags that wrap the highlighted terms can be customized to any other tags.  
  
  * Customizing the wrapping tags when querying an index is done exactly the same as when making  
    a dynamic query where a `HighlightingOptions` object is passed to the `Highlight` method.
  
  * Follow the example in [Highlight - customize tags](../../client-api/session/querying/text-search/highlight-query-results#highlight---customize-tags).

{PANEL/}

## Related articles

### Client API

- [Highlight search results](../../client-api/session/querying/text-search/highlight-query-results)
