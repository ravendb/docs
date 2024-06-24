# Indexes: Map-Reduce Indexes
---

{NOTE: }

* **Map-Reduce indexes** allow complex ***data aggregation*** that can be queried on 
  with very little cost, regardless of the data size.  

* To expedite queries and prevent performance degradation during queries, the aggregation 
  is done during the indexing phase, _not_ at query time.  

* Once new data enters the database, or existing documents are modified,  
  the Map-Reduce index will re-calculate the aggregated data so that the 
  aggregation results are always available and up-to-date.  

* The aggregation computation is done in two separate consecutive actions:  
  * **The `map` stage:**  
    This first stage runs the defined Map function(s) on each document, indexing the specified fields.  
  * **The `reduce` stage:**  
    This second stage groups the specified requested fields that were indexed in the Map stage,  
    and then runs the Reduce function to get a final aggregation result per field value.  

* In this page: 
   * [Creating Map Reduce Indexes](../indexes/map-reduce-indexes#creating-map-reduce-indexes)
   * [Reduce Results as Artificial Documents](../indexes/map-reduce-indexes#reduce-results-as-artificial-documents)
   * [Remarks](../indexes/map-reduce-indexes#remarks)  

{NOTE/}

{PANEL: Creating Map Reduce Indexes}

When it comes to index creation, the only difference between simple indexes and the map-reduce ones 
is an additional reduce function defined in the index definition.  
To deploy an index we need to create a definition and deploy it using one of the ways described in the 
[creating and deploying](../indexes/creating-and-deploying) article.  

---

#### Example I - Count

Let's assume that we want to count the number of products for each category.  
To do it, we can create the following index using `LoadDocument` inside:  
{CODE-TABS}
{CODE-TAB:python:Map_Reduce map_reduce_0_0@Indexes\MapReduceIndexes.py /}
{CODE-TAB:python:JavaScript map_reduce_0_0@Indexes\JavaScript.py /}}
{CODE-TABS/}

and issue the query:
{CODE-TABS}
{CODE-TAB:python:Query map_reduce_0_1@Indexes\MapReduceIndexes.py /}
{CODE-TAB-BLOCK:sql:RQL}
from 'Products/ByCategory'
where Category == 'Seafood'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

The above query will return one result for _Seafood_ with the appropriate number of products from that category.

---

#### Example II - Average

In this example, we will count an average product price for each category.  
The index definition:
{CODE-TABS}
{CODE-TAB:python:Map_Reduce map_reduce_1_0@Indexes\MapReduceIndexes.py /}
{CODE-TAB:python:JavaScript map_reduce_1_0@Indexes\JavaScript.py /}}
{CODE-TABS/}

and the query:  
{CODE-TABS}
{CODE-TAB:python:Query map_reduce_1_1@Indexes\MapReduceIndexes.py /}
{CODE-TAB-BLOCK:sql:RQL}
from 'Products/Average/ByCategory'
where Category == 'Seafood'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

---

#### Example III - Calculations

This example illustrates how we can put some calculations inside an index using 
one of the indexes available in the sample database (`Product/Sales`).

We want to know how many times each product was ordered and how much we earned for it.  
To extract that information, we need to define the following index:
{CODE-TABS}
{CODE-TAB:python:Map_Reduce map_reduce_2_0@Indexes\MapReduceIndexes.py /}
{CODE-TAB:python:JavaScript map_reduce_2_0@Indexes\JavaScript.py /}}
{CODE-TABS/}

And run the query:
{CODE-TABS}
{CODE-TAB:python:Query map_reduce_2_1@Indexes\MapReduceIndexes.py /}
{CODE-TAB-BLOCK:sql:RQL}
from 'Product/Sales'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Reduce Results as Artificial Documents}

#### Map-Reduce Output Documents

In addition to storing the aggregation results in the index, the map-reduce index can also output 
those reduce results as documents to a specified collection. In order to create these documents, 
called _"artificial",_ you need to define the target collection using the `output_reduce_to_collection` 
property in the index definition.  

Writing map-reduce outputs into documents allows you to define additional indexes on top of them 
that give you the option to create recursive map-reduce operations. This makes it cheap and easy 
to, for example, recursively create daily, monthly, and yearly summaries on the same data.  

In addition, you can also apply the usual operations on artificial documents (e.g. data 
subscriptions or ETL).  

If the aggregation value for a given reduce key changes, we overwrite the output document. If the 
given reduce key no longer has a result, the output document will be removed.  

#### Reference Documents

To help organize these output documents, the map-reduce index can also create an additional 
collection of artificial _reference documents_. These documents aggregate the output documents 
and store their document IDs in an array field `ReduceOutputs`.  

The document IDs of reference documents are customized to follow some pattern. The format you 
give to their document ID also determines how the output documents are grouped.  

Because reference documents have well known, predictable IDs, they are easier to plug into 
indexes and other operations, and can serve as an intermediary for the output documents whose 
IDs are less predictable. This allows you to chain map-reduce indexes in a recursive fashion, 
see [Example II](../indexes/map-reduce-indexes#example-ii).  

Learn more about how to configure output and reference documents in the 
[Studio: Create Map-Reduce Index](../studio/database/indexes/create-map-reduce-index).  

---

### Artificial Document Properties  

#### IDs

The identifiers of **map reduce output documents** have three components in this format:  

`<Output collection name>/<incrementing value>/<hash of reduce key values>`  

The index in [Example I](../indexes/map-reduce-indexes#example-i) might generate an output document 
ID like this:  

`DailyProductSales/35/14369232530304891504`  

* "DailyProductSales" is the collection name specified for the output documents.  
* The middle part is an incrementing integer assigned by the server. This number grows by some 
amount whenever the index definition is modified. This can be useful because when an index definition 
changes, there is a brief transition phase when the new output documents are being created, but the 
old output documents haven't been deleted yet (this phase is called 
["side-by-side indexing"](../studio/database/indexes/indexes-list-view#indexes-list-view---side-by-side-indexing)). 
During this phase, the output collection contains output documents created both by the old version 
and the new version of the index, and they can be distinguished by this value: the new output 
documents will always have a higher value (by 1 or more).  
* The last part of the document ID (the unique part) is the hash of the reduce key values - in this 
case: `hash(Product, Month)`.  

The identifiers of **reference documents** follow some pattern you choose, and this pattern 
determines which output documents are held by a given reference document.  

The index in [Example I](../indexes/map-reduce-indexes#example-i) has this pattern for reference documents:  

`sales/daily/{Date:yyyy-MM-dd}`

And this produces reference document IDs like this:

`sales/daily/1998-05-06`

The pattern is built using the same syntax as 
[the `StringBuilder.AppendFormat` method](https://docs.microsoft.com/en-us/dotnet/api/system.text.stringbuilder.appendformat). 
See [here](https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings) 
to learn about the date formatting in particular.  

---

#### Metadata

Artificial documents generated by map-reduce indexes get the following `@flags` in their metadata:  

{CODE-BLOCK:json}
"@flags": "Artificial, FromIndex"
{CODE-BLOCK/}

These flags are used internally by the database to filter out artificial documents during replication.  

---

### Syntax

The map-reduce output documents are configured with these properties of 
`IndexDefinition`:  

{CODE:python syntax_0@Indexes\MapReduceIndexes.py /}

| Parameters | Type | Description |
| - | - | - |
| **\_output_reduce_to_collection** | `str` | Collection name for the output documents. |
| **\_pattern_references_collection_name** | `str` | Optional collection name for the reference documents - by default it is `OutputReduceToCollection/References` |
| **\_pattern_for_output_reduce_to_collection_references** | `str` | Document ID format for reference documents. This ID references the fields of the reduce function output, which determines how the output documents are aggregated. The type of this parameter is different depending on if the index is created using [IndexDefinition](../indexes/creating-and-deploying#using-maintenance-operations) or [AbstractIndexCreationTask](../indexes/creating-and-deploying#using-abstractindexcreationtask). |

---

#### Example:

Here is a map-reduce index with output documents and reference documents:  

{CODE-TABS}
{CODE-TAB:python:Map_Reduce map_reduce_3_0@Indexes\MapReduceIndexes.py /}
{CODE-TAB:python:JavaScript map_reduce_3_0@Indexes\JavaScript.py /}
{CODE-TABS/}

In the index example above (which inherits `AbstractIndexCreationTask`), 
the reference document ID pattern is set with the expression:  

{CODE-BLOCK:python}
self._pattern_for_output_reduce_to_collection_references = "sales/daily/{Date:yyyy-MM-dd}"
{CODE-BLOCK/}  

This gives the reference documents IDs in this general format: `sales/monthly/1998-05-01`. 
The reference document with that ID contains the IDs of all the output documents from the 
month of May 1998.  
<br/>
In the **JavaScript** index example (which uses `IndexDefinition`), 
the reference document ID pattern is set with a `string`:  

{CODE-BLOCK:python}
pattern_for_output_reduce_to_collection_references="sales/daily/{Date:yyyy-MM-dd}"
{CODE-BLOCK/}  

This gives the reference documents IDs in this general format: `sales/daily/1998-05-06`. 
The reference document with that ID contains the IDs of all the output documents from 
May 6th 1998.  

{PANEL/}

{PANEL: Remarks}

#### Saving documents

[Artificial documents](../indexes/map-reduce-indexes#reduce-results-as-artificial-documents) 
are stored immediately after the indexing transaction completes.  

---

#### Recursive indexing loop

It is **forbidden** to output reduce results to collections such as the following:  

- A collection that the current index is already working on.  
  E.g., an index on a `DailyInvoices` collection outputs to `DailyInvoices`.  
- A collection that the current index is loading a document from.  
  E.g., an index with `LoadDocument(id, "Invoices")` outputs to `Invoices`.  
- Two collections, each processed by a map-reduce indexes,  
  when each index outputs to the second collection.  
  E.g.,  
  An index on the `Invoices` collection outputs to the `DailyInvoices` collection,  
  while an index on `DailyInvoices` outputs to `Invoices`.  

When an attempt to create such an infinite indexing loop is 
detected a detailed error is generated.  

---

#### Output to an Existing collection

Creating a map-reduce index which defines an output collection that already 
exists and contains documents, will result in an error.  
Delete all documents from the target collection before creating the index, 
or output results to a different collection.  

---

#### Modification of Artificial Documents

Artificial documents can be loaded and queried just like regular documents.  
However, it is **not** recommended to edit artificial documents manually since 
any index results update would overwrite all manual modifications made in them.  

{PANEL/}

## Related Articles

### Indexes
- [Map Indexes](../indexes/map-indexes)
- [Multi-Map Indexes](../indexes/multi-map-indexes)

### Querying
- [Query Overview](../client-api/session/querying/how-to-query)

### Studio
- [Indexes: Overview](../studio/database/indexes/indexes-overview)
- [Index List View](../studio/database/indexes/indexes-list-view)
- [Create Map Index](../studio/database/indexes/create-map-index)
- [Create Multi-Map Index](../studio/database/indexes/create-multi-map-index)
- [Map-Reduce Visualizer](../studio/database/indexes/map-reduce-visualizer)

---

## Code Walkthrough

- [Map Index](https://demo.ravendb.net/demos/python/static-indexes/map-index)
- [Map-Reduce Index](https://demo.ravendb.net/demos/python/static-indexes/map-reduce-index)
- [Multi-Map-Reduce-Index](https://demo.ravendb.net/demos/python/multi-map-indexes/multi-map-reduce-index#)

