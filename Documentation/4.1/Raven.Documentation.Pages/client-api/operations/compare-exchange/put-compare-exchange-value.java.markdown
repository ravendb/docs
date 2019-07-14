# Compare Exchange: How to Put Compare Exchange Value

---

{NOTE: }

* Use `PutCompareExchangeValueOperation` to save a compare-exchange _Value_ for the specified _Key_.  

* Create a new _Key_ or modify an existing one.  

* The _Value_ is saved only if the index passed is equal to the index currently stored in the server for the specified _Key_.  

* For an overview of the 'Compare Exchange' feature click: [Compare Exchange Overview](../../../client-api/operations/compare-exchange/overview)  

* In this page:  
  * [Syntax](../../../client-api/operations/compare-exchange/put-compare-exchange-value#syntax)  
  * [Example I - Create a New Key](../../../client-api/operations/compare-exchange/put-compare-exchange-value#example-i---create-a-new-key)  
  * [Example II - Update an Existing Key](../../../client-api/operations/compare-exchange/put-compare-exchange-value#example-ii---update-an-existing-key)  
{NOTE/}

---

{PANEL: Syntax}

**Method**:
{CODE:java put_0@ClientApi\Operations\CompareExchange.java /}

| Parameter | Type | Description |
| ----------| ---- |------------ |
| **key** | String | Object identifier under which _value_ is saved, unique in the database scope across the cluster. This string can be up to 512 bytes. |
| **value** | `T` | The value to be saved for the specified _key_. |
| **index** | long |  * `0` if creating a new key<br/>* The current version of _Value_ when updating a value for an existing key. |

**Returned object**:
{CODE:java compare_exchange_result@ClientApi\Operations\CompareExchange.java /}

| Return Value | Type | Description |
| ------------ | - | - |
| **Successful** | boolean | * _True_ if the save operation has completed successfully<br/>* _False_ if the save operation failed |
| **Value** | `T` | * The value that was saved if operation was successful<br/>* The currently existing value in the server upon failure |
| **Index** | long | * The version number of the value that was saved upon success<br/>* The currently existing version number in the server upon failure |

{NOTE: Note:}
When calling the 'Put' operation, the index from the request is compared to the index that is currently stored in the server (compare stage).  
The value is updated only if the two are **equal** (exchange stage).  
{NOTE/}
{PANEL/}

{PANEL: Example I - Create a New Key}

{CODE:java put_1@ClientApi\Operations\CompareExchange.java /}
{PANEL/}

{PANEL: Example II - Update an Existing Key}

{CODE:java put_2@ClientApi\Operations\CompareExchange.java /}
{PANEL/}

## Related Articles

### Compare Exchange

- [Overview](../../../client-api/operations/compare-exchange/overview)
- [Get a Compare-Exchange Value](../../../client-api/operations/compare-exchange/get-compare-exchange-value)
- [Get Compare-Exchange Values](../../../client-api/operations/compare-exchange/get-compare-exchange-values)
- [Delete a Compare-Exchange Value](../../../client-api/operations/compare-exchange/delete-compare-exchange-value)
