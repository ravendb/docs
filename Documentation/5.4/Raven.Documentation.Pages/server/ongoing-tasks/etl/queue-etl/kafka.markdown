# Queue ETL: Apache Kafka
---

{NOTE: }

* **Apache Kafka** is a distributed, high-performance, transactional 
  messaging platform, that remains performant as the number of messages 
  it needs to process increases and the number of events it needs to stream 
  climbs to the big-data zone.  
  
* Kafka's ETL support allows RavenDB to take the role of an events 
  producer in a Kafka-cluster architecture.  

* You can create a RavenDB Kafka ETL task to Extract data from the 
  database, Transform it by your custom script, and Load the resulting 
  JSON object to a Kafka destination as a CloudEvents message.  

* In this page:  
  * [Transformation Script](../../../../server/ongoing-tasks/etl/queue-etl/kafka#transformation-script)  
     * [Additional Cloud Event Attributes](../../../../server/ongoing-tasks/etl/queue-etl/kafka#additional-cloud-event-attributes)  
  * [Data Delivery](../../../../server/ongoing-tasks/etl/queue-etl/kafka#data-delivery)  
     * [What is Transferred](../../../../server/ongoing-tasks/etl/queue-etl/kafka#what-is-transferred)  
     * [How Are Messages Produced and Consumed](../../../../server/ongoing-tasks/etl/queue-etl/kafka#how-are-messages-produced-and-consumed)  
  * [Client API](../../../../server/ongoing-tasks/etl/queue-etl/kafka#client-api)  
     * [Add a Kafka Connection String](../../../../server/ongoing-tasks/etl/queue-etl/kafka#add-a-kafka-connection-string)  
     * [Add a Kafka ETL Task](../../../../server/ongoing-tasks/etl/queue-etl/kafka#add-a-kafka-etl-task)  
     * [Delete Processed Documents](../../../../server/ongoing-tasks/etl/queue-etl/kafka#delete-processed-documents)  

{NOTE/}

---

{PANEL: Transformation Script}

The basic characteristics of a Kafka ETL task's transformation script 
are similar to those of all other ETL types.  
The script defines what data to _Extract_ from the database, 
how to _Transform_ it and what Kafka topic to Load it to.  
Learn about ETL transformation scripts [here](../../../../server/ongoing-tasks/etl/basics).  

Objects are **Loaded** to the specified Kafka topic using the 
[loadTo\\<Topic\\>(obj)](../../../../server/ongoing-tasks/etl/basics#transform) command.  

* `Topic` is the name of the Kafka topic to which the data is transferred.  
* `obj` is an object defined by the script, that will be loaded to Kafka.  
* [Additional attributes](../../../../server/ongoing-tasks/etl/queue-etl/kafka#additional-cloud-event-attributes) 
  can also be specified.  

E.g., the following transformation script defines an `orderData` object and 
loads it to the `Orders` topic:  

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

// Load the orderData object to the Orders topic 
// optionally modify cloud event attributes 
loadToOrders(orderData, {  
        Id: id(this),
        PartitionKey: id(this),
        Type: 'special-promotion',
        Source: '/registrations/direct-signup'
    })
     {CODE-BLOCK/}

---

### Additional Cloud Event Attributes

As `loadTo` is called to wrap the JSON document in a CloudEvents message and load it to 
the Kafka destination, a few **cloud event attributes** are added to the message by default.  
You can explicitly set these attributes with values other than the default.  

To modify the attributes use this format:  
`loadTo<Topic>(obj, {attributes})`  

* `obj`: The JSON object
* `attributes`: A list of key/value attributes

E.g. -  
{CODE-BLOCK: JavaScript}
// Load the orderData object to the Orders topic 
// optionally modify cloud event attributes 
loadToOrders(orderData, {  
        Id: id(this),
        Type: '/promotion-campaigns/summer-sale',
        Source: '/registrations/direct-signup'
    })
     {CODE-BLOCK/}

* Read about the `Id`, `Type`, and `Source` attributes [here](../../../../server/ongoing-tasks/etl/queue-etl/overview#cloudevents).  

{PANEL/}

{PANEL: Data Delivery}

---

### What is Transferred

* **Only Documents**  
  A Kafka ETL task transfers **documents only**.  
  Document extensions like attachments, counters, or time series, will not be transferred.  

* **As CloudEvents Messages**  
  JSON objects produced by the task's transformation script are wrapped 
  and delivered as [CloudEvents Messages](../../../../server/ongoing-tasks/etl/queue-etl/overview#cloudevents).  

---

### How Are Messages Produced and Consumed  

The ETL task will send the CloudEvents messages it produces to Kafka **broker/s** 
by your [connection string](../../../../server/ongoing-tasks/etl/queue-etl/kafka#add-a-kafka-connection-string).  

Each message will then be sent to each topic defined for it in the transformation script, 
and pushed to the tail of the topic's queue. The enqueued message will advance in the queue 
as preceding messages are pulled, and finally reach the queue's head and become available 
for consumers.  

Read more about Kafka clusters, brokers, topics, partitions, and other related subjects, 
in the platform's [official documentation](https://kafka.apache.org/documentation/#gettingStarted) 
or a variety of other sources.  

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
    | **RabbitMqConnectionSettings** | `RabbitMqConnectionSettings` | Leave undefined when defining a Kafka connection string |

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

### ETL

- [ETL Basics](../../../server/ongoing-tasks/etl/basics)
- [SQL ETL Task](../../../server/ongoing-tasks/etl/sql)

### Client API

- [How to Add ETL](../../../client-api/operations/maintenance/etl/add-etl)
- [How to Update ETL](../../../client-api/operations/maintenance/etl/update-etl)
- [How to Reset ETL](../../../client-api/operations/maintenance/etl/reset-etl)

### Studio

- [Define RavenDB ETL Task in Studio](../../../studio/database/tasks/ongoing-tasks/ravendb-etl-task)
