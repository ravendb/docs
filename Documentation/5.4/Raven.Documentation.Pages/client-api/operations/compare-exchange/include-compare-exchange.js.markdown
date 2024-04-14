# Include Compare Exchange Values
---

{NOTE: }

* Compare-Exchange items can be included when [loading entities](../../../client-api/session/loading-entities) 
  and when making [queries](../../../client-api/session/querying/how-to-query).

* The Session [tracks](../../../client-api/session/what-is-a-session-and-how-does-it-work#tracking-changes) 
  the included Compare Exchange items which means their value can be accessed  
  in that Session without making an additional call to the server.

* In this page:
    * [Sample data](../../../client-api/operations/compare-exchange/include-compare-exchange#sample-data)
    * [Include CmpXchg items when loading](../../../client-api/operations/compare-exchange/include-compare-exchange#include-cmpxchg-items-when-loading)
    * [Include CmpXchg items when querying](../../../client-api/operations/compare-exchange/include-compare-exchange#include-cmpxchg-items-when-querying)
    * [Syntax](../../../client-api/operations/compare-exchange/include-compare-exchange#syntax)

{NOTE/}

---

{PANEL: Sample data}

* The examples in this article are based on the following __sample data__:

{CODE-TABS}
{CODE-TAB:nodejs:Sample_data sample_data@client-api/operations/compareExchange/includeCompareExchange.js /}
{CODE-TAB:nodejs:Company_class sample_class@client-api/operations/compareExchange/includeCompareExchange.js /}
{CODE-TABS/}

{PANEL/}

{PANEL: Include CmpXchg items when loading}

{NOTE: }

__Include single item__:

---

{CODE:nodejs include_1@client-api/operations/compareExchange/includeCompareExchange.js /}

{NOTE/}

{NOTE: }

__Include multiple items__:

---

{CODE:nodejs include_2@client-api/operations/compareExchange/includeCompareExchange.js /}

{NOTE/}

{PANEL/}

{PANEL: Include CmpXchg items when querying}

{NOTE: }

__dynamic query__:

---

{CODE-TABS}
{CODE-TAB:nodejs:Query include_3@client-api/operations/compareExchange/includeCompareExchange.js /}
{CODE-TAB:nodejs:RawQuery_using_RQL include_4@client-api/operations/compareExchange/includeCompareExchange.js /}
{CODE-TAB:nodejs:RawQuery_using_JS_function include_5@client-api/operations/compareExchange/includeCompareExchange.js /}
{CODE-TABS/}

{NOTE/}

{NOTE: }

__Index query__:

---

{CODE-TABS}
{CODE-TAB:nodejs:Query include_6@client-api/operations/compareExchange/includeCompareExchange.js /}
{CODE-TAB:nodejs:Index index@client-api/operations/compareExchange/includeCompareExchange.js /}
{CODE-TAB-BLOCK:sql:RQL}
// RQL that can be used with a Raw Query:
// ======================================

from index "Companies/ByName"
include cmpxchg("supplier")

// Using JS method:
// ================

declare function includeCmpXchg(company) {
    includes.cmpxchg(company.supplier);
    return company;
}

from index "Companies/ByName" as c
select includeCmpXchg(c)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

* Note:  
  Similar to the above dynamic query example, you can query the index with a [raw query](../../../indexes/querying/query-index#session.advanced.rawquery) using the provided RQL.

{NOTE/}

{PANEL/}

{PANEL: Syntax}

{NOTE: }

__When loading entities or making queries__:  

* Use method `includeCompareExchangeValue()` to include compare-exchange items along with `session.load()` or with queries.  

{CODE:nodejs syntax_1@client-api/operations/compareExchange/includeCompareExchange.js /}

| Parameter | Type      | Description                                                                                                |
|-----------|-----------|------------------------------------------------------------------------------------------------------------|
| __path__  | `string`  | The path of the document property that contains the key (or list of keys) of the CmpXchg items to include  |

{NOTE/}
{NOTE: }

__When querying with RQL__:  

* Use the [include](../../../client-api/session/querying/what-is-rql#include) keyword followed by `cmpxchg()` to include a compare-exchange item.  

{CODE-BLOCK:sql }
include cmpxchg(key)
{CODE-BLOCK/}

{NOTE/}
{NOTE: }

__When using javascript functions within RQL__:

* Use `includes.cmpxchg()` In [javascript functions](../../../client-api/session/querying/what-is-rql#declare) within RQL queries. 

{CODE-BLOCK:javascript }
includes.cmpxchg(key);
{CODE-BLOCK/}

| Parameter | Type      | Description                                                                      |
|-----------|-----------|----------------------------------------------------------------------------------|
| __key__   | `string`  | The key of the compare exchange value you want to include, or a path to the key. |

{NOTE/}
{PANEL/}

## Related Articles

### Client API

- [Session Change Tracking](../../../client-api/session/what-is-a-session-and-how-does-it-work#tracking-changes)
- [Handle Document Relationships](../../../client-api/how-to/handle-document-relationships)
- [Compare Exchange Overview](../../../client-api/operations/compare-exchange/overview)

### Indexes

- [Indexing Compare Exchange Values](../../../indexes/indexing-compare-exchange-values)
