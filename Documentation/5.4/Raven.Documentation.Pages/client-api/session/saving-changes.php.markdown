# Session: Saving changes

Pending session operations like `store`, `delete`, and many others, will not be sent to the server until `saveChanges` is called.

{INFO: }

When `saveChanges()` is applied to send a batch of operations (e.g. `put`, `update`, and `delete`) 
in a request, the server will wrap these operations in a [transaction](../../client-api/faq/transaction-support) 
upon execution in the database.  

Either all operations are performed as a single, atomic transaction, or none of them are.  
Once `saveChanges()` returns successfully, it is guaranteed that all changes are persisted in the database.  

{INFO/}

##Syntax

{CODE:php saving_changes_1@ClientApi\Session\SavingChanges.php /}

###Example

{CODE:php saving_changes_2@ClientApi\Session\SavingChanges.php /}

{PANEL:Waiting for Indexes}

You can request the server to wait until the indexes are caught up with changes made within 
the current session before `saveChanges` returns.

* You can set a timeout (default: 15 seconds).
* You can specify whether you want to throw on timeout (default: `false`).
* You can specify indexes that you want to wait for. If you don't specify anything here, RavenDB will automatically select just the indexes that are impacted 
by this write.

{CODE:php saving_changes_3@ClientApi\Session\SavingChanges.php /}

{PANEL/}

{PANEL:Transaction Mode - Cluster Wide}

Setting `TransactionMode` to `TransactionMode.clusterWide` will enable the [Cluster Transactions](../../server/clustering/cluster-transactions) feature.

With this feature enabled the [session](../../client-api/session/what-is-a-session-and-how-does-it-work) will support the following _write_ commands:

- `store`
- `delete`
- `createCompareExchangeValue`
- `updateCompareExchangeValue`
- `deleteCompareExchangeValue`

Here is an example of creating a unique user with cluster wide.

{CODE:php cluster_store_with_compare_exchange@ClientApi\Session\SavingChanges.php /}

{PANEL/}

## Related Articles

### Transactions

- [Transaction Support](../../client-api/faq/transaction-support)
- [Cluster Transactions](../../server/clustering/cluster-transactions)

### Session

- [What is a Session and How Does it Work](../../client-api/session/what-is-a-session-and-how-does-it-work) 
- [Opening a Session](../../client-api/session/opening-a-session)
- [Deleting Entities](../../client-api/session/deleting-entities)
- [Loading Entities](../../client-api/session/loading-entities)

### Querying

- [Query Overview](../../client-api/session/querying/how-to-query)

### Document Store

- [What is a Document Store](../../client-api/what-is-a-document-store)
