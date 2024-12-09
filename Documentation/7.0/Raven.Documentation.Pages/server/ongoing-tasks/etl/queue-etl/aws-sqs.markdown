# Queue ETL: Amazon SQS
---

{NOTE: }

* Amazon **SQS** (**S**imple **Q**ueue **S**ervice) is a distributed Message 
  Queue service (like Azure Queue Storage and others) that is widely used 
  for its scalability, durability, availability, and queueing methods:  
   * **Standard queueing**, offering an enormous throughput but lacking 
     the ability to ensure that messages would arrive in the same order 
     they were sent or to prevent message duplication.  
   * **FIFO queueing**, providing strict control over delivery order using 
     a First-In-First-Out queue and ensuring the delivery of each message 
     exactly once, in exchange for a much slower transfer rate than that of 
     the Standard Queueing method.  

       {INFO: }
        Read more about Amazon SQS in the platform's [official documentation](https://docs.aws.amazon.com/sqs/).
       {INFO/}

* Create an **Amazon SQS ETL Task** to:
  * **Extract** data from a RavenDB database,  
  * **Transform** the data using one or more custom scripts,  
  * and **Load** the resulting JSON object to an SQS destination in CloudEvent format.  

* Utilizing such tasks allows RavenDB to act as an event producer in an Amazon SQS architecture.  

* The loading of RavenDB messages to an SQS queue can automatically _trigger AWS 
  [Lambda Functions](https://docs.aws.amazon.com/lambda/latest/dg/welcome.html)_, 
  enabling economic processing and powerful workflows.  
  Enqueueing RavenDB messages using SQS can also be _integrated with other AWS services_ 
  such as [Amazon SNS](https://aws.amazon.com/sns/) to distribute message-related notifications 
  and [Step Functions](https://aws.amazon.com/step-functions/) to manage and visualize your workflows.  

---

{INFO: }
This article focuses on the creation of an Amazon SQS ETL task using the Client API.  
To define an Amazon SQS ETL task from Studio, see [Studio: Amazon SQS ETL Task](../../../../studio/database/tasks/ongoing-tasks/aws-sqs-etl).  
For an **overview of Queue ETL tasks**, see [Queue ETL tasks overview](../../../../server/ongoing-tasks/etl/queue-etl/overview).
{INFO/}

* In this page:  
  * [Add an Amazon SQS connection string](../../../../server/ongoing-tasks/etl/queue-etl/aws-sqs#add-an-amazon-sqs-connection-string)  
      * [Authentication methods](../../../../server/ongoing-tasks/etl/queue-etl/aws-sqs#authentication-methods)  
      * [Example](../../../../server/ongoing-tasks/etl/queue-etl/aws-sqs#example)  
      * [Syntax](../../../../server/ongoing-tasks/etl/queue-etl/aws-sqs#syntax)
  * [Add an Amazon SQS ETL task](../../../../server/ongoing-tasks/etl/queue-etl/aws-sqs#add-an-amazon-sqs-etl-task)  
      * [Example: Add SQS ETL task](../../../../server/ongoing-tasks/etl/queue-etl/aws-sqs#example-add-sqs-etl-task)  
      * [Delete processed documents](../../../../server/ongoing-tasks/etl/queue-etl/aws-sqs#delete-processed-documents)  
      * [Syntax](../../../../server/ongoing-tasks/etl/queue-etl/aws-sqs#syntax-1)
  * [The transformation script](../../../../server/ongoing-tasks/etl/queue-etl/aws-sqs#the-transformation-script)
      * [The loadTo method](../../../../server/ongoing-tasks/etl/queue-etl/aws-sqs#the-loadto-method)  

{NOTE/}

---

{PANEL: Add an Amazon SQS connection string}

Prior to setting up the ETL task, define a connection string that the task will use to access your SQS destination.  
The connection string includes the authorization credentials required to connect.

---

#### Authentication methods
The authorization method that the ETL task uses to access the SQS target is determined 
by properties of the connection string it uses, as shown in the example below.  

#### Example

{CODE add_sqs_connection_string@Server\OngoingTasks\ETL\Queue\AWS-SQS_Etl.cs /}

* **Passwordless**  
  Defines whether to use a password or not.  
  Set this property to `true` if the target machine is pre-authorized.  
  This authorization method can only be used in self-hosted mode.  

* **Basic**  
  Defines these authorization properties:  
   * `AccessKey`  
   * `SecretKey`  
   * `RegionName`  

---

#### Syntax

{CODE amazon_sqs_con_str_settings@Server\OngoingTasks\ETL\Queue\AWS-SQS_Etl.cs /}
{CODE queue_connection_string@Server\OngoingTasks\ETL\Queue\AWS-SQS_Etl.cs /} 
{CODE queue_broker_type@Server\OngoingTasks\ETL\Queue\AWS-SQS_Etl.cs /}

{PANEL/}

{PANEL: Add an Amazon SQS ETL task}

#### Example: Add SQS ETL task

* In this example, the Amazon SQS ETL Task will -  
  * Extract source documents from the "Orders" collection in RavenDB.  
  * Process each "Order" document using a defined script that creates a new `orderData` object.  
  * Load the `orderData` object to the "OrdersQueue" on an SQS destination.  
* For more details about the script and the `loadTo` method, see the transromation script section below.  

{CODE add_amazon_sqs_etl_task@Server\OngoingTasks\ETL\Queue\AWS-SQS_Etl.cs /}

---

#### Delete processed documents

* It is possible to **delete** documents from a RavenDB database once they have been processed by the Queue ETL task.
* To do this, set the optional `Queues` property in the ETL configuration with the list of SQS queues for which 
  processed documents are to be deleted.

{CODE sqs_delete_documents@Server\OngoingTasks\ETL\Queue\AWS-SQS_Etl.cs /}

---

#### Syntax

{CODE etl_configuration@Server\OngoingTasks\ETL\Queue\AWS-SQS_Etl.cs /}

{PANEL/}

{PANEL: The transformation script}

The [basic characteristics](../../../../server/ongoing-tasks/etl/basics) of an Azure Queue Storage ETL script are similar to those of other ETL types.  
The script defines what data to **extract** from the source document, how to **transform** this data,  
and which Azure Queue to **load** it to.

---

#### The loadTo method

To specify which Azure queue to load the data into, use either of the following methods in your script.  
The two methods are equivalent, offering alternative syntax:

* **`loadTo<QueueName>(obj, {attributes})`**
    * Here the target is specified as part of the function name.
    * The target _&lt;QueueName&gt;_ in this syntax is Not a variable and cannot be used as one,  
      it is simply a string literal of the target's name.

* **`loadTo('QueueName', obj, {attributes})`**
    * Here the target is passed as an argument to the method.
    * Separating the target name from the `loadTo` command makes it possible to include symbols like `'-'` and `'.'` in target names.
      This is not possible when the `loadTo<QueueName>` syntax is used because including special characters in the name of a JavaScript function makes it invalid.

   | Parameter      | Type   | Description                                                                                                                      |
   |----------------|--------|----------------------------------------------------------------------------------------------------------------------------------|
   | **QueueName**  | string | The name of the SQS Queue                                                                                                        |
   | **obj**        | object | The object to transfer                                                                                                           |
   | **attributes** | object | An object with optional & required [CloudEvents attributes](../../../../server/ongoing-tasks/etl/queue-etl/overview#cloudevents) |

For example, the following two calls, which load data to "OrdersQueue", are equivalent:

* `loadToOrdersQueue(obj, {attributes})`
* `loadTo('OrdersQueue', obj, {attributes})`

---

The following is a sample script that processes documents from the Orders collection:

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

// Load the object to the "OrdersQueue" in Azure
// =============================================
loadToOrdersQueue(orderData, {
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
- [RabbitMQ ETL](../../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq)
- [Kafka ETL](../../../../server/ongoing-tasks/etl/queue-etl/kafka)

### Studio

- [Studio: Amazon SQS ETL Task](../../../../studio/database/tasks/ongoing-tasks/aws-sqs-etl)
- [Studio: Kafka ETL Task](../../../../studio/database/tasks/ongoing-tasks/kafka-etl-task)
- [Studio: RabbitMQ ETL Task](../../../../studio/database/tasks/ongoing-tasks/rabbitmq-etl-task)
- [Studio: Azure Queue Storage ETL Task](../../../../studio/database/tasks/ongoing-tasks/azure-queue-storage-etl)
