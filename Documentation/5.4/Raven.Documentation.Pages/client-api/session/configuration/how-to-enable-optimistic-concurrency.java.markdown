# Session: How to Enable Optimistic Concurrency

By default, optimistic concurrency checks are **disabled**. Changes made outside our session object will be overwritten. Concurrent changes to the same document will use
the Last Write Wins strategy so a lost update anomaly is possible with the default configuration of the [session](../../../client-api/session/what-is-a-session-and-how-does-it-work).

You can enable the optimistic concurrency strategy either globally, at the document store level or a per session basis.  
In either case, with optimistic concurrency enabled, RavenDB will generate a concurrency exception (and abort all
modifications in the current transaction) when trying to save a document that has been modified on the server side after the client loaded and modified it.

The `ConcurrencyException` that might be thrown upon the `saveChanges` call, needs to be handled by the caller.  
The operation can be retried (the document needs to be reloaded since it got changed meanwhile) or handle the error in a way that is suitable in a given scenario.

{WARNING: }
Note that `useOptimisticConcurrency` only applies to documents that has been _modified_ by the session. Loading documents `users/1-A` and `users/2-A` in a session, modifying
`users/1-A` and then calling `saveChanges` will succeed, regardless of the optimistic concurrency setting, even if `users/2-A` has changed in the meantime. 
If the session were to try to save to `users/2-A` as well with optimistic concurrency enabled, then an exception will be raised and the updates to both `users/1-A` and `users/2-A`
will be cancelled.
{WARNING/}

You can also control optimistic concurrency per specific document. To enable it, [provide a Change Vector to Store](../../../client-api/session/storing-entities).  
If you do not provide a change vector or if the change vector is `null`, optimistic concurrency will be disabled.  

Setting the 'Change Vector' to an empty string will cause RavenDB to ensure that this document is a new one and doesn't already exists.

Setting optimistic concurrency per specific document overrides the use of the `useOptimisticConcurrency` field from the `advanced` session operations.

{INFO: }
For a detailed description of transactions and concurrency control in RavenDB please refer to the  
[Transaction support in RavenDB](../../../client-api/faq/transaction-support) article.
{INFO/}

## Enabling for a specific Session

{CODE:java optimistic_concurrency_1@ClientApi\Session\Configuration\OptimisticConcurrency.java /}

{WARNING: }

* Enabling optimistic concurrency in a session will ensure that changes made to a document will only be persisted
  if the version of the document sent in the `saveChanges()` call matches its version from the time it was initially read (loaded from the server).

* Note that it's necessary to enable optimistic concurrency for ALL sessions that modify the documents for which you want to guarantee that no writes will be silently discarded.
  If optimistic concurrency is enabled in some sessions but not in others, and they modify the same documents, the risk of the lost update anomaly still exists.

{WARNING/}

## Enabling Globally

The first example shows how to enable optimistic concurrency for a particular session. 
This can be also enabled globally, for all opened sessions by using the convention `store.getConventions().setUseOptimisticConcurrency`.

{CODE:java optimistic_concurrency_2@ClientApi\Session\Configuration\OptimisticConcurrency.java /}

## Disabling Optimistic Concurrency for a Single Document when it is Enabled on Session

Optimistic concurrency can be disabled for a single document by passing `null` as a change vector value to `store` method even when it is enabled for an entire session (or globally).

{CODE:java optimistic_concurrency_3@ClientApi\Session\Configuration\OptimisticConcurrency.java /}

## Enabling Optimistic Concurrency for a New Document when it is Disabled on Session

Optimistic concurrency can be enabled for a new document by passing `""` as a change vector value to `store` method even when it is disabled for an entire session (or globally).
It will cause to throw `ConcurrencyException` if the document already exists.

{CODE:java optimistic_concurrency_4@ClientApi\Session\Configuration\OptimisticConcurrency.java /}

## Related articles

### Configuration

- [Conventions](../../../client-api/configuration/conventions)

### FAQ

- [Transaction Support in RavenDB](../../../client-api/faq/transaction-support)
