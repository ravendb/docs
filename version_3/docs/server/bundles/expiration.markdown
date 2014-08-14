# Bundle: Expiration

The expiration bundle serves a very simple purpose, it deletes documents whose time have passed. Usage scenarios for the Expiration Bundle include storing user sessions in RavenDB or using RavenDB as a cache.

## Usage
You can set the expiration date for a document using the following code:

{CODE expiration1@Server\Bundles\Expiration.cs /}

As you can see, all we need to do is set the `Raven-Expiration-Date` property on the metadata for the appropriate date. And at the specified time, the document will automatically be deleted.

{NOTE The date must be UTC, not local time. /}

{WARNING When master-master [replication](../../scaling-out/replication) is set between servers then the Expiration bundle should be turned on **ONLY** on **one** server, otherwise conflicts will occur. /}

#### Related articles

TODO