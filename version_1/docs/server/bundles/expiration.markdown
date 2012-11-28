#Expiration Bundle
The expiration bundle serves a very simple purpose, it deletes documents whose time have passed. Usage scenarios for the Expiration Bundle include storing user sessions in RavenDB or using RavenDB as a cache.

##Installation
Simply place the Raven.Bundles.Expiration.dll in the Plugins directory.

##Usage
You can set the expiration date for a document using the following code:

{CODE expiration1@Server\Bundles.cs /}

As you can see, all we need to do is set the Raven-Expiraton-Date property on the metadata for the appropriate date. And at the specified time, the document will automatically be deleted.

**Note:** The date must be UTC, not local time.