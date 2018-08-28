# Session : Querying : How to use Highlighting

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

{CODE highlight_2@ClientApi\Session\Querying\HowToUseHighlighting.cs /}

## Related articles

### Session

- [How to Query](../../../client-api/session/querying/how-to-query)

### Indexes

- [Highlighting](../../../indexes/querying/highlighting)
