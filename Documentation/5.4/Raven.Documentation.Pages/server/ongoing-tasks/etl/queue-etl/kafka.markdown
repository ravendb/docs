# Queue ETL: Apache Kafka
---

{NOTE: }

* **Apache Kafka** is a high-performance message broker, suited to serve 
  consumers in big data environments.  

* A RavenDB Kafka ETL task Extracts data from RavenDB, Transforms it 
  into a new JSON object, and Loads it into Kafka topics using the 
  ClouodEvents library.  

* This page explains how to create a Kafka ETL task using code.  
  [Learn here](../../../../studio/database/tasks/ongoing-tasks/kafka-etl-task) 
  how to define a Kafka ETL task using code.  

* In this page:  
  * []  
  * [Client API](../../../../)  
     * [Add a Kafka Connection String](../../../../)  
     * [Add a Kafka ETL Task](../../../../)  

{NOTE/}

---

{PANEL: Transformation Script}

* The structure and syntax of a Kafka ETL transformation script are similar to 
  those of the other ETL types.  
  The script defines which documents will be _Extracted_ from the database, 
  _Transforms_ the retrieved data, and _Loads_ it to the specified Kafka topics.  
  Learn about ETL transformation scripts [here](../../../../server/ongoing-tasks/etl/basics#transform).  

* The script **Loads** data to the specified Kafka topic using the 
  [loadTo\\<Topic\\>(obj)](../../../../server/ongoing-tasks/etl/basics#transform) command.  
   * `Topic` is the name of the Kafka topic to which the data is transferred.  
   * `obj` is an object defined by the script, that will be loaded to Kafka.  
     It determines the shape and contents of the document that will be created on the Kafka topic.  
     E.g., the following script defines the `orderData` object and loads it to the `orders` topic:  
     {INFO: }
     If a topic of that name doesn't exist, it will be created.  
     {INFO/}
     {CODE-BLOCK: JavaScript}
     var orderData = { DocId: id(this),
                  OrderLinesCount: this.Lines.length,
                  TotalCost: 0 };

loadToOrders(orderData);
     {CODE-BLOCK/}

{PANEL/}

{PANEL: Client API}

---

### Add a Kafka Connection String

Prior to defining an ETL task, add a **connection string** that the task 
will use to connect the message broker's bootstrap servers.  

To create the connection string:  

* Prepare a `QueueConnectionString`object with the connection string configuration.  
* Pass this object to the `PutConnectionStringOperation` store operation to add the connection string.  

**Code Sample**:  
{CODE add_kafka_connection-string@Server\OngoingTasks\ETL\Queue\Queue.cs /}

* `QueueConnectionString`:  
  {CODE QueueConnectionString@Server\OngoingTasks\ETL\Queue\Queue.cs /}
  `QueueBrokerType`:  
  {CODE QueueBrokerType@Server\OngoingTasks\ETL\Queue\Queue.cs /}

    | Property | Type | Description |
    |:-------------|:-------------|:-------------|
    | **Name** | `string` | Connection string name |
    | **BrokerType** | `QueueBrokerType` | Set to `QueueBrokerType.Kafka` for a Kafka connection string |
    | **KafkaConnectionSettings** | `KafkaConnectionSettings[]` | A list of comma-separated host:port URLs to Kafka brokers |
    | **RabbitMqConnectionSettings** | `RabbitMqConnectionSettings` | Leave undefined when defining a Kafka connection string |

---

### Add a Kafka ETL Task

To create the ETL task:  

* Prepare a `QueueEtlConfiguration`object with the ETL task configuration.  
* Pass this object to the `AddEtlOperation` store operation to add the ETL task.  

**Code Sample**:  
{CODE add_kafka_etl-task@Server\OngoingTasks\ETL\Queue\Queue.cs /}

* `QueueEtlConfiguration`:  

    | Property | Type | Description |
    |:-------------|:-------------|:-------------|
    | **Name** | `string` | The ETL task name |
    | **ConnectionStringName** | `string` | The registered connection string name |
    | **Transforms** | `List<Transformation>[]` | You transformation script |
    | **Queues** | `List<EtlQueue>` | A list of used RavenDB collections / Kafka queues |
    | **BrokerType** | `QueueBrokerType` | Set to `QueueBrokerType.Kafka` to define a Kafka ETL task |

    `EtlQueue`
    {CODE EtlQueue@Server\OngoingTasks\ETL\Queue\Queue.cs /}

{PANEL/}

## Related Articles

### ETL

- [ETL Basics](../../../server/ongoing-tasks/etl/basics)
- [SQL ETL Task](../../../server/ongoing-tasks/etl/sql)

### Client API

- [How to Add ETL](../../../client-api/operations/maintenance/etl/add-etl)
- [How to Update ETL](../../../client-api/operations/maintenance/etl/update-etl)
- [How to Reset ETL](../../../client-api/operations/maintenance/etl/reset-etl)

### Studio

- [Define RavenDB ETL Task in Studio](../../../studio/database/tasks/ongoing-tasks/ravendb-etl-task)
