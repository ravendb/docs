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
| **sender** | `IDocumentStore ` | The subscribed store that triggered the event |
| **args** | `BeforeRequestEventArgs` | See details below |

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
| **sender** | `IDocumentStore ` | The subscribed store that triggered the event |
| **args** | `SucceedRequestEventArgs` | See details below |

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
This event is invoked immediately after a document store is disposed of.  
It should be defined with this signature:  
{CODE-BLOCK: csharp}
private void AfterDisposeEvent(object sender, EventHandler args);
{CODE-BLOCK/}

**Parameters**:  

| Parameter | Type | Description |
| --------- | ---- | ----------- |
| **sender** | `IDocumentStore ` | The subscribed store whose disposal triggered the event |
| **args** | `EventHandler` | **args** has no contents for this event |

## `BeforeDispose`
This event is invoked immediately before a document store is disposed of.  
It should be defined with this signature:  
{CODE-BLOCK: csharp}
private void BeforeDisposeEvent(object sender, EventHandler args);
{CODE-BLOCK/}

**Parameters**:  

| Parameter | Type | Description |
| --------- | ---- | ----------- |
| **sender** | `IDocumentStore ` | The subscribed store whose disposal triggered the event |
| **args** | `EventHandler` | **args** has no contents for this event |

## `RequestExecutorCreated`
This event is invoked when a Request Executor is created, 
allowing you to subscribe to various events of the request executor.  
It should be defined with this signature:  
{CODE-BLOCK: csharp}
private void RequestExecutorCreatedEvent(object sender, RequestExecutor args);
{CODE-BLOCK/}

**Parameters**:  

| Parameter | Type | Description |
| --------- | ---- | ----------- |
| **sender** | `IDocumentStore ` | The subscribed store that triggered the event |
| **args** | `RequestExecutor` | The created Request Executor instance |

## `OnSessionCreated`
This event is invoked after a session is created, allowing you, for example, 
to change session configurations.  
It should be defined with this signature:  
{CODE-BLOCK: csharp}
private void OnSessionCreatedEvent(object sender, SessionCreatedEventArgs args);
{CODE-BLOCK/}

**Parameters**:  

| Parameter | Type | Description |
| --------- | ---- | ----------- |
| **sender** | `IDocumentStore ` | The subscribed store that triggered the event |
| **args** | `SessionCreatedEventArgs` | The created Session |

`SessionCreatedEventArgs`:  
{CODE-BLOCK: csharp}
public class SessionCreatedEventArgs : EventArgs
{
    public InMemoryDocumentSessionOperations Session { get; }
}
{CODE-BLOCK/}

* **Example**  
  To define a method that would be activated when a session is created:  
  {CODE OnSessionCreatedEvent@ClientApi\DocumentStore\StoreEvents.cs /}

    To subscribe to the event:  
    {CODE SubscribeToOnSessionCreated@ClientApi\DocumentStore\StoreEvents.cs /}
    

## `OnFailedRequest`
This event is invoked before a request fails. It allows you, for example, to track 
and log failed requests.  
It should be defined with this signature:  
{CODE-BLOCK: csharp}
private void OnFailedRequestEvent(object sender, FailedRequestEventArgs args);
{CODE-BLOCK/}

**Parameters**:  

| Parameter | Type | Description |
| --------- | ---- | ----------- |
| **sender** | `IDocumentStore ` | The subscribed store that triggered the event |
| **args** | `FailedRequestEventArgs` | See details below |

`FailedRequestEventArgs`:  
{CODE-BLOCK: csharp}
public class FailedRequestEventArgs : EventArgs
{
    // Database Name
    public string Database { get; }
    // Database URL
    public string Url { get; }
    // The exception returned from the server
    public Exception Exception { get; }
    // The message returned from the server
    public HttpResponseMessage Response { get; }
    // The original request sent to the server
    public HttpRequestMessage Request { get; }
}
{CODE-BLOCK/}

* **Example**  
  To define a method that would be activated when a request fails:  
  {CODE OnFailedRequestEvent@ClientApi\DocumentStore\StoreEvents.cs /}

    To subscribe to the event:  
    {CODE SubscribeToOnFailedRequest@ClientApi\DocumentStore\StoreEvents.cs /}

## `OnTopologyUpdated`
This event is invoked by a topology update (e.g. when a node is added), 
**after** the topology is updated.  
It should be defined with this signature:  
{CODE-BLOCK: csharp}
private void OnTopologyUpdatedEvent(object sender, TopologyUpdatedEventArgs args);
{CODE-BLOCK/}

**Parameters**:  

| Parameter | Type | Description |
| --------- | ---- | ----------- |
| **sender** | `IDocumentStore ` | The subscribed store that triggered the event |
| **args** | `TopologyUpdatedEventArgs` | The updated list of nodes |

`TopologyUpdatedEventArgs`:  
{CODE-BLOCK: csharp}
public class TopologyUpdatedEventArgs : EventArgs
{
    public Topology Topology { get; }
}
{CODE-BLOCK/}

`Topology`:  
public class Topology
{
    public long Etag;
    public List<ServerNode> Nodes;
}

* **Example**  
  To define a method that would be activated on a topology update:  
  {CODE OnTopologyUpdatedEvent@ClientApi\DocumentStore\StoreEvents.cs /}

    To subscribe to the event:  
    {CODE SubscribeToOnTopologyUpdated@ClientApi\DocumentStore\StoreEvents.cs /}

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
* [OnSessionDisposing](../../client-api/session/how-to/subscribe-to-events#onsessiondisposing)  
  
{PANEL/}

## Related articles

### Document Store

- [What is a Document Store](../../client-api/what-is-a-document-store)

### Session

- [Subscribe to Session Events](../../client-api/session/how-to/subscribe-to-events)
