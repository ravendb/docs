# Get Compare Exchange Values Operation
---

{NOTE: }

* Use `GetCompareExchangeValuesOperation` to get multiple compare-exchange items by specifying either:
    * List of keys
    * A common prefix for keys to retrieve

* Compare-exchange items can also be managed via [advanced session methods](../../../client-api/session/cluster-transaction/compare-exchange),  
  which also expose getting the compare-exchange lazily,  
  or from the [Studio](../../../studio/database/documents/compare-exchange-view).

* In this page:
  * [Examples](../../../client-api/operations/compare-exchange/get-compare-exchange-values#examples) 
      * [Get cmpXchg items for specified keys](../../../client-api/operations/compare-exchange/get-compare-exchange-values#get-cmpxchg-items-for-specified-keys)  
      * [Get cmpXchg items for keys with common prefix](../../../client-api/operations/compare-exchange/get-compare-exchange-values#get-cmpxchg-items-for-keys-with-common-prefix)
  * [Syntax](../../../client-api/operations/compare-exchange/get-compare-exchange-values#syntax)    

{NOTE/}

---

{PANEL: Examples}

{NOTE: }

<a id="get-cmpxchg-items-for-specified-keys" /> __Get cmpXchg items for specified keys__:

---

{CODE:nodejs get_values_1@client-api\operations\compareExchange\getCompareExchangeValues.js /}

{NOTE/}

{NOTE: }

<a id="get-cmpxchg-items-for-keys-with-common-prefix" /> __Get cmpXchg items for keys with common prefix__:

---

{CODE:nodejs get_values_2@client-api\operations\compareExchange\getCompareExchangeValues.js /}

{NOTE/}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax_1@client-api\operations\compareExchange\getCompareExchangeValues.js /}

{CODE:nodejs syntax_2@client-api\operations\compareExchange\getCompareExchangeValues.js /}

| Return value                               | Description                                          |
|--------------------------------------------|------------------------------------------------------|
| `Dictionary<string, CompareExchangeValue>` | A Dictionary with a compare-exchange value per _key_ |

{CODE:nodejs syntax_3@client-api\operations\compareExchange\getCompareExchangeValues.js /}

{PANEL/}

## Related Articles

### Compare Exchange

- [Overview](../../../client-api/operations/compare-exchange/overview)
- [Get a Compare-Exchange Value](../../../client-api/operations/compare-exchange/get-compare-exchange-value)
- [Put a Compare-Exchange Value](../../../client-api/operations/compare-exchange/delete-compare-exchange-value)
- [Delete a Compare-Exchange Value](../../../client-api/operations/compare-exchange/delete-compare-exchange-value)

### Session

- [Cluster Transaction - Overview](../../../client-api/session/cluster-transaction/overview)
