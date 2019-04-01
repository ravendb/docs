# Bundle: Expiration

The expiration bundle serves a very simple purpose: it deletes the documents whose time has passed. Usage scenarios for the Expiration Bundle include storing user sessions in RavenDB or using RavenDB as a cache.

## Usage
You can set the expiration date for a document using the following code:

{CODE expiration1@Server\Bundles\Expiration.cs /}

As you can see, all we need to do is to set the `Raven-Expiration-Date` property in the metadata for the appropriate date, and, at the specified time, the document will be automatically deleted.

{NOTE The date must be UTC, not local time. /}

{WARNING When master-master [replication](../../server/scaling-out/replication/how-replication-works) is set between servers, the Expiration bundle should be turned on **ONLY** on **one** server, otherwise conflicts will occur. /}
