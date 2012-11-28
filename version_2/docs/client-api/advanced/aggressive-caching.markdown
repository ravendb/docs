#Aggressive Caching

The RavenDB client provides a caching feature out of the box. You can determine whether Raven should cache a request or not for a particular URL. By default all requests are cached:

{CODE should_cache_delegate@Consumer\AggressiveCaching.cs /}

The second cache option is the number of cached requests. The default value is 2048:

{CODE max_number_of_requests@Consumer\AggressiveCaching.cs /}

The client utilizes the notion of the `304 Not Modified` RavenDB server's response and will serve the data from the cache if available. 

The aggressive caching feature goes even further. If you enable it RavenDB will not even ask the server whether anything has changed,
it will simply return the reply directly from the local cache if it is there. Note that it means that you might get stale data, but it also means that you will get it fast.

To activate this mode use the following code:

{CODE aggressive_cache_load@Consumer\AggressiveCaching.cs /}

Now, if there is a value in the cache for `users/1` that is at most 5 minutes old, we can directly use that. The same mechanism works on queries as well:

{CODE aggressive_cache_query@Consumer\AggressiveCaching.cs /}

{NOTE The Silverlight version of the RavenDB client doesn't have own implementation of the aggressive caching because Silverlight already has the built-in and actively working cache. /}