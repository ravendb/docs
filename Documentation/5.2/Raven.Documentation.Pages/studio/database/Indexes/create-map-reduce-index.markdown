﻿# Create Map-Reduce Index
---

{NOTE: }

* **Map-Reduce indexes** allow you to perform complex ***data aggregation*** that can be queried on with very little cost,  
  regardless of the data size.  

* The aggregation is done during the indexing phase, _not_ at query time.  

* Once new data comes into the database, or existing documents are modified,  
  the Map-Reduce index will re-calculate the aggregated data,  
  so that the aggregation results are always available and up-to-date !  

* The aggregation computation is done in two separate consecutive actions: the `Map` and the `Reduce`.  

* **The Map stage:**  
  This first stage runs the defined Map function(s) on each document, indexing the specified fields.  

* **The Reduce stage:**  
  This second stage groups the specified requested fields that were indexed in the Map stage,  
  and then runs the Reduce function to get a final aggregation result per field value.  

* The Map-Reduce results can be visualized in the [Map-Reduce Visualizer](../../../studio/database/indexes/map-reduce-visualizer).  

* In this page:  
  * [The Map Stage](../../../studio/database/indexes/create-map-reduce-index#the-map-stage)  
  * [The Reduce Stage](../../../studio/database/indexes/create-map-reduce-index#the-reduce-stage)  
  * [Important Guidelines](../../../studio/database/indexes/create-map-reduce-index#important-guidelines)  
  * [Map-Reduce Query Results](../../../studio/database/indexes/create-map-reduce-index#map-reduce-query-results)  
  * [Multi-Map-Reduce](../../../studio/database/indexes/create-map-reduce-index#multi-map-reduce)    
  * [Saving Map-Reduce Results into a Collection (Artificial Documents)](../../../studio/database/indexes/create-map-reduce-index#saving-map-reduce-results-in-a-collection-(artificial-documents))  
      * [Artificial -vs- Regular Documents](../../../studio/database/indexes/create-map-reduce-index#artificial-documents--vs--regular-documents)  
      * [Artificial Documents Usage](../../../studio/database/indexes/create-map-reduce-index#artificial-documents-usage)  
      * [Artificial Documents Limitations](../../../studio/database/indexes/create-map-reduce-index#limitations)  
{NOTE/}

---

{PANEL: The Map Stage}

**Define a Map Function:**  

* In the following example, we want to get the following aggregated values:
  * The number of orders each company makes &  
  * The accumulative amount spent on all orders by each company.  

* Lets define the following Map function on the `Orders` collection:  

![Figure 1. The Map Function](images/create-map-reduce-index-1.png "The Map Function")

1. **Index Name** - An index name can be composed of letters, digits, `.`, `/`, `-`, and `_`. The name must be unique in the scope of the database.  
   * Uniqueness is evaluated in a _case-insensitive_ way - you can't create indexes named both `usersbyname` and `UsersByName`.  
   * The characters `_` and `/` are treated as equivalent - you can't create indexes named both `users/byname` and `users_byname`.  
   * If the index name contains the character `.`, it must have some other character on _both sides_ to be valid. `/./` is a valid index name, but 
   `./`, `/.`, and `/../` are all invalid.  

2. The Map function in defines the following 3 fields that will be indexed:  

   * **order.Company** -  
     The company  

   * **OrdersCount** -  
     In the Map stage, per single Order document, the value of this field is ***'1'***,  
     as each order document in the Order collection was made by a single, specific company.  
     This field will be aggregated later in the Reduce stage, accumulating the data from all the Orders documents, per company.  
     The accumulative value of this field will represent the number of all orders a company has made.  

   * **TotalOrdersAmount** -  
     In the Map stage, per single Order document, the value of this field is the total order amount for that document.  
     (Summing up all products in the 'Lines' field in the document, and taking the discount into account).  
     This field will be aggregated later in the Reduce stage, accumulating the data from all the Orders documents, per company.  
     The accumulative value of this field will represent the total amount spent by a company on _all_ orders.  

3. Next, click 'Add Reduction' to continue and add the 'Reduce' function. 
   See [The Reduce Stage](../../../studio/database/indexes/create-map-reduce-index#the-reduce-stage).
{PANEL/}

{PANEL: The Reduce Stage}

**Define a Reduce Function:**  

![Figure 2. The Reduce Function](images/create-map-reduce-index-2.png "The Reduce Function")

1. * In the Reduce function above, results are grouped by the `Company` field,  
     so that we can get the data per company. ``` (group result by result.Company)```  

   * The index results will show in the following format:  
   
     * **Company** - will be the company for which we see the results.  
   
     * **OrdersCount** - is the aggregation of the _orders count_ value from the Map stage  
       (How many orders were made by each company).  
   
     * **TotalOrdesAmount** - is the aggregation of the _total orders amount_ made by each company  
       (How much money the company has spent all together, on _all_ orders).  

2. Optional: The results of the Map-Reduce index can be saved in a new collection.  
   Learn more in [Saving Map-Reduce Results in a Collection (Artificial Documents)](../../../studio/database/indexes/create-map-reduce-index#saving-map-reduce-results-in-a-collection-(artificial-documents))
{PANEL/}

{PANEL: Important Guidelines}

* Both the Map and the Reduce functions must be **pure functions**, they should have no external input.  
  i.e. usage of Random, DateTime.Now or any similar calls is _not_ allowed.  
  Calling them with the same input must always return the same output.  

* The Reduce output must match the Map output, they must have the **same structure**.  
  RavenDB will error if you have a different shape for each of the functions.  
{PANEL/}

{PANEL: Map-Reduce Query Results}

![Figure 3. Map Reduce Query Result](images/create-map-reduce-index-3.png "Map-Reduce Query Result")

* In the **query results**, the number of orders per company is represented in the `OrdersCount` column.  
  The total amount of all orders per company is represented in the `TotalOrdersAmount` column.  
  The column names correspond to the Map-Reduce fields definition.  

* The Map-Reduce results can also be visualized in [Map-Reduce Visualizer](../../../studio/database/indexes/map-reduce-visualizer).  

{PANEL/}

{PANEL: Multi-Map-Reduce}

* Multi-Map-Reduce indexes allow us to aggregate data from multiple collections.  

* In the below example we define three maps, on the `Companies`, `Suppliers` and `Employees` collections.  
  In each map, we output a count for the type of the document we're mapping, as well as the relevant City.  

![Figure 4. Define Multi-Maps](images/create-map-reduce-index-4.png "Define Multi Maps")

* In the Reduce part we group by `City` and then sum up all the results from all the intermediate steps,  
  to get the final city count in each collection.  

![Figure 4.1 The Multi-Map-Reduce](images/create-map-reduce-index-4.1.png "The Multi-Map-Reduce")

{PANEL/}

{PANEL:  Saving Map-Reduce Results in a Collection (Artificial Documents)}

* The results of the Map-Reduce index can be saved as _output documents_ in a new output collection.  

* These output documents can be further aggregated by _reference documents_, documents that contain 
the document IDs of output documents.

* These documents created by Map-Reduce Indexes are called **Artificial Documents**.  

* Learn more about using Artificial Documents from the client code in [Map-Reduce Indexes: Reduce Results as Artificial Documents](../../../indexes/map-reduce-indexes#reduce-results-as-artificial-documents).  

![Figure 5. Save Map-Reduce Results into a Collection](images/create-map-reduce-index-5.png "Save Map-Reduce Results into a Collection")

1. Specify the name of the collection you want the output documents to be saved in.  
   Note: the collection specified must be _empty_.  

2. Specify a pattern for the reference document IDs. By including reduce function fields, this 
pattern determines which output documents will be included in each reference document.  

3. The name of the collection for the reference documents. By default, this is 
`<output collection name>/Reference`.  

![Figure 6. An Artificial Document](images/create-map-reduce-index-6.png "An Artificial Document in the collection CompaniesOrders")

![Figure 7. A Reference Document](images/create-map-reduce-index-7.png "A Reference Document in the collection CompaniesOrders/Reference")

{NOTE: }
####  Artificial Documents -vs- Regular Documents  

* Artificial documents are created by the index directly.  

* They behave just like standard documents except for that they are _not_ replicated to other nodes in the database group.  
  So while loading or querying them is just fine, modifying the content of an artificial document by hand is _not_ recommended, 
  as the next index results update will overwrite any changes that you have made to the document.  

* Artificial documents are updated whenever the index completes indexing a batch of documents.  
{NOTE/}

{NOTE: }
#### Artificial Documents Usage

* You can set up indexes on top of the Artificial Documents collection, including additional MapReduce indexes,  
  giving you the option to create recursive map-reduce operations.  

* You can set up a [RavenDB ETL Task](../../../studio/database/tasks/ongoing-tasks/ravendb-etl-task) 
  on the Artificial Documents collection to a dedicated database on a separate cluster for further processing, 
  as well as other ongoing tasks such as: [SQL ETL](../../../server/ongoing-tasks/etl/sql) and [Subscriptions](../../../client-api/data-subscriptions/what-are-data-subscriptions).
{NOTE/}

{NOTE: }
####  Limitations

* RavenDB will detect and generate an error if you have a cycle of artificial documents. 
  You can't define another Map-Reduce index that will output artificial documents 
  if that will trigger (directly or indirectly) the same index.  
  Otherwise, you might set up a situation where the indexes run in an infinite loop.  

* An empty collection must be used as the target collection for the artificial documents.  
  This is mandatory since the Map-Reduce index overwrites any existing document in the collection.  

* You have no control over the artificial documents IDs.  
  These identifiers are generated by RavenDB based on the hash of the reduce key.  

* Artificial documents are _not_ sent over replication,  
  each node in the database group has its own (independent) copy of the index results.  
  Therefore:

  1. It is recommended to use artificial documents with [Subscriptions](../../../client-api/data-subscriptions/what-are-data-subscriptions) only on a _single_ node.  
  A Subscription failover to another node may cause the subscription to send Artificial Documents  
  that the subscription has already acknowledged.  

  2. Artificial documents cannot use [Revisions](../../../server/extensions/revisions) or [Attachments](../../../document-extensions/attachments/what-are-attachments).  

{NOTE/}
{PANEL/}

## Related Articles

### Indexes
- [Map Indexes](../../../indexes/map-indexes)
- [Multi-Map Indexes](../../../indexes/multi-map-indexes)
- [Map-Reduce Indexes](../../../indexes/map-reduce-indexes)

### Studio
- [Indexes Overview](../../../studio/database/indexes/indexes-overview)
- [Indexes List View](../../../studio/database/indexes/indexes-list-view)
- [Create Map Index](../../../studio/database/indexes/create-map-index)
- [Create Multi-Map Index](../../../studio/database/indexes/create-multi-map-index)
- [Map-Reduce Visualizer](../../../studio/database/indexes/map-reduce-visualizer)
- [Index History View](../../../studio/database/indexes/index-history-view)
