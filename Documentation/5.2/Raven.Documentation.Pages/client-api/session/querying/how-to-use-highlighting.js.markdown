# Highlight Query Results
---

{NOTE: }

* When making a [Full-Text Search query](../../../client-api/session/querying/how-to-use-search),  
  in addition to retrieving documents that contain the searched terms in the results,  
  you can also request to get a __list of text fragments that highlight the searched terms__.

* The highlighted terms can enhance user experience when searching for documents with specific content.

* This article shows highlighting search results when making a __dynamic-query__.  
  For highlighting search results when querying a __static-index__ see [highlight index query results](../../../indexes/querying/highlighting).

---

* In this page:
  * [Highlight - basic example](../../../client-api/session/querying/how-to-use-highlighting#highlight---basic-example)
      * [Highlight tags](../../../client-api/session/querying/how-to-use-highlighting#highlight-tags)
      * [Highlight results in Studio](../../../client-api/session/querying/how-to-use-highlighting#highlight-results-in-studio)
  * [Highlight - customize tags](../../../client-api/session/querying/how-to-use-highlighting#highlight---customize-tags)
  * [Highlight - projected results](../../../client-api/session/querying/how-to-use-highlighting#highlight---projected-results)
  * [Syntax](../../../client-api/session/querying/how-to-use-highlighting#syntax)
  
{NOTE/}

---

{PANEL: Highlight - basic example}

{CODE-TABS}
{CODE-TAB:nodejs:Query highlight_1@ClientApi\Session\Querying\highlightQueryResults.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "Employees"
where search(Notes, "sales")
include highlight(Notes, 35, 4)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{CODE:nodejs fragments_1@ClientApi\Session\Querying\highlightQueryResults.js /}

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

* The html tags that wrap the highlighted terms can be __customized__ to any other tags.  
  See [customize tags](../../../client-api/session/querying/how-to-use-highlighting#highlight---customize-tags) below.

{NOTE/}

{NOTE: }

#### Highlight results in Studio

---

![Figure 1. Fragments results](images/fragmentsResults.png "View highlighted fragments in the Query View")

1. __Results tab__  
   The results tab contains the resulting __documents__ that match the provided RQL query.

2. __Highlight tab__  
   The highlight tab shows the resulting __fragments__ that were included in the query result.

{NOTE/}

{PANEL/}

{PANEL: Highlight - customize tags}

* The html tags that wrap the highlighted terms can be __customized__ to any other tags.

{CODE-TABS}
{CODE-TAB:nodejs:Query highlight_2@ClientApi\Session\Querying\highlightQueryResults.js /}
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

{CODE:nodejs fragments_2@ClientApi\Session\Querying\highlightQueryResults.js /}

{PANEL/}

{PANEL: Highlight - projected results}

* Highlighting can also be used when [projecting query results](../../../client-api/session/querying/how-to-project-query-results).

{CODE-TABS}
{CODE-TAB:nodejs:Query highlight_3@ClientApi\Session\Querying\highlightQueryResults.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "Employees" as x
where search(x.Notes, "manager german")
select { Name : "{0} {1}".format(x.FirstName, x.LastName), Title : x.Title }
include highlight(Notes, 35, 2)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{CODE:nodejs fragments_3@ClientApi\Session\Querying\highlightQueryResults.js /}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax_1@ClientApi\Session\Querying\highlightQueryResults.js /}

| Parameter                  | Type                          | Description                                                                                                                                                  |
|----------------------------|-------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------|
| __parameters__             | `HighlightingParameters`      | Parameters for the highlight method.                                                                                                                         |
| __hightlightingsCallback__ | `(highlightResults) => void)` | A callback function with an output parameter.<br>The parameter passed to the callback will be filled with the `Highlightings` object when query returns. |

<br>

__The Highlighting parameters:__

| Parameter          | Type            | Description                                                                                                                                                                                                                                                                                                          |
|--------------------|-----------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| __fieldName__      | string          | Name of the field that contains the searched terms to highlight.                                                                                                                                                                                                                                                     |
| __fragmentLength__ | number          | Maximum length of a text fragment. Must be `>= 18`.                                                                                                                                                                                                                                                                  |
| __fragmentCount__  | number          | Maximum number of text fragments that will be returned.                                                                                                                                                                                                                                                              |
| __groupKey__       | string          | Grouping key for the results.<br>Used when highlighting query results from a [Map-Reduce index](../../../indexes/querying/highlighting#highlight-results---map-reduce-index).<br>If `null` results are grouped by document ID (default).<br>Note: Highlighting is Not available for dynamic aggregation queries. |
| __preTags__        | string[]        | Array of PRE tags used to wrap the highlighted search terms in the text fragments.                                                                                                                                                                                                                                   |
| __postTags__       | string[]        | Array of POST tags used to wrap the highlighted search terms in the text fragments.                                                                                                                                                                                                                                  |

<br>

__The Highlightings object__:

{CODE:nodejs syntax_2@ClientApi\Session\Querying\highlightQueryResults.js /}

{PANEL/}

## Related articles

### Session

- [How to Query](../../../client-api/session/querying/how-to-query)

### Indexes

- [Highlight index query results](../../../indexes/querying/highlighting)
