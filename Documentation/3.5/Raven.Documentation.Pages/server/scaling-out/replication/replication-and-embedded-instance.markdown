# Replication: Using Embedded instance

[Replication](../../../server/scaling-out/replication/how-replication-works) works using plain HTTP requests to replicate all changes from one server instance to another. When running in embedded mode, you can't replicate to that instance (since it has no HTTP endpoints), unless you use [Embedded+HTTP mode](../../../server/installation/embedded) but you can replicate from that instance.

## Replicating from embedded instance

To replicate from embedded instance to another HTTP-enabled server instance you need to:

- enable `Replication` bundle on a database e.g. you can create new database with the following code:

{CODE from_embedded_server_1@Server\ScalingOut\Replication\FromEmbeddedServer.cs /}

- setup a replication by creating the `ReplicationDestinations` document with appropriate settings.

{CODE from_embedded_server_2@Server\ScalingOut\Replication\FromEmbeddedServer.cs /}

## Replicating to embedded instance

Replication requires HTTP endpoints on destination server to be running, yet, by default, when  an embedded instance is used, internal HTTP-server is disabled. To turn it on initialize `EmbeddableDocumentStore` with `UseEmbeddedHttpServer` set to `true` (you can read more [here](../../../server/installation/embedded)).

{CODE from_embedded_server_3@Server\ScalingOut\Replication\FromEmbeddedServer.cs /}

Now you can replicate to an embedded instance, not just from it.

## Related articles

- [Studio : **Walkthrough : How to setup replication?**](../../../studio/walkthroughs/how-to-setup-replication)
