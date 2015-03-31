#How to enable optimistic concurrency?

By default, optimistic concurrency checks are turned **off**, which means that changes made outside our session object will be overwritten if the `SaveChangesAsync` is called.
Checks can be turned on by setting the `UseOptimisticConcurrency` property from the `Advanced` session operations to `true` and may cause the `ConcurrencyExceptions` to be thrown.

## Example I

{CODE optimistic_concurrency_1@FileSystem\ClientApi\Session\Configuration\OptimisticConcurrency.cs /}

The above example shows how to enable optimistic concurrency for a particular session. However, that can be also turned on globally, that is for all opened sessions, by using the `DefaultUseOptimisticConcurrency` convention.

## Example II

{CODE optimistic_concurrency_2@FileSystem\ClientApi\Session\Configuration\OptimisticConcurrency.cs /}

## Example III

You can also force the concurrency check by explicitly passing an `Etag` parameter to the `RegisterXXX` method.

{CODE optimistic_concurrency_3@FileSystem\ClientApi\Session\Configuration\OptimisticConcurrency.cs /}
