# Operations : Compare Exchange : How to Delete Compare Exchange Value?

**DeleteCompareExchangeValueOperation** is used to delete a compare-exchange value.

{NOTE You can learn about 'Compare Exchange' concepts in [dedicated article](../../../server/compare-exchange) /}

{WARNING Value is deleted only if index in request matches index on server. /}

## Syntax

{CODE delete_0@ClientApi\Operations\CompareExchange.cs /}

{CODE compare_exchange_result@ClientApi\Operations\CompareExchange.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **key** | string | Object identifier |
| **index** | long |  Index representing version of value. Used for concurrency control - just like Etag for documents  |

| Return Value | | |
| ------------- | ----- | ---- |
| **Successful** | bool | True, if exchange was completed successfully |
| **Index** | long | Index representing version of value. Used for concurrency control - just like Etag for documents |
| **Value** | `T` | Current value |

## Example

{CODE delete_1@ClientApi\Operations\CompareExchange.cs /}

## Related Articles

- [How to **get compare-exchange** value?](../../../client-api/operations/compare-exchange/get-compare-exchange-value)
- [How to **get compare-exchange** values?](../../../client-api/operations/compare-exchange/get-compare-exchange-values)
- [How to **put compare-exchange** value?](../../../client-api/operations/compare-exchange/put-compare-exchange-value)
