# Session : Querying : How to Query With Exact Match

By default `Where` method in `Query` uses case-insensitive match.

To perform case-sensitive match you should use `exact` parameter. It can be used in following methods:

* `session.Query`
* `session.Advanced.DocumentQuery`
* `session.Advanced.RawQuery`

## Session.Query

Calling `Where` with `exact` parameter is possible with using following extension method:

### Syntax

{CODE query_1_0@ClientApi\Session\Querying\HowToQueryWithExactMatch.cs /}


| Parameters | | |
| ------------- | ------------- | ----- |
| **predicate** | Expression<Func<T, int, bool>> | Predicate with match condition |
| **exact** | bool | Indicates if `predicate` should be matched in case-sensitive manner |

| Return Value | |
| ------------- | ----- |
| IRavenQueryable | Instance implementing `IRavenQueryable` interface containing additional query methods and extensions |


###Example I - Basic Query With Exact Match

{CODE-TABS}
{CODE-TAB:csharp:Sync query_1_1@ClientApi\Session\Querying\HowToQueryWithExactMatch.cs /}
{CODE-TAB:csharp:Async query_1_1_async@ClientApi\Session\Querying\HowToQueryWithExactMatch.cs /}
{CODE-TABS/}

## Session.Advanced.DocumentQuery

Case sensitive match in `session.Advanced.DocumentQuery` can be achieved by passing extra parameter: `exact`.

### Example II

{CODE-TABS}
{CODE-TAB:csharp:Sync query_1_2@ClientApi\Session\Querying\HowToQueryWithExactMatch.cs /}
{CODE-TAB:csharp:Async query_1_2_async@ClientApi\Session\Querying\HowToQueryWithExactMatch.cs /}
{CODE-TABS/}

## Session.Advanced.RawQuery

Case sensitive match is also available in RQL syntax. Here is an example:

### Example III

{CODE-TABS}
{CODE-TAB-BLOCK:csharp:RQL}
from Employees where exact(FirstName == 'Robert')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Related Articles

- [What are indexes?](../../../indexes/what-are-indexes)
- [Indexes : Querying: Basics](../../../indexes/querying/basics)  
- [Session : Querying : What is a Document Query?](../document-query/what-is-document-query.dotnet)
