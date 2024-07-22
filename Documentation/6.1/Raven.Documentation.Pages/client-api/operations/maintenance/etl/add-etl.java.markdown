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

{CODE:java add_raven_etl@ClientApi\Operations\Maintenance\Etl\AddEtl.java /}

{NOTE: }

**Secure servers**:

To [connect secure RavenDB servers](../../../../server/security/authentication/certificate-management#enabling-communication-between-servers:-importing-and-exporting-certificates)
you need to

1. Export the server certificate from the source server.
2. Install it as a client certificate on the destination server.

This can be done in the RavenDB Studio -> Server Management -> [Certificates view](../../../../server/security/authentication/certificate-management#studio-certificates-management-view).
{NOTE/}

{PANEL/}

{PANEL: Example - add SQL ETL}

{CODE:java add_sql_etl@ClientApi\Operations\Maintenance\Etl\AddEtl.java /}

{PANEL/}

{PANEL: Example - add OLAP ETL}

{CODE:java add_olap_etl@ClientApi\Operations\Maintenance\Etl\AddEtl.java /}

{PANEL/}

{PANEL: Syntax}

{CODE:java add_etl_operation@ClientApi\Operations\Maintenance\Etl\AddEtl.java /}

| Parameter         | Type                  | Description                                           |
|-------------------|-----------------------|-------------------------------------------------------|
| **configuration** | `EtlConfiguration<T>` | ETL configuration where `T` is connection string type |

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
