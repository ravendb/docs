# Session: Querying: How to Use Search

More complex text searching can be achieved by using the `search()` method. This method allows you to pass one or more search terms that will be used in the searching process for a particular field (or fields).

## Syntax

{CODE:nodejs search_1@client-api\session\querying\howToUseSearch.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **fieldName** | string | Points index field that should be used for querying. |
| **searchTerms** | string | Space separated terms e.g. 'John Adam' means that we will look in selected field for 'John' or 'Adam'. Wildcards can be specified. |
| **boost** | number | Boost value. Default: `1`. |
| **operator** | `SearchOperator` | Explicitly set relation between each Search function. One of the following: `"OR"`, `"AND"`. |

## Example I - Dynamic Query

{CODE-TABS}
{CODE-TAB:nodejs:Node.js search_4@client-api\session\querying\howToUseSearch.js /}
{CODE-TAB-BLOCK:sql:RQL}
from Users 
where search(name, 'a*')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Example II - Query Using Static Index

{CODE-TABS}
{CODE-TAB:nodejs:Node.js search_2@client-api\session\querying\howToUseSearch.js /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Users/ByNameAndHobbies' 
where search(name, 'Adam') or search(hobbies, 'sport')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Example III - Boosting Usage

{CODE-TABS}
{CODE-TAB:nodejs:Node.js search_3@client-api\session\querying\howToUseSearch.js /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Users/ByHobbies' 
where boost(search(hobbies, 'I love sport'), 10) or boost(search(hobbies, 'but also like reading books'), 5)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE To leverage the searching capabilities with the usage of static indexes, please remember to enable full-text search in field settings of the index definition. /}

## Related Articles

### Session

- [How to Query](../../../client-api/session/querying/how-to-query)
- [How to Use Regex](../../../client-api/session/querying/how-to-use-regex)
- [How to Query With Exact Match](../../../client-api/session/querying/how-to-query-with-exact-match)

### Querying

- [Searching](../../../indexes/querying/searching)
