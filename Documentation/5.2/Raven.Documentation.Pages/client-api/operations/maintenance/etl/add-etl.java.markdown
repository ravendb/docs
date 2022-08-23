# Operations: How to Add ETL

You can add ETL task by using **AddEtlOperation**.

## Syntax

{CODE:java add_etl_operation@ClientApi\Operations\AddEtl.java /}

| Parameters | | |
| ------------- | ----- | ---- |
| **configuration** | `EtlConfiguration<T>` | ETL configuration where `T` is connection string type |

## Example - Add Raven ETL

{NOTE: Secure servers}
 To [connect secure RavenDB servers](../../../../server/security/authentication/certificate-management#enabling-communication-between-servers:-importing-and-exporting-certificates) 
 you need to 

  1. Export the server certificate from the source server. 
  2. Install it as a client certificate on the destination server.  

 This can be done in the RavenDB Studio -> Server Management -> [Certificates view](../../../../server/security/authentication/certificate-management#studio-certificates-management-view).
{NOTE/}


{CODE:java add_raven_etl@ClientApi\Operations\AddEtl.java /}

## Example - Add Sql ETL

{CODE:java add_sql_etl@ClientApi\Operations\AddEtl.java /}

## Example - Add OLAP ETL

{CODE:java add_olap_etl@ClientApi\Operations\AddEtl.java /}

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
