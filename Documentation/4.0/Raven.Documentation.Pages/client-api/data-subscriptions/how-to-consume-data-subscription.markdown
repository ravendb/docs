# How to consume a data subscription?

Subscriptions are consumed by the client using a 'SubscriptionWorker' object, This page will cover ways to obtain and configure the subscription workers.

{PANEL:Simple consumption example}

First, let us start from real simple subscription consumption example:
Here we create a subscription and have a worker processing documents

{CODE subscriptions_example@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

{PANEL/}

## Creating subscription worker object

{INFO:Connection to server}
The subscription worker object manages the subscription on the client side. Note that upon creation of the object, no connection will be created. A connection is created only when the received object's Run method is called.
{INFO/}


| Parameters | | |
| ------------- | ------------- | ----- |
| **id** | long | A data subscription identifier. |
| **options** | SubscriptionWorkerOptions | Worker options. |

| Return value | |
| ------------- | ----- |
| Subscription&lt;RavenJObject&gt; / Subscription&lt;T&gt; | Subscription instance. |

We have two method to open subscription. The first one is to deal with documents belonging to different collections - results are returned as `RavenJObject` objects then. The second one returns strongly
typed subscription where retrieved documents are converted to a given type.

{WARNING: Single subscription consumer at a time allowed}
There can be only a single open subscription connection per subscription. An attempt to open already being opened subscription will result in throwing an exception.
{WARNING/}

Documents are sent to a client in batches. `SubscriptionConnectionOptions` has `BatchOptions` property where you can specify:

* _MaxDocCount_ - max number of docs that can be sent in a single batch (default: 4096),
* _MaxSize_ - max total batch size in bytes (default: null - no size limit),
* _AcknowledgmentTimeout_ - max time within the subscription needs to confirm that the batch has been successfully processed (default: 1 minute).

Additionally connection options have the following settings:

- _IgnoreSubscribersErrors_ - determines if subscription should ignore errors thrown by subscription handlers (default: false),
- _ClientAliveNotificationInterval_ - specifies how often the subscription sends heart beats to the server (server keeps the subscription open until a connected client
sends these alive notifications - two undelivered notifications would let an another client to connect, default: 2 minutes),
- _Strategy_ - the enum that represents subscription opening strategy. There are four strategies available:

	- `OpenIfFree` - the client will successfully open a subscription only if there isn't any other currently connected client. Otherwise it will end up with `SubscriptionInUseException`,
	- `TakeOver` - the connecting client will successfully open a subscription even if there is another active subscription's consumer.
If the new client takes over the subscription then the existing one will get rejected. 
The subscription will always be processed by the last connected client.
	- `ForceAndKeep` - the client opening a subscription with forced strategy set will always get it and keep it open until another client with the same strategy gets connected.
	- `WaitForFree` - if the client currently cannot open the subscription because it is used by another client then it will subscribe Changes API to be notified about subscription status changes.
Every time `SubscriptionReleased` notification arrives, it will repeat an attempt to open the subscription. After it succeeds in opening, it will process docs as usual.

{INFO: Error handling}
By default the data subscription does not allow processing errors (`IgnoreSubscribersErrors: false`). So if any subscription handler fails,
then it will stop pulling documents and close the subscription connection immediately. If you set `IgnoreSubscribersErrors` to `true`, it will ignore an error raised by a handler and
keep retrieving next docs.
{INFO/}

{INFO: Acknowledgment timeout handling}
Under the scenes, once you have successfully processed a batch, the notification will send a confirmation to the server about it. The server keeps track of the last processed and
acknowledged document. If subscription handlers don't process the batch within the specified `AcknowledgmentTimeout`, then the server will resend the whole batch again. You will get 
the same documents over and over until you successfully processed it.
{INFO/}
 

{INFO: Crashing handling}
Tracking the last acknowledged Etag allows the data subscription to handle crashing scenarios. If there is a crash, we know what documents have been already processed. 
If you crashed midway, the database will just resend you the relevant documents when you open the subscription again. The data subscription automatically
retries to open the subscription connection every 15 seconds if it get lost.
{INFO/}

###Example I

{CODE open_2@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

##Processing documents

The result of opening subscription is `Subscription<T>` or `Subscription<RavenJObject>` instance. It implements `IObservable` interface, so it means that you can just utilize [Reactive Extensions](http://nuget.org/packages/Rx-Main)
and subscribe to the incoming documents in order to process them. Also you will continue to get them, even for items that were added after you opened the subscription, because under the hood the [Changes API](../changes/what-is-changes-api)
is used to get notifications about any document updates.

###Example II

{CODE open_3@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

###Example III

You may want to dynamically manage subscription handlers. The returned subscriber object is type of `IDisposable`, in order to detach it from subscription just call `Dispose` on it:

{CODE open_4@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

{NOTE:No subscriber attached}
The data subscription stops pulling docs if there is no subscriber attached.
{NOTE/}


### Get typed subscription worker

Here we get a subscription worker, only based on subscription name.
{CODE open_1@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

### Get typed subscription worker

Here we get a subscription worker, which in the case of another client consuming the connection, will wait for it to finish on the server side and only then start consuming the subscription. 
{CODE open_2@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

### Get typed subscription worker

Here we get a subscription worker as before, but also we set the maximum batch size to 500. And we define that if there is an error during the client's Run function, we will not abort the worker processing as defined by default, but we will continue processing.
{CODE open_3@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

## Related articles

- [What are data subscriptions?](../../client-api/data-subscriptions/what-are-data-subscriptions)
- [How to **create** a data subscription?](../../client-api/data-subscriptions/how-to-create-data-subscription)
