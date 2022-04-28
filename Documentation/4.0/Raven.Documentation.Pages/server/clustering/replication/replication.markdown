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

Replication consistency can be achieved using 
[Write Assurance](../../../client-api/session/saving-changes#waiting-for-replication---write-assurance).  
