# Session : How to Enable Optimistic Concurrency

By default, optimistic concurrency checks are turned **off**. Changes made outside our session object will be overwritten. 

Checks can be turned on by setting the `UseOptimisticConcurrency` property from the `Advanced` session operations to `true`, and may cause `ConcurrencyExceptions` to be thrown.   

Another option is to control optimistic concurrency per specific document.   

To enable it, you can [supply a Change Vector to Store](../storing-entities). If you don't supply a 'Change Vector' or if the 'Change Vector' is null, then optimistic concurrency will be disabled. 

Setting optimistic concurrency per specific document overrides the use of the `UseOptimisticConcurrency` property from the `Advanced` session operations.

## Enabling per Session

{CODE optimistic_concurrency_1@ClientApi\Session\Configuration\OptimisticConcurrency.cs /}

## Enabling Globally

The first example shows how to enable optimistic concurrency for a particular session. This can be also turned on globally, for all opened sessions by using the convention `store.Conventions.UseOptimisticConcurrency`.

{CODE optimistic_concurrency_2@ClientApi\Session\Configuration\OptimisticConcurrency.cs /}

## Turning Off Optimistic Concurrency for a Single Document when it is Enabled on Session

Optimistic concurrency can be turned off for a single document by passing `null` as a change vector value to `Store` method even when it is turned on for an entire session (or globally).

{CODE optimistic_concurrency_3@ClientApi\Session\Configuration\OptimisticConcurrency.cs /}

## Turning On Optimistic Concurrency for a New Document when it is Disabled on Session

Optimistic concurrency can be turned on for a new document by passing `string.Empty` as a change vector value to `Store` method even when it is turned off for an entire session (or globally).
It will cause to throw `ConcurrencyException` if the document already exists.

{CODE optimistic_concurrency_4@ClientApi\Session\Configuration\OptimisticConcurrency.cs /}
