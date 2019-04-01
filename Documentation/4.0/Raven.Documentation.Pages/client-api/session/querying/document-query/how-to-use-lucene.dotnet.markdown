# Session: Querying: How to use Lucene

Lucene flavored syntax can be used with the `WhereLucene` method, a part of the filtering methods available in `IDocumentQuery`.

## Syntax

{CODE lucene_1@ClientApi\Session\Querying\DocumentQuery\HowToUseLucene.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **fieldName** | string | Name of a field in an index (default field) |
| **whereClause** | string | Lucene-syntax based clause |

## Example

{CODE-TABS}
{CODE-TAB:csharp:Sync lucene_2@ClientApi\Session\Querying\DocumentQuery\HowToUseLucene.cs /}
{CODE-TAB:csharp:Async lucene_3@ClientApi\Session\Querying\DocumentQuery\HowToUseLucene.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from Companies 
where lucene(Name, 'bistro')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Advanced Usage

The `fieldName` argument corresponds to Lucene's default field convention. It is mandatory to pass it to the `.WhereLucene` but the `whereClause` can contain clause that omits the field entirely giving you the opportunity to pass a complex expression e.g. `.WhereLucene("Name", "Name:bistro OR Phone:981-443655")`. It is advised to use this approach against Static Index where all fields are known, because there is no guarantee that a proper Auto Index will be created or used.

## Related Articles

### Session

- [What is Document Query](../../../../client-api/session/querying/document-query/what-is-document-query)
