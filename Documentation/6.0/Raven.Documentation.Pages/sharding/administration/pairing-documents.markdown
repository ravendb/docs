# Sharding: Pairing Documents
---

{NOTE: }

* When a document is stored on a sharded database, RavenDB will normally 
  save it by its own considerations in some available bucket on one of 
  its shards, regardless of where other documents are stored.  
  **Paired** documents are an exception to this behavior: if a document 
  is paired with another document RavenDB will store the document on 
  the same shard as the document it is paired with.  
* To pair one document with another, we need to add to the document ID 
  the `$` symbol followed by the ID of the document we want to pair it with:  
  <`ID1`> + `$` + <`ID2`>  
* Making sure that documents share a shard is useful when, for example, 
  the documents are related and are expected to often be loaded in the 
  same transaction.  
  {NOTE: }
  Read [here](../../sharding/querying#querying-a-selected-shard) about querying a specific shard.  

* In this page:  
  * [Pairing Documents](../../sharding/administration/pairing-documents#pairing-documents)  
  * [Examples](../../sharding/administration/pairing-documents#examples)  
  
{NOTE/}

---

{PANEL: Pairing Documents}

Pairing a document, e.g. `ID1`, with another document, e.g. `ID2`, 
is done by adding `ID1`'s name a suffix: `$` + <`ID2`>  
Using this convention forces the cluster to calculate the bucket 
number for `ID1` by the ID of `ID2`, and therefore store `ID1` in 
the same bucket as `ID2`.  

**E.g.** -  
Original document ID: `Users/70`  
A document we want `Users/70` to share a bucket with: `Users/4`  
The new name we give `Users/70`: `Users/70$Users/4`  

!["Paired Documents"](images/overview_paired-documents.png "Paired Documents")

* We do **not** select the bucket (or shard) in which a document is stored.  
  The only thing that we **can** select, is the document/s a document 
  shares a bucket with.  

* It **is** possible to pair multiple documents with the same document.  
  E.g. naming three documents `Users/70$Users/4`, `Users/71$Users/4`, 
  and `Users/72$Users/4` will make the cluster store `Users/70`, 
  `Users/71`, and `Users/72` in the same bucket as `Users/4`.  

* It **is** possible to create a chain of paired documents.  
  E.g. naming three documents `Users/4$Users/5`, `Users/5$Users/6`, 
  and `Users/6$Users/7` will make the cluster store `Users/4`, `Users/5`, 
  `Users/6`, and `Users/7` in the same bucket.  

{NOTE: }
Note that a paired document is accessible only by its full name.  
E.g., `Users/4$Users/5` can only be accessed using the name `Users/4$Users/5` 
but **not** using the name `Users/4`.  
{NOTE/}

{WARNING: }
Please make sure that you do not force the storage of too many documents 
in the same bucket (and shard), to prevent the creation of an imbalanced 
database in which one of the shards is overpopulated and others are underpopulated.  
{WARNING/}

{PANEL/}

{PANEL: Examples}

### Example 1

Explicitly store a document with another document's name as a suffix, 
to keep both documents in the same bucket.  
In this case, keep an invoice in the same bucket as its order.  

{CODE storeInvoiceInOrderBucketExplicitNaming@Sharding\ShardingAdministration.cs /}

### Example 2

Define and use a naming convention for invoices.  
Whenever an invoice is stored, the $ symbol and an order ID are automatically added 
to the invoice's ID to assure that invoices and orders are kept in the same bucket.  
{CODE storeInvoiceInOrderBucketNamingConvention@Sharding\ShardingAdministration.cs /}

{PANEL/}

## Related articles

### Client

- [Sharding Overview](../../sharding/overview)  
- [Sharding Queries](../../sharding/querying)  
- [Querying a Selected Shard](../../sharding/querying#querying-a-selected-shard)  
