# Pull Replication

The pull replication feature allow an inter-cluster database replication to be initiated from the *destination*.

There are two roles:

1. The **Hub**, a database from which the data will be pulled from.
2. The **Sink**, a database that will initiate the connection and receive the data from the Hub.

## Why Pull Replication

Creating a Hub-Sink definition pair only once and pass the Sink definition to RavenDB instances that have to be synced by the Hub, will simplify deployment scenarios where RavenDB instances work independently but have to be in sync with the main server.  

For example 
- Ships at sea that need to sync with the head quarters when docked.
- Clinics in health care providers that need to communicate with the central database.

{INFO: Configure the Pull Replication via the Studio}

Checkout the studio [walk-through](../../studio/database/tasks/ongoing-tasks/pull-replication) for configuring the pull replication.

{INFO/}

## Pull Replication Work-flow 

1. Define Hub on the _source_ database group.
2. Define or import Sink definition to the _destination_ database group.
3. The Sink will fetch the source topology and initiate the request to the Hub.
4. The Hub will accept the connection and start sending the data just like a usual [Replication](../../server/ongoing-tasks/extrnal-replication).
5. On a connection breakdown the Sink will be responsible to reconnect. 

## Using Certificates

In a secured environment, an unsecured pull replication isn't allowed.

The certificate is defined on the Hub and shard with the Sink. 
It is used for both authorization and authentication, but allow access only to this feature and only for the specified database.
Although it is defined on the Hub it must include the _private_ key when imported into the Sink, since the Sink initiate the connection.  

The certificate could be a self-signed or signed by any other (even untrusted) CA, since RavenDB trusts the pull replication certificate based on its thumbprint.

{WARNING: Don't expose you database record}

The entire certificate with its private key is kept on the Sink database record.

{WARNING/}

## Failover

Since the destination (Sink) initiate the replication, it is also the Sink's responsibility to reconnect on network failure. 

### Failure on the Hub
As a part of the connection handshake the Sink will fetch an ordered list of nodes from the Hub cluster (if defined, the preferred node will be at the top of it). It will try to connect to the first node and if failure occurred on the Hub side, the Sink will try to re-establish the connection to the next node in the list, if all fail, the Sink will request to fetch the list again. 

### Failure in the Sink
If the failure occurred on the Sink node, the Sink cluster will select a different node to perform the job.

## Related Articles

[External Replication](../../server/ongoing-tasks/extrnal-replication)

Relevant blog posts @ ayende.com  
[Pull replication & edge processing](https://ayende.com/blog/185153-C/ravendb-4-2-features-pull-replication-edge-processing)  
[Pull replication has landed](https://ayende.com/blog/186177-A/ravendb-4-2-features-pull-replication-has-landed)
