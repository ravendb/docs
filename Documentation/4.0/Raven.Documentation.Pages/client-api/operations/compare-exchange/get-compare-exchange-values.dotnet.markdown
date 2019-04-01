# Compare Exchange: How to Get Compare Exchange Values

---

{NOTE: }

* Use `GetCompareExchangeValuesOperation` to return the saved compare-exchange _Values_ for the specified _Keys_.  

* For an overview of the 'Compare Exchange' feature click: [Compare Exchange Overview](../../../client-api/operations/compare-exchange/overview)

* In this page:  
  * [Syntax](../../../client-api/operations/compare-exchange/get-compare-exchange-values#syntax)  
  * [Example I - Get Value for Specified Keys](../../../client-api/operations/compare-exchange/get-compare-exchange-values#example-i---get-values-for-specified-keys)  
  * [Example II - Get Values for Keys with Common Prefix](../../../client-api/operations/compare-exchange/get-compare-exchange-values#example-ii---get-values-for-keys-with-common-prefix)  
{NOTE/}

---

{PANEL: Syntax}

**Methods**:
{CODE get_list_0@ClientApi\Operations\CompareExchange.cs /}

{CODE get_list_1@ClientApi\Operations\CompareExchange.cs /}

| Parameters | Type | Description |
| ------------- | ------------- | ----- |
| **keys** | string[] | List of keys to get |
| **startWith** | string | A common prefix for those keys whose values should be returned |
| **start** | int | The number of items that should be skipped |
| **pageSize** | int | The maximum number of values that will be retrieved |

**Returned object**:
{CODE compare_exchange_value@ClientApi\Operations\CompareExchange.cs /}

| Return Value | Description |
| ------------- | ----- |
| `Dictionary<string, CompareExchangeValue<T>>` | A Dictionary containing _'Key'_ to _'CompareExchangeValue'_ associations |
{PANEL/}

{PANEL: Example I - Get Values for Specified Keys}

{CODE get_list_2@ClientApi\Operations\CompareExchange.cs /}  
{PANEL/}

{PANEL: Example II - Get Values for Keys with Common Prefix}

{CODE get_list_3@ClientApi\Operations\CompareExchange.cs /}  
{PANEL/}

## Related Articles

### Compare Exchange

- [Overview](../../../client-api/operations/compare-exchange/overview)
- [Get a Compare-Exchange Value](../../../client-api/operations/compare-exchange/get-compare-exchange-value)
- [Put a Compare-Exchange Value](../../../client-api/operations/compare-exchange/delete-compare-exchange-value)
- [Delete a Compare-Exchange Value](../../../client-api/operations/compare-exchange/delete-compare-exchange-value)
