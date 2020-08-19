# Compare Exchange Expiration
---

{NOTE: }

* Compare exchange value expiration works very similar to [document expiration](../../../server/extensions/expiration).  

* Use the `@expires` field in a [compare exchange value's metadata](../../../client-api/operations/compare-exchange/compare-exchange-metadata) to schedule its expiration.  

* In this page:
  * [Syntax](../../../client-api/operations/compare-exchange/compare-exchange-expiration#syntax)
  * [Examples](../../../client-api/operations/compare-exchange/compare-exchange-expiration#examples)

{NOTE/}

---

{PANEL: Syntax}

`@expires` is a metadata property that holds a `DateTime` value. Once the 
specified date and time has passed, the compare exchange value is set for 
deletion by the expiration feature. The _exact_ time this happens depends 
on the expiration frequency and other 
[expiration configurations](../../../server/extensions/expiration#configuring-the-expiration-feature).  

To set a compare exchange value to expire, simply put a `DateTime` value 
(in UTC format) in the `@expires` field, then to send it to the server.  

{PANEL/}

{PANEL: Examples}

Creating a new key with `CreateCompareExchangeValue()`:

{CODE:csharp expiration_0@ClientApi/CompareExchange.cs /}

Updating an existing key with `PutCompareExchangeValueOperation<T>`:

{CODE:csharp expiration_1@ClientApi/CompareExchange.cs /}

{PANEL/}

## Related Articles

### Client API
- [Session: How to Get and Modify Entity Metadata](../../../client-api/session/how-to/get-and-modify-entity-metadata)
- [Compare Exchange: Overview](../../../client-api/operations/compare-exchange/overview)
- [Compare Exchange Metadata](../../../client-api/operations/compare-exchange/compare-exchange-metadata)

### Server
- [Document Expiration](../../../server/extensions/expiration)
