# Operations: How to Add ETL

This API article demonstrates how to create various ETL tasks and the connection strings required.  

* You can add an ETL task by using the [**AddEtlOperation** method](../../../../client-api/operations/maintenance/etl/add-etl#addetloperation).  

* ETL tasks are ongoing tasks that:
  1. Extract selected data from your source database when changes are made or new data is added.
  2. Apply a transform script on the data.
  3. Load the transformed data to a destination that you designate.  

* To learn more about ETL (Extract, Transfer, Load) ongoing tasks, see this article on [ETL Basics](../../../../server/ongoing-tasks/etl/basics).  

* To learn how to manage ETL tasks using the Studio see: [RavenDB ETL Task](../../../../studio/database/tasks/ongoing-tasks/ravendb-etl-task).  

---

In this page:

* [AddEtlOperation](../../../../client-api/operations/maintenance/etl/add-etl#addetloperation)  
* [Add Raven ETL](../../../../client-api/operations/maintenance/etl/add-etl#add-raven-etl)  
    * [Creating a Connection String for Raven ETL](../../../../client-api/operations/maintenance/etl/add-etl#creating-a-connection-string-for-raven-etl)  
    * [Code Sample to Add Raven ETL](../../../../client-api/operations/maintenance/etl/add-etl#code-sample-to-add-raven-etl)  

* [Add Sql ETL](../../../../client-api/operations/maintenance/etl/add-etl#add-sql-etl)  
    * [Creating a Connection String for Sql ETL](../../../../client-api/operations/maintenance/etl/add-etl#creating-a-connection-string-for-sql-etl)  
    * [Code Sample to Add Sql ETL](../../../../client-api/operations/maintenance/etl/add-etl#code-sample-to-add-sql-etl)  



You can add ETL task by using the **AddEtlOperation** method.

## AddEtlOperation

{CODE add_etl_operation@ClientApi\Operations\AddEtl.cs /}

| Parameters | | |
| ------------- | ----- | ---- |
| **configuration** | `EtlConfiguration<T>` | ETL configuration where `T` is connection string type |

## Add Raven ETL

[Raven ETL](../../../../server/ongoing-tasks/etl/raven) tasks enable ongoing Extract, Transform, Load functionality from a RavenDB source database to a RavenDB destination.  

### Creating a Connection String for Raven ETL

* **Secure clusters**  
  In addition to defining a connection string, to connect secure RavenDB clusters you must [export the server certificate](../../../../server/security/authentication/certificate-management#enabling-communication-between-servers:-importing-and-exporting-certificates) 
  from the source cluster and install it into the destination cluster.  

* After passing the certificate, you can either create an ETL with a connection string and transformation script [via the studio](../../../../studio/database/tasks/ongoing-tasks/ravendb-etl-task) 
  or with the following API.  
  
{CODE raven_etl_connection_string@ClientApi\Operations\AddEtl.cs /}

### Code Sample to Add Raven ETL

{CODE add_raven_etl@ClientApi\Operations\AddEtl.cs /}

## Add Sql ETL

[SQL ETL](../../../../server/ongoing-tasks/etl/sql) tasks enable ongoing Extract, Transform, Load functionality from RavenDB to SQL servers. 


### Creating a Connection String for Sql ETL

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
