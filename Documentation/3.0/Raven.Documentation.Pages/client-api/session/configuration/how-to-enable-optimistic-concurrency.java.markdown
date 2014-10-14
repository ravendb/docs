# Session : How to enable optimistic concurrency?

By default, optimistic concurrency checks are turned **off**, which means that changes made outside our session object will be overwritten.
Checks can be turned on by setting `setUseOptimisticConcurrency` method from `advanced` session operations to `true` and may cause `ConcurrencyExceptions` to be thrown.

## Example

{CODE:java optimistic_concurrency_1@ClientApi\Session\Configuration\OptimisticConcurrency.java /}

## Related articles

TODO