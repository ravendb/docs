# Client API: Subscribing to Store Events

---

{NOTE: }

* **Events** allow users to perform custom actions in response to operations made in 
  a `Document Store` or a `Session`.  

* An event is invoked when the selected action is executed on an entity, 
  or querying is performed.  

* Subscribing to an event at the `DocumentStore` level subscribes to this 
  event in all subsequent sessions.  

* Subscribing to an event in a `Session` is valid only for this session.  
  Read more about `Session` events [here](../../client-api/session/how-to/subscribe-to-events).  

* In this page:  
   * [Store Events](../../client-api/how-to/subscribe-to-store-events#store-events)
      * [OnBeforeRequest](../../client-api/how-to/subscribe-to-store-events#section)  
      * [OnSucceedRequest](../../client-api/how-to/subscribe-to-store-events#section-1)  
      * [AfterDispose](../../client-api/how-to/subscribe-to-store-events#section-2)  
      * [BeforeDispose](../../client-api/how-to/subscribe-to-store-events#section-3)  
      * [RequestExecutorCreated](../../client-api/how-to/subscribe-to-store-events#section-4)  
      * [OnSessionCreated](../../client-api/how-to/subscribe-to-store-events#section-5)  
      * [OnFailedRequest](../../client-api/how-to/subscribe-to-store-events#section-6)  
      * [OnTopologyUpdated](../../client-api/how-to/subscribe-to-store-events#section-7)  
   * [Store/Session Events](../../client-api/how-to/subscribe-to-store-events#store/session-events)

{NOTE/}

---

{PANEL: Store Events}

You can subscribe to the following events only at the store level, not within a session.  

## `OnBeforeRequest`

This event is invoked by sending a request to the server, before the request 
is actually sent.  
It should be defined with this signature:  
{CODE-BLOCK: csharp}
private void OnBeforeRequestEvent(object sender, BeforeRequestEventArgs args);
{CODE-BLOCK/}

**Parameters**:  

| Parameter | Type | Description |
| --------- | ---- | ----------- |
| **sender** | `IDocumentStore ` | The store that the request was sent from, triggering this event |
| **args** | `BeforeRequestEventArgs` | the database name and URL, and the intended request for the server |

`BeforeRequestEventArgs`:  
{CODE-BLOCK: csharp}
public class BeforeRequestEventArgs : EventArgs
{
    // Database Name
    public string Database { get; }
    // Database URL
    public string Url { get; }
    // The request intended to be sent to the server  
    public HttpRequestMessage Request { get; }
    // The number of attempts made to send the request to the server
    public int AttemptNumber { get; }
}
{CODE-BLOCK/}

* **Example**:  
  To define a method that checks URLs sent in a document store request:  
  {CODE OnBeforeRequestEvent@ClientApi\DocumentStore\StoreEvents.cs /}

    To subscribe to the event:  
    {CODE SubscribeToOnBeforeRequest@ClientApi\DocumentStore\StoreEvents.cs /}

## `OnSucceedRequest`

This event is invoked by receiving a successful reply from the server.  
It should be defined with this signature:  
{CODE-BLOCK: csharp}
private void OnSucceedRequestEvent(object sender, SucceedRequestEventArgs args);
{CODE-BLOCK/}

**Parameters**:  

| Parameter | Type | Description |
| --------- | ---- | ----------- |
| **sender** | `IDocumentStore ` | The store that the request was sent from, triggering this event |
| **args** | `SucceedRequestEventArgs` | the database name and URL, and the messages sent to the server and returned from it |

`SucceedRequestEventArgs`:  
{CODE-BLOCK: csharp}
public class SucceedRequestEventArgs : EventArgs
{
    // Database Name
    public string Database { get; }
    // Database URL
    public string Url { get; }
    // The message returned from the server
    public HttpResponseMessage Response { get; }
    // The original request sent to the server
    public HttpRequestMessage Request { get; }
    // The number of attempts made to send the request to the server
    public int AttemptNumber { get; }
}
{CODE-BLOCK/}

* **Example**  
  To define a method that would be activated when a request succeeds:  
  {CODE OnSucceedRequestEvent@ClientApi\DocumentStore\StoreEvents.cs /}

    To subscribe to the event:  
    {CODE SubscribeToOnSucceedRequest@ClientApi\DocumentStore\StoreEvents.cs /}

## `AfterDispose`
This event is invoked just after a document store is disposed of.  

## `BeforeDispose`
This event is invoked just before a document store is disposed of.  

## `RequestExecutorCreated`
This event is invoked when a Request Executor is created, 
allowing you to subscribe to various events of the request executor.  

## `OnSessionCreated`
This event is invoked after a session is created.  

## `OnFailedRequest`
This event is invoked before a request fails.  

## `OnTopologyUpdated`
This event is invoked by a topology update (e.g. when a node is added), 
**after** the topology is updated.  

{PANEL/}

{PANEL: Store/Session Events}
You can subscribe to the following events both at the store level and in a session.  

{NOTE: }

  * Subscribing to an event in a session limits the scope of the subscription to this session.  
  * When you subscribe to an event at the store level, the subscription is inherited by 
    all subsequent sessions.  

{NOTE/}

* [OnBeforeStore](../../client-api/session/how-to/subscribe-to-events#onbeforestore)  
* [OnAfterSaveChanges](../../client-api/session/how-to/subscribe-to-events#onaftersavechanges)  
* [OnBeforeDelete](../../client-api/session/how-to/subscribe-to-events#onbeforedelete)  
* [OnBeforeQuery](../../client-api/session/how-to/subscribe-to-events#onbeforequery)  
* [OnBeforeConversionToDocument](../../client-api/session/how-to/subscribe-to-events#onbeforeconversiontodocument)  
* [OnAfterConversionToDocument](../../client-api/session/how-to/subscribe-to-events#onafterconversiontodocument)  
* [OnBeforeConversionToEntity](../../client-api/session/how-to/subscribe-to-events#onbeforeconversiontoentity)  
* [OnAfterConversionToEntity](../../client-api/session/how-to/subscribe-to-events#onafterconversiontoentity)  
* `OnSessionDisposing`  
  This event is invoked by the disposal of a session, **before** the session is disposed of.  

{PANEL/}

## Related articles

### Document Store

- [What is a Document Store](../../client-api/what-is-a-document-store)

### Session

- [Subscribe to Session Events](../../client-api/session/how-to/subscribe-to-events)
