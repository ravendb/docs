# Compare Exchange: How to Delete Compare Exchange Value

---

{NOTE: }

* Use `DeleteCompareExchangeValueOperation` to delete a _Key_ and its _Value_.  

* The _Key_ and its _Value_ are deleted only if the _index_ in the request matches the current index stored in the server for the specified key.  

* For an overview of the 'Compare Exchange' feature click: [Compare Exchange Overview](../../../client-api/operations/compare-exchange/overview)

* In this page:  
  * [Syntax](../../../client-api/operations/compare-exchange/get-compare-exchange-values#syntax)  
  * [Example](../../../client-api/operations/compare-exchange/delete-compare-exchange-value#example)  
{NOTE/}

---

{PANEL: Syntax}

**Method**:
{CODE:java delete_0@ClientApi\Operations\CompareExchange.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **key** | String | The key to be deleted |
| **index** | long |  The version number of the value to be deleted |

**Returned object**:
{CODE:java compare_exchange_result@ClientApi\Operations\CompareExchange.java /}

| Return Value | | |
| ------------- | ----- | ---- |
| **Successful** | boolean | * _True_ if the delete operation was successfully completed<br/> * _True_ if _key_ doesn't exist<br/> * _False_ if the delete operation failed |  
| **Value** | `T` | * The value that was deleted upon a successful delete<br/>* 'null' if _key_ doesn't exist<br/>* The currently existing value on the server if delete operation failed |  
| **Index** | long | * The next available version number upon success<br/>* The next available version number if _key_ doesn't exist<br/>* The currently existing index on the server if the delete operation failed |  
{PANEL/}

{PANEL: Example}

{CODE:java delete_1@ClientApi\Operations\CompareExchange.java /}  
{PANEL/}

## Related Articles

### Compare Exchange

- [Overview](../../../client-api/operations/compare-exchange/overview)
- [Get a Compare-Exchange Value](../../../client-api/operations/compare-exchange/get-compare-exchange-value)
- [Get Compare-Exchange Values](../../../client-api/operations/compare-exchange/get-compare-exchange-values)
- [Put a Compare-Exchange Value](../../../client-api/operations/compare-exchange/delete-compare-exchange-value)
