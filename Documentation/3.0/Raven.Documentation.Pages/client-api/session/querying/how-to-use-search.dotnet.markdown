# Session: Querying: How to use search?

More complex text searching can be achieved by using `Search` extension method. This method allows you to pass one or more search terms that will be used in searching process for a particular field (or fields).

## Syntax

{CODE search_1@ClientApi\Session\Querying\HowToUseSearch.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **fieldSelector** | Expression<Func&lt;TResult&gt;> | Expression marking a field in which terms should be looked for. |
| **searchTerms** | string | Space separated terms e.g. 'John Adam' means that we will look in selected field for 'John' or 'Adam'. |
| **boost** | decimal | Boost value. Default: `1`. |
| **options** | SearchOptions | Explicitly set relation between each Search functions. One of the following: `Or`, `And`, `Not`, `Guess`. Default: `SearchOptions.Guess`. |
| **escapeQueryOptions** | [EscapeQueryOptions]() | Terms escaping strategy. One of the following: `EscapeAll`, `AllowPostfixWildcard`, `AllowAllWildcards`, `RawQuery`. Default: `EscapeQueryOptions.EscapeAll`. |

| Return Value | |
| ------------- | ----- |
| IRavenQueryable | Instance implementing IRavenQueryable interface containing additional query methods and extensions. |

## Example I

{CODE search_2@ClientApi\Session\Querying\HowToUseSearch.cs /}

## Example II

{CODE search_3@ClientApi\Session\Querying\HowToUseSearch.cs /}

## Related articles

- [Indexes : Querying : Searching](../../../indexes/querying/searching)

