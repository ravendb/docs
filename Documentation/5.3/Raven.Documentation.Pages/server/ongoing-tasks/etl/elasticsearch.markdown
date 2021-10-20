# Ongoing Tasks: Elasticsearch ETL

---

{NOTE: }

* An Elasticsearch [ETL](../../../server/ongoing-tasks/etl/basics) task **Extracts** chosen documents from RavenDB, 
  **Transforms** them by a user defined transformation script, and **Loads** the documents to Elasticsearch.  

* You can define an Elasticsearch ETL task using [Studio](../../../studio/database/tasks/ongoing-tasks/elasticsearch-etl-task#elasticsearch-indexes) 
  or your [client](../../../client-api/operations/maintenance/etl/add-etl#example---add-elasticsearch-etl-task).  

* The task sends Elasticsearch -  
   * a [_refresh](https://www.elastic.co/guide/en/elasticsearch/reference/current/indices-refresh.html) 
     comnmand, to ensure that the index Elasticsearch uses with RavenDB documents is updated.  
   * an [optional](../../../studio/database/tasks/ongoing-tasks/elasticsearch-etl-task#elasticsearch-indexes) 
     [_delete_by_query](https://www.elastic.co/guide/en/elasticsearch/reference/current/docs-delete-by-query.html) 
     command, to delete existing document versions before appending new ones.  
   * a [_bulk ](https://www.elastic.co/guide/en/elasticsearch/reference/current/docs-bulk.html) command 
     to append the transformed documents to the Elasticsearch destination.  
     
* XXXsupported versions: reebXXX

* In this page:  
  * [Client API](../../../server/ongoing-tasks/etl/elasticsearch#client-api)  
     * [Add an Elasticsearch Connection String](../../../server/ongoing-tasks/etl/elasticsearch#add-an-elasticsearch-connection-string)  
     * [Add an Elasticsearch ETL Task](../../../server/ongoing-tasks/etl/elasticsearch#add-an-elasticsearch-etl-task)  
  * [Transform Script](../../../server/ongoing-tasks/etl/olap#transform-script)  
  * [Athena Examples](../../../server/ongoing-tasks/etl/olap#athena-examples)  

{NOTE/}

---

{PANEL: Client API}

Creating an Elasticsearch ETL task through the client is very similar to the creation of 
RavenDB, SQL, and OLAP ETL tasks. First we need to prepare a **Connection String** (see below) 
to an Elasticsearch destination, and then we need to 
[add the task](../../../server/ongoing-tasks/etl/elasticsearch#add-an-elasticsearch-etl-task).  

## Add an Elasticsearch Connection String

Create a connection string to an Elasticsearch destination as shown below, or use an existing connection string.  
An Elasticsearch connection string determines not only the destination's URL, but also the authentication method 
the client would use to connect it.  
{CODE create-connection-string@ClientApi\Operations\AddEtl.cs /}
{NOTE: Connection String Properties}
* `ElasticSearchConnectionString` (the configuration for each ETL task destination) =  

    | Property | Type | Description |
    |:-------------|:-------------|:-------------|
    | `Name` | `string` | Connection string Name |
    | `Nodes` | `string[]` | The RavenDB Document property by which transferred documents are stored in Elasticsearch. |
    | `Authentication` | `Authentication` | Optional authentication method selection, if required. Can be: <br> **ApiKey** (type: `ApiKeyAuthentication`), <br> **Basic** (type: `BasicAuthentication`), <br> **Certificate** (type: `CertificateAuthentication`). |

* `BasicAuthentication` (to authenticate transfers by **user name** and **password**)  

      | Property | Type |
      |:-------------|:-------------|
      | `Username` | `string` |
      | `Password` | `string` |

* `ApiKeyAuthentication` (to authenticate transfers by an **API key**)  

    | Property | Type |
    |:-------------|:-------------|
    | `ApiKeyId` | `string` |
    | `ApiKey` | `string` |

* `CertificateAuthentication` (to authenticate transfers by **certificate**)  

    | Property | Type | Description |
    |:-------------|:-------------|:-------------|
    | `CertificatesBase64` | `string[]` | A valid certificate string |

{NOTE/}

## Add an Elasticsearch ETL Task  

Use the [AddEtlOperation](../../../client-api/operations/maintenance/etl/add-etl) API method to add 
an Elasticsearch ETL task, passing it an [ElasticSearchEtlConfiguration](../../../server/ongoing-tasks/etl/elasticsearch#section) 
instance with the task's settings and transformation script as shown below.  
{CODE add_elasticsearch_etl@ClientApi\Operations\AddEtl.cs /}
{NOTE: Task Properties}

* `ElasticSearchEtlConfiguration`  

    | Property | Type | Description |
    |:-------------|:-------------|:-------------|
    | `ConnectionStringName` | `string` | The name of the connection string used by this task |
    | `Name` | `string` | ETL Task Name |
    | `ElasticIndexes` | `List<ElasticSearchIndex>` | A list of Elasticsearch indexes (see below) |
    | `Transforms ` | `List<Transformation>` | A list of transformation scripts |

* `ElasticSearchIndex` (a list of indexes used by the task) -  

    | Property | Type | Description |
    |:-------------|:-------------|:-------------|
    | `IndexName` | `string` | Elasticsearch Index name |
    | `IndexIdProperty` | `string` | The RavenDB Document property by which transferred documents are stored in Elasticsearch. |
    | `InsertOnlyMode` | `bool` | `true` - Do not delete existing documents before appending new ones. <br>  `false` - Delete existing document versions before appending documents.|

{NOTE/}

{PANEL/}

{PANEL: Transform Script}

Transformation scripts are similar to those in the RavenDB ETL and SQL ETL tasks, 
learn about them [here](../../../server/ongoing-tasks/etl/basics#transform).  

* As in other ETL tasks, an Elasticsearch transformation script passes data to Elasticsearch 
  using a [loadTo\\<Target\\>(obj)](../../../../server/ongoing-tasks/etl/basics#transform) command.  
    * `Target` is the name of the Elasticsearch index to which the data is transferred.  
      Make sure that the indexes you use are defined in the  
    * `obj` is the object to be passed to Elasticsearch.  




* The value Make sure that the IndexIdProperty of every index defined, 
  matches a RavenDB document property name that is passed to Elasticsearch 
  in the transformation script.  
  Documents will be stored on the Elasticsearch destination using this 
  property as an ID, and then deleted and modified by it.  
  
  
  indexes defined in any index you define using `y ID The major difference is that data output 
by the ETL task can be divided into folders and child folders called _partitions_. Querying the data usually involves scanning 
the entire folder, so there is an advantage in efficiency to dividing the data into more folders.  

#### The `key` Parameter

As with other ETL tasks, the method that actually loads an entry to its destination is `loadTo<folder name>()`, 
but unlike the other ETL tasks the method takes two parameters: the entry itself, and an additional 'key'. 
This `key` determines how many partitions there are and what their names are.  

{CODE-BLOCK: javascript}
loadTo<folder name>(key, object)
{CODE-BLOCK/}

The method's name determines the name of the parent folder that the method outputs to. If you want to output 
data to a folder called "Sales", use the method `loadToSales()`. The parameter key determines the names of 
one or more layers of child folders that contain the actual destination table.  

The actual value that you pass as the `key` for `loadTo<folder name>()` is one of two methods:  

* `partitionBy()` - creates one or more child folders (one inside the other).  
* `noPartition()` - creates no child folders.  

The child folders created by OLAP ETL are considered a sort of 'virtual column' of the destination table. 
This just means that all child folder names have this format: `[virtual column name]=[partition value]`, 
i.e. two strings separated by a `=`. The default virtual column name is `_partition`.  

`partitionBy()` can take one or more folder names in the following ways:  

* **`partitionBy(key)`** - takes a partition value and uses the default virtual column 
name `_partition`. The partition value can be a string, number, date, etc.
* **`partitionBy(['name', key])`** - takes a virtual column name and a partition value as an array of size two.  
* **`partitonBy(['name1', key1], ['name2', key2], ... )`** - takes multiple arrays of size two, each with a virtual 
column name and a partition value. Each pair represents a child folder of the preceding pair.  

Here are examples of possible values for `partitionBy()`, and the resulting folder names:  

{CODE-BLOCK: javascript}
loadToMyFolder(
    partitionBy('one'),
    object
)
//Loads the data to /MyFolder/_partition=one/

loadToMyFolder(
    partitionBy(['month', 'August']),
    object
)
//Loads the data to /MyFolder/month=August/

loadToMyFolder(
    partitionBy(['month', 'August'], ['day', '22'], ['hour', '17']),
    object
)
//Loads the data to /MyFolder/month=August/day=22/hour=17

loadToMyFolder(
    partitionBy(this.Company),
    object
)
// Loads the data to e.g. /MyFolder/_partition=Apple

loadToMyFolder(
    partitionBy(['month', new Date(this.OrderedAt).getMonth()]),
    obj
)
//Loads the data to e.g. /MyFolder/month=8
{CODE-BLOCK/}

#### The Custom Partition Value

The custom partition value is a string value that can be set in the 
[`OlapEtlConfiguration` object](../../../server/ongoing-tasks/etl/olap#section). This value can be 
referenced in the transform script as `$customPartitionValue`. This setting gives you another way 
to distinguish data from different ETL tasks that use the same transform script.  

Suppose you want to create multiple OLAP ETL tasks that all use the same transform script and 
connection string. All the tasks will output to the same destination folders, but suppose you 
want to be able to indicate which data came from which task. This custom partition value gives 
you a simple way to achieve this: all the tasks can run the same script, and each script can 
output the data to a destination folder with the name determined by that task's custom partition 
value setting.  

{CODE-BLOCK: javascript}
partitionBy(['source_ETL_task', $customPartitionValue])
{CODE-BLOCK/}

In the case of multiple partitions, the custom partition value can be used more than once, and it 
can appear anywhere in the folder structure.  

#### Script Example

{CODE script@Server\OngoingTasks\ETL\ElasticsearchETL.cs /}

{PANEL/}

{PANEL: Athena Examples}

Athena is a SQL query engine in the AWS environment that can both read directly from S3 buckets and 
output to S3 buckets.  

Here are a few examples of queries you can run in Athena. But first you need to configure the 
destination for your query results: go to settings, and under "query result location" input the path 
to your preferred bucket. [Read more here](https://docs.aws.amazon.com/athena/latest/ug/querying.html#query-results-specify-location-console)

Create a `monthly_sales` table from parquet data stored in s3:
{CODE-BLOCK: sql}
CREATE EXTERNAL TABLE mydatabase.monthly_sales (
    `_id` string,
    `Qty` int,
    `Product` string,
    `Cost` int,
    `_lastModifiedTime` int
)
PARTITIONED BY (`dt` string)
STORED AS parquet
LOCATION 's3://ravendb-test/olap/tryouts/data/Sales'
{CODE-BLOCK/}

Load all partitions:
{CODE-BLOCK: sql}
MSCK REPAIR TABLE monthly_sales
{CODE-BLOCK/}

Select everything in the table:
{CODE-BLOCK: sql}
select *
from monthly_sales
{CODE-BLOCK/}

Select specific fields:
{CODE-BLOCK: sql}
select _id orderId, qty quantity, product, cost
from monthly_sales
{CODE-BLOCK/}

Filter based on product name:
{CODE-BLOCK: sql}
select *
from monthly_sales
where product = 'Products/2'
{CODE-BLOCK/}

Filter based on date (this is where partitioning adds efficiency - only the relevant folders are scanned):
{CODE-BLOCK: sql}
select *
from monthly_sales
where dt >= '2020-01-01' and dt <= '2020-02-01'
{CODE-BLOCK/}

From all items sold, select the maximum cost (price) per *order*:
{CODE-BLOCK: sql}
select _id orderId, max(cost) cost
from monthly_sales
group by _id
{CODE-BLOCK/}

Querying for most recent version in an append-only table:
e.g. select everything in the table, and in case we have duplicates (multiple rows with the same id)
- only take the most recent version (the one with the highest _lastModifiedTime):
{CODE-BLOCK: sql}
SELECT DISTINCT o.*
FROM monthly_orders o
INNER JOIN
   (SELECT _id,
        MAX(_lastModifiedTime) AS latest
   FROM monthly_orders
   GROUP BY  _id) oo
   ON o._id = oo._id
       AND o._lastModifiedTime = oo.latest
{CODE-BLOCK/}

#### Apache Parquet

Parquet is an open source text-based file format. Like [ORC](https://orc.apache.org/), columns are stored together 
instead or rows being stored together (the same fields from multiple documents, rather than 
whole documents). This makes queries more efficient.  

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
