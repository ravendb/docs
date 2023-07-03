# Count Query Results  

---

{NOTE: }

* Count query results using one of the methods described below.  

* In This Page:  
    * [CountAsync](../../../client-api/session/querying/how-to-count-query-results#countasync)  
    * [LongCount and LongCountAsync](../../../client-api/session/querying/how-to-count-query-results#longcount-and-longcountasync)  

{NOTE/}

---

{PANEL: CountAsync}

To count query results where an `Int32` variable is sufficient, use the `CountAsync` method.  
{CODE CountAsync@ClientApi\Session\Querying\HowToCountResults.cs /}

{PANEL/}

{PANEL: LongCount and LongCountAsync}

To count query results where an `Int64` variable is needed, use the 
`LongCount` or `LongCountAsync` method.  

{CODE-TABS}
{CODE-TAB:csharp:LongCount LongCount@ClientApi\Session\Querying\HowToCountResults.cs /}
{CODE-TAB:csharp:LongCount_async LongCount_async@ClientApi\Session\Querying\HowToCountResults.cs /}
{CODE-TABS/}

{PANEL/}

## Related Articles

### Client API

- [What is a Document Query](../../../client-api/session/querying/document-query/what-is-document-query)
- [How to Filter by Field Presence](../../../client-api/session/querying/how-to-filter-by-field)

### Querying

- [Querying: Filtering](../../../indexes/querying/filtering)
- [Query vs DocumentQuery](../../../client-api/session/querying/document-query/query-vs-document-query)
- [RQL - Raven Query Language](../../../client-api/session/querying/what-is-rql)

---

### Code Walkthrough

- [Queries - Filtering Results - Basics](https://demo.ravendb.net/demos/csharp/queries/filtering-results-basics)
- [Queries - Filtering with Multiple Conditions](https://demo.ravendb.net/demos/csharp/queries/filtering-results-multiple-conditions)
