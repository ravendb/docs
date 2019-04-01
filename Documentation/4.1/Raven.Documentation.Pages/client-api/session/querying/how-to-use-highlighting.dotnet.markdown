# Session: Querying: How to use Highlighting

**Highlighting** can be a great feature for increasing search UX. To take leverage of it use `Highlight` method.

## Syntax

{CODE highlight_1@ClientApi\Session\Querying\HowToUseHighlighting.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **fieldName** | string | Name of a field to highlight. |
| **path** | `Expression<Func<T, object>>` | Path to a field to highlight. |
| **fragmentLength** | int | Maximum length of text fragments that will be returned. |
| **fragmentCount** | int | Maximum number of fragments that will be returned. |
| **options** | `HighlightingOptions`  | Options that can be used for customization. |
| **highlightings** | `Highlightings` | Instance of a Highlightings that contains the highlight fragments for each returned result. |

### Options

{CODE options@ClientApi\Session\Querying\HowToUseHighlighting.cs /}

| Options | | |
| ------------- | ------------- | ----- |
| **GroupKey** | string | Grouping key for the results. If `null` results are grouped by document ID (default). |
| **PreTags** | `string[]` | Array of pre tags used when highlighting. |
| **PostTags** | `string[]` | Array of post tags used when highlighting. |

## Example

{CODE-TABS}
{CODE-TAB:csharp:Sync highlight_2@ClientApi\Session\Querying\HowToUseHighlighting.cs /}
{CODE-TAB:csharp:Async highlight_3@ClientApi\Session\Querying\HowToUseHighlighting.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'ContentSearchIndex'
where search(Text, 'raven')
include highlight(Text, 128, 1)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Remarks

{NOTE:Note}
Default `<b></b>` tags are coloured and colours are returned in following order:

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
{NOTE/}

## Related articles

### Session

- [How to Query](../../../client-api/session/querying/how-to-query)

### Indexes

- [Highlighting](../../../indexes/querying/highlighting)
