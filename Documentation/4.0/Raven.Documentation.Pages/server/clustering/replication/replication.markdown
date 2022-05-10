# Replication

Replication in RavenDB is the process of transferring data from one database to another.  

{INFO: The transferred entities are:}

  * Documents 
  * Revisions 
  * Attachments 
  * Conflicts  

{INFO/}

## How does Replication works

* The replication process sends data over a TCP connection by the modification order, from the oldest to the newest.   
* Every database has an [ETag](../../../glossary/etag), which is incremented on _every_ modification in the database's storage.   
* Each replication process has a _source_ , _destination_ and a last confirmed `ETag` which is the cursor to where the replication process is.   
* The data is sent in batches. When the _destination_ confirms getting the data, the last accepted `ETag` is then advanced and the next batch of data is sent. 

{NOTE: Replication Failure} 
In case of failure it will re-start with the [Initial Handshake Procedure](../../../server/clustering/replication/replication#replication-handshake-procedure), which will make sure we will start replicating from the last accepted `ETag`.
{NOTE/}

## Replication Transaction Boundary

The boundary of a transaction is extended across multiple nodes.  
If there are several documents in the same transaction they will be sent in the same replication 
batch to keep the data consistent.  

However this doesn't always ensure the data consistency, since the same document can be modified in a different 
transaction and be sent in a different batch.  

#### Replication consistency can be achieved by -  

* Using [Write Assurance](../../../client-api/session/saving-changes#waiting-for-replication---write-assurance).  
* Enabling [Revisions](../../../server/extensions/revisions).  
  When documents that own revisions are replicated, their revisions will be replicated with them.  
  
     {INFO: Let's see how the replication of revisions helps data consistency.}
     Consider a scenario in which two documents, `Users/1` and `Users/2`, 
     are **created in the same transaction**, and then `Users/2` is **modified 
     in a different transaction**.  
     
     * **How will `Users/1` and `Users/2` be replicated?**  
       When RavenDB creates replication batches, it keeps the 
       [transaction boundary](../../../server/clustering/replication/replication#replication-transaction-boundary) 
       by always sending documents that were modified in the same transaction, 
       **in the same batch**.  
       In our scenario, however, `Users/2` was modified after its creation, it 
       is now recognized by its Etag as a part of a different transaction than 
       that of `Users/1`, and the two documents may be replicated in two different 
       batches, `Users/1` first and `Users/2` later.  
       If this happens, `Users/1` will be replicated to the destination without `Users/2` 
       though they were created in the same transaction, causing a data inconsistency that 
       will persist until the arrival of `Users/2`.  
     
     * **The scenario will be different if revisions are enabled.**  
       In this case the creation of `Users/1` and `Users/2` will also create revisions 
       for them both. These revisions will continue to carry the Etag given to them 
       at their creation, and will be replicated in the same batch.  
       When the batch arrives at the destination, data consistency will be kept:  
       `Users/1` will be stored, and so will the `Users/2` revision, that will become 
       a live `Users/2` document.  

     {INFO/}
