# Get Compare Exchange Value Operation
---

{NOTE: }

* Use the `GetCompareExchangeValueOperation` operation to get a compare-exchange item by its _key_.  

* Compare-exchange items can also be managed via [advanced session methods](../../../client-api/session/cluster-transaction/compare-exchange),  
  which also expose getting the compare-exchange lazily,  
  or from the [Studio](../../../studio/database/documents/compare-exchange-view).

* In this page:
  * [Examples](../../../client-api/operations/compare-exchange/get-compare-exchange-value#examples)
      * [Get cmpXchg item that has a number and metadata](../../../client-api/operations/compare-exchange/get-compare-exchange-value#get-cmpxchg-item-that-has-a-number-and-metadata)
      * [Get cmpXchg item that has an object](../../../client-api/operations/compare-exchange/get-compare-exchange-value#get-cmpxchg-item-that-has-an-object)
  * [Syntax](../../../client-api/operations/compare-exchange/get-compare-exchange-value#syntax)  

{NOTE/}

---

{PANEL: Examples} 

{NOTE: }

<a id="get-cmpxchg-item-that-has-a-number-and-metadata" /> __Get cmpXchg item that has a number and metadata__:

---

{CODE:nodejs get_1@client-api\operations\compareExchange\getCompareExchangeValue.js /}

{NOTE/}

{NOTE: }

<a id="get-cmpxchg-item-that-has-an-object" /> __Get cmpXchg item that has an object__:

---

{CODE-TABS}
{CODE-TAB:nodejs:Get_operation get_2@client-api\operations\compareExchange\getCompareExchangeValue.js /}
{CODE-TAB:nodejs:Employee_class employee_class@client-api\operations\compareExchange\getCompareExchangeValue.js /}
{CODE-TABS/}

{NOTE/}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax_1@client-api\operations\compareExchange\getCompareExchangeValue.js /}

| Parameter               | Type      | Description                                                                                                     |
|-------------------------|-----------|-----------------------------------------------------------------------------------------------------------------|
| __key__                 | `string`  | The unique identifier of the cmpXchg item.                                                                      |
| __clazz__               | `object`  | The class type of the item's value.                                                                             |
| __materializeMetadata__ | `boolean` | The Metadata will be retrieved and available regardless of the value of this param. Used for internal purposes. |

{CODE:nodejs syntax_2@client-api\operations\compareExchange\getCompareExchangeValue.js /}

| Parameter    | Type     | Description                                           |
|--------------|----------|-------------------------------------------------------|
| __key__      | `string` | The unique identifier of the cmpXchg item.            |
| __value__    | `object` | The existing `value` of the returned cmpXchg item.    |
| __metadata__ | `object` | The existing `metadata` of the returned cmpXchg item. |
| __index__    | `number` | The compare-exchange item's version.                  |

{PANEL/}

## Related Articles

### Compare Exchange

- [Overview](../../../client-api/operations/compare-exchange/overview)
- [Get Compare-Exchange Values](../../../client-api/operations/compare-exchange/get-compare-exchange-values)
- [Put a Compare-Exchange Value](../../../client-api/operations/compare-exchange/delete-compare-exchange-value)
- [Delete a Compare-Exchange Value](../../../client-api/operations/compare-exchange/delete-compare-exchange-value)

### Client API

- [Operations overview](../../../client-api/operations/what-are-operations)
- [Cluster Transaction - Overview](../../../client-api/session/cluster-transaction/overview)
