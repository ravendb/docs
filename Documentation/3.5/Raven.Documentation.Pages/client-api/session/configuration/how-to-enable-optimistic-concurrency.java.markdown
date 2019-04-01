# Session: How to enable optimistic concurrency?

By default, optimistic concurrency checks are turned **off**, which means that changes made outside our session object will be overwritten.
Checks can be turned on by setting `setUseOptimisticConcurrency` method from `advanced` session operations to `true` and may cause `ConcurrencyExceptions` to be thrown.


## Example I

{CODE:java optimistic_concurrency_1@ClientApi\Session\Configuration\OptimisticConcurrency.java /}

The above example shows how to enable optimistic concurrency for a particular session. However that can be also turned on globally, that is for all opened sessions 
by using the convention `setDefaultUseOptimisticConcurrency`.

## Example II

{CODE:java optimistic_concurrency_2@ClientApi\Session\Configuration\OptimisticConcurrency.java /}

## Remarks

{INFO:Optimistic Concurrency and Replication}

When Optimistic Concurrency is used with Replication then we advise to set `forceReadFromMaster` when only read operations are load-balanced (FailoverBehavior.ReadFromAllServers) and you want to perform document update.

{CODE:java optimistic_concurrency_3@ClientApi\Session\Configuration\OptimisticConcurrency.java /}

{INFO/}

## Related articles

- [Revisions and Concurrency with E-Tags](../../concurrency/revisions-and-concurrency-with-etags)



