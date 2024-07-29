# Anchoring Documents to a Bucket
---

{NOTE: }

* By default, RavenDB selects the bucket for storing a document based on a [hash code](../../sharding/overview#buckets-population) derived from the entire document ID.
  To give you more control over the document's bucket selection, RavenDB allows you to **Anchor documents to a bucket**,
  which ensures that multiple documents **share the same bucket**.

* Making documents share a bucket is useful when, for example, the documents are related and are expected to be frequently loaded in the same transaction.  

* To gain control over which specific shard a document will reside in, see [sharding by prefix](../../sharding/administration/sharding-by-prefix). 

* In this page:  
  * [Overview](../../sharding/administration/anchoring-documents#overview)  
  * [Anchor multiple documents to the same bucket as a specific document](../../sharding/administration/anchoring-documents#anchor-multiple-documents-to-the-same-bucket-as-a-specific-document)  
  * [Anchor multiple documents to the same bucket](../../sharding/administration/anchoring-documents#anchor-multiple-documents-using-a-common-suffix)  
  * [Examples](../../sharding/administration/anchoring-documents#examples)  
  
{NOTE/}

---

{PANEL: Overview}

* Anchoring documents to a bucket is done by appending a suffix to the document ID.  
  You cannot explicitly select a bucket by number; instead, the bucket is determined based on the suffix used.  
  The suffix is composed of the `$` symbol + your choice of `suffix-text`.  

* RavenDB will run the hash algorithm only over the ID part following the `$` symbol to determine which bucket the document will be placed in.
  In a document ID that contains multiple `$` symbols, only the suffix following the **last** `$` will be used to calculate the bucket number.
 
* Documents whose IDs end with the **same suffix**, will **share the same bucket** (and therefore a shard).

{WARNING: }
Avoid anchoring too many documents to the same bucket to prevent creating an oversized bucket  
that cannot be split and resharded if needed to balance the database.
{WARNING/}

{PANEL/}

{PANEL: Anchor multiple documents to the same bucket as a specific document }

* To store a document in the same bucket as another specific document,  
  use the following format for the new document's ID:  
  <`new document ID`> + `$` + <`ID of the document you want to anchor it with`>  
  The new anchored document will be stored in the same bucket as that other document.

* For example, creating a document with the following ID: <`Users/70`> + `$` + <`Users/4`>  
  will place document `Users/70$Users/4` in the same bucket as `Users/4`.

    !["Anchored Documents"](images/anchored-documents.png '"Users/70$Users/4" is stored in the same bucket as "Users/4"')

* An anchored document is accessible only by its full name.  
  E.g., `Users/70` and `Users/70$Users/4` are two different documents,  
  where `Users/70$Users/4` is anchored to `Users/4` and `Users/70` is not.

* It is possible to anchor multiple documents to the same document.  
  E.g., naming three documents `Users/70$Users/4`, `Users/71$Users/4`, and `Users/72$Users/4`  
  will make the database store these documents in the same bucket as `Users/4`.

{PANEL/}

{PANEL: Anchor multiple documents using a common suffix}

* It is possible to anchor multiple documents to the same bucket using an arbitrary suffix that does not correspond  
  to an existing document.  

* E.g., `Users/1$foo` and `Users/2$foo` will be stored in the same bucket.

{PANEL/}

{PANEL: Examples}

#### Example 1

Explicitly store a document with another document's name as a suffix, to keep both documents in the same bucket.  
In this case, keep an invoice in the same bucket as its order.  

{CODE storeInvoiceInOrderBucketExplicitNaming@Sharding\ShardingAdministration.cs /}

---

#### Example 2

Define and use a naming convention for invoices.  
Whenever an invoice is stored, the $ symbol and an order ID are automatically added to the invoice's ID  
to assure that invoices and orders are kept in the same bucket.

{CODE storeInvoiceInOrderBucketNamingConvention@Sharding\ShardingAdministration.cs /}

{PANEL/}

## Related articles

### Sharding

- [Sharding Overview](../../sharding/overview)
- [Sharding by prefix](../../sharding/sharding-by-prefix)

### Client

- [Sharding Queries](../../sharding/querying)  
- [Querying a Selected Shard](../../sharding/querying#querying-a-selected-shard)  
