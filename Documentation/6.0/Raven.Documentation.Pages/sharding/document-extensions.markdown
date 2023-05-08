# Sharding: Document Extensions
---

{NOTE: }

* [Document extensions](../document-extensions/overview-extensions) are 
  data entities that are associated with documents. They currently include 
  [Counters](../document-extensions/counters/overview), 
  [Attachments](../document-extensions/attachments/what-are-attachments), 
  [Time Series](../document-extensions/timeseries/overview), 
  and [Revisions](../document-extensions/revisions/overview).  

* From a user's point of view, document extensions behave similarly 
  under sharded and non-sharded databases and are handled using the 
  same API commands and Studio views.  

* Document extensions are identified by the ID of their parent document, 
  and are always stored in [the same bucket](../sharding/overview#document-extensions-storage) 
  as the document.  

     When a document is resharded, its document extensions are transferred 
     along with it to the new shard.  

* In this page:  
   * [Document Extensions and Resharding](../sharding/document-extensions#document-extensions-and-resharding)  
   * [Precautions and Recommendations](../sharding/document-extensions#precautions-and-recommendations)  
       * [Precautions](../sharding/document-extensions#precautions)  
       * [Recommendations](../sharding/document-extensions#recommendations)  

{NOTE/}

---

{PANEL: Document Extensions and Resharding}

When RavenDB runs resharding to balance the data load between shards, it copies 
documents from one shard to another and then removes their original copy.  

If a change occurs in the original data after some of it was copied to its 
new location (e.g. a time series has updated entries in the original bucket after 
its parent document was copied), RavenDB will **not** remove the original 
document and its extensions from their original location until the new/modified 
data is relocated as well.  

In all other respects, we handle the original document as if it had already moved, 
including reading from and writing to only the new document in its new bucket and shard.

{PANEL/}

{PANEL: Precautions and Recommendations}

## Precautions

The main contribution of sharded databases is their ability to manage huge 
volumes of data efficiently by serving it from multiple shards.  
We should take extra care, then, to help the database maintain its ability 
to divide the data between shards.  
The following points relate to this issue.  

* **Time Series**  
  Some time series can get very large. As they reside in a single bucket 
  with their parent document, they cannot be spread between shards and may 
  become hard to manage and use.  
  We recommend keeping the number of time series added to each document 
  fairly small, and using practices such as [rollup and retention](../document-extensions/timeseries/rollup-and-retention).  

* **Revisions**  
  Revisions may accumulate in a large database, especially in an 
  environment of rapid document modification. We can, however, create 
  a [revisions configuration](../document-extensions/revisions/overview#revisions-configuration) 
  that would take this into account, limit revisions quantity by number 
  and age, and automatically remove those that are no longer needed.  

* **Attachments**  
  Remain aware of the size and amount of attachments in your database as well, 
  and try to avoid adding many or oversized attachments to the same document, 
  especially as a recurring method.  
    
* **Counters**  
  Counters are tiny entities, that weigh much less on the system than 
  time series, revisions, or attachments. It is, however, recommended to 
  keep an eye on them as well, to make sure they are not used in quantities 
  that do pose a problem.  

## Recommendations

* While planning our data model, we should prefer **a larger amount of smaller 
  documents** over **a smaller amount of heavier documents** that are harder to 
  relocate and balance.  

* As explained above, we should limit the size and amount of document extensions 
  and spread them among many documents. Where possible, we can use wise features 
  like time series rollups to summarize a large amount of data using a tiny amount 
  of space.  

* When documents and their extensions are queried and/or included, it takes longer 
  to retrieve documents and extensions when they are stored on different shards.  
  To accelerate these operations, we can place related documents 
  [in the same bucket](../sharding/overview#pairing-documents) 
  in advance.  

{PANEL/}

## Related articles

### Counters
- [Counters](../document-extensions/counters/overview)
- [Indexing with Counters](../document-extensions/counters/indexing)
- [Counters With Other Features](../document-extensions/counters/counters-and-other-features#counters-and-other-features)

### Attachments
- [Attachments](../document-extensions/attachments/what-are-attachments)

### Time Series
- [Time Series](../document-extensions/timeseries/overview)
- [Incremental Time Series](../document-extensions/timeseries/incremental-time-series/overview)

### Revisions
- [Document Revisions](../document-extensions/revisions/overview)
- [Conflict Revisions](../document-extensions/revisions/client-api/operations/conflict-revisions-configuration)

