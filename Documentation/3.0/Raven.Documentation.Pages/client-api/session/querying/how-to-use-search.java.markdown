# Session: Querying: How to use search?

More complex text searching can be achieved by using `search` extension method. This method allows you to pass one or more search terms that will be used in searching process for a particular field (or fields).

## Syntax

{CODE:java search_1@ClientApi\Session\Querying\HowToUseSearch.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **fieldSelector** | Path | Expression marking a field in which terms should be looked for. |
| **searchTerms** | String | Space separated terms e.g. 'John Adam' means that we will look in selected field for 'John' or 'Adam'. |
| **boost** | double | Boost value. Default: `1`. |
| **options** | SearchOptions | Explicitly set relation between each Search functions. One of the following: `Or`, `And`, `Not`, `Guess`. Default: `SearchOptions.Guess`. |
| **escapeQueryOptions** | [EscapeQueryOptions]() | Terms escaping strategy. One of the following: `EscapeAll`, `AllowPostfixWildcard`, `AllowAllWildcards`, `RawQuery`. Default: `EscapeQueryOptions.EscapeAll`. |

| Return Value | |
| ------------- | ----- |
| IRavenQueryable | Instance implementing IRavenQueryable interface containing additional query methods and extensions. |

## Example I

{CODE:java search_2@ClientApi\Session\Querying\HowToUseSearch.java /}

## Example II

{CODE:java search_3@ClientApi\Session\Querying\HowToUseSearch.java /}

## Related articles

- [Indexes : Querying : Searching](../../../indexes/querying/searching)

