# Add Connection String Operation
---

{NOTE: }

* Use the [PutConnectionStringOperation](../../../../client-api/operations/maintenance/connection-strings/add-connection-string#the%C2%A0putconnectionstringoperation%C2%A0method) method to define a connection string in your database.

* In this page:  
    * [Add a RavenDB connection string](../../../../client-api/operations/maintenance/connection-strings/add-connection-string#add-a-ravendb-connection-string)  
    * [Add an SQL connection string](../../../../client-api/operations/maintenance/connection-strings/add-connection-string#add-an-sql-connection-string)  
    * [Add a Snowflake connection string](../../../../client-api/operations/maintenance/connection-strings/add-connection-string#add-a-snowflake-connection-string)  
    * [Add an OLAP connection string](../../../../client-api/operations/maintenance/connection-strings/add-connection-string#add-an-olap-connection-string)  
    * [Add an Elasticsearch connection string](../../../../client-api/operations/maintenance/connection-strings/add-connection-string#add-an-elasticsearch-connection-string)  
    * [Add a Kafka connection string](../../../../client-api/operations/maintenance/connection-strings/add-connection-string#add-a-kafka-connection-string)  
    * [Add a RabbitMQ connection string](../../../../client-api/operations/maintenance/connection-strings/add-connection-string#add-a-rabbitmq-connection-string)  
    * [Add an Azure Queue Storage connection string](../../../../client-api/operations/maintenance/connection-strings/add-connection-string#add-an-azure-queue-storage-connection-string)  
    * [Add an Amazon SQS connection string](../../../../client-api/operations/maintenance/connection-strings/add-connection-string#add-an-amazon-sqs-connection-string)  
    * [The PutConnectionStringOperation method](../../../../client-api/operations/maintenance/connection-strings/add-connection-string#the%C2%A0putconnectionstringoperation%C2%A0method)

{NOTE/}

---

{PANEL: Add a RavenDB connection string}

RavenDB connection strings are used by RavenDB [RavenDB ETL Tasks](../../../../server/ongoing-tasks/etl/raven).

#### Example:
{CODE add_raven_connection_string@ClientApi\Operations\Maintenance\ConnectionStrings\AddConnectionStrings.cs /}

#### Syntax:
{CODE raven_connection_string@ClientApi\Operations\Maintenance\ConnectionStrings\AddConnectionStrings.cs /}

{NOTE: }

**Secure servers**  

To [connect to secure RavenDB servers](../../../../server/security/authentication/certificate-management#enabling-communication-between-servers:-importing-and-exporting-certificates)
you need to:  
1. Export the server certificate from the source server.  
2. Install it as a client certificate on the destination server.  
  
This can be done from the Studio [Certificates view](../../../../server/security/authentication/certificate-management#studio-certificates-management-view).  

{NOTE/}

{PANEL/}

{PANEL: Add an SQL connection string}

SQL connection strings are used by RavenDB [SQL ETL Tasks](../../../../server/ongoing-tasks/etl/sql).

#### Example:
{CODE add_sql_connection_string@ClientApi\Operations\Maintenance\ConnectionStrings\AddConnectionStrings.cs /}

#### Syntax:
{CODE sql_connection_string@ClientApi\Operations\Maintenance\ConnectionStrings\AddConnectionStrings.cs /}

{PANEL/}

{PANEL: Add a Snowflake connection string}

[Snowflake connection strings](https://github.com/snowflakedb/snowflake-connector-net/blob/master/doc/Connecting.md) 
are used by RavenDB [Snowflake ETL Tasks](../../../../server/ongoing-tasks/etl/snowflake).  

#### Example:
{CODE add_snowflake_connection_string@ClientApi\Operations\Maintenance\ConnectionStrings\AddConnectionStrings.cs /}

{PANEL/}

{PANEL: Add an OLAP connection string}

OLAP connection strings are used by RavenDB [OLAP ETL Tasks](../../../../server/ongoing-tasks/etl/olap).  

#### Example: To a local machine
{CODE add_olap_connection_string_1@ClientApi\Operations\Maintenance\ConnectionStrings\AddConnectionStrings.cs /}

#### Example: To a cloud-based server
  
* The following example shows a connection string to Amazon AWS.  
* Adjust the parameters as needed if you are using other cloud-based servers (e.g. Google, Azure, Glacier, S3, FTP).  
* The available parameters are listed in [ETL destination settings](../../../../server/ongoing-tasks/etl/olap#etl-destination-settings).  

{CODE add_olap_connection_string_2@ClientApi\Operations\Maintenance\ConnectionStrings\AddConnectionStrings.cs /}

#### Syntax:
{CODE olap_connection_string@ClientApi\Operations\Maintenance\ConnectionStrings\AddConnectionStrings.cs /}

{PANEL/}

{PANEL: Add an Elasticsearch connection string}

Elasticsearch connection strings are used by RavenDB [Elasticsearch ETL Tasks](../../../../server/ongoing-tasks/etl/elasticsearch).

#### Example:
{CODE add_elasticsearch_connection_string@ClientApi\Operations\Maintenance\ConnectionStrings\AddConnectionStrings.cs /}

#### Syntax:
{CODE elasticsearch_connection_string@ClientApi\Operations\Maintenance\ConnectionStrings\AddConnectionStrings.cs /}

{PANEL/}

{PANEL: Add a Kafka connection string}

Kafkah connection strings are used by RavenDB [Kafka Queue ETL Tasks](../../../../server/ongoing-tasks/etl/queue-etl/kafka).  
Learn how to add a Kafka connection string in the [Add a Kafka connection string]( ../../../../server/ongoing-tasks/etl/queue-etl/kafka#add-a-kafka-connection-string) section.

{PANEL/}

{PANEL: Add a RabbitMQ connection string}

RabbitMQ connection strings are used by RavenDB [RabbitMQ Queue ETL Tasks](../../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq).  
Learn how to add a RabbitMQ connection string in the [Add a RabbitMQ connection string]( ../../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq#add-a-rabbitmq-connection-string) section.

{PANEL/}

{PANEL: Add an Azure Queue Storage connection string}

Azure Queue Storage connection strings are used by RavenDB [Azure Queue Storage ETL Tasks](../../../../server/ongoing-tasks/etl/queue-etl/azure-queue).  
Learn to add an Azure Queue Storage connection string in the [Add an Azure Queue Storage connection string]( ../../../../server/ongoing-tasks/etl/queue-etl/azure-queue#add-an-azure-queue-storage-connection-string) section.

{PANEL/}

{PANEL: Add an Amazon SQS connection string}

Amazon SQS connection strings are used by RavenDB [Amazon SQS ETL Tasks](../../../../server/ongoing-tasks/etl/queue-etl/aws-sqs).  
Learn to add an SQS connection string in [this section](../../../../server/ongoing-tasks/etl/queue-etl/aws-sqs#add-an-aws-sqs-connection-string).  

{PANEL/}

{PANEL: The&nbsp;`PutConnectionStringOperation`&nbsp;method}

{CODE put_connection_string@ClientApi\Operations\Maintenance\ConnectionStrings\AddConnectionStrings.cs /}

| Parameters           | Type                            | Description                                        |
|----------------------|---------------------------------|----------------------------------------------------|
| **connectionString** | `RavenConnectionString`         | Object that defines the RavenDB connection string. |
| **connectionString** | `SqlConnectionString`           | Object that defines the SQL Connection string. |
| **connectionString** | `SnowflakeConnectionString`     | Object that defines the Snowflake connction string. |
| **connectionString** | `OlapConnectionString`          | Object that defines the OLAP connction string. |
| **connectionString** | `ElasticSearchConnectionString` | Object that defines the Elasticsearch connction string. |
| **connectionString** | `QueueConnectionString`         | Object that defines the connection string for the Queue ETLs tasks (Kafka, RabbitMQ, Azure Queue Storage, and Amazon SQS). |

{CODE connection_string_class@ClientApi\Operations\Maintenance\ConnectionStrings\AddConnectionStrings.cs /}

{PANEL/}

## Related Articles

### Connection Strings

- [Get](../../../../client-api/operations/maintenance/connection-strings/get-connection-string)
- [Remove](../../../../client-api/operations/maintenance/connection-strings/remove-connection-string)

### ETL (Extract, Transform, Load) Tasks

- [Operations: How to Add ETL](../../../../client-api/operations/maintenance/etl/add-etl)
- [Ongoing Tasks: ETL Basics](../../../../server/ongoing-tasks/etl/basics)

### External Replication

- [External Replication Task](../../../../studio/database/tasks/ongoing-tasks/external-replication-task)
- [How Replication Works](../../../../server/clustering/replication/replication)

