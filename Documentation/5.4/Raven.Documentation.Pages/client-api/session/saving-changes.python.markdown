# Session: Saving changes

Pending session operations e.g. `store`, `delete` and many others will not be send to the server until `save_changes` is called.

{INFO: }

Whenever you execute `save_changes()` to send a batch of operations like put, update, or delete in a request,  
the server will wrap these operations in a [transaction](../../client-api/faq/transaction-support) upon execution in the database.  

Either all operations will be saved as a single, atomic transaction or none of them will be.  
Once `save_changes()` returns successfully, it is guaranteed that all changes are persisted in the database.  

{INFO/}

##Syntax

{CODE:python saving_changes_1@ClientApi\Session\savingChanges.py /}

###Example

{CODE:python saving_changes_2@ClientApi\Session\savingChanges.py /}


{PANEL:Waiting for Indexes}

You can ask the server to wait until the indexes are caught up with changes made within the current session before the `save_changes` returns.

* You can set a timeout (default: 15 seconds).
* You can specify whether you want to throw on timeout (default: `False`).
* You can specify indexes that you want to wait for. If you don't specify anything here, RavenDB will automatically select just the indexes that are impacted 
by this write.

{CODE:python saving_changes_3@ClientApi\Session\savingChanges.py /}

{PANEL/}

{PANEL:Waiting for Replication - Write Assurance}

Sometimes you might need to ensure that changes made in the session will be replicated to more than one node of the cluster before the `save_changes` returns.
It can be useful if you have some writes that are really important so you want to be sure the stored values will reside on multiple machines. Also it might be necessary to use
when you customize [the read balance behavior](../../client-api/configuration/load-balance/read-balance-behavior) and need to ensure the next request from the user 
will be able to read what he or she just wrote (the next open session might access a different node).

You can ask the server to wait until the replication is caught up with those particular changes.

* You can set a timeout (default: 15 seconds).
* You can specify whether you want to throw on timeout, which may happen in case of network issues (default: `True`).
* You can specify to how many replicas (nodes) the currently saved write must be replicated, before the `save_changes` returns (default: 1).
* You can specify whether the `save_changes` will return only when the current write was replicated to majority of the nodes (default: `False`).

{CODE:python saving_changes_4@ClientApi\Session\savingChanges.py /}

{WARNING:Important}
The `wait_for_replication_after_save_changes` method waits only for replicas which are part of the cluster. It means that external replication destinations are not counted towards the number specified in `replicas` parameter, since they are not part of the cluster.
{WARNING/}

{WARNING:Important}

Even if RavenDB was not able to write your changes to the number of replicas you specified, the data has been already written to some nodes. You will get an error but data is already there.

This is a powerful feature, but you need to be aware of the possible pitfalls of using it.

{WARNING/}

{PANEL/}

{PANEL:Transaction Mode - Cluster Wide}

Setting `TransactionMode` to `TransactionMode.CLUSTER_WIDE` will enable the [Cluster Transactions](../../server/clustering/cluster-transactions) feature.

With this feature enabled the [session](../../client-api/session/what-is-a-session-and-how-does-it-work) will support the following _write_ commands:

- `store`
- `delete`
- `create_compare_exchange_value`
- `update_compare_exchange_value`
- `delete_compare_exchange_value`


Here is an example of creating a unique user with cluster wide.

{CODE:python cluster_store_with_compare_exchange@ClientApi\Session\savingChanges.py /}

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
