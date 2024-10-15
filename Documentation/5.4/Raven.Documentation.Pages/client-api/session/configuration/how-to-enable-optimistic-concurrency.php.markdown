# How to Enable Optimistic Concurrency
---

{NOTE: }

* By default, optimistic concurrency checks are **disabled**. Changes made outside of the session object will be overwritten. 
  Concurrent changes to the same document will use the _Last Write Wins_ strategy so a lost update anomaly is possible 
  with the default configuration of the [session](../../../client-api/session/what-is-a-session-and-how-does-it-work).

* Optimistic concurrency can be **enabled** for:
   * A specific document  
   * A specific session (enable on a per-session basis)  
   * All sessions (enable globally, at the document store level)  

* With optimistic concurrency enabled, RavenDB will generate a concurrency exception (and abort all modifications in 
  the current transaction) when trying to save a document that has been modified on the server side after the client 
  loaded and modified it.

* The `ConcurrencyException` that might be thrown upon the `saveChanges` call needs to be handled by the caller. 
  The operation can be retried (the document needs to be reloaded since it got changed meanwhile) or handle the error 
  in a way that is suitable in a given scenario.

* In this page:
  * [Enable for specific session](../../../client-api/session/configuration/how-to-enable-optimistic-concurrency#enable-for-specific-session)
  * [Enable globally](../../../client-api/session/configuration/how-to-enable-optimistic-concurrency#enable-globally)
  * [Disable for specific document (when enabled on session)](../../../client-api/session/configuration/how-to-enable-optimistic-concurrency#disable-for-specific-document-(when-enabled-on-session))
  * [Enable for specific document (when disabled on session)](../../../client-api/session/configuration/how-to-enable-optimistic-concurrency#enable-for-specific-document-(when-disabled-on-session)) 

{WARNING: }

* Note that the `UseOptimisticConcurrency` setting only applies to documents that have been modified by the current session. 
  E.g., if you load documents `users/1-A` and `users/2-A` in a session, make modifications only to `users/1-A`, and then call `saveChanges`, 
  the operation will succeed regardless of the optimistic concurrency setting, even if `users/2-A` has been changed by another process in the meantime.

* However, if you modify both documents and attempt to save changes with optimistic concurrency enabled, an exception will be raised 
  if `users/2-A` has been modified externally.  
  In this case, the updates to both `users/1-A` and `users/2-A` will be cancelled.

{WARNING/}

{INFO: }

A detailed description of transactions and concurrency control in RavenDB is available here: 
[Transaction support in RavenDB](../../../client-api/faq/transaction-support)

{INFO/}

{NOTE/}

---

{PANEL: Enable for specific session}

Enable optimistic concurrency for a session using the advanced session `setUseOptimisticConcurrency` method.

{CODE:php optimistic_concurrency_1@ClientApi\Session\Configuration\OptimisticConcurrency.php /}

{WARNING: }

* Enabling optimistic concurrency in a session will ensure that changes made to a document will only be persisted 
  if the version of the document sent in the `saveChanges` call matches its version from the time it was initially 
  read (loaded from the server).
 
* Note that it's necessary to enable optimistic concurrency for ALL sessions that modify the documents for 
  which you want to guarantee that no writes will be silently discarded.  
  If optimistic concurrency is enabled in some sessions but not in others, and they modify the same documents, 
  the risk of the lost update anomaly still exists.

{WARNING/}

{PANEL/}

{PANEL: Enable globally}

* Optimistic concurrency can also be enabled for all sessions that are opened under a document store.

* Use the store `setUseOptimisticConcurrency` method to enable globally.

{CODE:php optimistic_concurrency_2@ClientApi\Session\Configuration\OptimisticConcurrency.php /}

{PANEL/}

{PANEL: Disable for specific document (when enabled on session)}

* Optimistic concurrency can be _disabled_ when **storing** a specific document,  
  even when it is _enabled_ for an entire session (or globally).

* This is done by passing `None` as a change vector value to the [store](../../../client-api/session/storing-entities) method.

{CODE:php optimistic_concurrency_3@ClientApi\Session\Configuration\OptimisticConcurrency.php /}

{PANEL/}

{PANEL: Enable for specific document (when disabled on session)}

* Optimistic concurrency can be _enabled_ when **storing** a specific document,  
  even when it is _disabled_ for an entire session (or globally).

* This is done by passing an empty string as the change vector value to the [store](../../../client-api/session/storing-entities) method.  
  Setting the change vector to an empty string will cause RavenDB to ensure that this document is a new one and doesn't already exist.  
  A `ConcurrencyException` will be thrown if the document already exists. 

* If you do not provide a change vector or if the change vector is `None`, optimistic concurrency will be disabled.  

* Setting optimistic concurrency for a specific document overrides the advanced session `setUseOptimisticConcurrency` operation.  

{CODE:php optimistic_concurrency_4@ClientApi\Session\Configuration\OptimisticConcurrency.php /}

{PANEL/}

## Related articles

### Configuration

- [Conventions](../../../client-api/configuration/conventions)

### FAQ

- [Transaction Support in RavenDB](../../../client-api/faq/transaction-support)
