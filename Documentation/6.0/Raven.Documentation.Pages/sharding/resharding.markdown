# Sharding: Resharding
---

{NOTE: }

* **Resharding** is the relocation of data placed on one shard, on 
  another shard, to maintain a balanced database in which all shards 
  handle about the same volume of data.  
* The resharding process moves all the data related to a certain 
  bucket, including documents, document extensions, tombstones, etc., 
  to a different shard, and then associates the bucket with the new shard.  
* Distributing data and work load evenly between all shards keeps resources 
  usage at bay, and maintains a better availability of the database as a whole.  
* Resharding is currently performed manually and is available via Studio.  

* In this page:  
  * [Resharding](../)  
      [Resharding and other features](../)  
  * [Manual Resharding via Studio](../)  
  * [](../)  
  * [](../)  
  * [](../)  
  * [](../)  
  * [](../)  
  * [](../)  

{NOTE/}

---

{PANEL: Resharing}

{PANEL/}

{PANEL: Manual Resharding Via Studio}

!["Document View"](images/resharding_document-view.png "Document View")

!["Stats View"](images/resharding_stats.png "Stats View")

!["Buckets Report"](images/resharding_buckets-report.png "Buckets Report")

!["Exploring Buckets 1"](images/resharding_diving-into-bucket-01.png "Exploring Buckets 1")

!["Exploring Buckets 2"](images/resharding_diving-into-bucket-02.png "Exploring Buckets 2")

!["Exploring Buckets 3"](images/resharding_diving-into-bucket-03.png "Exploring Buckets 3")

!["Exploring Buckets 4"](images/resharding_diving-into-bucket-04.png "Exploring Buckets 4")

!["Exploring Buckets 5"](images/resharding_diving-into-bucket-05.png "Exploring Buckets 15")

!["Reshard"](images/resharding_diving-into-bucket-06.png "Reshard")

!["Confirm Resharding"](images/resharding_confirm-resharding.png "Confirm Resharding")

!["Finished Resharding"](images/resharding_finished-resharding.png "Finished Resharding")

!["Document On Two Shards"](images/resharding_over-two-shards.png "Document On Two Shards")

!["Post Resharding"](images/resharding_post-resharding.png "Post Resharding")

{PANEL/}

## Related articles

**AAA**  
[BBB](../)  
[BBB](../)  

