# Session: Querying: How to Use Search

More complex text searching can be achieved by using the `Search` extension method. This method allows you to pass one or more search terms that will be used in the searching process for a particular field (or fields).

## Syntax

{CODE search_1@ClientApi\Session\Querying\HowToUseSearch.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **fieldSelector** | Expression<Func&lt;TResult&gt;> | Points index field that should be used for querying. |
| **searchTerms** | string | Space separated terms e.g. 'John Adam' means that we will look in selected field for 'John' or 'Adam'. Wildcards can be specified. |
| **boost** | decimal | Boost value. Default: `1`. |
| **options** | SearchOptions | Explicitly set relation between each Search function. One of the following: `Or`, `And`, `Not`, `Guess`. Default: `SearchOptions.Guess`. |

## Example I - Dynamic Query

{CODE-TABS}
{CODE-TAB:csharp:Sync search_4@ClientApi\Session\Querying\HowToUseSearch.cs /}
{CODE-TAB:csharp:Async search_4_async@ClientApi\Session\Querying\HowToUseSearch.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from Users 
where search(Name, 'a*')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Example II - Query Using Static Index

{CODE-TABS}
{CODE-TAB:csharp:Sync search_2@ClientApi\Session\Querying\HowToUseSearch.cs /}
{CODE-TAB:csharp:Async search_2_async@ClientApi\Session\Querying\HowToUseSearch.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Users/ByNameAndHobbies' 
where search(Name, 'Adam') or search(Hobbies, 'sport')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Example III - Boosting Usage

{CODE-TABS}
{CODE-TAB:csharp:Sync search_3@ClientApi\Session\Querying\HowToUseSearch.cs /}
{CODE-TAB:csharp:Async search_3_async@ClientApi\Session\Querying\HowToUseSearch.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Users/ByHobbies' 
where boost(search(Hobbies, 'I love sport'), 10) or boost(search(Hobbies, 'but also like reading books'), 5)
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
