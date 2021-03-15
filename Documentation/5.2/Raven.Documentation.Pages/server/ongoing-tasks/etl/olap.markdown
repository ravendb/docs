# Ongoing Tasks: OLAP ETL

---

{NOTE: }

* The **OLAP ETL TASK** creates an ETL process from a RavenDB database to an S3 bucket, 
a type of storage available in AWS.  

* The data is stored in the _Parquet_ format, a compact text format analogous to CSV or 
JSON.  

* This storage setup is useful for OLAP.  

{NOTE/}

---

{PANEL: }

### ETL in Batches

ETL is performed only as batches at regular intervals, rather than being triggered anew each time 
the database updates. The ETL script must specify the time frame in which the data for each batch 
was created (or updated). This is because an AWS S3 is divided into folders, and it's necessary 
to prevent too much data accumulating in one folder because queries scan an entire folder at a 
time. Time can be set manually in a `TimeSpan` format, or it could be taken from a document's 
metadata.  

### Client API

Transformation scripts are similar to those in the [RavenDB ETL]() and [SQL ETL]() tasks. In 
order to set the interval for your OLAP ETL, the `loadTo<CollectionName>()` method 
([read more here](../../../server/ongoing-tasks/etl/raven#transformation-script-options)) 
takes an additional parameter `key` with a `TimeSpan` format.  

{CODE add_olap_etl@ClientApi\Operations\AddEtl.cs /}

The folder name consists of a customized 'tag' plus the date-time value.

To actually create the ETL task, use the function `AddEtlOperation()` with and `OlapConnectionString`. 
Connection string can be created for either a remote S3 or a local folder.  

{CODE connection_string@Server\OngoingTasks\ETL\OlapETL.cs /}

If a doc has been updated after ETL (even if updated data is not actually loaded) they are 
distinguished by lastmodified *ticks*.  

### Apache Parquet

Parquet is an open source text-based file format. Like [ORC](), columns are stored together 
instead or rows being stored together (the same fields from multiple documents, rather than 
whole documents). This makes queries more efficient.  

{PANEL/}

## Related Articles

### ETL

- [ETL Basics](../../../server/ongoing-tasks/etl/basics)
- [SQL ETL Task](../../../server/ongoing-tasks/etl/sql)

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
