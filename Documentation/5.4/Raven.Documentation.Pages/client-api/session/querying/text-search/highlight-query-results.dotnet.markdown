# Highlight Search Results
---

{NOTE: }

* When making a [Full-Text Search query](../../../../client-api/session/querying/text-search/full-text-search),  
  in addition to retrieving documents that contain the searched terms in the results,  
  you can also request to get a **list of text fragments that highlight the searched terms**.

* The highlighted terms can enhance user experience when searching for documents with specific content.

* This article shows highlighting search results when making a **dynamic-query**.  
  For highlighting search results when querying a **static-index** see [highlight index search results](../../../../indexes/querying/highlighting).

---

* In this page:
  * [Highlight - basic example](../../../../client-api/session/querying/text-search/highlight-query-results#highlight---basic-example)
      * [Highlight tags](../../../../client-api/session/querying/text-search/highlight-query-results#highlight-tags)
      * [Highlight results in Studio](../../../../client-api/session/querying/text-search/highlight-query-results#highlight-results-in-studio)
  * [Highlight - customize tags](../../../../client-api/session/querying/text-search/highlight-query-results#highlight---customize-tags)
  * [Highlight - projected results](../../../../client-api/session/querying/text-search/highlight-query-results#highlight---projected-results)
  * [Syntax](../../../../client-api/session/querying/text-search/highlight-query-results#syntax)
  
{NOTE/}

---

{PANEL: Highlight - basic example}

{CODE-TABS}
{CODE-TAB:csharp:Query highlight_1@ClientApi\Session\Querying\TextSearch\HighlightQueryResults.cs /}
{CODE-TAB:csharp:Query_async highlight_2@ClientApi\Session\Querying\TextSearch\HighlightQueryResults.cs /}
{CODE-TAB:csharp:DocumentQuery highlight_3@ClientApi\Session\Querying\TextSearch\HighlightQueryResults.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Employees"
where search(Notes, "sales")
include highlight(Notes, 35, 4)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{CODE fragments_1@ClientApi\Session\Querying\TextSearch\HighlightQueryResults.cs /}

{NOTE: }

#### Highlight tags

---

* By default, the highlighted term is wrapped with the following html:  
  `<b style="background:yellow">term</b>`  

* When requesting to highlight multiple terms,  
  the background color returned for each different term will be in the following order:

  - <span style="border-left: 10px solid yellow">&nbsp;</span>yellow,
  - <span style="border-left: 10px solid lawngreen">&nbsp;</span>lawngreen,
  - <span style="border-left: 10px solid aquamarine">&nbsp;</span>aquamarine,
  - <span style="border-left: 10px solid magenta">&nbsp;</span>magenta,
  - <span style="border-left: 10px solid palegreen">&nbsp;</span>palegreen,
  - <span style="border-left: 10px solid coral">&nbsp;</span>coral,
  - <span style="border-left: 10px solid wheat">&nbsp;</span>wheat,
  - <span style="border-left: 10px solid khaki">&nbsp;</span>khaki,
  - <span style="border-left: 10px solid lime">&nbsp;</span>lime,
  - <span style="border-left: 10px solid deepskyblue">&nbsp;</span>deepskyblue,
  - <span style="border-left: 10px solid deeppink">&nbsp;</span>deeppink,
  - <span style="border-left: 10px solid salmon">&nbsp;</span>salmon,
  - <span style="border-left: 10px solid peachpuff">&nbsp;</span>peachpuff,
  - <span style="border-left: 10px solid violet">&nbsp;</span>violet,
  - <span style="border-left: 10px solid mediumpurple">&nbsp;</span>mediumpurple,
  - <span style="border-left: 10px solid palegoldenrod">&nbsp;</span>palegoldenrod,
  - <span style="border-left: 10px solid darkkhaki">&nbsp;</span>darkkhaki,
  - <span style="border-left: 10px solid springgreen">&nbsp;</span>springgreen,
  - <span style="border-left: 10px solid turquoise">&nbsp;</span>turquoise,
  - <span style="border-left: 10px solid powderblue">&nbsp;</span>powderblue

* The html tags that wrap the highlighted terms can be **customized** to any other tags.  
  See [customize tags](../../../../client-api/session/querying/text-search/highlight-query-results#highlight---customize-tags) below.

{NOTE/}

{NOTE: }

#### Highlight results in Studio

---

![Figure 1. Fragments results](images/fragmentsResults.png "View highlighted fragments in the Query View")

1. **Auto-Index**  
   This is the auto-index that was created by the server to serve the dynamic-query.  

2. **Results tab**  
   The results tab contains the resulting **documents** that match the provided RQL query.

3. **Highlight tab**  
   The highlight tab shows the resulting **fragments** that were included in the query result.

{NOTE/}

{PANEL/}

{PANEL: Highlight - customize tags}

* The html tags that wrap the highlighted terms can be **customized** to any other tags.

{CODE-TABS}
{CODE-TAB:csharp:Query highlight_4@ClientApi\Session\Querying\TextSearch\HighlightQueryResults.cs /}
{CODE-TAB:csharp:Query_async highlight_5@ClientApi\Session\Querying\TextSearch\HighlightQueryResults.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Employees"
where (search(Notes, "sales") or search(Title, "manager"))
include highlight(Notes, 35, 1, $p0), highlight(Title, 35, 1, $p1)
{
"p0":{"PreTags":["+++","<<<"],"PostTags":["+++",">>>"]},
"p1":{"PreTags":["+++","<<<"],"PostTags":["+++",">>>"]}
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{CODE fragments_2@ClientApi\Session\Querying\TextSearch\HighlightQueryResults.cs /}

{PANEL/}

{PANEL: Highlight - projected results}

* Highlighting can also be used when [projecting query results](../../../../client-api/session/querying/how-to-project-query-results).

{CODE-TABS}
{CODE-TAB:csharp:Query highlight_6@ClientApi\Session\Querying\TextSearch\HighlightQueryResults.cs /}
{CODE-TAB:csharp:Query_async highlight_7@ClientApi\Session\Querying\TextSearch\HighlightQueryResults.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Employees" as x
where search(x.Notes, "manager german")
select { Name : "{0} {1}".format(x.FirstName, x.LastName), Title : x.Title }
include highlight(Notes, 35, 2)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{CODE fragments_3@ClientApi\Session\Querying\TextSearch\HighlightQueryResults.cs /}

{PANEL/}

{PANEL: Syntax}

{CODE syntax_1@ClientApi\Session\Querying\TextSearch\HighlightQueryResults.cs /}

| Parameter          | Type                          | Description                                                                               |
|--------------------|-------------------------------|-------------------------------------------------------------------------------------------|
| **fieldName**      | string                        | Name of the field that contains the searched terms to highlight.                          |
| **path**           | `Expression<Func<T, object>>` | Path to the field that contains the searched terms to highlight.                          |
| **fragmentLength** | int                           | Maximum length of a text fragment. Must be `>= 18`.                                       |
| **fragmentCount**  | int                           | Maximum number of text fragments that will be returned.                                   |
| **options**        | `HighlightingOptions`         | Customizing options.                                                                      |
| **highlightings**  | `Highlightings`               | An 'out' param that will contain the highlighted text fragments for each returned result. |

<br>

**Highlighting options**:

{CODE syntax_2@ClientApi\Session\Querying\TextSearch\HighlightQueryResults.cs /}

| Option       | Type      | Description                                                                                                                                                                                                                                                                                                      |
|--------------|-----------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **GroupKey** | string    | Grouping key for the results.<br>Used when highlighting query results from a [Map-Reduce index](../../../../indexes/querying/highlighting#highlight-results---map-reduce-index).<br>If `null` results are grouped by document ID (default).<br>Note: Highlighting is Not available for dynamic aggregation queries. |
| **PreTags**  | string[]  | Array of PRE tags used to wrap the highlighted search terms in the text fragments.                                                                                                                                                                                                                               |
| **PostTags** | string[]  | Array of POST tags used to wrap the highlighted search terms in the text fragments.                                                                                                                                                                                                                              |

<br>

**Highlightings object**:

{CODE syntax_3@ClientApi\Session\Querying\TextSearch\HighlightQueryResults.cs /}

| Property          | Type                 | Description                                                      |
|-------------------|----------------------|------------------------------------------------------------------|
| **FieldName**     | string               | Name of the field that contains the searched terms to highlight. |
| **ResultIndents** | `IEumerable<string>` | The resulting keys (document IDs, or the map-reduce keys).        |

{CODE syntax_4@ClientApi\Session\Querying\TextSearch\HighlightQueryResults.cs /}

| Method           | Description                                                                                           |
|------------------|-------------------------------------------------------------------------------------------------------|
| **GetFragments** | Returns the list of the highlighted text fragments for the passed document ID, or the map-reduce key. |

{PANEL/}

## Related articles

### Session

- [Query overview](../../../../client-api/session/querying/how-to-query)

### Indexes

- [Highlight index search results](../../../../indexes/querying/highlighting)
