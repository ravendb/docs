# Operations : Compare Exchange : How to Get Compare Exchange Values?

**GetCompareExchangeValuesOperation** is used to return a saved compare-exchange values.

{NOTE You can learn about 'Compare Exchange' concepts in [dedicated article](../../../server/compare-exchange) /}

## Syntax

{CODE get_list_0@ClientApi\Operations\CompareExchange.cs /}

{CODE get_list_1@ClientApi\Operations\CompareExchange.cs /}

{CODE compare_exchange_value@ClientApi\Operations\CompareExchange.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **keys** | string[] | List of compare-exchange names to get |
| **startWith** | string | prefix for which compare-exchange values should be returned |
| **start** | int | number of compare-exchange values that should be skipped  |
| **pageSize** | int | maximum number of compare-exchange values that will be retrieved |


| Return Value | |
| ------------- | ----- |
| `Dictionary<string, CompareExchangeValue<T>>` | Dictionary contains key to value associations |

## Example I 

{CODE get_list_2@ClientApi\Operations\CompareExchange.cs /}

## Example II

{CODE get_list_3@ClientApi\Operations\CompareExchange.cs /}

## Related Articles

- [How to **get compare-exchange** value?](../../../client-api/operations/compare-exchange/get-compare-exchange-value)
- [How to **put compare-exchange** value?](../../../client-api/operations/compare-exchange/put-compare-exchange-value)
- [How to **delete compare-exchange** value?](../../../client-api/operations/compare-exchange/delete-compare-exchange-value)
