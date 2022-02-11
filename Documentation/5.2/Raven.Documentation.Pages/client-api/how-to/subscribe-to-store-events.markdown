# Client API: Subscribing to Store Events

---

{NOTE: }

* **Events** allow users to perform custom actions in response to operations made in 
  a `Document Store` or a `Session`.  

* An event is invoked when the selected action is executed on an entity, or querying is performed.  

* An event subscribed to in the `DocumentStore` level is valid for all succeeding sessions.  

* An event subscribed to in a `Session` is valid in this session.  
  Subscribing to an event within a session overrides subscribing 
  to it in the `DocumentStore` level.  
  Read more about `Session` events [here](../../client-api/session/how-to/subscribe-to-events).  

* In this page:  
   * [OnBeforeRequest](../../client-api/how-to/subscribe-to-store-events#onbeforerequest)  
   * [OnSucceedRequest](../../client-api/how-to/subscribe-to-store-events#onsucceedrequest)  

{NOTE/}

---

{PANEL: OnBeforeRequest}

This event is invoked as a part of sending a request to the server, but before 
the request is actually sent.  
It should be defined with this signature:  

{CODE-BLOCK: csharp}
private void OnBeforeRequestEvent(object sender, BeforeRequestEventArgs args);
{CODE-BLOCK/}

**Parameters**:  

| Parameter | Type | Description |
| --------- | ---- | ----------- |
| IDocumentStore | `string` | The store that the request was sent from, triggering this event |
| args | `BeforeRequestEventArgs` | the database name and URL, and the intended request for the server |

The class `BeforeRequestEventArgs`:  
{CODE-BLOCK: csharp}
public class BeforeRequestEventArgs : EventArgs
{
   // Database Name
   public string Database { get; }
   // Database URL
   public string Url { get; }
   // Intended request from the server  
   public HttpRequestMessage Request { get; }
   public int AttemptNumber { get; }
}
{CODE-BLOCK/}

### Example

If we want to check URLs sent in requests by our document store, we can define a method as follows:  
{CODE OnBeforeRequestEvent@ClientApi\DocumentStore\Events.cs /}

And then subscribe the event as follows:
{CODE SubscribeToOnBeforeRequest@ClientApi\DocumentStore\Events.cs /}

{PANEL/}

{PANEL: OnSucceedRequest}

This event is executed after receiving a successful reply from the server.  
It should be defined with this signature:  

{CODE-BLOCK: csharp}
private void OnSucceedRequestEvent(object sender, SucceedRequestEventArgs args);
{CODE-BLOCK/}

**Parameters**:  

| Parameter | Type | Description |
| --------- | ---- | ----------- |
| IDocumentStore | `string` | The store whose request succeeded, triggering this event |
| args | `SucceedRequestEventArgs` | the database name and URL, and the messages sent to the server and returned from it |

The class `SucceedRequestEventArgs`:  
{CODE-BLOCK: csharp}
public class SucceedRequestEventArgs : EventArgs
{
   // Database Name
   public string Database { get; }
   // Database URL
   public string Url { get; }
   // Message returned from the server
   public HttpResponseMessage Response { get; }
   // Request from the server
   public HttpRequestMessage Request { get; }
   public int AttemptNumber { get; }
}
{CODE-BLOCK/}


### Example

If we want to check whether our request was successful, we can define a method as follows:  
{CODE OnSucceedRequestEvent@ClientApi\DocumentStore\Events.cs /}

And then subscribe the event as follows:
{CODE SubscribeToOnSucceedRequest@ClientApi\DocumentStore\Events.cs /}

{PANEL/}

## Related articles

### Document Store

- [What is a Document Store](../../client-api/what-is-a-document-store)

### Session

- [Subscribe to Session Events](../../client-api/session/how-to/subscribe-to-events)
