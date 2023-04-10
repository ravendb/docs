# Sharding: Resharding
---

{NOTE: }

* **Resharding** is the relocation of data stored on one [shard](../sharding/overview#shards) 
  to another shard, to maintain an overall balanced database in which all 
  shards handle about the same volume of data.  
* The resharding process moves all the data related to a certain [bucket](../sharding/overview#buckets) 
  (including documents, document extensions, tombstones, etc.), to a different 
  shard, and then associates the bucket with the new shard.  
* An even distribution of data and workload between all shards maintains 
  a steadier overall usage of resources like disk space, memory, and bandwidth, 
  improves availability, and eases database management.  
* Resharding can currently be initiated only manually, via Studio.  
  A user can reshard a range of buckets as well as a single bucket.  
* When resharding is initiated, RavenDB implements it gradually, one bucket 
  at a time, to avoid resource overuse.  

* In this page:  
  * [Resharding](../sharding/resharding#resharding)  
  * [The Resharding Process](../sharding/resharding#the-resharding-process)  
  * [Change Vector on a Sharded Database](../sharding/resharding#change-vector-on-a-sharded-database)  
  * [Resharding and Other Features](../sharding/resharding#resharding-and-other-features)  
  * [Execute Resharding](../sharding/resharding#execute-resharding)  

{NOTE/}

---

{PANEL: Resharding}

Over time, data may be distributed unevenly between the database's shards, until 
some shards may host and handle a much bigger portion of the overall load than others.  

Resharding is the process of re-distributing stored data between the shards.  

Keeping about the same amount of data on all shards helps maintain an equal 
level of resource usage and improves the database's overall availability and 
querying speed.  

**Resharding is currently manual**.  
Resharding can currently be initiated manually, via Studio. 
RavenDB provides a comfortable resharding interface and does alert 
its users when disk space and other resources are exhausted, but it 
is up to you to commence the resharding process when it is needed.  

**Resharding is carried out one bucket at a time**.  
The smallest unit that can be resharded is a single bucket with all its contents.  
It is also possible to reshard ranges of buckets, but resharding is always done 
one bucket at a time to keep the process light, avoiding any additional burden to 
shards that may already be preoccupied.  

## The Resharding Process

Let's follow the resharding of a range of buckets from shard #0 to shard #2, 
using a database like the one shown below:  

!["Sharded Database"](images/resharding_sharded-database.png "Sharded Database")

1. The client requests resharding buckets from shard #0 to shard #2.  
2. Shard #0 connects shard #2 and transfers to it all the content of 
   the first bucket.  
3. Shard #0 remains the owner of the bucket until:  
    * All bucket content is transferred to [all replicas](../sharding/overview#shard-replication) 
      of shard #2.  
    * Shard #2 replies: `Ownership Transferred`.  
      At this point, the transfer of the next bucket can start since 
      no files are being transferred.  
    * All bucket content is deleted from all replicas of shard #0.  
      The bucket is **not** remapped to shard #2 until all of its files 
      have been removed from the original shard. Until then, new content 
      (document modification, new revision, time series update..) continues 
      to be stored in the original shard and bucket. 
      When the original bucket is cleared, the bucket is remapped 
      to shard #2 and document modifications start to be routed there.  

* Learn [below](../sharding/resharding#execute-resharding) 
  how to reshard buckets using Studio.  
* The resharding of some documents, e.g. of ones that include large 
  attachments or time series, may take a long time. The process is 
  reported by Studio, and can also be seen by opening the Database 
  Record:  

    !["Database Record"](images/resharding_database-record.png "Database Record")

    **1** Click to open the Settings  
    **2** Click to view or edit the [database record](../studio/database/settings/database-record)  

{NOTE: Racing}
It may happen that a file would find its way into a bucket whose 
ownership has already shifted to another shard. RavenDB runs an ongoing 
**periodic documents migrator** routine that constantly checks the 
system for such occurrences, and If any is found a new resharding process 
is automatically initiated for this bucket.  
{NOTE/}

{PANEL/}

{PANEL: Change Vector on a Sharded Database}

On a non-sharded database, a document's [change vector](../server/clustering/replication/change-vector) 
indicates both the document's **version** (so we can tell which version of it 
to replicate, for example) and its **order** on the database (so we can tell, 
for example, whether to replicate or skip it).  

On a sharded database, the latter (order) property may turn meaningless, because 
resharding may change the order of documents: an old document may be moved to a shard 
that contains newer documents, and get a change vector newer than theirs.  

To overcome this problem, resharded documents receive an altered change vector 
that explicitly defines both its version and its order, using this format:  
`<document name> <order>|<version>`

* E.g. `Users/1 A:3|B:7`  
  In the example above:
    * `Users/1` - the document  
    * `A:3` - the change vector part that indicates the document's **order**  
    * `|` - the pipe symbol  
    * `B:7` - the change vector part that indicates the document's **version**  

{PANEL/}

{PANEL: Resharding and Other Features}

### Resharding and External Replication

During [external replication](../sharding/external-replication), 
we check **only the version part** of the document's change vector.  

If a replicated document exists on the destination side:  
To decide whether to replicate the document, we compare the source 
document's **version** (right) part of the change vector with the 
change vector of the destination document.  

The **Order** part of the change vector is not used.  

---

### Resharding and ETL

[ETL tasks](../sharding/etl) cannot determine whether a document that 
was resharded does or doesn't exist on their target.  
Therefore ETL tasks consider **all** resharded documents (that match the 
transform script) new/modified and transfer them all to the destination.  

---

### Resharding and Data Subscriptions

Our promise to [data subscription](../sharding/subscriptions) workers is that we 
send all data **at least once**.  
We also do our best not to send documents twice, but it is the responsibility of 
the worker to check whether a document is duplicated or not.  

---

### Resharding and Querying

Though resharding large documents may duplicate these documents 
for a long time until the transfer and the deletion of the source 
document are complete, [Querying](../sharding/querying) them during 
resharding will **not** produce duplicate results as RavenDB filters 
out duplications due to resharding.  

{PANEL/}

{PANEL: Execute Resharding}

To reshard selected buckets, open the **Stats** view.

!["Stats View"](images/resharding_stats.png "Stats View")

---

Open the **Buckets Report** view.  

!["Buckets Report"](images/resharding_buckets-report.png "Buckets Report")

---

Selecting a range of buckets will present the selected range.  
You can reshard a whole range of buckets, or continue increasing the 
resolution until you present a single bucket and the files it contains.  

!["Range of Buckets"](images/resharding_diving-into-bucket-01.png "Range of Buckets")

!["Single Bucket"](images/resharding_diving-into-bucket-05.png "Single Bucket")

---

Select the shard you want to transfer the bucket/s to and confirm the transfer  

!["Reshard"](images/resharding_diving-into-bucket-06.png "Reshard")

!["Confirm Resharding"](images/resharding_confirm-resharding.png "Confirm Resharding")

---

Resharding has ended.  

!["Finished Resharding"](images/resharding_finished-resharding.png "Finished Resharding")

---

For a while, as long as there are still files to transfer to the new 
shard's replicas or delete from the old shard, transferred buckets are 
presented as if they reside on both their old and new shards.  

!["Document On Two Shards"](images/resharding_over-two-shards.png "Document On Two Shards")

---

Eventually, the bucket/s reside on their new shard.  

!["Post Resharding"](images/resharding_post-resharding.png "Post Resharding")

{PANEL/}

## Related articles

**Sharding**  
[Shards](../sharding/overview#shards)  
[Buckets](../sharding/overview#buckets)  
[Replicas](../sharding/overview#shard-replication)  
[External Replication](../sharding/external-replication)  
[ETL Tasks](../sharding/etl)  
[Data Subscription](../sharding/subscriptions)  
[Querying](../sharding/querying)  

**Server**  
[Change Vector](../server/clustering/replication/change-vector)  
