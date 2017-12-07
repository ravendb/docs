# How to Setup Aggressive Caching

## Standard Cache Configuration

The RavenDB client provides a caching mechanism out of the box. The default caching configuration is to cache all requests.

The default value of number of cached requests per database is 512 MB, But you can change it by changing `MaxHttpCacheSize` in the store `Conventions`.

{CODE max_number_of_requests@ClientApi\HowTo\SetupAggressiveCaching.cs /}

The client utilizes the notion of the `304 Not Modified` server's response and will serve the data from the cache if available.

{NOTE: Disable caching} 
To disable the caching you can change the `MaxHttpCacheSize` value to zero:
{CODE disable_http_cache@ClientApi\HowTo\SetupAggressiveCaching.cs /}

**In this scenario all the requests will be sent to the server to fetch the data.**

{NOTE/}


## Aggressive Mode

The aggressive caching feature goes even further. Enabling it means that the client doesn't need to ask the server. It will simply return the response directly from a local cache, without any usage of `304 Not Modified` status. 
Result will be returned very fast. The way it works: The client subscribes to [server notifications](../changes/what-is-changes-api). By taking advantage of them, he is able to invalidate cached documents when they are changed.
The client knows when it can serve the response from the cache, and when it has to send the request to get the up-to-date result. 

{WARNING:Important}
Despite the fact that the aggressive cache uses the notifications to invalidate the cache, it is still possible to get stale data because of the time needed to receive the notification from the server.
{WARNING/}

To activate the aggressive caching mode, use the code:

{CODE aggressive_cache_load@ClientApi\HowTo\SetupAggressiveCaching.cs /}

If there is a value in the cache for `orders/1` that is at most 5 minutes old and we haven't got any change notification about it, we can directly return it. The same mechanism works on queries as well:

{CODE aggressive_cache_query@ClientApi\HowTo\SetupAggressiveCaching.cs /}

The usage of the notification system means that you can set an aggressive cache duration to a longer period. The document store exposes the method:

{CODE aggressive_cache_for_one_day_1@ClientApi\HowTo\SetupAggressiveCaching.cs /}

which is equivalent to:

{CODE aggressive_cache_for_one_day_2@ClientApi\HowTo\SetupAggressiveCaching.cs /}
