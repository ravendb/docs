# How to setup aggressive caching?

## Standard cache configuration

The RavenDB client provides a caching mechanism out of the box. You can decide if Raven should cache a request or not for a particular URL. 
The default caching configuration is to cache all request:

{CODE should_cache_delegate@ClientApi\HowTo\SetupAggressiveCaching.cs /}

The second cache option is the number of cached requests. The default value is 2048:

{CODE max_number_of_requests@ClientApi\HowTo\SetupAggressiveCaching.cs /}

The client utilizes the notion of the `304 Not Modified` server's response and will serve the data from the cache if available. 

## Aggressive mode

The aggressive caching feature goes even further. Enabling it causes that the client can do not even ask the server and simply return the response directly from a local cache, without any usage of `304 Not Modified` status. 
It means that a result will be returned very fast. The way it works is that the client subscribes to [server notifications](../changes/what-is-changes-api) and by taking advantage of them is able to invalidate cached documents
when they are changed. Hence the client knows when it can serve the response from the cache, and when it has to send the request to get the up-to-date result. 

{WARNING:Important}
Despite the fact that the aggressive cache uses the notifications to invalidate the cache, it is still possible to get a stale data because of the time needed to receive the notification from the server.
{WARNING/}

To activate the aggressive caching mode use the code:

{CODE aggressive_cache_load@ClientApi\HowTo\SetupAggressiveCaching.cs /}

Now, if there is a value in the cache for `orders/1` that is at most 5 minutes old and we haven't got any change notification about it, we can directly return it. The same mechanism works on queries as well:

{CODE aggressive_cache_query@ClientApi\HowTo\SetupAggressiveCaching.cs /}

The usage of the notification system means that you can set an aggressive cache duration to longer period. The document store exposes the method:

{CODE aggressive_cache_for_one_day_1@ClientApi\HowTo\SetupAggressiveCaching.cs /}

which is equivalent to:

{CODE aggressive_cache_for_one_day_2@ClientApi\HowTo\SetupAggressiveCaching.cs /}

### Disabling the use of notifications

You can disable the usage of Changes API by the aggressive cache to track cached items and invalidate the cache when needed:

{CODE disable_changes_tracking@ClientApi\HowTo\SetupAggressiveCaching.cs /}

Note that it makes that it becomes more likely to get stale results.

### Invalidate on SaveChanges

In addition to the use of notifications, the aggressive cache also revalidates the cache after calling `session.SaveChanges()`. This option is configurable by the following convention:

{CODE should_save_changes_force_aggressive_cache_check_convention@ClientApi\HowTo\SetupAggressiveCaching.cs /}

Its default value is `true`. This will work nicely as long as you have just a single client. For multiple clients you should use `ShouldAggressiveCacheTrackChanges`.
