# Session : How to enable optimistic concurrency?

By default, optimistic concurrency checks are turned **off**, which means that changes made outside our session object will be overwritten.
Checks can be turned on by setting `UseOptimisticConcurrency` property from `Advanced` session operations to `true` and may cause `ConcurrencyExceptions` to be thrown.

## Example I

{CODE optimistic_concurrency_1@ClientApi\Session\Configuration\OptimisticConcurrency.cs /}

The above example shows how to enable optimistic concurrency for a particular session. However that can be also turned on globally, that is for all opened sessions 
by using the convention `DefaultUseOptimisticConcurrency`.

## Example II

{CODE optimistic_concurrency_2@ClientApi\Session\Configuration\OptimisticConcurrency.cs /}

## Related articles

- [Revisions and Concurrency with E-Tags](../../concurrency/revisions-and-concurrency-with-etags)