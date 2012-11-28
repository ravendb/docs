# Enabling Replication from an Embedded RavenDB instance to a server instance

[RavenDB's replication](../replication) works using plain HTTP requests to replicate all changes in one instance of Raven to another instance of Raven. When running in embedded mode, you can't replicate to that instance (since it has no HTTP endpoint, unless you use [Embedded+HTTP mode](../deployment/embedded) but you can replicate from that instance.

Here are the steps to enable that scenario:

* Ensure that the Raven.Bundles.Replication.dll is located in the Plugins directory for your embedded RavenDB instance.
* Execute the following code for every RavenDB instance that you wish to replicate to:

{CODE-START:csharp /}
    using (var session = store.OpenSession())
    {
        session.Store(new ReplicationDocument
        {
               Destinations = { new ReplicationDestination { Url = "https://ravendb-replica" }, }
        });
        session.SaveChanges();
    }
{CODE-END /}

Now, any changes that you make on the embedded RavenDB instance will be replicated to the remote instance. If you wish to replicate from a remote RavenDB instance to an embedded RavenDB instance:

* Run RavenDB in the [Embedded+HTTP mode](../../deployment/embedded)
* Ensure that the Raven.Bundles.Replication.dll is located in the Plugins directory for your embedded RavenDB instance.

And now you can replicate to the embedded instance, not just from it.