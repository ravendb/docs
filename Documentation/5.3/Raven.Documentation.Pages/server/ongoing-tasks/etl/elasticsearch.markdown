# Ongoing Tasks: Elasticsearch ETL

---

{NOTE: }

* An **Elasticsearch** [ETL](../../../server/ongoing-tasks/etl/basics) task creates an ETL process 
  from selected collections in a RavenDB database to Elasticsearch destinations.

* You can define an Elasticsearch ETL task using [Studio](../../../studio/database/tasks/ongoing-tasks/elasticsearch-etl-task) 
  or your [client](../../../client-api/operations/maintenance/etl/add-etl#example---add-elasticsearch-etl-task).  

* In this page:  
  * [Elasticsearch ETL](../../../server/ongoing-tasks/etl/elasticsearch#elasticsearch-etl)  
  * [Transformation Script](../../../server/ongoing-tasks/etl/elasticsearch#transformation-script)  
  * [Data Delivery](../../../server/ongoing-tasks/etl/elasticsearch#data-delivery)  
     * [What is Transferred](../../../server/ongoing-tasks/etl/elasticsearch#what-is-transferred)  
     * [Document Identifiers](../../../server/ongoing-tasks/etl/elasticsearch#document-identifiers)  
     * [Transactions](../../../server/ongoing-tasks/etl/elasticsearch#transactions)  
     * [Insert Only Mode](../../../server/ongoing-tasks/etl/elasticsearch#insert-only-mode)  
  * [Elasticsearch Index Definition](../../../server/ongoing-tasks/etl/elasticsearch#elasticsearch-index-definition)  
  * [Client API](../../../server/ongoing-tasks/etl/elasticsearch#client-api)  
     * [Add an Elasticsearch ETL Task](../../../server/ongoing-tasks/etl/elasticsearch#add-an-elasticsearch-etl-task)  
     * [Add an Elasticsearch Connection String](../../../server/ongoing-tasks/etl/elasticsearch#add-an-elasticsearch-connection-string)  
  * [Supported Elasticsearch Versions](../../../server/ongoing-tasks/etl/elasticsearch#supported-elasticsearch-versions)  

{NOTE/}

---

{PANEL: Elasticsearch ETL}

* The following steps are required when creating an Elasticsearch ETL task:  
   * Define a [connection string](../../../server/ongoing-tasks/etl/elasticsearch#add-an-elasticsearch-connection-string) which includes:  
      * URLs to Elasticsearch nodes.  
      * Authentication method required by the Elasticsearch nodes.  
   * **Define the Elasticsearch Indexes**  
      * Indexes are used by Elasticsearch to store and locate documents.  
      * The ETL task will insert new documents to the specified Elasticsearch destinations.  
      * If not otherwise specified, existing Elasticsearch documents will be removed before adding new documents.  
      * A [document identifier](../../../server/ongoing-tasks/etl/elasticsearch#document-identifiers) 
        field property is defined per document, and used by the delete command to locate the matching documents.  
   * **Define Transformation Scripts**.  
     The transformation script determines which RavenDB documents will be transferred, 
     to which Elasticsearch Indexes, and in what form.  

* For a thorough step-by-step explanation:  
   * Learn [here](../../../server/ongoing-tasks/etl/elasticsearch#add-an-elasticsearch-etl-task) 
     to define an Elasticsearch ETL task using **code**.  
   * Learn [here](../../../studio/database/tasks/ongoing-tasks/elasticsearch-etl-task) 
     to define an Elasticsearch ETL task using **Studio**.  

{PANEL/}

{PANEL: Transformation Script}

* The structure and syntax of an Elasticsearch ETL transformation script are similar to 
  those of all other ETL types ([RavenDB ETL](../../../server/ongoing-tasks/etl/raven), 
  [SQL ETL](../../../server/ongoing-tasks/etl/sql), and 
  [OLAP ETL](../../../server/ongoing-tasks/etl/olap)) scripts.  
  The script defines which documents will be _Extracted_ from the database, 
  _Transforms_ the retrieved data, and _Loads_ it to the Elasticsearch destination.
  Learn about ETL transformation scripts [here](../../../server/ongoing-tasks/etl/basics#transform).  

* The script **Loads** data to the Elasticsearch destination using the 
  [loadTo\\<Target\\>(obj)](../../../server/ongoing-tasks/etl/basics#transform) command.  
   * `Target` is the name of the Elasticsearch index to which the data is transferred.  
       * **In the task settings**:  
         Define Elasticsearch Index names using only lower-case characters (as required by Elasticsearch).  
         E.g. orders
       * **In the transformation script**:  
         The target can be defined using both upper and lower-case characters.  
         The task will transform the index name to all lower-case characters before sending it to Elasticsearch.  
         E.g. use either loadToOrders or loadToorders.  
   * `obj` is an object defined by the script, that will be loaded to Elasticsearch.  
     It determines the shape and contents of the document that will be created on the Elasticsearch Index.  
     E.g., the following script defines the `orderData` object and loads it to the `orders` index:  
     {CODE-BLOCK: JavaScript}
     var orderData = { DocId: id(this),
                  OrderLinesCount: this.Lines.length,
                  TotalCost: 0 };

loadToOrders(orderData);
     {CODE-BLOCK/}

{PANEL/}

{PANEL: Data Delivery}

---

### What is Transferred

An Elasticsearch ETL task transfers **documents only**.  
Document extensions like attachments, counters, or time series, will not be transferred.  

---

### Document Identifiers

* When Elasticsearch stores RavenDB documents, it provides each of them 
  with an automatically-generated iD.  
* RavenDB needs to delete and replace documents, but cannot do it 
  using Elasticsearch's arbitrarily generated IDs.  
  Instead, one of the transferred document's properties is used as ID.  
* The identifier must be a property that the transformation script passes to Elasticsearch.  
  To achieve this:  
   * Add a dedicated property to the transferred data structure in your script, 
     that will hold the original RavenDB document ID.  
     The property's Name can be any name of your choice.  
     The property's Value must be: `id(this)`  
   * E.g., the **DocId** property below is used to hold the RavenDB document ID in the transferred document.  
     {CODE-BLOCK: JavaScript}
  var orderData = {
                 DocId: id(this), // document ID property
                 OrderLinesCount: this.Lines.length,
                 TotalCost: 0
                  };

loadToOrders(orderData);
  {CODE-BLOCK/}
* In addition to specifying this document property in the script, it must be defined for the ETL task:  
   * Either set `DocumentIdProperty` through code (see [code sample](../../../server/ongoing-tasks/etl/elasticsearch#add-an-elasticsearch-etl-task)),  
   * or Set the [Document ID Property Name](../../../studio/database/tasks/ongoing-tasks/elasticsearch-etl-task#elasticsearch-indexes) field via Studio.  

---

### Transactions

The task delivers the data to the Elasticsearch destination in one or two calls per index.  

1. [_delete_by_query](https://www.elastic.co/guide/en/elasticsearch/reference/current/docs-delete-by-query.html):  
   An optional command, to delete existing versions of RavenDB documents from Elasticsearch 
   before appending new ones.  
   {CODE-BLOCK: JavaScript}
   POST orders/_delete_by_query?refresh=true
{"query":{"terms":{"DocID":["orders/1-a"]}}}
     {CODE-BLOCK/}
2. [_bulk ](https://www.elastic.co/guide/en/elasticsearch/reference/current/docs-bulk.html):  
   Append RavenDB documents to the Elasticsearch destination.  
   {CODE-BLOCK: JavaScript}
   POST orders/_bulk?refresh=wait_for
{"index":{"_id":null}}
{"OrderLinesCount":3,"TotalCost":0,"DocID":"orders/1-a"}
     {CODE-BLOCK/}

---

### Insert Only Mode

You can enable the task's **Insert Only** mode using [code](../../../server/ongoing-tasks/etl/elasticsearch#add-an-elasticsearch-etl-task) 
or via [Studio](../../../studio/database/tasks/ongoing-tasks/elasticsearch-etl-task#elasticsearch-indexes), 
to **omit** _delete_by_query commands and so refrain from deleting documents before the transfer.  
{NOTE: }
 Enabling **Insert Only** can boost the task's performance when there is no need to delete documents before loading them.  
{NOTE/}
{WARNING: }
 Be aware that enabling Insert Only mode will append documents to Elasticsearch whenever they 
 are modified on RavenDB, without removing existing documents. If document versions that are not 
 needed accumulate and storage space is a concern, keep Insert Only disabled.  
{WARNING/}

{PANEL/}

{PANEL: Elasticsearch Index Definition}

* When the Elasticsearch ETL task runs for the very first time, it will create any Elsasticsearch index defined in 
  the task that doesn't exist yet.  

* When creating the index, the document property that will hold the RavenDB document ID will be defined as a non-analyzed field, 
  with type [keyword](https://www.elastic.co/guide/en/elasticsearch/reference/7.15/keyword.html) to avoid having full-text-search 
  on it.  
  This way the RavenDB document identifiers won't be analyzed and the task will be able to `_delete_by_query` using exact match on those IDs.  
  I.e.  
  {CODE-BLOCK: JavaScript}
  PUT /newIndexName
{
  "mappings": {
      "properties": {
          "DocId": {   // the DocumentIdProperty
              "type": "keyword"
          }
      }
   }
}
  {CODE-BLOCK/}

{WARNING: }
If you choose to create the Elasticsearch Index on your own (before running the 
Elasticsearch ETL task), you must define the `DocumentIdProperty` **type** property 
as **"keyword"** in your index definition.  
{WARNING/}

{PANEL/}

{PANEL: Client API}

### Add an Elasticsearch ETL Task  

* To define an Elasticsearch ETL task through the client, use the 
  [AddEtlOperation](../../../client-api/operations/maintenance/etl/add-etl) API method 
  as shown below.  
  Pass it an `ElasticSearchEtlConfiguration`instance with -  
   * The name of a defined **Connection String**.  
     You can define a connection string 
     [using code](../../../server/ongoing-tasks/etl/elasticsearch#add-an-elasticsearch-connection-string) 
     or via [Studio](../../../studio/database/tasks/ongoing-tasks/elasticsearch-etl-task#define-the-elasticsearch-etl-task).  
   * A list of **Elasticsearch Indexes**.  
   * A list of **Transformation Scripts**.  

**Code Sample**:  
{CODE add_elasticsearch_etl@ClientApi\Operations\AddEtl.cs /}
{NOTE: Task Properties}

* `ElasticSearchEtlConfiguration`  

    | Property | Type | Description |
    |:-------------|:-------------|:-------------|
    | **Name** | `string` | ETL Task Name |
    | **ConnectionStringName** | `string` | The name of the connection string used by this task |
    | **ElasticIndexes** | `List<ElasticSearchIndex>` | A list of Elasticsearch indexes |
    | **Transforms** | `List<Transformation>` | A list of transformation scripts |

* `ElasticSearchIndex`  

    | Property | Type | Description |
    |:-------------|:-------------|:-------------|
    | **IndexName** | `string` | Elasticsearch Index name. <br> Name indexes **using lower-case characters only**, e.g. `orders`. |
    | **DocumentIdProperty** | `string` | The [document ID property](../../../server/ongoing-tasks/etl/elasticsearch#document-identifiers) defined on the transferred document object inside the transformation script. |
    | **InsertOnlyMode** | `bool` | `true` - Do not delete existing documents before appending new ones. <br>  `false` - Delete existing document versions before appending documents.|

{NOTE/}

---

### Add an Elasticsearch Connection String

* An Elasticsearch connection string includes a list of **Elasticsearch destinations URLs**, 
  and determines the **Authentication Method** required to access them.  
   * Omit the Authentication property if the Elasticsearch destination requires no authentication.  
   * Add a connection string as shown below.  

**Code Sample**:  
{CODE create-connection-string@ClientApi\Operations\AddEtl.cs /}
{NOTE: Connection String Object}  

* `ElasticSearchConnectionString`  

    | Property | Type | Description |
    |:-------------|:-------------|:-------------|
    | **Name** | `string` | Connection string Name |
    | **Nodes** | `string[]` | A list of URLs to Elasticsearch destinations |
    | **Authentication** | `Authentication` | Optional authentication method <br> (Do not use when no authentication is required) |

* `Authentication` (Authentication methods)  

    | Property | Type | Description |
    |:-------------|:-------------|:-------------|
    | **Basic** | `BasicAuthentication` | Authenticate connection by **username** and **password** |
    | **ApiKey** | `ApiKeyAuthentication` | Authenticate connection by an **API key** |
    | **Certificate** | `CertificateAuthentication` | Authenticate connection by **certificate** |

* `BasicAuthentication` (Authenticate transfers by **user name** and **password**)  

      | Property | Type |
      |:-------------|:-------------|
      | **Username** | **string** |
      | **Password** | **string** |

* `ApiKeyAuthentication` (Authenticate transfers by an **API key**)  

    | Property | Type |
    |:-------------|:-------------|
    | **ApiKeyId** | `string` |
    | **ApiKey** | `string` |

* `CertificateAuthentication` (Authenticate transfers by **certificate**)  

    | Property | Type | Description |
    |:-------------|:-------------|:-------------|
    | **CertificatesBase64** | `string[]` | A valid certificate string |

{NOTE/}

{PANEL/}

{PANEL: Supported Elasticsearch Versions}
RavenDB supports **Elasticsearch Server version 7 and up**.  
{PANEL/}

## Related Articles

### ETL

- [ETL Basics](../../../server/ongoing-tasks/etl/basics)
- [SQL ETL Task](../../../server/ongoing-tasks/etl/sql)
- [RavenDB ETL Task](../../../server/ongoing-tasks/etl/raven)

### Client API

- [How to Add ETL](../../../client-api/operations/maintenance/etl/add-etl)
- [How to Update ETL](../../../client-api/operations/maintenance/etl/update-etl)
- [How to Reset ETL](../../../client-api/operations/maintenance/etl/reset-etl)

### Studio

- [Define Elasticsearch ETL Task in Studio](../../../studio/database/tasks/ongoing-tasks/elasticsearch-etl-task)
- [Define OLAP ETL Task in Studio](../../../studio/database/tasks/ongoing-tasks/olap-etl-task)
