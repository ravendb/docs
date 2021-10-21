# Ongoing Tasks: Elasticsearch ETL

---

{NOTE: }

* An Elasticsearch [ETL](../../../server/ongoing-tasks/etl/basics) task _Extracts_ chosen documents 
  from RavenDB, _Transforms_ them by a user defined transformation script, and _Loads_ the documents 
  to an Elasticsearch destination.  

* You can define an Elasticsearch ETL task using [Studio](../../../studio/database/tasks/ongoing-tasks/elasticsearch-etl-task#elasticsearch-indexes) 
  or your [client](../../../client-api/operations/maintenance/etl/add-etl#example---add-elasticsearch-etl-task).  

* In this page:  
  * [Elasticsearch ETL](../../../server/ongoing-tasks/etl/elasticsearch#elasticsearch-etl)  
  * [Transformation Script](../../../server/ongoing-tasks/etl/elasticsearch#transformation-script)  
  * [Data Delivery](../../../server/ongoing-tasks/etl/elasticsearch#data-delivery)  
     * [What is Transferred](../../../server/ongoing-tasks/etl/elasticsearch#what-is-transferred)  
     * [Transactions](../../../server/ongoing-tasks/etl/elasticsearch#transactions)  
     * [Document Identifiers](../../../server/ongoing-tasks/etl/elasticsearch#document-identifiers)  
     * [Insert Only Mode](../../../server/ongoing-tasks/etl/elasticsearch#insert-only-mode)  
  * [Client API](../../../server/ongoing-tasks/etl/elasticsearch#client-api)  
     * [Add an Elasticsearch ETL Task](../../../server/ongoing-tasks/etl/elasticsearch#add-an-elasticsearch-etl-task)  
     * [Add an Elasticsearch Connection String](../../../server/ongoing-tasks/etl/elasticsearch#add-an-elasticsearch-connection-string)  
  * [Supported Elasticsearch Versions](../../../server/ongoing-tasks/etl/elasticsearch#supported-elasticsearch-versions)  

{NOTE/}

---

{PANEL: Elasticsearch ETL}

* The main phases in crteating an Elasticsearch ETL task are -  
   * Defining a [connection string](../../../server/ongoing-tasks/etl/elasticsearch#add-an-elasticsearch-connection-string) 
     with URLs to Elasticsearch destinations.  
     While defining a connection string, you can also define the 
     authentication method your task would use with Elasticsearch.  
   * **Defining Elasticsearch Indexes** that your task will use.  
     Indexes are used by Elasticsearch to store and locate documents, 
     and the task is required to define the indexes it will access Elasticsearch with.  
     {NOTE: }
      Our task defines Elasticsearch indexes by the index name, and by  
      an identifier that is common to the transferred documents (e.g. 
      the documents ID) by which RavenDB will later on be able to locate 
      these documents.  
     {NOTE/}
   * **Defining Transformation Scripts**.  
     A transformation script is the "heart" of an ETL task, 
     determining which documents would be transferred, where to 
     and in what form.  

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
  The script is used to select the documents the task would _Extract_ from the database, 
  _Transform_ the retrieved data, and _Load_ it to the Elasticsearch destination.  
  Learn about ETL transformation scripts [here](../../../server/ongoing-tasks/etl/basics#transform).  

* The script **Loads** data to the Elasticsearch destination using a 
  [loadTo\\<Target\\>(obj)](../../../server/ongoing-tasks/etl/basics#transform) command.  
   * `Target` is the name of the Elasticsearch index to which the data is transferred.  
      * In your task settings, define Elasticsearch Index names using **lower-case characters**.  
        E.g. **orders**  
      * In your transformation script however, you can define `Target` using higher and lower case 
        characters, as you prefer. (the task will transform the index name to lower-case caharacters 
        while connecting Elasticsearch).  
        E.g. use either **loadToOrders** or **loadToorders**.  
   * `obj` is an object defined by the script, that will be loaded to Elasticsearch.  
     E.g. `orderData` in the following script:  
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

### Transactions

The task delivers the data to the Elasticsearch destination in two or three calls per index.  

1. [_refresh](https://www.elastic.co/guide/en/elasticsearch/reference/current/indices-refresh.html) 
   comnmand, that triggers Elasticsearch to synchronize the index that the ETL task uses with the 
   documents that are actually stored on the destination.  
2. an optional [_delete_by_query](https://www.elastic.co/guide/en/elasticsearch/reference/current/docs-delete-by-query.html) 
   command, to delete existing versions of RavenDB documents from Elasticsearch before appending new ones.  
   {CODE-BLOCK: JavaScript}
   POST orders/_delete_by_query
{"query":{"terms":{"Id":["orders/1-a"]}}}
     {CODE-BLOCK/}
3. [_bulk ](https://www.elastic.co/guide/en/elasticsearch/reference/current/docs-bulk.html) command, 
   to append RavenDB documents to the Elasticsearch destination.  
   {CODE-BLOCK: JavaScript}
   POST orders/_bulk
{"index":{"_id":null}}
{"OrderLinesCount":3,"TotalCost":0,"Id":"orders/1-a"}
     {CODE-BLOCK/}

---

### Document Identifiers

* When Elasticsearch stores RavenDB documents, it provides each of them 
  with an automatically-generated iD.  
* RavenDB needs to delete and replace documents, but it cannot do this 
  using Elasticsearch's arbitrary generated IDs.  
  Instead, it uses one of the document's properties as ID.  
* You need to decide which document property RavenDB would use as a document identifier.  
  To define it:  
   * Set `IndexIdProperty` through code (see [code sample](../../../server/ongoing-tasks/etl/elasticsearch#add-an-elasticsearch-etl-task)).  
   * Or set the [Document ID Property Name](../../../studio/database/tasks/ongoing-tasks/elasticsearch-etl-task#elasticsearch-indexes) field via Studio.  
* The identifier must be a property that the transformation script passes to Elasticsearch.  
  E.g., of the following script, you can use **DocId** as identifier.  
  {CODE-BLOCK: JavaScript}
  var orderData = {
                 DocId: id(this),
                 OrderLinesCount: this.Lines.length,
                 TotalCost: 0
                  };

loadToOrders(orderData);
  {CODE-BLOCK/}

---

### Insert Only Mode

You can enable the task's **Insert Only** mode using [code](../../../server/ongoing-tasks/etl/elasticsearch#add-an-elasticsearch-etl-task) 
or via [Studio](../../../studio/database/tasks/ongoing-tasks/elasticsearch-etl-task#elasticsearch-indexes), 
to **omit** _delete_by_query commands and so refrain from deleting documents before the transfer.  
{NOTE: }
 Enabling Insert Only can boost the task's performance when deleting documents before transfers is not needed.  
{NOTE/}
{WARNING: }
 Be aware that enabling Insert Only mode will append documents to Elasticsearch whenever they 
 are modified on RavenDB, without removing existing documents. If document versions that are not 
 needed accumulate and storage space is a concern, keep Insert Only disables.  
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

* `ElasticSearchIndex` (A list of Elasticsearch indexes)  

    | Property | Type | Description |
    |:-------------|:-------------|:-------------|
    | **IndexName** | `string` | Elasticsearch Index name. <br> Name indexes **using lower-case characters only**, e.g. `orders`. |
    | **IndexIdProperty** | `string` | Documents identifier for RavenDB usage, <br> that is stored on Elasticsearch as a document property.<br> RavenDB delets, stores and modifies documents by it. <br> Must be a property that the script defined and passed Elasticsearch. <br> e.g. **DocID** where the script defines **DocId: id(this)**. |
    | **InsertOnlyMode** | `bool` | `true` - Do not delete existing documents before appending new ones. <br>  `false` - Delete existing document versions before appending documents.|

{NOTE/}

---

### Add an Elasticsearch Connection String

* An Elasticsearch connection string includes a list of **Elasticsearch destinations URLs**, 
  and determines the **Authentication Method** the client needs to access theem.  
   * Omit the Authentication property if the Elasticsearch destination requires no authentication.  
   * Add a connection string as shown below.  

**Code Sample**:  
{CODE create-connection-string@ClientApi\Operations\AddEtl.cs /}
{NOTE: Connection String Properties}  

* `ElasticSearchConnectionString` (the configuration for each ETL task destination) =  

    | Property | Type | Description |
    |:-------------|:-------------|:-------------|
    | **Name** | `string` | Connection string Name |
    | **Nodes** | `string[]` | A list of URLs to Elasticsearch destinations |
    | **Authentication** | `Authentication` | Optional authentication methods |

* `Authentication` (Authentication methods)  

    | Property | Type | Description |
    |:-------------|:-------------|:-------------|
    | **Basic** | `BasicAuthentication` | Authenticate transfers by **user name** and **password** |
    | **ApiKey** | `ApiKeyAuthentication` | Authenticate transfers by an **API key** |
    | **Certificate** | `CertificateAuthentication` | Authenticate transfers by **certificate** |

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

- [Define OLAP ETL Task in Studio](../../../studio/database/tasks/ongoing-tasks/olap-etl-task)
