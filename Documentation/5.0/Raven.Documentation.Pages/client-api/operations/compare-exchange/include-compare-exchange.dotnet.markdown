# Include Compare Exchange Values
---

{NOTE: }

* Compare Exchange Values can be included in `Session.Load` calls and in queries.  

* The Session [tracks](../../../client-api/session/what-is-a-session-and-how-does-it-work#tracking-changes) 
Compare Exchange Values, which means that after it has been included, the value can 
be accessed and modified in that Session without requiring an additional call to 
the server.  

* The Compare Exchange include syntax is based on the [include builder](../../../client-api/how-to/handle-document-relationships#includes).  

* In this page:  
  * [Syntax](../../../client-api/operations/compare-exchange/include-compare-exchange#syntax)  
  * [Examples](../../../client-api/operations/compare-exchange/include-compare-exchange#examples)  

{NOTE/}

---

{PANEL: Syntax}

#### Include builder

Chain the method `IncludeCompareExchangeValue()` to include compare exchange values 
along with `Session.Load()` or LINQ queries.  

{CODE:csharp include_builder@ClientApi/CompareExchange.cs /}

| Parameter | Type | Description |
| - | - | - |
| **path** | `string`;<br/>`Expression<Func<T, string>>`;<br/>`Expression<Func<T, IEnumerable<string>>>` | The key of the compare exchange value you want to include, a path to the key, or a path to a `string[]` of keys. |

#### RQL

Use the [`includes.cmpxchg()` method](../../../indexes/querying/what-is-rql#include) in a 
declared function to include compare exchange values.

{CODE-BLOCK:javascript }
declare function foo(key) {
    includes.cmpxchg(key);
}
{CODE-BLOCK/}

| Parameter | Type | Description |
| - | - | - |
| **key** | `string`;<br/>path to `string` | The key of the compare exchange value you want to include, or a path to the key. |

{PANEL/}

{PANEL: Examples}

{CODE-TABS}
{CODE-TAB:csharp:Load include_load@ClientApi/CompareExchange.cs /}
{CODE-TAB:csharp:LoadAsync include_load_async@ClientApi/CompareExchange.cs /}
{CODE-TAB:csharp:Query include_linq_query@ClientApi/CompareExchange.cs /}
{CODE-TAB:csharp:RawQuery(RQL) include_raw_query@ClientApi/CompareExchange.cs /}
{CODE-TABS/}

{PANEL/}

## Related Articles

### Client API

- [Session Change Tracking](../../../client-api/session/what-is-a-session-and-how-does-it-work#tracking-changes)
- [How to: Handle Document Relationships](../../../client-api/how-to/handle-document-relationships)
- [Compare Exchange Overview](../../../client-api/operations/compare-exchange/overview)

### Indexes

- [Indexing Compare Exchange Values](../../../indexes/indexing-compare-exchange-values)
