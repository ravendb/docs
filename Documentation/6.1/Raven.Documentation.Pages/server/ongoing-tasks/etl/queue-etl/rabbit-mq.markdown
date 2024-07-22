# Queue ETL: RabbitMQ
---

{NOTE: }

* RabbitMQ exchanges are designed to disperse data to multiple queues,  
  creating a flexible data channeling system that can easily handle complex message streaming scenarios.

* Create a **RabbitMQ ETL Task** to:
  * Extract data from a RavenDB database
  * Transform the data using one or more custom scripts
  * Load the resulting JSON object to a RabbitMQ destination as a CloudEvents message  

* Utilizing this task allows RavenDB to act as an event producer in a RabbitMQ architecture.

* Read more about RabbitMQ in the platform's [official documentation](https://www.rabbitmq.com/).

---

* This article focuses on how to create a RabbitMQ ETL task using the Client API.  
  To define a RabbitMQ ETL task from the Studio see [Studio: RabbitMQ ETL Task](../../../../studio/database/tasks/ongoing-tasks/rabbitmq-etl-task).  
  For an **overview of Queue ETL tasks**, see [Queue ETL tasks overview](../../../../server/ongoing-tasks/etl/queue-etl/overview).

* In this page:
    * [Add a RabbitMQ connection string](../../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq#add-a-rabbitmq-connection-string)
        * [Exmaple](../../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq#example)
        * [Syntax](../../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq#syntax)
    * [Add a RabbitMQ ETL task](../../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq#add-a-rabbitmq-etl-task)
        * [Example - basic](../../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq#example-basic)
        * [Example - delete processed documents](../../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq#delete-processed-documents)
        * [Syntax](../../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq#syntax-1)
    * [The transformation script](../../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq#the-transformation-script)
        * [The loadTo method](../../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq#the-loadto-method)
        * [Available method overloads](../../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq#available-method-overloads)

{NOTE/}

---

{PANEL: Add a RabbitMQ connection string}

Before setting up the ETL task, define a connection string that the task will use to connect to RabbitMQ.

---

#### Example

{CODE add_rabbitMq_connection_string@Server\OngoingTasks\ETL\Queue\RabbitMqEtl.cs /}

---

#### Syntax

{CODE queue_connection_string@Server\OngoingTasks\ETL\Queue\RabbitMqEtl.cs /}
{CODE queue_broker_type@Server\OngoingTasks\ETL\Queue\RabbitMqEtl.cs /}
{CODE rabbitMq_con_str_settings@Server\OngoingTasks\ETL\Queue\RabbitMqEtl.cs /}

{PANEL/}

{PANEL: Add a RabbitMQ ETL task}

{NOTE: }

<a id="example-basic" /> __Example - basic__:

---

* In this example, the RabbitMQ ETL Task will -
    * Extract source documents from the "Orders" collection in RavenDB.
    * Process each "Order" document using a defined script that creates a new `orderData` object.
    * Load the `orderData` object to the "OrdersExchange" in a RabbitMQ broker.
* For more details about the script and the `loadTo` method overloads, see the [transromation script](../../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq#the-transformation-script) section below.

{CODE add_rabbitMq_etl_task@Server\OngoingTasks\ETL\Queue\RabbitMqEtl.cs /}

{NOTE/}
{NOTE: }

<a id="delete-processed-documents" /> __Example - delete processed documents__:

---

* You have the option to delete documents from your RavenDB database once they have been processed by the Queue ETL task.

* Set the optional `Queues` property in your ETL configuration with the list of RabbitMQ queues for which processed documents should be deleted.

{CODE rabbitMq_delete_documents@Server\OngoingTasks\ETL\Queue\RabbitMqEtl.cs /}

{NOTE/}

---

#### Syntax

{CODE etl_configuration@Server\OngoingTasks\ETL\Queue\RabbitMqEtl.cs /}

{PANEL/}

{PANEL: The transformation script}

The [basic characteristics](../../../../server/ongoing-tasks/etl/basics) of a RabbitMQ ETL script are similar to those of other ETL types.  
The script defines what data to **extract** from the source document, how to **transform** this data,  
and which RabbitMQ Exchange to **load** it to.

---

#### The loadTo method

To specify which RabbitMQ Exchange to load the data into, use either of the following methods in your script.  
The two methods are equivalent, offering alternative syntax:

  * **`loadTo<ExchangeName>(obj, 'routingKey', {attributes})`**  
    * Here the target is specified as part of the function name.
    * The target _&lt;ExchangeName&gt;_ in this syntax is Not a variable and cannot be used as one,  
      it is simply a string literal of the target's name.

  * **`loadTo('ExchangeName', obj, 'routingKey', {attributes})`**  
    * Here the target is passed as an argument to the method.
    * Separating the target name from the `loadTo` command makes it possible to include symbols like `'-'` and `'.'` in target names.
      This is not possible when the `loadTo<ExchangeName>` syntax is used because including special characters in the name of a JavaScript function makes it invalid.

      | Parameter        | Type    | Description                                                                                                                  |
      |------------------|---------|------------------------------------------------------------------------------------------------------------------------------|
      | **ExchangeName** | string  | The name of the RabbitMQ exchange.                                                                                           |
      | **obj**          | object  | The object to transfer.                                                                                                      |
      | **routingKey**   | string  | The RabbitMQ exchange evaluates this attribute to determine how to route the message to queues based on the exchange type.   |
      | **attributes**   | object  | An object with [CloudEvents attributes](../../../../server/ongoing-tasks/etl/queue-etl/overview#cloudevents).                |

For example, the following two calls, which load data to the Orders exchange, are equivalent:

  * `loadToOrdersExchange(obj, 'users', {attributes})`
  * `loadTo('OrdersExchange', obj, 'users', {attributes})`

---

#### Available method overloads

  * `loadTo('', obj, 'routingKey', {attributes})`  
    When replacing the exchange name with an empty string,  
    the message will be routed using the routingKey via the default exchange, which is predefined by the broker.

  * `loadTo<ExchangeName>(obj)`  
    `loadTo<ExchangeName>(obj, {attributes})`  
    When omitting the routingKey, messages delivery will depend on the exchange type.

  * `loadTo<ExchangeName>(obj, 'routingKey')`  
    When omitting the attributes, default attribute values will be assigned.

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

// Load the object to "OrdersExchange" in RabbitMQ
// ===============================================
loadToOrdersExchange(orderData, 'users-queue', {
    Id: id(this),
    Type: 'com.example.promotions',
    Source: '/promotion-campaigns/summer-sale'
})
{CODE-BLOCK/}

{PANEL/}

## Related Articles

### Server

- [ETL Basics](../../../../server/ongoing-tasks/etl/basics)
- [Queue ETL Overview](../../../../server/ongoing-tasks/etl/queue-etl/overview)
- [Kafka ETL](../../../../server/ongoing-tasks/etl/queue-etl/kafka)
- [Azure Queue Storage ETL](../../../../server/ongoing-tasks/etl/queue-etl/azure-queue)

### Studio

- [Studio: RabbitMQ ETL Task](../../../../studio/database/tasks/ongoing-tasks/rabbitmq-etl-task)
- [Studio: Kafka ETL Task](../../../../studio/database/tasks/ongoing-tasks/kafka-etl-task)
- [Studio: Azure Queue Storage ETL Task](../../../../studio/database/tasks/ongoing-tasks/azure-queue-storage-etl)
