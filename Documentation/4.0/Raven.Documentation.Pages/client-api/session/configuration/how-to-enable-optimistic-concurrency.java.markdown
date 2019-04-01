# Session: How to Enable Optimistic Concurrency

By default, optimistic concurrency checks are turned **off**. Changes made outside our session object will be overwritten. Concurrent changes to the same document will use
the Last Write Wins strategy. 

You can enable the optimistic concurrency strategy either globally, at the document store level or a per session basis. 
In either case, with optimistic concurrency enabled, RavenDB will generate a concurrency exception (and abort all 
modifications in the current transaction) when the document has been modified on the server side after the client received and modified it.
You can see the sample code below on the specific on

Note that `useOptimisticConcurrency` only applies to documents that has been _modified_ by the session. Loading documents `users/1-A` and `users/2-A` in a session, modifying
`users/1-A` and then calling `saveChanges` will succeed, regardless of the optimistic concurrency setting, even if `users/2-A` has changed in the meantime. 
If the session were to try to save to `users/2-A` as well with optimistic concurrency turned on, then an exception will be raised and the updates to both `users/1-A` and `users/2-A`
will be cancelled. 

Another option is to control optimistic concurrency per specific document.   
To enable it, you can [supply a Change Vector to Store](../../../client-api/session/storing-entities). If you don't supply a 'Change Vector' or if the 'Change Vector' is null, 
then optimistic concurrency will be disabled. Setting the 'Change Vector' to an empty string will cause RavenDB to ensure that this document is a new one and doesn't already 
exists.

Setting optimistic concurrency per specific document overrides the use of the `useOptimisticConcurrency` field from the `advanced` session operations.

## Enabling for a specific Session

{CODE:java optimistic_concurrency_1@ClientApi\Session\Configuration\OptimisticConcurrency.java /}

## Enabling Globally

The first example shows how to enable optimistic concurrency for a particular session. 
This can be also turned on globally, for all opened sessions by using the convention `store.getConventions().setUseOptimisticConcurrency`.

{CODE:java optimistic_concurrency_2@ClientApi\Session\Configuration\OptimisticConcurrency.java /}

## Turning Off Optimistic Concurrency for a Single Document when it is Enabled on Session

Optimistic concurrency can be turned off for a single document by passing `null` as a change vector value to `store` method even when it is turned on for an entire session (or globally).

{CODE:java optimistic_concurrency_3@ClientApi\Session\Configuration\OptimisticConcurrency.java /}

## Turning On Optimistic Concurrency for a New Document when it is Disabled on Session

Optimistic concurrency can be turned on for a new document by passing `""` as a change vector value to `store` method even when it is turned off for an entire session (or globally).
It will cause to throw `ConcurrencyException` if the document already exists.

{CODE:java optimistic_concurrency_4@ClientApi\Session\Configuration\OptimisticConcurrency.java /}

## Related articles

### Configuration

- [Conventions](../../../client-api/configuration/conventions)
