# Add ETL Operation
---

{NOTE: }

* Use the `AddEtlOperation` method to add a new ongoing ETL task to your database.  

* To learn about ETL (Extract, Transfer, Load) ongoing tasks, see the [ETL Basics](../../../../server/ongoing-tasks/etl/basics) article.  
  To learn how to manage ETL tasks from Studio, see [Ongoing tasks - overview](../../../../studio/database/tasks/ongoing-tasks/general-info).  

* In this page:

  * [Add RavenDB ETL task](../../../../client-api/operations/maintenance/etl/add-etl#add-ravendb-etl-task)  
  * [Add SQL ETL task](../../../../client-api/operations/maintenance/etl/add-etl#add-sql-etl-task)  
  * [Add Snowflake ETL task](../../../../client-api/operations/maintenance/etl/add-etl#add-snowflake-etl-task)  
  * [Add OLAP ETL task](../../../../client-api/operations/maintenance/etl/add-etl#add-olap-etl-task)  
  * [Add Elasticsearch ETL task](../../../../client-api/operations/maintenance/etl/add-etl#add-elasticsearch-etl-task)  
  * [Add Kafka ETL task](../../../../client-api/operations/maintenance/etl/add-etl#add-kafka-etl-task)  
  * [Add RabbitMQ ETL task](../../../../client-api/operations/maintenance/etl/add-etl#add-rabbitmq-etl-task)  
  * [Add Azure Queue Storage ETL task](../../../../client-api/operations/maintenance/etl/add-etl#add-azure-queue-storage-etl-task)
  * [Syntax](../../../../client-api/operations/maintenance/etl/add-etl#syntax)

{NOTE/}

---

{PANEL: Add RavenDB ETL task}

* Learn about the RavenDB ETL task in the **[RavenDB ETL task](../../../../server/ongoing-tasks/etl/raven)** article.  
* Learn how to define a connection string for the RavenDB ETL task here: **[Add a RavenDB connection string](../../../../client-api/operations/maintenance/connection-strings/add-connection-string#add-a-ravendb-connection-string)**  
* To manage the RavenDB ETL task from Studio, see **[Studio: RavenDB ETL task](../../../../studio/database/tasks/ongoing-tasks/ravendb-etl-task)**.  

---

The following example adds a RavenDB ETL task:

{CODE add_raven_etl@ClientApi\Operations\Maintenance\Etl\AddEtl.cs /}

{PANEL/}

{PANEL: Add SQL ETL task}

* Learn about the SQL ETL task in the **[SQL ETL task](../../../../server/ongoing-tasks/etl/sql)** article.  
* Learn how to define a connection string for the SQL ETL task here: **[Add an SQL connection string](../../../../client-api/operations/maintenance/connection-strings/add-connection-string#add-an-sql-connection-string)**  

---

The following example adds an SQL ETL task:

{CODE add_sql_etl@ClientApi\Operations\Maintenance\Etl\AddEtl.cs /}

{PANEL/}

{PANEL: Add Snowflake ETL task}

* Learn about the Snowflake ETL task in the **[Snowflake ETL task](../../../../server/ongoing-tasks/etl/snowflake)** article.  
* Learn how to define a connection string for the Snowflake ETL task here: **[Add a Snowflake connection string](../../../../client-api/operations/maintenance/connection-strings/add-connection-string#add-a-snowflake-connection-string)**  

---

The following example adds a Snowflake ETL task:

{CODE add_snowflake_etl@ClientApi\Operations\Maintenance\Etl\AddEtl.cs /}

{PANEL/}

{PANEL: Add OLAP ETL task}

* Learn about the OLAP ETL task in the **[OLAP ETL task](../../../../server/ongoing-tasks/etl/olap)** article.  
* Learn how to define a connection string for the OLAP ETL task here: **[Add an OLAP connection string](../../../../client-api/operations/maintenance/connection-strings/add-connection-string#add-an-olap-connection-string)**  
* To manage the OLAP ETL task from Studio, see **[Studio: OLAP ETL task](../../../../studio/database/tasks/ongoing-tasks/olap-etl-task)**.  

---

The following example adds an OLAP ETL task:

{CODE add_olap_etl@ClientApi\Operations\Maintenance\Etl\AddEtl.cs /}

{PANEL/}

{PANEL: Add Elasticsearch ETL task}

* Learn about the Elasticsearch ETL task in the **[Elasticsearch ETL task](../../../../server/ongoing-tasks/etl/elasticsearch)** article.  
* Learn how to define a connection string for the Elasticsearch ETL task here: **[Add an Elasticsearch connection string](../../../../client-api/operations/maintenance/connection-strings/add-connection-string#add-an-elasticsearch-connection-string)**  
* To manage the Elasticsearch ETL task from Studio, see **[Studio: Elasticsearch ETL task](../../../../studio/database/tasks/ongoing-tasks/elasticsearch-etl-task)**.  

---

The following example adds an Elasticsearch ETL task:

{CODE add_elasticsearch_etl@ClientApi\Operations\Maintenance\Etl\AddEtl.cs /}

{PANEL/}

{PANEL: Add Kafka ETL task}

* Learn about the Kafka ETL task in the **[Kafka ETL task](../../../../server/ongoing-tasks/etl/queue-etl/kafka)** article.  
* Learn how to define a connection string for the Kafka ETL task here: **[Add a Kafka connection string](../../../../server/ongoing-tasks/etl/queue-etl/kafka#add-a-kafka-connection-string)**  
* To manage the Kafka ETL task from Studio, see **[Studio: Kafka ETL task](../../../../studio/database/tasks/ongoing-tasks/kafka-etl-task)**.  

---

* Examples showing how to add a Kafka ETL task are available in the **[Add a Kafka ETL task](../../../../server/ongoing-tasks/etl/queue-etl/kafka#add-a-kafka-etl-task)** section.  

{PANEL/}

{PANEL: Add RabbitMQ ETL task}

* Learn about the RabbitMQ ETL task in the **[RabbitMQ ETL task](../../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq)** article.  
* Learn how to define a connection string for the RabbitMQ ETL task here: **[Add a RabbitMQ connection string](../../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq#add-a-rabbitmq-connection-string)**  
* To manage the RabbitMQ ETL task from Studio, see **[Studio: RabbitMQ ETL task](../../../../studio/database/tasks/ongoing-tasks/rabbitmq-etl-task)**.  

---

* Examples showing how to add a RabbitMQ ETL task are available in the **[Add a RabbitMQ ETL task](../../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq#add-a-rabbitmq-etl-task)** section.  

{PANEL/}

{PANEL: Add Azure Queue Storage ETL task}

* Learn about the Azure Queue Storage ETL task in the **[Azure Queue Storage ETL task](../../../../server/ongoing-tasks/etl/queue-etl/azure-queue)** article.  
* Learn how to define a connection string for the Azure Queue Storage ETL task here: 
  **[Add an Azure Queue Storage connection string](../../../../server/ongoing-tasks/etl/queue-etl/azure-queue#add-an-azure-queue-storage-connection-string)**  
* To manage the Azure Queue Storage ETL task from Studio, see **[Studio: Azure Queue Storage ETL task](../../../../studio/database/tasks/ongoing-tasks/azure-queue-storage-etl)**.  

---

* Examples showing how to add an Azure Queue Storage ETL task are available in the **[Add a Azure Queue Storage ETL task](../../../../server/ongoing-tasks/etl/queue-etl/azure-queue#add-an-azure-queue-storage-etl-task)** section.  

{PANEL/}

{PANEL: Syntax}

{CODE add_etl_operation@ClientApi\Operations\Maintenance\Etl\AddEtl.cs /}

| Parameter         | Type                  | Description                                                          |
|-------------------|-----------------------|----------------------------------------------------------------------|
| **configuration** | `EtlConfiguration<T>` | The ETL configuration object where `T` is the connection string type |

{PANEL/}

## Related Articles

### Server

- [ETL basics](../../../../server/ongoing-tasks/etl/basics)
- [RavenDB ETL](../../../../server/ongoing-tasks/etl/raven)
- [SQL ETL](../../../../server/ongoing-tasks/etl/sql)
- [OLAP ETL](../../../../server/ongoing-tasks/etl/olap)
- [Elasticsearch ETL](../../../../server/ongoing-tasks/etl/elasticsearch)
- [Kafka ETL](../../../../server/ongoing-tasks/etl/queue-etl/kafka)
- [RabbitMQ ETL](../../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq)
- [Azure Queue Storage ETL](../../../../server/ongoing-tasks/etl/queue-etl/azure-queue)

### Studio

- [Ongoing tasks - overview](../../../../studio/database/tasks/ongoing-tasks/general-info)
- [RavenDB ETL Task](../../../../studio/database/tasks/ongoing-tasks/ravendb-etl-task)
- [OLAP ETL Task](../../../../studio/database/tasks/ongoing-tasks/olap-etl-task)
- [Elasticsearch ETL Task](../../../../studio/database/tasks/ongoing-tasks/elasticsearch-etl-task)
- [Kafka ETL Task](../../../../studio/database/tasks/ongoing-tasks/kafka-etl-task)
- [RabbitMQ ETL Task](../../../../studio/database/tasks/ongoing-tasks/rabbitmq-etl-task)
- [Azure Queue Storage ETL Task](../../../../studio/database/tasks/ongoing-tasks/azure-queue-storage-etl)

### Connection Strings

- [Add](../../../../client-api/operations/maintenance/connection-strings/add-connection-string)
- [Get](../../../../client-api/operations/maintenance/connection-strings/get-connection-string)
- [Remove](../../../../client-api/operations/maintenance/connection-strings/remove-connection-string)
