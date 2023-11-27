# Queue ETL: Apache Kafka
---

{NOTE: }

* **Apache Kafka** is a distributed, high-performance, transactional 
  messaging platform, that remains performant as the number of messages 
  it needs to process increases and the number of events it needs to stream 
  climbs to the big-data zone.  
  
* Kafka's ETL support allows RavenDB to take the role of an events 
  producer in a Kafka architecture.  

* You can create a RavenDB Kafka ETL task to Extract data from the 
  database, Transform it by your custom script, and Load the resulting 
  JSON object to a Kafka destination as a CloudEvents message.  

* In this page:  
  * [Transformation Script](../../../../server/ongoing-tasks/etl/queue-etl/kafka#transformation-script)  
     * [Alternative Syntax](../../../../server/ongoing-tasks/etl/queue-etl/kafka#alternative-syntax)  
  * [Data Delivery](../../../../server/ongoing-tasks/etl/queue-etl/kafka#data-delivery)  
     * [What is Transferred](../../../../server/ongoing-tasks/etl/queue-etl/kafka#what-is-transferred)  
     * [How Are Messages Produced and Consumed](../../../../server/ongoing-tasks/etl/queue-etl/kafka#how-are-messages-produced-and-consumed)  
     * [Idempotence and Message Duplication](../../../../server/ongoing-tasks/etl/queue-etl/kafka#idempotence-and-message-duplication)  
  * [Client API](../../../../server/ongoing-tasks/etl/queue-etl/kafka#client-api)  
     * [Add a Kafka Connection String](../../../../server/ongoing-tasks/etl/queue-etl/kafka#add-a-kafka-connection-string)  
     * [Add a Kafka ETL Task](../../../../server/ongoing-tasks/etl/queue-etl/kafka#add-a-kafka-etl-task)  
     * [Delete Processed Documents](../../../../server/ongoing-tasks/etl/queue-etl/kafka#delete-processed-documents)  

{NOTE/}

---

{PANEL: Transformation Script}

The [basic characteristics](../../../../server/ongoing-tasks/etl/basics) of a Kafka ETL script 
are similar to those of the other ETL types.  
The script defines what data to **Extract** from the database, how to **Transform** this data, and 
which Kafka topic to **Load** it to.  

To load the data to a Kafka topic use the [loadTo\\<Topic\\>](../../../../server/ongoing-tasks/etl/basics#transform) 
command as follows:  
`loadTo<Topic>(obj, {attributes})`  

* **Topic**:  
  The Kafka topic name  
* **obj**:  
  The object to transfer  
* **attributes**:  
  [Optional attributes](../../../../server/ongoing-tasks/etl/queue-etl/overview#cloudevents)  

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

// Topic name: Orders
// Loaded object name: orderData
// Attributes: Id, PartitionKey, Type, Source
loadToOrders(orderData, {  
    Id: id(this),
    PartitionKey: id(this),
    Type: 'special-promotion',
    Source: '/promotion-campaigns/summer-sale'
})
{CODE-BLOCK/}

#### Alternative Syntax

The target topic name can be passed to the `loadTo` command separately, as a string argument, 
using this syntax: `loadTo('topic_name', obj, {attributes})`  

* **Example**:  
  The following two calls to `loadTo` are equivalent.  
  `loadToOrders(obj, {attributes})`  
  `loadTo('Orders', obj, {attributes})`  

Passing topic names as arguments allows the usage of names that include symbols like `-` and `.`.  
This is an advantage over the standard `loadToOrders(obj, {attributes})` syntax, that does not 
allow the passing of such characters because they are invalid in the name of a JS function.  

{PANEL/}

{PANEL: Data Delivery}

---

#### What is Transferred

* **Only Documents**  
  A Kafka ETL task transfers **documents only**.  
  Document extensions like attachments, counters, or time series, will not be transferred.  

* **As JSON Messages**  
  JSON objects produced by the task's transformation script are wrapped 
  and delivered as [CloudEvents Messages](../../../../server/ongoing-tasks/etl/queue-etl/overview#cloudevents).  

---

#### How Are Messages Produced and Consumed  

The ETL task will send the JSON messages it produces to Kafka **broker/s** 
by your [connection string](../../../../server/ongoing-tasks/etl/queue-etl/kafka#add-a-kafka-connection-string).  

Each message will then be pushed to the tail of each topic assigned to it in the transformation script, 
advance in the queue as preceding messages are pulled, and finally reach the queue's head and become 
available for consumers.  

{INFO: }
RavenDB publishes messages to Kafka using **transactions**.  
{INFO/}

{NOTE: }
Read more about Kafka clusters, brokers, topics, partitions, and other related subjects, 
in the platform's [official documentation](https://kafka.apache.org/documentation/#gettingStarted) 
or a variety of other sources.  
{NOTE/}

---

### Idempotence and Message Duplication

RavenDB is an **idempotent producer**, that will **not** typically send duplicate 
messages to topics.  

* It **is** possible, however, that duplicate messages will be sent to the broker.  
  For example:  
  Different nodes of a RavenDB cluster will be regarded as different 
  producers by the broker.  
  If the node responsible for the ETL task fails while sending a batch of messages, 
  the new responsible node may resend messages that were already received by the broker.  

* It is, therefore, **the consumer's own responsibility** (if processing each message 
  only once is important to it) to verify the uniqueness of each consumed message.  

{PANEL/}

{PANEL: Client API}

This section explains how to create a Kafka ETL task using code.  
[Learn here](../../../../studio/database/tasks/ongoing-tasks/kafka-etl-task) 
how to define a Kafka ETL task using Studio.  

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

---

### Add a Kafka ETL Task

To create the ETL task:  

* Prepare a `QueueEtlConfiguration`object with the ETL task configuration.  
* Pass this object to the `AddEtlOperation` store operation to add the ETL task.  

**Code Sample**:  
{CODE add_kafka_etl-task@Server\OngoingTasks\ETL\Queue\Queue.cs /}

* `QueueEtlConfiguration` properties:  

    | Property | Type | Description |
    |:-------------|:-------------|:-------------|
    | **Name** | `string` | The ETL task name |
    | **ConnectionStringName** | `string` | The registered connection string name |
    | **Transforms** | `List<Transformation>[]` | Your transformation script |
    | **Queues** | `List<EtlQueue>` | Optional actions to take when a document is processed, see [Delete Processed Documents](../../../../server/ongoing-tasks/etl/queue-etl/kafka#delete-processed-documents) below.  |
    | **BrokerType** | `QueueBrokerType` | Set to `QueueBrokerType.Kafka` to define a Kafka ETL task |

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
{CODE kafka_EtlQueue@Server\OngoingTasks\ETL\Queue\Queue.cs /}

{PANEL/}

## Related Articles

### Server

- [ETL Basics](../../../../server/ongoing-tasks/etl/basics)
- [Queue ETL Overview](../../../../server/ongoing-tasks/etl/queue-etl/overview)
- [RabbitMQ ETL](../../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq)

### Studio

- [Studio: Kafka ETL Task](../../../../studio/database/tasks/ongoing-tasks/kafka-etl-task)
- [Studio: RabbitMQ ETL Task](../../../../studio/database/tasks/ongoing-tasks/rabbitmq-etl-task)
