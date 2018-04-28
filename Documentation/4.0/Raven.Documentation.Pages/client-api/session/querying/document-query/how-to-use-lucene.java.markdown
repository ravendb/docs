# Session : Querying : How to use Lucene

Lucene flavored syntax can be used with the `whereLucene` method, a part of the filtering methods available in `IDocumentQuery`.

## Syntax

{CODE:java lucene_1@ClientApi\Session\Querying\DocumentQuery\HowToUseLucene.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **fieldName** | String | Name of a field in an index |
| **whereClause** | String | Lucene-syntax based clause for a given field |

## Example

{CODE-TABS}
{CODE-TAB:java:Java lucene_2@ClientApi\Session\Querying\DocumentQuery\HowToUseLucene.java /}
{CODE-TAB-BLOCK:sql:RQL}
from Companies 
where lucene(name, 'bistro')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Related Articles

### Session

- [What is Document Query](../../../../client-api/session/querying/document-query/what-is-document-query)
