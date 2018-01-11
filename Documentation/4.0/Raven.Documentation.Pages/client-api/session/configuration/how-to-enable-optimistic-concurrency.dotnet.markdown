# Session : How to enable optimistic concurrency?

By default, optimistic concurrency checks are turned **off**, which means that changes made outside our session object will be overwritten.
Checks can be turned on by setting `UseOptimisticConcurrency` property from `Advanced` session operations to `true` and may cause `ConcurrencyExceptions` to be thrown.   

Another option is to control optimistic concurrency per specific document.   
To enable it, you can [supply a Change Vector to Store](../storing-entities). If you don't supply a 'Change Vector' or if the 'Change Vector' is null, then optimistic concurrency will be disabled. 
Note that setting optimistic concurrency per specific document overrides the use of `UseOptimisticConcurrency` property from `Advanced` session operations.

## Enabling per Session

{CODE optimistic_concurrency_1@ClientApi\Session\Configuration\OptimisticConcurrency.cs /}

## Enabling globally

The first example shows how to enable optimistic concurrency for a particular session. However that can be also turned on globally, that is for all opened sessions 
by using the convention `store.Conventions.UseOptimisticConcurrency`.

{CODE optimistic_concurrency_2@ClientApi\Session\Configuration\OptimisticConcurrency.cs /}

## Turning off optimistic concurrency for a single document when it is enabled on Session

Optimistic concurrency can be turned off for a single document by passing `null` as a change vector value to `Store` method even when it is turned on for an entire session (or globally).

{CODE optimistic_concurrency_3@ClientApi\Session\Configuration\OptimisticConcurrency.cs /}
