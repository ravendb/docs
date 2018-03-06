# Operations : Compare Exchange : How to Put Compare Exchange Value?

**PutCompareExchangeValueOperation** is used to save a compare-exchange value.

{NOTE You can learn about 'Compare Exchange' concepts in [dedicated article](../../../server/compare-exchange) /}

{WARNING When saving value first the index from request is compared to index on server (compare stage). When it is equal, value is updated (exchange stage).  /}

## Syntax

{CODE put_0@ClientApi\Operations\CompareExchange.cs /}

{CODE compare_exchange_result@ClientApi\Operations\CompareExchange.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **key** | string | Object identifier |
| **value** | `T` | Actual value |
| **index** | long |  Index representing version of value. Used for concurrency control - just like Etag for documents  |

| Return Value | | |
| ------------- | ----- | ---- |
| **Successful** | bool | True, if exchange was completed successfully |
| **Index** | long | Index representing version of value. Used for concurrency control - just like Etag for documents |
| **Value** | `T` | Current value |

## Example I 

{CODE put_1@ClientApi\Operations\CompareExchange.cs /}

## Example II

{CODE put_2@ClientApi\Operations\CompareExchange.cs /}

## Related Articles

- [How to **get compare-exchange** value?](../../../client-api/operations/compare-exchange/get-compare-exchange-value)
- [How to **get compare-exchange** values?](../../../client-api/operations/compare-exchange/get-compare-exchange-values)
- [How to **delete compare-exchange** value?](../../../client-api/operations/compare-exchange/delete-compare-exchange-value)
