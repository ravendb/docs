# Delete Compare Exchange Operation

---

{NOTE: }

* Use `DeleteCompareExchangeValueOperation` to delete a compare-exchange item.  

* The compare-exchange item is only deleted if the _index_ in the request is __equal__ to the current _index_  
  stored on the server for the specified _key_.  

* Compare-exchange items can also be deleted via [advanced session methods](../../../client-api/session/cluster-transaction/compare-exchange), 
  or from the [Studio](../../../studio/database/documents/compare-exchange-view).

* In this page:  
  * [Delete compare exchange item](../../../client-api/operations/compare-exchange/delete-compare-exchange-value#delete-compare-exchange-item)
  * [Syntax](../../../client-api/operations/compare-exchange/delete-compare-exchange-value#syntax)  

{NOTE/}

---

{PANEL: Delete compare exchange item}

{CODE:nodejs delete_1@client-api\operations\compareExchange\deleteCompareExchange.js /}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax_1@client-api\operations\compareExchange\deleteCompareExchange.js /}

| Parameter   | Type   | Description                                                                  |
|-------------|--------|------------------------------------------------------------------------------|
| __key__     | string | The key of the item to be deleted                                            |
| __index__   | number | The version number of the item to be deleted                                 |
| __clazz__   | object | When the item's value is a class, you can specify its type in this parameter |

{CODE:nodejs syntax_2@client-api\operations\compareExchange\deleteCompareExchange.js /}

| Return Value   |         |                                                                                                                                                                                                 |
|----------------|---------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| __successful__ | boolean | * `true` if the delete operation was successfully completed<br/> * `true` if _key_ doesn't exist<br/> * `false` if the delete operation failed                                                  |  
| __value__      | object  | * The value that was deleted upon a successful delete<br/>* `null` if _key_ doesn't exist<br/>* The currently existing value on the server if delete operation failed                           |  
| __index__      | number  | * The next available version number upon success<br/>* The next available version number if _key_ doesn't exist<br/>* The currently existing index on the server if the delete operation failed |  

{PANEL/}

## Related Articles

### Compare Exchange

- [Overview](../../../client-api/operations/compare-exchange/overview)
- [Get a Compare-Exchange Value](../../../client-api/operations/compare-exchange/get-compare-exchange-value)
- [Get Compare-Exchange Values](../../../client-api/operations/compare-exchange/get-compare-exchange-values)
- [Put a Compare-Exchange Value](../../../client-api/operations/compare-exchange/delete-compare-exchange-value)

### Client API

- [Operations overview](../../../client-api/operations/what-are-operations)
