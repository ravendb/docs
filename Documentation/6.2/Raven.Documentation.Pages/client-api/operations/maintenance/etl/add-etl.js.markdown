# Add ETL Operation
---

{NOTE: }

* Use the `AddEtlOperation` method to add a new ongoing ETL task to your database.

* To learn about ETL (Extract, Transfer, Load) ongoing tasks, see article [ETL Basics](../../../../server/ongoing-tasks/etl/basics).  
  To learn how to manage ETL tasks from the Studio, see [Ongoing tasks - overview](../../../../studio/database/tasks/ongoing-tasks/general-info).

* In this page:
    * [Example - add Raven ETL](../../../../client-api/operations/maintenance/etl/add-etl#example---add-raven-etl)
    * [Example - add SQL ETL](../../../../client-api/operations/maintenance/etl/add-etl#example---add-sql-etl)
    * [Example - add OLAP ETL](../../../../client-api/operations/maintenance/etl/add-etl#example---add-olap-etl)
    * [Syntax](../../../../client-api/operations/maintenance/etl/add-etl#syntax)

{NOTE/}

---

{PANEL: Example - add Raven ETL}

{CODE:nodejs add_raven_etl@client-api\operations\maintenance\etl\addEtl.js  /}

{PANEL/}

{PANEL: Example - add SQL ETL}

{CODE:nodejs add_sql_etl@client-api\operations\maintenance\etl\addEtl.js  /}

{PANEL/}

{PANEL: Example - add OLAP ETL}

{CODE:nodejs add_olap_etl@client-api\operations\maintenance\etl\addEtl.js  /}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs add_etl_operation@client-api\operations\maintenance\etl\addEtl.js  /}

| Parameter         | Type                      | Description                       |
|-------------------|---------------------------|-----------------------------------|
| **configuration** | `EtlConfiguration` object | The ETL task configuration to add |

{CODE:nodejs syntax@client-api\operations\maintenance\etl\addEtl.js  /}

{PANEL/}

## Related Articles

### ETL

- [Basics](../../../../server/ongoing-tasks/etl/basics)
- [RavenDB ETL](../../../../server/ongoing-tasks/etl/raven)
- [SQL ETL](../../../../server/ongoing-tasks/etl/sql)

### Studio

- [RavenDB ETL Task](../../../../studio/database/tasks/ongoing-tasks/ravendb-etl-task)

### Connection Strings

- [Add](../../../../client-api/operations/maintenance/connection-strings/add-connection-string)
- [Get](../../../../client-api/operations/maintenance/connection-strings/get-connection-string)
- [Remove](../../../../client-api/operations/maintenance/connection-strings/remove-connection-string)
