# Perform requests lazily
---

{NOTE: }

* **Lazy request**:

    * You can define a lazy request within a session (e.g. a lazy-query or a lazy-load request)  
      and defer its execution until actually needed.

    * The lazy request definition is stored in the session and a `Lazy<T>` instance is returned.  
      The request will be sent to the server and executed only when you access the value of this instance.

* **Multiple lazy requests**:

    * Multiple lazy requests can be defined within the same session.

    * When triggering the deferred execution (whether implicitly or explicitly),  
      ALL pending lazy requests held up by the session will be sent to the server in a single call.  
      This can help reduce the number of remote calls made to the server over the network.

* In this page:
    * [Requests that can be executed lazily](../../../client-api/session/how-to/perform-operations-lazily#operations-that-can-be-executed-lazily)
        * [Load entities](../../../client-api/session/how-to/perform-operations-lazily#load-entities)
        * [Load entities with include](../../../client-api/session/how-to/perform-operations-lazily#load-entities-with-include)
        * [Load entities starting with](../../../client-api/session/how-to/perform-operations-lazily#load-entities-starting-with)
        * [Conditional load](../../../client-api/session/how-to/perform-operations-lazily#conditional-load)
        * [Run query](../../../client-api/session/how-to/perform-operations-lazily#run-query)
        * [Get revisions](../../../client-api/session/how-to/perform-operations-lazily#get-revisions)
        * [Get compare-exchange value](../../../client-api/session/how-to/perform-operations-lazily#get-compare-exchange-value)
    * [Multiple lazy requests](../../../client-api/session/how-to/perform-operations-lazily#multiple-lazy-requests)
        * [Execute all requests - implicitly](../../../client-api/session/how-to/perform-operations-lazily#execute-all-requests---implicitly)
        * [Execute all requests - explicitly](../../../client-api/session/how-to/perform-operations-lazily#execute-all-requests---explicitly)

{NOTE/}

---

{PANEL: Operations that can be executed lazily}

### Load entities

[Load](../../../client-api/session/loading-entities#load) loads a document entity from 
the database into the session.  
Loading entities can be executed **lazily**.   

{CODE lazy_Load@ClientApi\Session\HowTo\Lazy.cs /}

---

### Load entities with include

[Load with include](../../../client-api/session/loading-entities#load-with-includes) loads both the document and the specified related document.    
Loading entities with include can be executed **lazily**.

{CODE-TABS}
{CODE-TAB:csharp:Lazy_load_with_include lazy_LoadWithInclude@ClientApi\Session\HowTo\Lazy.cs /}
{CODE-TAB:csharp:The_document lazy_productClass@ClientApi\Session\HowTo\Lazy.cs /}
{CODE-TABS/}

---

### Load entities starting with

[LoadStartingWith](../../../client-api/session/loading-entities#loadstartingwith) loads entities whose ID starts with the specified prefix.  
Loading entities with a common prefix can be executed **lazily**.

{CODE lazy_LoadStartingWith@ClientApi\Session\HowTo\Lazy.cs /}

---

### Conditional load

[ConditionalLoad](../../../client-api/session/loading-entities#conditionalload) logic is: 

* If the entity is already loaded to the session:  
  no server call is made, the tracked entity is returned.  
* If the entity is Not already loaded to the session:  
  the document will be loaded from the server only if the change-vector provided to the 
  method is older than the one in the server (i.e. if the document in the server is newer).  
* Loading entities conditionally can be executed **lazily**.  

{CODE lazy_ConditionalLoad@ClientApi\Session\HowTo\Lazy.cs /}

---

### Run query

A Query can be executed **lazily**.  
Learn more about running queries lazily in [lazy queries](../../../client-api/session/querying/how-to-perform-queries-lazily).

{CODE lazy_Query@ClientApi\Session\HowTo\Lazy.cs /}

---

### Get revisions

All methods for [getting revisions](../../../document-extensions/revisions/client-api/session/loading) and their metadata can be executed **lazily**.

{CODE lazy_Revisions@ClientApi\Session\HowTo\Lazy.cs /}

---

### Get compare-exchange value

[Getting compare-exchange](../../../client-api/session/cluster-transaction/compare-exchange#get-compare-exchange) 
values can be executed **lazily**.

{CODE lazy_CompareExchange@ClientApi\Session\HowTo\Lazy.cs /}

{PANEL/}

{PANEL: Multiple lazy requests }

### Execute all requests - implicitly

Accessing the value of ANY of the lazy instances will trigger
the execution of ALL pending lazy requests held up by the session, 
in a SINGLE server call.  

{CODE lazy_ExecuteAll_Implicit@ClientApi\Session\HowTo\Lazy.cs /}

---

### Execute all requests - explicitly

Explicitly calling `ExecuteAllPendingLazyOperations` will execute 
ALL pending lazy requests held up by the session, in a SINGLE server call.  

{CODE lazy_ExecuteAll_Explicit@ClientApi\Session\HowTo\Lazy.cs /}

{PANEL/}

## Related Articles

### Session

- [How to Perform Queries Lazily](../../../client-api/session/querying/how-to-perform-queries-lazily)
- [Cluster Transaction - Overview](../../../client-api/session/cluster-transaction/overview)
