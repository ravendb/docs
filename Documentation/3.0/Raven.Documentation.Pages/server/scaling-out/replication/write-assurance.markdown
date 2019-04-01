# Replication: Write Assurance

RavenDB handles the replication in an asynchronous manner. Saving an entity on a server will cause that an async task which will replicate the newly added document to configured destination servers will be run.
From the code perspective it means that when you call `session.SaveChanges()`, the document is stored on the primary node and it does not wait for the replication to the other ones.

In order to handle write assurance, the client API allows you to wait until replication is completed. Here is the code:

{CODE write_assurance_1@Server\ScalingOut\Replication\WriteAssurance.cs /}

The Raven client will ping all of the replicas, waiting to see if the replication matches or exceeds the ETag that we just wrote. 
You can specify the number of replicas that are required to consider the document write as "safe". Optionally, you can also provide a timeout, and if the nodes aren't reachable, 
you will get an error concerning this. As the result, the `WaitAsync` method returns the number of nodes that caught up to the specified ETag.

You can also use parameterless version of the `WaitAsync` method:

{CODE write_assurance_2@Server\ScalingOut\Replication\WriteAssurance.cs /}

Then the default parameters that will be used are as follows:

* *etag* - last written ETag in the document store (`DocumentStore.LastEtagHolder.GetLastWrittenEtag()`),
* *timeout* - no timeout,
* *database* - default database of the document store,
* *replicas* - 2 destination servers.

## Related articles

- [Studio : **Walkthrough : How to setup replication?**](../../../studio/walkthroughs/how-to-setup-replication)
- [Client API : Replication : How client integrates with replication bundle?](../../../client-api/bundles/how-client-integrates-with-replication-bundle)
