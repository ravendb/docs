#How to open data subscription?

If you have a subscription identifier you can open it. Together with id you need to specify some details about the subscription connection:

{CODE open_1@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **id** | long | A data subscription identifier. |
| **options** | SubscriptionConnectionOptions | Connection options. |

| Return value | |
| ------------- | ----- |
| Subscription&lt;RavenJObject&gt; / Subscription&lt;T&gt; | Subscription instance. |

We have two method to open subscription. The first one is to deal with documents of different type - then results are returned as `RavenJObject`s. The second one is strongly typed version
and will return entities of a given type. 

{WARNING: Single subscription consumer at a time allowed}
There can be only a single open subscription connection per subscription. An attempt to open already being opened subscription will result in throwing an exception.
{WARNING/}

Documents are sent to a client in batches. `SubscriptionConnectionOptions` has `BatchOptions` property where you can specify:

* _MaxDocCount_ - max number of docs that can be sent in a single batch (default: 4096),
* _MaxSize_ - max total batch size in bytes (default: `null` - no limit),
* _AcknowledgmentTimeout_ - max time within the subscription needs to confirm that the batch has been successfully processed (default: 1 minute).

Additionally connection options have the following settings:

- _IgnoreSubscribersErrors_ - determines if subscription should ignore errors thrown by subscription handlers (default: false),
- _ClientAliveNotificationInterval_ - specifies how often the subscription sends heart beats to the server (server keeps the subscriptions open for a given client
 until it sends these alive notifications, default: 2 minutes).

{INFO: Error handling}
By default the data subscription doesn't allow errors (`IgnoreSubscribersErrors: false`). So if any of the processing handlers fails, you will get the same document 
over and over until you have successfully processed it. If you set `IgnoreSubscribersErrors` to `true`, it will ignore errors raised by handles.
{INFO/}

{INFO: Crashing handling}
Under the scenes, once you have successfully processed a batch, the notification will send a confirmation to the server about it.
So if there is a crash, we know what documents have been already processed. If you crashed midway, the database will just resend you the relevant documents when you open 
the subscription again. The data subscription tries to automatically reopen after loosing a connection with the server.
{INFO/}

###Example I

{CODE open_2@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

##Processing documents

The result of opening subscription is `Subscription<T>` object that implements `IObservable` interface, so it means that you can just utilize Reactive Extensions
and subscribe to the incoming documents to process them. Also you will continue to get them, even for items that were added after you opened the subscription.

###Example II

{CODE open_3@ClientApi\DataSubscriptions\DataSubscriptions.cs /}
