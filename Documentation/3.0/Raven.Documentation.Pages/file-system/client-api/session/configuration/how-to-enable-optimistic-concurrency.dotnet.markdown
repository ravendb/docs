#How to enable optimistic concurrency?

By default, optimistic concurrency checks are turned **off**, which means that changes made outside our session object will be overwritten if `SaveChangesAsync` is called.
Checks can be turned on by setting `UseOptimisticConcurrency` property from `Advanced` session operations to `true` and may cause `ConcurrencyExceptions` to be thrown.

## Example I

{CODE optimistic_concurrency_1@FileSystem\ClientApi\Session\Configuration\OptimisticConcurrency.cs /}

The above example shows how to enable optimistic concurrency for a particular session. However that can be also turned on globally, that is for all opened sessions 
by using the convention `DefaultUseOptimisticConcurrency`.

## Example II

{CODE optimistic_concurrency_2@FileSystem\ClientApi\Session\Configuration\OptimisticConcurrency.cs /}

## Example III

You can also force to perform concurrency check by explicitly passing `Etag` parameter to `RegisterXXX` method.

{CODE optimistic_concurrency_3@FileSystem\ClientApi\Session\Configuration\OptimisticConcurrency.cs /}
