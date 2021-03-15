# Ongoing Tasks: OLAP ETL

---

{NOTE: }

* The **OLAP ETL task** creates an ETL process from a RavenDB database to an [AWS S3 bucket](https://aws.amazon.com/s3/), 
a type of storage available on Amazon Web Services.  

* The data is encoded in the [_Parquet_ format](https://parquet.apache.org/documentation/latest/), an 
alternative to CSV that is much faster to query. Unlike CSV, Parquet groups the data according to its 
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

This is an example of a basic OLAP ETL creation operation.  

{CODE add_olap_etl@ClientApi\Operations\AddEtl.cs /}

#### `OlapEtlConfiguration`

| Property | Type | Description |
| - | - | - |
| `RunFrequency` | `TimeSpan` | How often the server will execute the ETL process. This is different from the `TimeSpan` for the partitions |
| `KeepFilesOnDisk` | `bool` | Whether to keep the data in memory after it has been transformed, or to delete it as soon as the ETL completes |
| `PartitionFieldName` | `string` | Name of the partition |
| `MaxNumberOfItemsInRowGroup` | `int?` | The maximum number of items in the row group |
| `CustomPrefix` | `string` | A custom prefix for the folder name. Default: "_dt" |
| `OlapTables` | `List<OlapEtlTable>` | List of naming configurations for individual tables. See more details below. |

#### `OlapConnectionString`

The OLAP connection string can configure S3 storage, local storage, or both.  

| Property | Type | Description |
| - | - | - |
| `LocalSettings` | `LocalSettings` | Settings for storing the data locally. |
| `S3Settings` | `S3Settings` | Information about the S3 bucket and the AWS server in general. |

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

{PANEL/}

{PANEL: Transform Script}

Transformation scripts are similar to those in the [RavenDB ETL](../../../server/ongoing-tasks/etl/raven) and [SQL ETL](../../../server/ongoing-tasks/etl/sql) tasks. The 
major difference has to do with the way S3 storage is partitioned. Records from the same collection 
are further divided into folders. Querying the data involves scanning the entire folder, so the more 
the data is divided into smaller folders, the more efficient querying will be. Data is divided into 
folders according to the time the document was created or when it was last updated. All folders represent 
an equal interval of time - an hour, a day, a month, etc.  

This interval is determined by the transform script with the `TimeSpan` parameter `key`. `key` also 
becomes a part of the name of the folder: the name of each folder is an optional custom "tag", plus the key.  

As with other ETL tasks, the method that actually loads the data to its destination is the `loadTo<TableName>()` 
`loadTo<TableName>()` takes an additional parameter `key` with a `TimeSpan` format.  

{CODE script@Server\OngoingTasks\ETL\OlapETL.cs /}

Unlike other ETL tasks, OLAP ETL operates only in batches at regular intervals, rather than triggering a 
new round every time a document updates.  
If a document has been updated after ETL (even if updated data has not actually been loaded) they are 
distinguished by `_lastmodifiedticks`, the value of the `last-modified` field in a document's 
metadata, measured in ticks (1/10,000th of a second). This field appears as another column in the S3 
tables.  

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

- [Define RavenDB ETL Task in Studio](../../../studio/database/tasks/ongoing-tasks/ravendb-etl-task)

### Document Extensions

- [Attachments](../../../document-extensions/attachments/what-are-attachments)
- [Counters](../../../document-extensions/counters/overview)
- [Time Series](../../../document-extensions/timeseries/overview)
