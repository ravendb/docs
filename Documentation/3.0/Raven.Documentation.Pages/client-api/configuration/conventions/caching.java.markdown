#Conventions related to caching

###ShouldCacheRequest

Use this convention to determine if the client should cache the request for the given URL. By default all requests are cached:

{CODE:java should_cache@ClientApi\Configuration\Conventions\Caching.java /}

###ShouldAggressiveCacheTrackChanges

It decides whether the client being in [the aggressive cache](../../how-to/setup-aggressive-caching) mode should use [Changes API](../../client-api/changes/what-is-changes-api) to monitor notifications and rebuild the cache based on them.
If the notification arrives then an outdated item from cache will be revalidated. 

{CODE:java should_aggressive_cache_track_changes@ClientApi\Configuration\Conventions\Caching.java /}

Note that it is still possible to get a stale result because of the time needed to receive the notification and checking for cached data.

###ShouldSaveChangesForceAggressiveCacheCheck

In addition to the use of notifications, the aggressive cache also revalidates the cache after calling `session.saveChanges()`. 
This behavior is configurable by the following convention:

{CODE:java should_save_changes_force_aggressive_cache_check@ClientApi\Configuration\Conventions\Caching.java /}

##Related articles

- [Commands : How to disable caching?](../../commands/how-to/disable-caching)
- [How to setup aggressive caching?](../../how-to/setup-aggressive-caching)