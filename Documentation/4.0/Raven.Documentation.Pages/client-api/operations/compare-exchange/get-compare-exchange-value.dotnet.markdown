# Operations : Compare Exchange : How to Get Compare Exchange Value?

**GetCompareExchangeValueOperation** is used to return a saved compare-exchange value by its key.

{NOTE You can learn about 'Compare Exchange' concepts in [dedicated article](../../../server/compare-exchange) /}

## Syntax

{CODE get_0@ClientApi\Operations\CompareExchange.cs /}

{CODE compare_exchange_value@ClientApi\Operations\CompareExchange.cs /}

| Return Value | | |
| ------------- | ----- | ---- |
| **Key** | string | Object identifier |
| **Index** | long | Index representing version of value. Used for concurrency control - just like Etag for documents |
| **Value** | `T` | Actual value |

## Example I 

{CODE get_1@ClientApi\Operations\CompareExchange.cs /}

## Example II

{CODE get_2@ClientApi\Operations\CompareExchange.cs /}

## Related Articles

- [How to **get compare-exchange** values?](../../../client-api/operations/compare-exchange/get-compare-exchange-values)
- [How to **put compare-exchange** value?](../../../client-api/operations/compare-exchange/put-compare-exchange-value)
- [How to **delete compare-exchange** value?](../../../client-api/operations/compare-exchange/delete-compare-exchange-value)
