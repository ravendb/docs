# Replication: Write Assurance

RavenDB handles the replication in an asynchronous manner. Saving an entity on a server will cause that an async task which will replicate the newly added document to configured destination servers will be run.
From the code perspective it means that when you call `session.SaveChanges()`, the document is stored on the primary node and it does not wait for the replication to the other ones.

In order to handle write assurance, the client API allows you to wait until replication is completed. Here is the code:

{CODE write_assurance_1@Server\ScalingOut\Replication\WriteAssurance.cs /}

The Raven client will send a request to the server and wait until response, the server then will check if the replication matches or exceeds the ETag that we just wrote.   
You can specify the number of replicas that are required to consider the document write as "safe" (The replicas won't be bigger then the replication destinations). 
Optionally, you can also provide a timeout, and if the server failed to check all the destinations or they are not rechable, you will get an error concerning this.

You can also use parameterless version of the `WaitAsync` method:

{CODE write_assurance_2@Server\ScalingOut\Replication\WriteAssurance.cs /}

Then the default parameters that will be used are as follows:

* *etag* - last written ETag in the document store (`DocumentStore.LastEtagHolder.GetLastWrittenEtag()`),
* *timeout* - 60 seconds,
* *database* - default database of the document store,
* *replicas* - 2 additional servers.

Another option to handle write assurance is to use the `WaitForReplicationAfterSaveChanges` this option will add a flag to the save changes request and make the server wait for the document to be confirmed write to at least N replicas or until timeout before sending any response .

To enable this option we need to add this method just before the session.saveChanges():

{CODE write_assurance_4@Server\ScalingOut\Replication\WriteAssurance.cs /}

Like the WaitAsync You can also use parameterless version to the method:

{CODE write_assurance_3@Server\ScalingOut\Replication\WriteAssurance.cs /}

Then the default parameters that will be used are as follows:

* *timeout* - 15 seconds,
* *throwOnTimeout* - true (set to false only if you dont want to get any exception on timeout),
* *replicas* - 1 additional server
## Related articles

- [Studio : **Walkthrough : How to setup replication?**](../../../studio/walkthroughs/how-to-setup-replication)
- [Client API : Replication : How client integrates with replication bundle?](../../../client-api/bundles/how-client-integrates-with-replication-bundle)
