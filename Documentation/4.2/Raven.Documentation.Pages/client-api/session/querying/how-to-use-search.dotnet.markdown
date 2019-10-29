# Session: Querying: How to Use Search

Use the `Search()` extension method to perform a full-text search on a particular field. `Search()` accepts a string containing 
the desired search terms separated by spaces. These search terms are matched with the terms in the index being queried.  

As of v4.2 you can also pass an array of the search terms.  

An index's terms are derived from the values of the documents' fields. These values were each converted into one or more terms 
depending on which [Lucene analyzer](../../../indexes/using-analyzers) the index used.  

## Syntax

{CODE search_1@ClientApi\Session\Querying\HowToUseSearch.cs /}

| Parameters | Type | Description |
| ------------- | ------------- | ----- |
| **fieldSelector** | Expression<Func&lt;TResult&gt;> | Points index field that should be used for querying. |
| **searchTerms** | string <br/>(or `IEnumerable<string>`) | A string of the desired search terms separated by spaces (i.e. to search for "john" or "jack", pass the string "john&nbsp;jack"). <br/>Alternatively, you can pass an array (or other `IEnumerable`) of search terms. <br/>Wildcards can be used: `?` for any single character, `*` for any substring. |
| **boost** | decimal | Boost value. Default: `1`. |
| **options** | [SearchOptions](../../../indexes/querying/searching#search-options) | Set the operator between this `Search` method and the preceding extension method in the query. Can be set to one of the following: `Or`, `And`, `Not`, `Guess`. Default: `SearchOptions.Guess`. |
| **@operator** | [SearchOperator](../../../indexes/querying/searching#search-operator) | The operator between the individual terms. Can be set to `Or` or `And`. Default: `SearchOperation.Or`. |

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
where search(Name, 'Steve') or search(Hobbies, 'sport')
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

### Indexes

- [Analyzers](../../../indexes/using-analyzers)
- [Querying: Searching](../../../indexes/querying/searching)
