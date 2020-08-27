# Compare Exchange Metadata
---

{NOTE: }

* RavenDB 5.0 added metadata to compare exchange values.  

* In this page:  
  * [Syntax](../../../client-api/operations/compare-exchange/compare-exchange-metadata#syntax)

{NOTE/}

---

{PANEL: Syntax}

A compare exchange value's metadata is very similar to a 
[document's metadata](../../../client-api/session/how-to/get-and-modify-entity-metadata).  

The metadata can be used to set [compare exchange expiration](../../../client-api/operations/compare-exchange/compare-exchange-expiration).  

The metadata is accessible as a root property of the compare exchange value object:  

{CODE:csharp metadata_0@ClientApi/CompareExchange.cs /}

You can send it as a parameter of the 
[Put Compare Exchange Value operation](/client-api/operations/compare-exchange/put-compare-exchange-value):  

{CODE:csharp metadata_1@ClientApi/CompareExchange.cs /}


{PANEL/}

## Related Articles

### Client API  
- [Session: How to Get and Modify Entity Metadata](../../../client-api/session/how-to/get-and-modify-entity-metadata)  
- [Compare Exchange: Overview](../../../client-api/operations/compare-exchange/overview)  
- [Compare Exchange Expiration](../../../client-api/operations/compare-exchange/compare-exchange-expiration)  

### Server  
- [Document Expiration](../../../server/extensions/expiration)  
