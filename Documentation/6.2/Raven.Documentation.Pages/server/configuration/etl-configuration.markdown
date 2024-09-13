# Configuration: ETL Options
---

{NOTE: }

* In this page:  
    * [ETL.ExtractAndTransformTimeoutInSec](../../server/configuration/etl-configuration#etl.extractandtransformtimeoutinsec)
    * [ETL.MaxBatchSizeInMb](../../server/configuration/etl-configuration#etl.maxbatchsizeinmb)
    * [ETL.MaxFallbackTimeInSec](../../server/configuration/etl-configuration#etl.maxfallbacktimeinsec)
    * [ETL.MaxNumberOfExtractedDocuments](../../server/configuration/etl-configuration#etl.maxnumberofextracteddocuments)
    * [ETL.MaxNumberOfExtractedItems](../../server/configuration/etl-configuration#etl.maxnumberofextracteditems)
    * [ETL.OLAP.MaxNumberOfExtractedDocuments](../../server/configuration/etl-configuration#etl.olap.maxnumberofextracteddocuments)
    * [ETL.Queue.AzureQueueStorage.TimeToLiveInSec](../../server/configuration/etl-configuration#etl.queue.azurequeuestorage.timetoliveinsec)
    * [ETL.Queue.AzureQueueStorage.VisibilityTimeoutInSec](../../server/configuration/etl-configuration#etl.queue.azurequeuestorage.visibilitytimeoutinsec)
    * [ETL.Queue.Kafka.InitTransactionsTimeoutInSec](../../server/configuration/etl-configuration#etl.queue.kafka.inittransactionstimeoutinsec)
    * [ETL.SQL.CommandTimeoutInSec](../../server/configuration/etl-configuration#etl.sql.commandtimeoutinsec)

{NOTE/}

---

{PANEL: ETL.ExtractAndTransformTimeoutInSec}

Number of seconds after which extraction and transformation will end and loading will start.

- **Type**: `int`
- **Default**: `30`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL: ETL.MaxNumberOfExtractedDocuments}

* Max number of extracted documents in an ETL batch.
* If value is not set, or set to null, the number of extracted documents fallbacks to `ETL.MaxNumberOfExtractedItems` value.

---

- **Type**: `int`
- **Default**: `8192`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL: ETL.MaxBatchSizeInMb}

* Maximum size in megabytes of a batch of data (documents and attachments) that will be sent to the destination as a single batch after transformation.
* If value is not set, or set to null, the size of the batch isn't limited in the processed ETL batch.

---

- **Type**: `Size`
- **Size Unit**: `Megabytes`
- **Default**: `64`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL: ETL.MaxFallbackTimeInSec}

* Maximum number of seconds the ETL process will be in a fallback mode after a load connection failure to a destination.
* The fallback mode means suspending the process.

---

- **Type**: `int`
- **Default**: `900`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL: ETL.MaxNumberOfExtractedItems}

* Max number of extracted items (documents, counters, etc) in an ETL batch.
* If value is not set, or set to null, the number of extracted items isn't limited in the processed ETL batch.

---

- **Type**: `int`
- **Default**: `8192`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL: ETL.OLAP.MaxNumberOfExtractedDocuments}

Max number of extracted documents in OLAP ETL batch.

- **Type**: `int`
- **Default**: `64 * 1024`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL: ETL.Queue.AzureQueueStorage.TimeToLiveInSec}

Lifespan of a message in the queue.

- **Type**: `int`
- **Default**: `604800` (7 days)
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL: ETL.Queue.AzureQueueStorage.VisibilityTimeoutInSec}

How long a message is hidden after being retrieved but not deleted.

- **Type**: `int`
- **Default**: `0`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL: ETL.Queue.Kafka.InitTransactionsTimeoutInSec}

Timeout to initialize transactions for the Kafka producer.

- **Type**: `int`
- **Default**: `60`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL: ETL.SQL.CommandTimeoutInSec}

Number of seconds after which the SQL command will timeout.

- **Type**: `int`
- **Default**: `null` (use provider default)
- **Scope**: Server-wide or per database

{PANEL/}
