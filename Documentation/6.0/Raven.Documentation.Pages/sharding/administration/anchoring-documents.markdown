# Sharding: Anchoring Documents
---

{NOTE: }

* **Anchoring a document** means using another document as its 
  anchor to a bucket.  
  An anchored document will always be stored by RavenDB in the 
  same bucket (and therefore in the same shard) as the document 
  it is anchored to.  
* Making sure that documents share a bucket is useful when, for example, 
  the documents are related and are expected to often be loaded in the 
  same transaction.  

* In this page:  
  * [Overview](../../sharding/administration/anchoring-documents#overview)  
  * [Examples](../../sharding/administration/anchoring-documents#examples)  
  
{NOTE/}

---

{PANEL: Overview}

To anchor a document, add its ID a suffix: the `$` symbol, 
followed by the ID of the anchor document.  
<`anchored document ID`> + `$` + <`anchor document ID`>  

{NOTE: }
To determine which bucket a document will be stored in, RavenD 
normally [runs a hash algorithm](../../sharding/overview#buckets-population) 
over the entire document ID and uses the bucket pointed at by the 
resulting hash code.  
Anchored documents are an exception to this behavior, as RavenDB 
runs the hash algorithm only over the ID part following the `$` symbol.  
If this part is the ID of an anchor document, the anchored and anchor 
documents will have the same hash code and be stored in the same bucket.  
{NOTE/}

In the example below, the document named `Users/70$Users/4` is placed 
in the same bucket as its anchor, `Users/4`.  

!["Anchored Documents"](images/anchored-documents.png "Anchored Documents")

* Users **cannot** directly select which bucket (or shard) to store 
  a document at, but only which document to anchor a document to.  

* It **is** possible to anchor multiple documents to the same document.  
  E.g., naming three documents `Users/70$Users/4`, `Users/71$Users/4`, 
  and `Users/72$Users/4` will make the database store these documents 
  in the same bucket as `Users/4`.  

* It **is** possible to anchor multiple documents to the same bucket 
  using an arbitrary name that represents no existing document.  
  E.g., `Users/1$foo` and `Users/2$foo` **will** share a bucket: 
  the hash code for both documents' bucket will be calculated 
  by `foo` though this is an arbitrary name that represents no 
  existing document.  

{WARNING: }
Please make sure that you do not anchor too many documents to the same bucket, 
to prevent the creation of an oversized bucket that cannot be split and resharded, 
if needed, to balance the database.  
{WARNING/}

{NOTE: Notes about document names:}

* In a document ID that contains multiple `$` symbols, only the 
  suffix following the last `$` will be used to calculate the bucket number.  

* An anchored document is accessible only by its full name.  
  E.g., `Users/4` and `Users/4$Users/5` are two different documents, 
  where `Users/4$Users/5` is anchored to `Users/5` and `Users/4` is not.  
{NOTE/}

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
