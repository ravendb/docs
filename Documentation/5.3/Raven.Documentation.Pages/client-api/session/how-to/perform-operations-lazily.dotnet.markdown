# Session: How to Perform Operations Lazily

---

{NOTE: }

* **Performing an operation lazily** allows you to defer the execution of the operation 
  until it is needed.  

* Operations that can be executed lazily include: Loading documents and included documents, 
  Retrieving revisions, Getting compare exchange values and related data, and various 
  other operations specified below.  

* You can execute **all** pending lazy operations using 
  [ExecuteAllPendingLazyOperations](../../../client-api/session/how-to/perform-operations-lazily#executing-all-pending-lazy-operations).  

* In this page:  
   * [Lazy Operations](../../../client-api/session/how-to/perform-operations-lazily#lazy-operations)  
      * [Load](../../../client-api/session/how-to/perform-operations-lazily#section)  
      * [Load with Include](../../../client-api/session/how-to/perform-operations-lazily#with-)  
      * [Query](../../../client-api/session/how-to/perform-operations-lazily#section-1)  
      * [LoadStartingWith](../../../client-api/session/how-to/perform-operations-lazily#section-2)  
      * [ConditionalLoad](../../../client-api/session/how-to/perform-operations-lazily#section-3)  
      * [Revisions](../../../client-api/session/how-to/perform-operations-lazily#revisions)  
      * [GetCompareExchangeValue](../../../client-api/session/how-to/perform-operations-lazily#section-4)  
   * [Executing All Pending Lazy Operations](../../../client-api/session/how-to/perform-operations-lazily#executing-all-pending-lazy-operations)  

{NOTE/}

{PANEL: Lazy Operations}

Operations that can be executed lazily include:

---

### `Load`
You can load entities lazily using the [Load](../../../client-api/session/loading-entities#load) method.  

#### Example
{CODE lazy_Load@ClientApi\Session\HowTo\Lazy.cs /}

---

### `Load` with `Include`
You can [load entities witn includes](../../../client-api/session/loading-entities#load-with-includes) lazily.  

#### Example
{CODE lazy_UserDefinition@ClientApi\Session\HowTo\Lazy.cs /}
{CODE lazy_LoadWithIncludes@ClientApi\Session\HowTo\Lazy.cs /}

---

### `Query`

Learn more [here](../../../client-api/session/querying/how-to-perform-queries-lazily) 
about running queries lazily.  

#### Example
{CODE lazy_Query@ClientApi\Session\HowTo\Lazy.cs /}

---

### `LoadStartingWith`
`LoadStartingWith` loads entities whose ID starts with a specified prefix. 
Learn more about it [here](../../../client-api/session/loading-entities#loadstartingwith).  
Use `LoadStartingWith` lazily as shown in the example below.  

#### Example
{CODE lazy_LoadStartingWith@ClientApi\Session\HowTo\Lazy.cs /}

---

### `ConditionalLoad`
`ConditionalLoad` loads only documents whose change vector has changed since 
the last load.  
Learn more about it [here](../../../client-api/session/loading-entities#conditionalload).  
Use `ConditionalLoad` lazily as shown in the example below.  

#### Example
{CODE lazy_ConditionalLoad@ClientApi\Session\HowTo\Lazy.cs /}

---

### Revisions
[Document Revisions](../../../document-extensions/revisions/overview) 
and data related to them can be loaded using several methods. Each of these methods 
can also be used lazily.  

| Revision_Method | Lazy_Version    |
| --------------- | --------------- |
| [Get](../../../document-extensions/revisions/client-api/session/loading#get) | `session.Advanced.Revisions.Lazily.Get` |
| [GetFor](../../../document-extensions/revisions/client-api/session/loading#getfor) | `session.Advanced.Revisions.Lazily.GetFor` |
| [GetMetadataFor](../../../document-extensions/revisions/client-api/session/loading#getmetadatafor) | `session.Advanced.Revisions.Lazily.GetMetadataFor` |

#### Example
{CODE lazy_Revisions@ClientApi\Session\HowTo\Lazy.cs /}

---

### `GetCompareExchangeValue`

Use `GetCompareExchangeValue` to retrieve Compare Exchange values.  
Learn [here](../../../client-api/session/cluster-transaction#get-compare-exchange-lazily) 
how to use this method lazily.  

{PANEL/}

{PANEL: Executing All Pending Lazy Operations}

To execute **all** pending lazy operations at once, use the 
`session.Advanced.Eagerly.ExecuteAllPendingLazyOperations` method.  

#### Example
{CODE lazy_ExecuteAllPendingLazyOperations@ClientApi\Session\HowTo\Lazy.cs /}

{PANEL/}

## Related Articles

### Session

- [How to Perform Queries Lazily](../../../client-api/session/querying/how-to-perform-queries-lazily)
- [Cluster Transaction - Overview](../../../client-api/session/cluster-transaction)
