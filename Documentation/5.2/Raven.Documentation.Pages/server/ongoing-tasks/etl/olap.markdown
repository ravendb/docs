# Ongoing Tasks: OLAP ETL

---

{NOTE: }

* ETL to Amazon S3 bucket* in Parquet format, useful for OLAP

{NOTE/}

---

{PANEL: }

Not applicable to ssas?

ETL to S3, and thence to Athena.

~Possibly others in the future (presto). Probably ORC in the near future~

Currently only client API, not studio.

ETL script must specify the time. ETL is performed only as batches at regular intervals, rather than being triggered anew each time the database updates
(year, month). S3 is divided into folders, and it's necessary to prevent too much data in one folder because it results in costly ?folder? scans. Time can be set manually or it could be metadata:last modified

folder name = customized 'tag' + date-time key

Connection string -> remote S3 -> local folder

`bool` erase (default = `true`)

if a doc has been updated after ETL (even if updated data is not actually loaded) they are distinguished by lastmodified *ticks*

{PANEL/}
