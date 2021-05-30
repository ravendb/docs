# Ongoing Tasks: OLAP ETL

---

{NOTE: }

* The **OLAP ETL task** creates an ETL process from a RavenDB database to an [AWS S3 bucket](https://aws.amazon.com/s3/), 
a type of storage available on Amazon Web Services.  

* The data is encoded in the [Apache Parquet format](https://parquet.apache.org/documentation/latest/), 
an alternative to CSV that is much faster to query. Unlike CSV, Parquet groups the data according to its 
column (by field) instead of by row (by document).  

* The data can then be queried using [AWS Athena](https://aws.amazon.com/athena/), an SQL query engine 
that both reads from S3 buckets and outputs to them.  

* In this page:  
  * [Client API](../../../server/ongoing-tasks/etl/olap#client-api)  
  * [Transform Script](../../../server/ongoing-tasks/etl/olap#transform-script)  
  * [Athena Examples](../../../server/ongoing-tasks/etl/olap#athena-examples)  

{NOTE/}

---

{PANEL: Client API}

Creating an OLAP ETL task through the client is very similar to creating a RavenDB or SQL ETL task. 
All cases use [the `AddEtlOperation`](../../../client-api/operations/maintenance/etl/add-etl). For 
OLAP you will need an `OlapEtlConfiguration` which itself needs an `OlapConnectionString`. Their 
configuration options are listed below.  

This is an example of a basic OLAP ETL creation operation:  

{CODE add_olap_etl@ClientApi\Operations\AddEtl.cs /}

#### `OlapEtlConfiguration`

| Property | Type | Description |
| - | - | - |
| `RunFrequency` | `string` | Takes a [cron expression](https://docs.oracle.com/cd/E12058_01/doc/doc.1014/e12030/cron_expressions.htm) which determines how often the server will execute the ETL process. |
| `CustomPartitionValue` | `string` | A value that can be used as a partition name in multiple scripts. See [below](). |
| `OlapTables` | `List<OlapEtlTable>` | List of naming configurations for individual tables. See more details below. |

#### `OlapConnectionString`

The OLAP connection string can configure S3 storage, local storage, or both.  

| Property | Type | Description |
| - | - | - |
| `LocalSettings` | `LocalSettings` | Settings for storing the data locally. |
| `S3Settings` | `S3Settings` | Information about the S3 bucket and the AWS server in general. |

{NOTE: ETL Destination Settings}

#### `LocalSettings`

| Property | Type | Description |
| - | - | - |
| `FolderPath` | `string` | Path to local folder. If not empty, backups will be held in this folder and not deleted. Otherwise, backups will be created in the TempDir of a database and deleted after successful upload to S3. |  

#### `S3Settings`

| Property | Type | Description |
| - | - | - |
| `AwsAccessKey` | `string` | Main certificate for the AWS server |
| `AwsSecretKey` | `string` | Encryption certificate for the AWS server |
| `AwsSessionToken` | `string` | AWS session token |
| `AwsRegionName` | `string` | The AWS server region |
| `RemoteFolderName` | `string` | Name of the S3 partition folder |
| `BucketName` | `string` | The name of the S3 bucket that is the destination for this ETL |
| `CustomServerUrl` | `string` | The custom URL to the S3 bucket, if you have one |

{NOTE/}

#### `OlapEtlTable`

Optional, more detailed naming configuration.  

| Property | Type | Description |
| - | - | - |
| `TableName` | `string` | The name of the table. This should usually be the name of the source collection. |
| `DocumentIdColumn` | `string` | A name for the id column of the table. Default: "_id" |
| `PartitionColumn` | `string` | A name for the column that indicates which partition a given row is in. Default: "_dt" |

The folder name consists of a customized 'tag' plus the date-time value `key`.

To actually create the ETL task, use the function `AddEtlOperation()` with an `OlapConnectionString`. 
The connection string can be created for either a remote S3 or a local folder.  

{CODE connection_string@Server\OngoingTasks\ETL\OlapETL.cs /}

#### ETL Run Frequency

Unlike other ETL tasks, OLAP ETL operates only in batches at regular intervals, rather than triggering a 
new round every time a document updates.  
If a document has been updated after ETL (even if updated data has not actually been loaded) they are 
distinguished by `_lastmodifiedticks`, the value of the `last-modified` field in a document's 
metadata, measured in ticks (1/10,000th of a second). This field appears as another column in the S3 
tables.  

{PANEL/}

{PANEL: Transform Script}

Transformation scripts are similar to those in the RavenDB ETL and SQL ETL tasks - see more about this in 
[ETL Basics](../../../server/ongoing-tasks/etl/basics#transform). The major difference is the way 
the data is partitioned at the destination: data extracted from the same collection can be further divided 
into folders and child folders. Querying the data usually involves scanning the entire folder, so there is 
an efficiency advantage to using more folders.  

#### The `key` Parameter

As with other ETL tasks, the method that actually loads an entry to its destination is `loadTo<folder name>()`,
but unlike the other ETL tasks the method takes two parameters: the object itself and an additional key. 
The key determines one or more layers of child folders that contain the actual destination table.  

{CODE-BLOCK: javascript}
loadTo<folder name>(key, object)
{CODE-BLOCK/}

The child folders created by OLAP ETL are a sort of 'virtual column'. This just means that in the address 
or URL of the folder, the folder name looks like this: `/[virtual column name]=[key]/`. You don't have to 
set the virtual column name - its default value is `_partition`.  

The actual value that you pass as the `key` loadTo<folder name> is one of two methods:  
* `partitionBy(key)` - takes one or more child folder names.  
* `noPartition()` - creates no child folders.  

`partitionBy()` can take the following types of values:  
* One `string` which serves as partition name.  
* A pair of `string` which are the virtual column name and partition name. Written like this: `[string, string]`  
* A list of the two types of values above.  

Here is an example of possible values for `partitionBy()` and the resulting folder names:  

{CODE-BLOCK: javascript}
loadToMyFolder(
    partitionBy('one'),
    object
)
//Loads the object to /MyFolder/_partition=one/

loadToMyFolder(
    partitionBy(['month', 'August']),
    object
)
//Loads the object to /MyFolder/month=August/

loadToMyFolder(
    partitionBy('byMonth', ['month', 'August'], ['week', 'two'], 'Monday'),
    object
)
//Loads the object to /MyFolder/_partition=byMonth/month=August/week=two/_partition=Monday/
{CODE-BLOCK/}

#### Script Example

{CODE script@Server\OngoingTasks\ETL\OlapETL.cs /}

!!!$customPartitionValue

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
    `_lastModifiedTicks` bigint
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

Same as the above query, but this time we're only taking the top 20 results
(order by 'cost' from highest to lowest, limit to 20 results):
{CODE-BLOCK: sql}
select _id orderId, max(cost) cost
from monthly_sales
group by _id
order by cost desc
limit 20
{CODE-BLOCK/}

Querying for most recent version in an append-only table:
e.g. select everything in the table, and in case we have duplicates (multiple rows with the same id)
- only take the most recent version (the one with the highest _lastmodifiedticks):
{CODE-BLOCK: sql}
SELECT DISTINCT o.*
FROM monthly_orders o
INNER JOIN
   (SELECT _id,
        MAX(_lastmodifiedticks) AS latest
   FROM monthly_orders
   GROUP BY  _id) oo
   ON o._id = oo._id
       AND o._lastmodifiedticks = oo.latest
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
