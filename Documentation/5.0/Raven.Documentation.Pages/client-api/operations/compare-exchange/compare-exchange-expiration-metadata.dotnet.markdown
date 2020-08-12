# Compare Exchange Expiration and Metadata
---

{NOTE: }

* RavenDB 5.0 added metadata to compare exchange values.  

* Use the `@expires` field in the metadata to schedule an expiration for a 
compare exchange value. This works very similar to [document expiration](../../../server/extensions/expiration).  

* In this page:
  * [syntax](../../../client-api/operations/compare-exchange/compare-exchange-expiration-metadata#syntax)
  * [examples](../../../client-api/operations/compare-exchange/compare-exchange-expiration-metadata#examples)

{NOTE/}

---

{PANEL: Syntax}

`@expires` is a metadata property that holds a `DateTime` value. Once the 
specified date and time has passed, the compare exchange value is set for 
deletion by the expiration feature. The _exact_ time this happens depends 
on the expiration frequency and other 
[expiration configurations](../../../server/extensions/expiration#configuring-the-expiration-feature).  

Editing compare exchange metadata works much the same as editing a 
[document's metadata](../../../client-api/session/how-to/get-and-modify-entity-metadata) 
- it is a root property of the compare exchange value object.  

To set a compare exchange value to expire, simple put a `DateTime` value 
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

### Server
- [Document Expiration](../../../server/extensions/expiration)
