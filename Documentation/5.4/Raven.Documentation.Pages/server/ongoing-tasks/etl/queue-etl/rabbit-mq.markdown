# Queue ETL: RabbitMQ
---

{NOTE: }

* **RabbitMQ** exchanges are designed to disperse data to multiple queues, 
  making for a flexible data channeling system that can easily handle complex 
  message streaming scenarios.  

* RabbitMQ's ETL support allows RavenDB to take the role of a producer 
  in a RabbitMQ architecture.  

* You can create a RavenDB RabbitMQ ETL task to Extract data from the 
  database, Transform it by your custom script, and Load the resulting 
  JSON object to a RabbitMQ exchange as a ClouodEvents message.  

* In this page:  
  * [Transformation Script](../../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq#transformation-script)  
     * [Additional Cloud Event Attributes](../../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq#additional-cloud-event-attributes)  
  * [Data Delivery](../../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq#data-delivery)  
     * [What is Transferred](../../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq#what-is-transferred)  
     * [How Are Messages Consumed](../../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq#how-are-messages-consumed)  
  * [Client API](../../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq#client-api)  
     * [Add a RabbitMQ Connection String](../../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq#add-a-rabbitmq-connection-string)  
     * [Add a RabbitMQ ETL Task](../../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq#add-a-rabbitmq-etl-task)  

{NOTE/}

---

{PANEL: Transformation Script}

The basic characteristics of a RabbitMQ ETL task's transformation script 
are similar to those of all other ETL types.  
The script defines what data to _Extract_ from the database, 
how to _Transform_ this data, and which RabbitMQ queue to Load it to.  
Learn about ETL transformation scripts [here](../../../../server/ongoing-tasks/etl/basics).  

Objects are **Loaded** to the specified RabbitMQ exchange using the 
[loadTo\\<Exchange\\>()](../../../../server/ongoing-tasks/etl/basics#transform) command.  

* `Exchange` is the name of the RabbitMQ exchange to which the data is transferred.  
* [A Routing Key and Additional attributes](../../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq#additional-cloud-event-attributes) 
  can also be specified.  

E.g., the following script defines an `orderData` object and loads it to the `orders` exchange:  

{CODE-BLOCK: JavaScript}
// Create an OrderData JSON object
var orderData = {
        Id: id(this), 
        OrderLinesCount: this.Lines.length,
        TotalCost: 0
    };

// Update orderData's TotalCost field
for (var i = 0; i < this.Lines.length; i++) {
        var line = this.Lines[i];
        var cost = (line.Quantity * line.PricePerUnit) * ( 1 - line.Discount);
        orderData.TotalCost += cost;
    }

// Load the orderData object to the Orders queue (create the queue if it doesn't exist)
loadToOrders(orderData, `routingKey`, {  
        Id: id(this),
        PartitionKey: id(this),
        Type: 'com.github.users',
        Source: '/registrations/direct-signup'
    });
{CODE-BLOCK/}

---

#### Additional Cloud Event Attributes

As `loadTo` is called to wrap the JSON document in a CloudEvents message and load it to 
the RabbitMQ exchange, a few **cloud event attributes** are added to the message by default.  
You can explicitly set these attributes with your own values.  

`loadTo<Exchange>({attributes}, routingKey)`  

* `Exchange` is the name of the RabbitMQ exchange to which the data is transferred.  
  Omit the exchange to use the default value. E.g. -  
  {CODE-BLOCK: JavaScript}
  loadTo({Name: 'Factory-22'})
  {CODE-BLOCK/}
* `attributes`: A list of key/value attributes  
  Read more about available attributes [here](https://github.com/cloudevents/spec/blob/main/cloudevents/spec.md#required-attributes).  
* `routingKey`: A key specifying where to route the messages to  
  E.g., the command below uses the `Orders` exchange, explicitly sets the Name attribute, 
  and routes messages to `admins`.  
  {CODE-BLOCK: JavaScript}
  loadToOrders({Name: 'Factory-22'}, `admins`)
  {CODE-BLOCK/}

{PANEL/}


{PANEL: Data Delivery}

#### What is Transferred

* **Only Documents**  
  A RabbitMQ ETL task transfers **documents only**.  
  Document extensions like attachments, counters, or time series, will not be transferred.  

* **As CloudEvents Messages**  
  JSON objects produced by the task's transformation script are wrapped 
  and delivered as [CloudEvents Messages](../../../../server/ongoing-tasks/etl/queue-etl/overview#cloudevents).  

---

#### How Are Messages Consumed  

The ETL task will send the CloudEvents messages it produces to a RabbitMQ **exchange** 
by the address provided in your [connection string](../../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq#add-a-rabbitmq-connection-string).  

The message will then be pushed to the tail of the queue defined for it in the transformation script, 
advance in the queue as preceding messages are pulled, and finally reach the queue's head and become 
available for consumers.  

Read more about RabbitMQ in the platform's [official documentation](https://www.rabbitmq.com/) 
or a variety of other sources.  

{PANEL/}

{PANEL: Client API}

This section explains how to create a RabbitMQ ETL task using code.  
[Learn here](../../../../studio/database/tasks/ongoing-tasks/rabbitmq-etl-task) 
how to define a RabbitMQ ETL task using Studio.  

---

#### Add a RabbitMQ Connection String

Prior to defining an ETL task, add a **connection string** that the task 
will use to connect the RabbitMQ exchange.  

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
    | **KafkaConnectionSettings** | `KafkaConnectionSettings[]` | Leave undefined when defining a RabbitMQ connection string |
    | **RabbitMqConnectionSettings** | `RabbitMqConnectionSettings` | A single string that specifies the RabbitMQ exchange connection details |

---

#### Add a RabbitMQ ETL Task

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
    | **Queues** | `List<EtlQueue>` | A list of used RavenDB collections / RabbitMQ queues |
    | **BrokerType** | `QueueBrokerType` | Set to `QueueBrokerType.RabbitMq` to define a RabbitMQ ETL task |
    | **SkipAutomaticQueueDeclaration** | `bool` | Set to `true` to skip automatic queue declaration <br> Use this option when you prefer to define Exchanges, Queues & Bindings manually. |

    `EtlQueue`
    {CODE EtlQueue@Server\OngoingTasks\ETL\Queue\Queue.cs /}

{PANEL/}

## Related Articles

### ETL

- [ETL Basics](../../../../server/ongoing-tasks/etl/basics)
- [SQL ETL Task](../../../../server/ongoing-tasks/etl/sql)

### Client API

- [How to Add ETL](../../../../client-api/operations/maintenance/etl/add-etl)
- [How to Update ETL](../../../../client-api/operations/maintenance/etl/update-etl)
- [How to Reset ETL](../../../../client-api/operations/maintenance/etl/reset-etl)

### Studio

- [Define RavenDB ETL Task in Studio](../../../../studio/database/tasks/ongoing-tasks/ravendb-etl-task)
