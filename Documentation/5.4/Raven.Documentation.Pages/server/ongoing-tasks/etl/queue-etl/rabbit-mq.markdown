# Ongoing Tasks: Queue ETL Overview
---

{NOTE: }

* **RabbitMq** exchanges are designed to disperse data to multiple queues, 
  making for a flexible data channeling system that can ease the management 
  of complex scenarios.  
  For example, a RabbitMq exchange can inform different bank departments 
  of the same event via different queues so each department would handle 
  the event from its own perspective of it.  

* A RavenDB RabbitMq ETL task Extracts data from RavenDB, Transforms it 
  into a new JSON object, and Loads it to a RabbitMq Exhange using the 
  ClouodEvents library.  

* This page explains how to create a RabbitMq ETL task using code.  
  [Learn here](../../../../studio/database/tasks/ongoing-tasks/rabbitmq-etl-task) 
  how to define a RabbitMq ETL task using code.  

* In this page:  
  * []  
  * [Client API](../../../../)  
     * [Add a RabbitMq Connection String](../../../../)  
     * [Add a RabbitMq ETL Task](../../../../)  

{NOTE/}

---

{PANEL: Transformation Script}

* The structure and syntax of a RabbitMq ETL transformation script are similar to 
  those of the other ETL types.  
  The script defines which documents will be _Extracted_ from the database, 
  _Transforms_ the retrieved data, and _Loads_ it to the specified RabbitMq exchange.  
  Learn about ETL transformation scripts [here](../../../../server/ongoing-tasks/etl/basics#transform).  

* The script **Loads** data to the specified RabbitMq exchange using the 
  [loadTo\\<Exchange\\>(obj)](../../../../server/ongoing-tasks/etl/basics#transform) command.  
   * `Exchange` is the name of the RabbitMq exchnge to which the data is transferred.  
   * `obj` is an object defined by the script, that will be loaded to RabbitMq.  
     It determines the shape and contents of the document that will be created on RabbitMq queues.  
     E.g., the following script defines the `orderData` object and loads it to the `orders` exchange:  

     {CODE-BLOCK: JavaScript}
     var orderData = { DocId: id(this),
                  OrderLinesCount: this.Lines.length,
                  TotalCost: 0 };

loadToOrders(orderData);
     {CODE-BLOCK/}

{PANEL/}

{PANEL: Client API}

---

### Add a RabbitMq Connection String

Prior to defining an ETL task, add a **connection string** that the task 
will use to connect the RabbitMq exchange.  

To create the connection string:  

* Prepare a `QueueConnectionString`object with the connection string configuration.  
* Pass this object to the `PutConnectionStringOperation` store operation to add the connection string.  

**Code Sample**:  
{CODE add_rabbitMQ_connection-string@Server\OngoingTasks\ETL\Queue\Queue.cs /}

* `QueueConnectionString`:  
  {CODE QueueConnectionString@Server\OngoingTasks\ETL\Queue\Queue.cs /}
  `QueueBrokerType`:  
  {CODE QueueBrokerType@Server\OngoingTasks\ETL\Queue\Queue.cs /}

    | Property | Type | Description |
    |:-------------|:-------------|:-------------|
    | **Name** | `string` | Connection string name |
    | **BrokerType** | `QueueBrokerType` | Set to `QueueBrokerType.RabbitMq` for a Kafka connection string |
    | **KafkaConnectionSettings** | `KafkaConnectionSettings[]` | Leave undefined when defining a RabbitMq connection string |
    | **RabbitMqConnectionSettings** | `RabbitMqConnectionSettings` | A single string that specifies the RabbitMq exchange connection details |

---

### Add a RabbitMq ETL Task

To create the ETL task:  

* Prepare a `QueueEtlConfiguration`object with the ETL task configuration.  
* Pass this object to the `AddEtlOperation` store operation to add the ETL task.  

**Code Sample**:  
{CODE add_rabbitmq_etl-task@Server\OngoingTasks\ETL\Queue\Queue.cs /}

* `QueueEtlConfiguration`:  

    | Property | Type | Description |
    |:-------------|:-------------|:-------------|
    | **Name** | `string` | The ETL task name |
    | **ConnectionStringName** | `string` | The registered connection string name |
    | **Transforms** | `List<Transformation>[]` | You transformation script |
    | **Queues** | `List<EtlQueue>` | A list of used RavenDB collections / RabbitMq queues |
    | **BrokerType** | `QueueBrokerType` | Set to `QueueBrokerType.RabbitMq` to define a RabbitMq ETL task |
    | **SkipAutomaticQueueDeclaration** | `bool` | Set to `true` to skip automatic queue declaration <br> Use this option when you prefer to define Exchanges, Queues & Bindings manually. |

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
