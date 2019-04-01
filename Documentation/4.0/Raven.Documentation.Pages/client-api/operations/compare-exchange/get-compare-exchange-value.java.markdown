# Compare Exchange: How to Get Compare Exchange Value

---

{NOTE: }

* Use `GetCompareExchangeValueOperation` to return the saved compare-exchange _Value_ for the specified _Key_.  

* For an overview of the 'Compare Exchange' feature click: [Compare Exchange Overview](../../../client-api/operations/compare-exchange/overview)

* In this page:  
  * [Syntax](../../../client-api/operations/compare-exchange/get-compare-exchange-value#syntax)  
  * [Example I - Value is 'long'](../../../client-api/operations/compare-exchange/get-compare-exchange-value#example-i---value-is-)  
  * [Example II - Value is a custom object](../../../client-api/operations/compare-exchange/get-compare-exchange-value#example-ii---value-is-a-custom-object)  
{NOTE/}

---

{PANEL: Syntax}

**Method**:
{CODE:java get_0@ClientApi\Operations\CompareExchange.java /}

**Returned object**:
{CODE:java compare_exchange_value@ClientApi\Operations\CompareExchange.java /}

| ------------- | ----- | ---- |
| **Key** | String | The unique object identifier |
| **Value** | `T` | The existing value that _Key_ has |
| **Index** | long |  The version number of the _Value_ that is stored for the specified _Key_ |

{PANEL/}

{PANEL: Example I - Value is 'long'} 
{CODE:java get_1@ClientApi\Operations\CompareExchange.java /}
{PANEL/}

{PANEL: Example II - Value is a custom object} 
{CODE:java get_2@ClientApi\Operations\CompareExchange.java /}
{PANEL/}

## Related Articles

### Compare Exchange

- [Overview](../../../client-api/operations/compare-exchange/overview)
- [Get Compare-Exchange Values](../../../client-api/operations/compare-exchange/get-compare-exchange-values)
- [Put a Compare-Exchange Value](../../../client-api/operations/compare-exchange/delete-compare-exchange-value)
- [Delete a Compare-Exchange Value](../../../client-api/operations/compare-exchange/delete-compare-exchange-value)
