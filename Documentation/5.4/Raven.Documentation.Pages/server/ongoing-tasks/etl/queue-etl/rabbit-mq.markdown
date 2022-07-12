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
  JSON object to a RabbitMQ exchange as a CloudEvents message.  

* In this page:  
  * [Transformation Script](../../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq#transformation-script)  
     * [Additional Cloud Event Attributes](../../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq#additional-cloud-event-attributes)  
  * [Data Delivery](../../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq#data-delivery)  
     * [What is Transferred](../../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq#what-is-transferred)  
     * [How Are Messages Produced and Consumed](../../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq#how-are-messages-produced-and-consumed)  
  * [Client API](../../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq#client-api)  
     * [Add a RabbitMQ Connection String](../../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq#add-a-rabbitmq-connection-string)  
     * [Add a RabbitMQ ETL Task](../../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq#add-a-rabbitmq-etl-task)  
     * [Delete Processed Documents](../../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq#delete-processed-documents)  
{NOTE/}

---

{PANEL: Transformation Script}

The [basic characteristics](../../../../server/ongoing-tasks/etl/basics) of a RabbitMQ ETL script 
are similar to those of the other ETL types.  
The script defines what data to **Extract** from the database, how to **Transform** this data, and 
which queue/s to **Load** it to.  

To load the data to RabbitMQ, use the [loadTo\\<Exchange\\>](../../../../server/ongoing-tasks/etl/basics#transform) 
command as follows:  
`loadTo<Exchange>(obj, routingKey, {attributes})`  

* **Exchange**: the RabbitMQ exchange name  
* **obj**: the object to transfer  
* **routingKey**: a key by which binding is made between the RabbitMQ exchange and target queue/s  
* **attributes**: [Additional attributes](../../../../server/ongoing-tasks/etl/queue-etl/overview#cloudevents)  

For example:  

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

// Exchange name: Orders
// Loaded object name: orderData
// Routing key: admin 
// Attributes: Id, PartitionKey, Type, Source
loadToOrders(orderData, `admin`, {  
        Id: id(this),
        PartitionKey: id(this),
        Type: 'special-promotion',
        Source: '/registrations/direct-signup'
    });
{CODE-BLOCK/}

---

### Partial `loadTo` Syntax

Partial `loadTo` syntaxes are valid, including:  

* Ommitted exchange name, as in: `loadTo('', orderData, 'admin')`  
  In this case a default, pre-defined, direct exchange will be used, 
  with a queue name similar to the routing key and the messages 
  delivered directly to the queue.  

* Ommitted routing key, as in: `loadToOrders(orderData)`  
  Note that in the lack of routing keys messages delivery will 
  depend upon the type of exchange you use.  

* Ommitted attributes (Id, Type, Source..)  
  No attributes are mandatory, they can all be ommitted.  

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

#### How Are Messages Produced and Consumed  

The ETL task will send the CloudEvents messages it produces to a RabbitMQ **exchange** 
by the address provided in your [connection string](../../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq#add-a-rabbitmq-connection-string).  

The message will then be pushed to the tail of the queue defined for it in the transformation script, 
advance in the queue as preceding messages are pulled, and finally reach the queue's head and become 
available for consumers.  

Read more about RabbitMQ in the platform's [official documentation](https://www.rabbitmq.com/) 
or a variety of other sources.  

---

#### Idempotence

RavenDB is an **idempotent producer**, that will **not** typically send duplicates 
messages to its queues' consumers.  

It **is** possible, however, that duplicate messages will be sent to the exchange.  

It is therefore the consumer's own responsibility to verify the uniqueness of the 
messages it processes.  

{INFO: }
Consider, for example, the following scenario, in which duplicate messages may be 
sent by RavenDB and enqueued by the RabbitMQ exchange.  

* The RavenDB node responsible for the ETL task, failed while sending messages.  
* Another RavenDB node was assigned with the ETL task.  
  This node is considered a **new producer** by the exchange.  
  Confirmation messages sent by the exchange to the former producer, weren't 
  received by the new producer.  
* Being a transactional server, RavenDB now resends the entire transaction that 
  was cut short by the failure of the previous node.  
* The messages that were previously received by the exchange, are now resent 
  by RavenDB, received by the exchange and enqueued again.  

{INFO/}

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
    | **Queues** | `List<EtlQueue>` | Optional actions to take when a document is processed, see [Delete Processed Documents](../../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq#delete-processed-documents) below.  |
    | **BrokerType** | `QueueBrokerType` | Set to `QueueBrokerType.RabbitMq` to define a RabbitMQ ETL task |
    | **SkipAutomaticQueueDeclaration** | `bool` | Set to `true` to skip automatic queue declaration <br> Use this option when you prefer to define Exchanges, Queues & Bindings manually. |

---

### Delete Processed Documents

You can include an optional `EtlQueue` property in the ETL configuration to 
trigger additional actions.  
An action that you can trigger this way, is the **deletion of RavenDB documents** 
once they've been processed by the ETL task.  

`EtlQueue`
{CODE EtlQueueDefinition@Server\OngoingTasks\ETL\Queue\Queue.cs /}

| Property | Type | Description |
|:-------------|:-------------|:-------------|
| **Name** | `string` | Queue name |
| **DeleteProcessedDocuments** | `bool` | if `true`, delete processed documents from RavenDB |

**Code Sample**:  
{CODE rabbitmq_EtlQueue@Server\OngoingTasks\ETL\Queue\Queue.cs /}

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
