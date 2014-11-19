# DTC transactions

{DANGER:Voron does not support DTC}
DTC transactions aren't supported by Voron storage engine. If you want to use DTC feature you need to setup your database to use Esent storage.
{DANGER/}

Sometimes we need multiple calls to `SaveChanges` for one reason or another, but we want those calls to be contained within a single atomic operation. 
RavenDB supports `System.Transactions` for multiple operations against a single database or multiple ones on the same server or even against different servers (distributed transactions).

The client code looks as follow:

{CODE transaction_scope_usage@ClientApi\Session\TransactionSupport\DtcTransactions.cs /}
	
As you can see DTC transaction can happen on multiple requests. If at any point any of this code fails, none of the changes will be committed.

In order to handle such transactions the client sends a transaction identifier and its timeout in `Raven-Transaction-Information` header of HTTP request:

{CODE-START:json /}
POST /bulk_docs HTTP/1.1
Raven-Transaction-Information: 975ee0bf-cac9-4b8e-ba29-377de722f037, 00:01:00
{CODE-END /}

According to the two-phase commit (2PC) implementation of the DTC protocol, a `transaction.Complete()` call requires to perform two HTTP requests.
Phase one called _Prepare_ executes the first request when the actual work is made (but the transaction is not committed yet):

{CODE-START:json /}
	POST /transaction/prepare?tx=975ee0bf-cac9-4b8e-ba29-377de722f037 HTTP/1.1
{CODE-END /}

If _Prepare_ phase succeeded then the actual transaction commit is made by sending the request:

{CODE-START:json /}
	POST /transaction/commit?tx=975ee0bf-cac9-4b8e-ba29-377de722f037 HTTP/1.1
{CODE-END /}

All the intermediate states are durable between requests of the DTC transaction and any document that has been modified is locked for modifications for other transactions. The modifications aren't visible to others until the DTC transaction is committed. Once the transaction is committed, standard transaction rules apply.

{NOTE Although RavenDB supports `System.Transactions`, you should only use DTC transactions if you really need them (for example, to coordinate between multiple transactional resources). There is an additional cost for using `System.Transactions` and distributed transactions over simply using the standard API and the transactional `SaveChanges`. /}

#Transaction storage recovery 

`System.Transactions` allows you to enlist a volatile resource manager or a durable one to participate in a transaction.
Depending on your needs you can decide to use a volatile one and then a transaction's state will be stored in memory or use
a resource manager with a durable enlistment which will be able to perform a recovery if it experiences a failure.

In order to manage where a resource manager persist data the RavenDB client exposes the property `DocumentStore.TransactionRecoveryStorage`.
By default the volatile storage is use, so the resource manager will not be able to recover from a failure to complete a transaction:

{CODE default_transaction_recovery_storage@ClientApi\Session\TransactionSupport\DtcTransactions.cs /}

To obtain a durable storage you can use:

* IsolatedStorageTransactionRecoveryStorage which doesn't require any configuration, it uses the isolated storage to keep data what can intermittently cause failures because files can be in use simultaneously by operating system features such as virus scanners and file indexers,
* LocalDirectoryTransactionRecoveryStorage the recommended option to use, it requires just to specify a local path where data will be persisted.

{CODE local_directory_transaction_recovery_storage@ClientApi\Session\TransactionSupport\DtcTransactions.cs /}