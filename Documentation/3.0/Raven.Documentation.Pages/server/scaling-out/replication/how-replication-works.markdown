# Replication

RavenDB supports out of the box replication. To enable it, you have to activate a built-in replication bundle when creating a new database. 

You can read about potential deployment options for the replication bundle here: [mixing Replication and Sharding](../../../server/scaling-out/replication-with-sharding).

Enabling the replication module will have the following effects on your system:

* Track the server where the document was originally written. The replication bundle uses this information to determine if a replicated document is conflicting with the existing document.
* Documents that encountered a conflict will be marked appropriately and require automated or user involvement to resolve.
* Document deletes result in delete markers, which the replication bundle needs in order to be able to replicate deletes to sibling instances. This is an implementation detail and is not noticeable to clients.
* Several new endpoints will begin responding, including `/replication/replicate` and `/replication/lastEtag` among others.
* The replication bundle will not replicate any system documents (whose key starts in Raven/)

In order to setup a replication, you can create the following documents:

* `Raven/Replication/Destinations` - List of servers we need to replicate to
* `Raven/Replication/Sources/[server]` - Information about the data replicated from a particular server

## The destinations document

The destination document is saved with an ID of `Raven/Replication/Destinations`, which tells the RavenDB instance where to replicate to. It's format is as follows:

{CODE-BLOCK:json}
{
    "Destinations": [
		{
			"Url": "http://raven_two:8080/"
		},
		{
			"Url": "http://raven_three:8080/"
		}
    ]
}
{CODE-BLOCK/}

With an object containing a url per each instance to replicate to, whenever this document is updated, replication kicks off and starts replicating to the updates destination list.

## How replication works?

On every transaction commit, Raven will look up the list of replication destinations. For each of the destinations, the replication bundle will:

* Query the remote instance for the last document that we replicated to that instance.
* Start sending batches of updates that happened since the last replication.

Replication happens in the background and in parallel. 

## What about failures?

Because of its design, a node can fail for a long period of time, and yet still come up later and start accepting everything that was missed meanwhile. The Replication Bundle keeps track only of a single data item per each replicating server, the last etag seen from that server. This means that we don't have to worry about missing replication windows.

When the replication bundle encountered a failure when replicating to a server, it has an intelligent error handling strategy. It is meant to be hands off, so nodes can fail and come back up without any administrative intervention. The strategy is outlined below:

* If this is the first failure encountered, immediately try to replicate the same information again. This is done under the assumption that most failures are transient in nature.
* If the second attempt fails as well, the failure is noted (in Raven/Replication/Destinations/[server url])
* After ten consecutive failures, Raven will start replicating to this node less often
* * Once every 10 replication cycles, until failure count reaches 100
* * Once every 100 replication cycles, until failure count reaches 1,000
* * Once every 1,000 replication cycles, when failure count is above 1,000
* Any successful replication will reset the failure count, on the assumption that since we reached the server, it is now healthy again.

## Can I bring up a new node and start replicating to it?
Yes, you can. You can edit the Raven/Replication/Destinations document in the replicating instance to add the new node, and the Replication Bundle will immediately start replicating to that server.

## What about timeouts?
By default, the replication bundle has a timeout of 60 seconds. You can control that by specifying "Raven/Replication/ReplicationRequestTimeout" in the configuration file <appSettings/> section.

The default value assumes servers that are near one another, so for the replication over the WAN you should have more time. Please note that timeout failures for the replication bundle count as failures, and the replication bundle will reduce the number of replication attempts against a node that fails often.

## What happens if there is a conflict?

In a replicating system, it is possible that two writes to the same document will occur on two different servers, resulting in two independent versions of the same document. When the replication occurs between those two versions, the Replication Bundle is faced with a problem. It has two authentic versions of the same entity, saying different things. At that point, the Replication Bundle will mark this document as conflicting, store all the conflicting documents in a safe place, and set the document content to point to the conflicting documents.

Resolving a conflict is easy, you just need to PUT a new version of the document. On PUT, the Replication Bundle will consider the conflict resolved.

More details about conflicts can be found here: [dealing with replication conflicts](../../../server/scaling-out/replication/replication-conflicts).

## Replication & other bundles

{WARNING:Expiration}
If a master-master replication is set up and the [Expiration bundle](../../../server/bundles/expiration) is used on more than one server, then the conflicts will occur. To solve them, use the expiration bundle only on one server.
{WARNING/}

## Related articles

- [Studio : **Walkthrough : How to setup replication?**](../../../studio/walkthroughs/how-to-setup-replication)
- TODO

