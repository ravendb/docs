# Data Subscriptions: How to Consume a Data Subscription

---

{NOTE: }

Subscriptions are consumed by processing batches of documents received from the server. 
A `SubscriptionWorker` object manages the documents processing and the communication between the client and the server according to a set of configurations received upon it's creation. 
We've introduced several ways to create and configure a SubscriptionWorker, starting from just giving a subscription name, and ending with a detailed configuration object - `SubscriptionWorkerOptions`.

In this page:

[SubscriptionWorker lifecycle](../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription#subscriptionworker-lifecycle)  
[Error handling](../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription#error-handling)  
[Workers interplay](../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription#workers-interplay)

{NOTE/}

---

{PANEL:SubscriptionWorker lifecycle}
A `SubscriptionWorker` object starts its life from being generated by the `DocumentsStore.subscriptions`:

{CODE:nodejs subscription_open_simple@client-api\dataSubscriptions\dataSubscriptions.js /}

The worker is going to connect to the server asynchronously, when a listener for the `"batch"` event is registered using `on()` method. 

{CODE:nodejs subscription_run_simple@client-api\dataSubscriptions\dataSubscriptions.js /}

From this point on, the subscription worker will start processing batches. If for any reason, the processing is aborted an `"error"` is going to be emitted with an `Error` argument.

{PANEL/}


{PANEL:Error handling}
There are two categories of errors that may occur during subscription processing:

{INFO:Internal mechanism errors}
Those errors occur during the normal server-client communication between the worker and the server (those would not be emitted via `"error"` event).  
If an unexpected error occurs, the worker will try to reconnect to the server. There are conditions in which the worker will cease its operation and will not try to reconnect:  

* The subscription does not exist or was deleted  

* Another worker took over the subscription (see connection strategy)

* The worker could not connect to any of the servers

* The worker could not receive the node in charge of the task (this can happen when there is no leader)

* Authorization exception

* Exception during connection establishment

{INFO/}

{INFO:User's batch processing logic unhandled exception}
Example:
{CODE:nodejs throw_during_user_logic@client-api\dataSubscriptions\dataSubscriptions.js /}

If an exception is thrown, the worker will abort the current batch process. 
A worker can be configured to treat the thrown exception by either of the following two ways:  

* By default, the worker will wrap the thrown exception with a `SubscriberErrorException` exception and rethrow it,  
  terminating the subscription execution without acknowledging progress or retrying. An `"error"` is going to be emitted.

* If `SubscriptionWorkerOptions`'s value `ignoreSubscriberErrors` is set to true, the erroneous batch will get acknowledged without retrying and the next batches will continue processing. 
{INFO/}

{INFO: Reconnecting}
In the cases above, we described situations in which a worker will try to reconnect with the server. There are two key `SubscriptionWorkerOptions` fields controlling this state:

* `timeToWaitBeforeConnectionRetry` - The time that the worker will 'sleep' before trying to reconnect.

* `maxErroneousPeriod` - The maximum time in which the worker is allowed to be in erroneous state. After that time passes, the worker will stop trying to reconnect
{INFO/}

{INFO: OnUnexpectedSubscriptionError}
`OnUnexpectedSubscriptionError` is the event raised when a connection failure occurs 
between the subscription worker and the server and it throws an unexpected exception. 
When this occurs, the worker will automatically try to reconnect again. This event is 
useful for logging these unexpected exceptions.
{INFO/}

{PANEL/}

{PANEL: Workers interplay}
There can only be one active subscription worker working on a subscription. 
Nevertheless, there are scenarios where it is required to interact between an existing subscription worker and one that tries to connect. 
This relationship and interoperation is configured by the `SubscriptionConnectionOptions` `Strategy` field.  
The strategy field is an enum, having the following values:  

* `OpenIfFree` - the server will allow the worker to connect only if there isn't any other currently connected workers.  
  If there is a existing connection, the incoming worker will throw a *SubscriptionInUseException*.  
* `WaitForFree` - If the client currently cannot open the subscription because it is used by another client, it will wait for the previous client to disconnect and only then will connect.  
  This is useful in client failover scenarios where there is one active client and another one already waiting to take its place.  
* `TakeOver` - the server will allow an incoming connection to overthrow an existing one. It will behave according to the existing connection strategy:
  * The existing connection has a strategy that is not `TakeOver`. In this case, the incoming connection will take over it causing the existing connection to throw a *SubscriptionInUseException* exception.  
  * The existing connection has a strategy that is `TakeOver`. In this case, the incoming connection will throw a *SubscriptionInUseException* exception.  
{PANEL/}

## Related Articles

**Data Subscriptions**:

- [What are Data Subscriptions](../../../client-api/data-subscriptions/what-are-data-subscriptions)
- [How to Create a Data Subscription](../../../client-api/data-subscriptions/creation/how-to-create-data-subscription)
- [How to Consume a Data Subscription](../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription)
