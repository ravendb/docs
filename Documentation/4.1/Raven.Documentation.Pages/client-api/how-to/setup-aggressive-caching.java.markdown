# Client API: How to Setup Aggressive Caching

## Standard Cache Configuration

The RavenDB client provides a caching mechanism out of the box. The default caching configuration is to cache all requests.

The size of cache can be configured by changing [`MaxHttpCacheSize` convention](../../client-api/configuration/conventions#maxhttpcachesize).

The client utilizes the notion of the `304 Not Modified` server's response and will serve the data from the cache if available.

## Aggressive Mode

The aggressive caching feature goes even further. Enabling it means that the client doesn't need to ask the server. It will simply return the response directly from a local cache without any usage of `304 Not Modified` status. 
Results will be returned very fast. 

Here's how it works: The client subscribes to server notifications using the [Changes API](../changes/what-is-changes-api). By taking advantage of them, he is able to invalidate cached documents when they are changed.
The client knows when it can serve the response from the cache, and when it has to send the request to get the up-to-date result. 

{WARNING:Important}
Despite the fact that the aggressive cache uses the notifications to invalidate the cache, it is still possible to get stale data because of the time needed to receive the notification from the server.
{WARNING/}

Options for aggressive caching can be set in the Document Store conventions:

{CODE:java aggressive_cache_conventions@ClientApi\HowTo\SetupAggressiveCaching.java /}

We can activate this mode globally from the store or per session.

To activate this mode globally from the store we just need to add one of the following lines:

{CODE:java aggressive_cache_global@ClientApi\HowTo\SetupAggressiveCaching.java /}

If we want to activate this mode only in the session we need to add this in the session:

{CODE:java aggressive_cache_load@ClientApi\HowTo\SetupAggressiveCaching.java /}

If there is a value in the cache for `orders/1` that is at most 5 minutes old and we haven't got any change notification about it, we can directly return it. The same mechanism works on queries as well:

{CODE:java aggressive_cache_query@ClientApi\HowTo\SetupAggressiveCaching.java /}

The usage of the notification system means that you can set an aggressive cache duration to a longer period. The document store exposes the method:

{CODE:java aggressive_cache_for_one_day_1@ClientApi\HowTo\SetupAggressiveCaching.java /}

which is equivalent to:

{CODE:java aggressive_cache_for_one_day_2@ClientApi\HowTo\SetupAggressiveCaching.java /}

###Disable Change Tracking

The client subscribes to change notifications from the server using the [Changes API](../changes/what-is-changes-api). You can choose to ignore 
these notifications by changing the `AggressiveCacheMode` in the Document Store conventions.  

The modes are:  
* `AggressiveCacheMode.TRACK_CHANGES` - The default value. When the server sends a notification that some items (documents or indexes) have changed, 
those items are invalidated from the cache. The next time these items are loaded they will be retrieved from the server.  
* `AggressiveCacheMode.DO_NOT_TRACK_CHANGES` - Notifications from the server will be ignored. For the aggressive cache `Duration`, results will be 
retrieved from the cache and may therefore be stale.  

{CODE:java disable_change_tracking@ClientApi\HowTo\SetupAggressiveCaching.java /}

###Disable Aggressive Mode

We can disable the aggressive mode by simply using `documentStore.disableAggressiveCaching();`. In that way we will disable the aggressive caching 
globally in the store. But what if we need to disable the aggressive caching only for a specific call, or to manually update the cache, just like before we can use `disableAggressiveCaching()`
per session?

{CODE:java disable_aggressive_cache@ClientApi\HowTo\SetupAggressiveCaching.java /}
