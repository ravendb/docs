# Session: Querying: How to use Lucene

Lucene flavored syntax can be used with the `whereLucene()` method, a part of the filtering methods available in `IDocumentQuery`.

## Syntax

{CODE:nodejs lucene_1@client-api\session\querying\documentQuery\howToUseLucene.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **fieldName** | string | Name of a field in an index (default field) |
| **whereClause** | string | Lucene-syntax based clause |
| **exact** | boolean | (optional, default false) Match exact |

## Example

{CODE-TABS}
{CODE-TAB:nodejs:Node.js lucene_2@client-api\session\querying\documentQuery\howToUseLucene.js /}
{CODE-TAB-BLOCK:sql:RQL}
from Companies 
where lucene(name, 'bistro')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Advanced Usage

The `fieldName` argument corresponds to Lucene's default field convention. It is mandatory to pass it to the `whereLucene()` but the `whereClause` can contain clause that omits the field entirely giving you the opportunity to pass a complex expression e.g. `.whereLucene("name", "name:bistro OR phone:981-443655")`. It is advised to use this approach against Static Index where all fields are known, because there is no guarantee that a proper Auto Index will be created or used.

## Related Articles

### Session

- [What is Document Query](../../../../client-api/session/querying/document-query/what-is-document-query)
