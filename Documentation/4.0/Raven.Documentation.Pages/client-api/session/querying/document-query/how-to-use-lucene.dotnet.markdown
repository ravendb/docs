# Session : Querying : How to use Lucene

Lucene flavored syntax can be used with the `WhereLucene` method, a part of the filtering methods available in `IDocumentQuery`.

## Syntax

{CODE lucene_1@ClientApi\Session\Querying\DocumentQuery\HowToUseLucene.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **fieldName** | string | Name of a field in an index |
| **whereClause** | string | Lucene-syntax based clause for a given field |

## Example

{CODE-TABS}
{CODE-TAB:csharp:Sync lucene_2@ClientApi\Session\Querying\DocumentQuery\HowToUseLucene.cs /}
{CODE-TAB:csharp:Async lucene_3@ClientApi\Session\Querying\DocumentQuery\HowToUseLucene.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from Companies 
where lucene(Name, 'bistro')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Related Articles

### Session

- [What is Document Query](../../../../client-api/session/querying/document-query/what-is-document-query)
