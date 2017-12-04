# Session : Querying : How to use search?

More complex text searching can be achieved by using `Search` extension method. This method allows you to pass one or more search terms that will be used in searching process for a particular field (or fields).

## Syntax

{CODE search_1@ClientApi\Session\Querying\HowToUseSearch.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **fieldSelector** | Expression<Func&lt;TResult&gt;> | Expression marking a field in which terms should be looked for. |
| **searchTerms** | string | Space separated terms e.g. 'John Adam' means that we will look in selected field for 'John' or 'Adam'. Wildcards can be specified. |
| **boost** | decimal | Boost value. Default: `1`. |
| **options** | SearchOptions | Explicitly set relation between each Search functions. One of the following: `Or`, `And`, `Not`, `Guess`. Default: `SearchOptions.Guess`. |

| Return Value | |
| ------------- | ----- |
| IRavenQueryable | Instance implementing IRavenQueryable interface containing additional query methods and extensions. |

## Example I - Dynamic query

{CODE-TABS}
{CODE-TAB:csharp:Sync search_4@ClientApi\Session\Querying\HowToUseSearch.cs /}
{CODE-TAB:csharp:Async search_4_async@ClientApi\Session\Querying\HowToUseSearch.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from Users 
where search(Name, 'a*')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Example II - Query using static index

{CODE-TABS}
{CODE-TAB:csharp:Sync search_2@ClientApi\Session\Querying\HowToUseSearch.cs /}
{CODE-TAB:csharp:Async search_2_async@ClientApi\Session\Querying\HowToUseSearch.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from index 'Users/ByNameAndHobbies' 
where search(Name, 'Adam') OR search(Hobbies, 'sport')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Example III - Boosting usage

{CODE-TABS}
{CODE-TAB:csharp:Sync search_3@ClientApi\Session\Querying\HowToUseSearch.cs /}
{CODE-TAB:csharp:Async search_3_async@ClientApi\Session\Querying\HowToUseSearch.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from index 'Users/ByHobbies' 
where boost(search(Hobbies, 'I love sport'), 10) OR boost(search(Hobbies, 'but also like reading books'), 5)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE In order to search using static index you have to indicate that indexed fields supports full-text search in the index definition /}

## Related articles

- [Indexes : Querying : Searching](../../../indexes/querying/searching)

