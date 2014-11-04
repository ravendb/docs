# Troubleshooting : Debug endpoints

{INFO All endpoints that are **not `<system>`-only** can be accessed for particular database by adding `/databases/{database_name}/` prefix. /}

## Overview

| Endpoint | Method | Parameters | Description | `<system>`-only |
| ----- |:-----:|:-----| ----- |:-----:|
| debug/prefetch-status | GET | | Return prefetching statuses for all indexing groups. | |
| debug/format-index | POST | - _(Content)_ JSON-array of indexing functions | Simplifies and formats given indexing functions. | |
| debug/indexing-perf-stats?format={format} | GET | - _format_ - output format: `json` or `csv` (if empty: json) | Indexing performance statistics like duration and input/output count. | |
| debug/plugins | GET | | Returns active plugins e.g. triggers and startup tasks. | |
| [debug/changes](../../server/troubleshooting/debug-endpoints#debugchanges) | GET | | Returns Changes API connection details e.g. connection identifier or subscribtion details. | |
| debug/sql-replication-stats | GET | | Various metrics with SQL Replication performance. | |
| debug/metrics | GET | | All database metrics. | |
| debug/config | GET | | Database configuration file. | |
| debug/docrefs?id={id}&op={op} | GET | - _id_ - id of a document to check<br />- _op_ - operation type: `from` or `to` | Returns count of references and array of referenced document keys. | |
| debug/index-fields | POST | - _(Content)_ - indexing function | Returns array of field names extracted from indexing function. | |
| debug/list?id={id}&key={key} | GET | - _id_ - list name<br />- _key_ - value | Returns value found under _key_ from list. | |
| debug/list-all?id={id} | GET | - _id_ - list name | Returns all values for given list. | |
| debug/queries | GET | | Returns list of currently running queries. | |
| debug/suggest-index-merge | GET | | Returns index merge suggestions. | |
| debug/sl0w-d0c-c0unts | GET | | Counts the number and size of documents for each collection. CAUTION: resource intensive and slow. | |
| debug/user-info | GET | | Returns information about current authenticated user. | |
| debug/tasks | GET | | Returns list of all current database tasks. | |
| debug/routes | GET | | Returns list of all available endpoints. | Y |
| debug/currently-indexing | GET | | Returns current indexing operations details (e.g. what indexes are working and what is the current indexing rate) | |
| debug/request-tracing | GET | | Returns the list of recent requests with detailed info (e.g. headers, execution type). | |
| debug/identities?start={start}&pageSize={pageSize} | GET | - paging parameters | Returns next identity values for collection types, indexes, transformers, etc. | |
| debug/info-package | GET | | Creates debug info package that contains detailed information about the database (e.g. replication information, statistics, queries, requests, hardware information, etc.) | |
| debug/transactions | GET | | Returns information about current DTC transactions. | |
| | | | |
| admin/stats | GET | | Returns server-wide statistics (e.g. server name, uptime, memory statistics, loaded databases information). More [here](../../server/administration/statistics). | |
| admin/gc | GET | | Starts the garbage collection process. | |
| admin/loh-compaction | GET | | Starts the garbage collection with LOH compaction process. | |
| admin/debug/info-package | GET | - _(Optional)_ stacktrace - indicates if stacktraces should be included in package (may freeze server for some time to gather them). | Returns debug info package for all loaded databases. |
| admin/detailed-storage-breakdown | GET | | Returns storage report (e.g. number of documents, indexes, attachments and other storage-dependant statistics). | |
| | | | |
| build/version | GET | | Returns product and build version. | Y |
| | | | |
| indexes/{index_name} ?op=forceWriteToDisk | POST | - _index_name_ - index name | Force in-memory auto-index persistence. | |

## Studio

Most of the endpoints can be accessed from the Studio by accessing the `Debug` section in  the `Status` tab.

![Figure 1: Studio : Debug Endpoints](images\debug-endpoints-studio.png)