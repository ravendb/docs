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

| Return Value   |         |                                                                                                                                                                                                                             |
|----------------|---------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| __successful__ | boolean | <ul><li>`true` if the delete operation was successfully completed.</li><li>`true` if _key_ doesn't exist.</li><li>`false` if the delete operation has failed,<br/>e.g. when the index version doesn't match.</li><ul>       |  
| __value__      | object  | <ul><li>The value that was deleted upon a successful delete.</li><li>`null` if _key_ doesn't exist.</li><li>The currently existing _value_ on the server if the delete operation has failed.</li><ul>                       |  
| __index__      | number  | <ul><li>The next available version number upon success.</li><li>The next available version number if _key_ doesn't exist.</li><li>The currently existing _index_ on the server if the delete operation has failed.</li><ul> |  

{PANEL/}

## Related Articles

### Compare Exchange

- [Overview](../../../client-api/operations/compare-exchange/overview)
- [Get a Compare-Exchange Value](../../../client-api/operations/compare-exchange/get-compare-exchange-value)
- [Get Compare-Exchange Values](../../../client-api/operations/compare-exchange/get-compare-exchange-values)
- [Put a Compare-Exchange Value](../../../client-api/operations/compare-exchange/delete-compare-exchange-value)

### Client API

- [Operations overview](../../../client-api/operations/what-are-operations)
