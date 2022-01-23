# Operations: How to Add ETL

To learn more about ETL (Extract, Transfer, Load) ongoing tasks, see this article on [ETL Basics](../../../../server/ongoing-tasks/etl/basics).  

You can add ETL task by using **AddEtlOperation**.  

To manage them in the Studio read this article about [ETLs and how to add them via the GUI Studio](../../../../studio/database/tasks/ongoing-tasks/ravendb-etl-task).  

---

In this page:

* [Syntax](../../../../client-api/operations/maintenance/etl/add-etl#syntax)  
* [Add Raven ETL](../../../../client-api/operations/maintenance/etl/add-etl#add-raven-etl)  
    * [Connection String for Raven ETL](../../../../client-api/operations/maintenance/etl/add-etl#connection-string-for-raven-etl)  
    * [Code Sample to Add Raven ETL](../../../../client-api/operations/maintenance/etl/add-etl#code-sample-to-add-raven-etl)  

* [Add Sql ETL](../../../../client-api/operations/maintenance/etl/add-etl#add-sql-etl)  
    * [Connection String for Sql ETL](../../../../client-api/operations/maintenance/etl/add-etl#connection-string-for-sql-etl)  
    * [Code Sample to Add Sql ETL](../../../../client-api/operations/maintenance/etl/add-etl#code-sample-to-add-sql-etl)  



You can add ETL task by using **AddEtlOperation**.

## Syntax

{CODE add_etl_operation@ClientApi\Operations\AddEtl.cs /}

| Parameters | | |
| ------------- | ----- | ---- |
| **configuration** | `EtlConfiguration<T>` | ETL configuration where `T` is connection string type |

## Add Raven ETL
Add [Raven ETL](../../../../server/ongoing-tasks/etl/raven)

**Secure servers**  
In addition to defining a connection string, to connect secure RavenDB servers you must [export the server certificate](../../../../server/security/authentication/certificate-management) 
from the source server and install it into the destination server.  

After passing the certificate, you can either create an ETL with a connection string and transform script [via the studio](../../../../studio/database/tasks/ongoing-tasks/ravendb-etl-task) 
or with the following API.  


### Connection String for Raven ETL

**Secure servers**  
To connect a secure RavenDB server you need to [export the certificate](../../../../server/security/authentication/certificate-management) from the source server and install it into the destination server.  
Be sure that the node definition in the connection string has the "s" in https:  

{CODE raven_etl_connection_string@ClientApi\Operations\AddEtl.cs /}

### Code Sample to Add Raven ETL

{CODE add_raven_etl@ClientApi\Operations\AddEtl.cs /}

## Add Sql ETL
Add [SQL ETL](../../../../server/ongoing-tasks/etl/sql)

### Connection String for Sql ETL

{CODE sql_etl_connection_string@ClientApi\Operations\AddEtl.cs /}

### Code Sample to Add Sql ETL

{CODE add_sql_etl@ClientApi\Operations\AddEtl.cs /}

## Related Articles

### ETL

- [Basics](../../../../server/ongoing-tasks/etl/basics)
- [RavenDB ETL](../../../../server/ongoing-tasks/etl/raven)
- [SQL ETL](../../../../server/ongoing-tasks/etl/sql)

### Studio

- [RavenDB ETL Task](../../../../studio/database/tasks/ongoing-tasks/ravendb-etl-task)
