
# Aggressive Caching

## Cache options
The RavenDB client provides a caching feature out of the box. You can determine whether Raven should cache a request or not for a particular URL. By default all requests are cached:

{CODE should_cache_delegate@ClientApi\Advanced\AggressiveCaching.cs /}

The second cache option is the number of cached requests. The default value is 2048:

{CODE max_number_of_requests@ClientApi\Advanced\AggressiveCaching.cs /}

The client utilizes the notion of the `304 Not Modified` RavenDB server's response and will serve the data from the cache if available. 

## Aggressive mode

The aggressive caching feature goes even further. If you enable it RavenDB can do not even ask the server and simply return the reply directly from a local cache if it is there,
when means that you will get it very fast. By default the client subscribes to [server notifications](../changes-api) and by taking advantage of them it is able to invalidate documents in the cache 
when they have been changed. It makes that the client knows when it needs to ask the server and when can serve the response from the cache. However you need to be aware that it is still possible to get 
stale data because of the time needed to receive the notification from the server.

You can also disable the mechanism of changes tracking by using the following convention:

{CODE disable_changes_tracking@ClientApi\Advanced\AggressiveCaching.cs /}

Note that it makes that it becomes more likely that you might get stale results.

To activate the aggressive caching mode use the code:

{CODE aggressive_cache_load@ClientApi\Advanced\AggressiveCaching.cs /}

Now, if there is a value in the cache for `users/1` that is at most 5 minutes old and we haven't get any change notification about it, we can directly use that. The same mechanism works on queries as well:

{CODE aggressive_cache_query@ClientApi\Advanced\AggressiveCaching.cs /}

The usage of the notification system means that you can set an aggressive cache duration to longer period. The document store exposes the method:

{CODE aggressive_cache_for_one_day_1@ClientApi\Advanced\AggressiveCaching.cs /}

which is equivalent to:

{CODE aggressive_cache_for_one_day_2@ClientApi\Advanced\AggressiveCaching.cs /}

## Saving changes

An another aggressive cache specific convention is:

{CODE should_save_changes_force_aggressive_cache_check_convention@ClientApi\Advanced\AggressiveCaching.cs /}

This option set to `true` (default) forces the client to check the cache with the server after calling `SaveChanges()` (when you know that things have been changed  - by you).

{NOTE The Silverlight version of the RavenDB client doesn't have own implementation of the aggressive caching because Silverlight already has the built-in and actively working cache. /}
