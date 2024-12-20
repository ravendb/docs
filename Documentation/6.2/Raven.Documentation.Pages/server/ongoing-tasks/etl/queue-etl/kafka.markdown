# Queue ETL: Apache Kafka
---

{NOTE: }

* Apache Kafka is a distributed, high-performance, transactional messaging platform that remains performant 
  as the number of messages it needs to process increases and the number of events it needs to stream climbs to the big-data zone.  

* Create a **Kafka ETL Task** to:
  * Extract data from a RavenDB database
  * Transform the data using one or more custom scripts
  * Load the resulting JSON object to a Kafka destination as a CloudEvents message  

* Utilizing this task allows RavenDB to act as an event producer in a Kafka architecture.

* Read more about Kafka clusters, brokers, topics, partitions, and other related subjects,
  in the platform's [official documentation](https://kafka.apache.org/documentation/#gettingStarted).

---

* This article focuses on how to create a Kafka ETL task using the Client API.  
  To define a Kafka ETL task from the Studio, see [Studio: Kafka ETL Task](../../../../studio/database/tasks/ongoing-tasks/kafka-etl-task).  
  For an **overview of Queue ETL tasks**, see [Queue ETL tasks overview](../../../../server/ongoing-tasks/etl/queue-etl/overview).  

* In this page:
    * [Add a Kafka connection string](../../../../server/ongoing-tasks/etl/queue-etl/kafka#add-a-kafka-connection-string)
        * [Exmaple](../../../../server/ongoing-tasks/etl/queue-etl/kafka#example)
        * [Syntax](../../../../server/ongoing-tasks/etl/queue-etl/kafka#syntax)
    * [Add a Kafka ETL task](../../../../server/ongoing-tasks/etl/queue-etl/kafka#add-a-kafka-etl-task)
        * [Example - basic](../../../../server/ongoing-tasks/etl/queue-etl/kafka#example-basic)
        * [Example - delete processed documents](../../../../server/ongoing-tasks/etl/queue-etl/kafka#delete-processed-documents)
        * [Syntax](../../../../server/ongoing-tasks/etl/queue-etl/kafka#syntax-1)
    * [The transformation script](../../../../server/ongoing-tasks/etl/queue-etl/kafka#the-transformation-script)
        * [The loadTo method](../../../../server/ongoing-tasks/etl/queue-etl/kafka#the-loadto-method)

{NOTE/}

---

{PANEL: Add a Kafka connection string}

Before setting up the ETL task, define a connection string that the task will use to connect to the message broker's bootstrap servers.

---

#### Example

{CODE add_kafka_connection_string@Server\OngoingTasks\ETL\Queue\KafkaEtl.cs /}

---

#### Syntax

{CODE queue_connection_string@Server\OngoingTasks\ETL\Queue\KafkaEtl.cs /}
{CODE queue_broker_type@Server\OngoingTasks\ETL\Queue\KafkaEtl.cs /}
{CODE kafka_con_str_settings@Server\OngoingTasks\ETL\Queue\KafkaEtl.cs /}

{PANEL/}

{PANEL: Add a Kafka ETL task}

{NOTE: }

<a id="example-basic" /> **Example - basic**:

---

* In this example, the Kafka ETL Task will -
    * Extract source documents from the "Orders" collection in RavenDB.
    * Process each "Order" document using a defined script that creates a new `orderData` object.
    * Load the `orderData` object to the "OrdersTopic" in a Kafka broker.
* For more details about the script and the `loadTo` method, see the [transromation script](../../../../server/ongoing-tasks/etl/queue-etl/kafka#the-transformation-script) section below.

{CODE add_kafka_etl_task@Server\OngoingTasks\ETL\Queue\KafkaEtl.cs /}

{NOTE/}
{NOTE: }

<a id="delete-processed-documents" /> **Example - delete processed documents**:

---

* You have the option to delete documents from your RavenDB database once they have been processed by the Queue ETL task.

* Set the optional `Queues` property in your ETL configuration with the list of Kafka topics for which processed documents should be deleted.

{CODE kafka_delete_documents@Server\OngoingTasks\ETL\Queue\KafkaEtl.cs /}

{NOTE/}

---

#### Syntax

{CODE etl_configuration@Server\OngoingTasks\ETL\Queue\KafkaEtl.cs /}

{PANEL/}

{PANEL: The transformation script}

The [basic characteristics](../../../../server/ongoing-tasks/etl/basics) of a Kafka ETL script are similar to those of other ETL types.  
The script defines what data to **extract** from the source document, how to **transform** this data,  
and which Kafka Topic to **load** it to.

---

#### The loadTo method

To specify which Kafka topic to load the data into, use either of the following methods in your script.  
The two methods are equivalent, offering alternative syntax:

  * **`loadTo<TopicName>(obj, {attributes})`**  
    * Here the target is specified as part of the function name.
    * The target _&lt;TopicName&gt;_ in this syntax is Not a variable and cannot be used as one,  
      it is simply a string literal of the target's name.

  * **`loadTo('TopicName', obj, {attributes})`**  
    * Here the target is passed as an argument to the method.
    * Separating the target name from the `loadTo` command makes it possible to include symbols like `'-'` and `'.'` in target names.
      This is not possible when the `loadTo<TopicName>` syntax is used because including special characters in the name of a JavaScript function makes it invalid.

      | Parameter      | Type   | Description                                                                                                                      |
      |----------------|--------|----------------------------------------------------------------------------------------------------------------------------------|
      | **TopicName**  | string | The name of the Kafka topic                                                                                                      |
      | **obj**        | object | The object to transfer                                                                                                           |
      | **attributes** | object | An object with optional & required [CloudEvents attributes](../../../../server/ongoing-tasks/etl/queue-etl/overview#cloudevents) |

For example, the following two calls, which load data to "OrdersTopic", are equivalent:

  * `loadToOrdersTopic(obj, {attributes})`
  * `loadTo('OrdersTopic', obj, {attributes})`

---

A sample script that process documents from the Orders collection:

{CODE-BLOCK: JavaScript}
// Create an orderData object
// ==========================
var orderData = {
    Id: id(this),
    OrderLinesCount: this.Lines.length,
    TotalCost: 0
};

// Update the orderData's TotalCost field
// ======================================
for (var i = 0; i < this.Lines.length; i++) {
    var line = this.Lines[i];
    var cost = (line.Quantity * line.PricePerUnit) * ( 1 - line.Discount);
    orderData.TotalCost += cost;
}

// Load the object to the "OrdersTopic" in Kafka
// =============================================
loadToOrders(orderData, {
    Id: id(this),
    PartitionKey: id(this),
    Type: 'com.example.promotions',
    Source: '/promotion-campaigns/summer-sale'
})
{CODE-BLOCK/}

{PANEL/}

## Related Articles

### Server

- [ETL Basics](../../../../server/ongoing-tasks/etl/basics)
- [Queue ETL Overview](../../../../server/ongoing-tasks/etl/queue-etl/overview)
- [RabbitMQ ETL](../../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq)
- [Azure Queue Storage ETL](../../../../server/ongoing-tasks/etl/queue-etl/azure-queue)

### Studio

- [Studio: Kafka ETL Task](../../../../studio/database/tasks/ongoing-tasks/kafka-etl-task)
- [Studio: RabbitMQ ETL Task](../../../../studio/database/tasks/ongoing-tasks/rabbitmq-etl-task)
- [Studio: Azure Queue Storage ETL Task](../../../../studio/database/tasks/ongoing-tasks/azure-queue-storage-etl)
