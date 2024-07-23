# Queue ETL: Azure Queue Storage
---

{NOTE: }

* Azure Queue Storage is a Microsoft Azure service that allows for the storage and retrieval of large numbers of messages, 
  enabling communication between applications by allowing them to asynchronously send and receive messages. 
  Each message in a queue can be up to 64 KB in size, and a queue can contain millions of messages,
  providing a robust and scalable solution for data processing.

* Create an **Azure Queue Storage ETL Task** to:
  * Extract data from a RavenDB database   
  * Transform the data using one or more custom scripts
  * Load the resulting JSON object to an Azure Queue destination as a CloudEvents message

* Utilizing this task allows RavenDB to act as an event producer in an Azure Queue architecture.

* [Azure Functions](https://learn.microsoft.com/en-us/azure/azure-functions/functions-overview?pivots=programming-language-csharp)
  can be triggered to consume and process messages that are sent to Azure queues,   
  enabling powerful and flexible workflows.
  The message visibility period and life span in the Queue can be customized through these [ETL configuration options](../../../../server/configuration/etl-configuration#etl.queue.azurequeuestorage.timetoliveinsec).

* Read more about Azure Queue Storage in the platform's [official documentation](https://learn.microsoft.com/en-us/azure/storage/queues/storage-queues-introduction).

---

* This article focuses on how to create an Azure Queue Storage ETL task using the Client API.  
  To define an Azure Queue Storage ETL task from the Studio, see [Studio: Azure Queue Storage ETL Task](../../../../studio/database/tasks/ongoing-tasks/azure-queue-storage-etl).  
  For an **overview of Queue ETL tasks**, see [Queue ETL tasks overview](../../../../server/ongoing-tasks/etl/queue-etl/overview).

* In this page:  
  * [Add an Azure Queue Storage connection string](../../../../server/ongoing-tasks/etl/queue-etl/azure-queue#add-an-azure-queue-storage-connection-string)  
      * [Authentication methods](../../../../server/ongoing-tasks/etl/queue-etl/azure-queue#authentication-methods)  
      * [Exmaple](../../../../server/ongoing-tasks/etl/queue-etl/azure-queue#example)  
      * [Syntax](../../../../server/ongoing-tasks/etl/queue-etl/azure-queue#syntax)
  * [Add an Azure Queue Storage ETL task](../../../../server/ongoing-tasks/etl/queue-etl/azure-queue#add-an-azure-queue-storage-etl-task)  
      * [Example](../../../../server/ongoing-tasks/etl/queue-etl/azure-queue#example-basic)  
      * [Delete processed documents](../../../../server/ongoing-tasks/etl/queue-etl/azure-queue#delete-processed-documents)  
      * [Syntax](../../../../server/ongoing-tasks/etl/queue-etl/azure-queue#syntax-1)
  * [The transformation script](../../../../server/ongoing-tasks/etl/queue-etl/azure-queue#the-transformation-script)
      * [The loadTo method](../../../../server/ongoing-tasks/etl/queue-etl/azure-queue#the-loadto-method)  

{NOTE/}

---

{PANEL: Add an Azure Queue Storage connection string}

Prior to setting up the ETL task, define a connection string that the task will use to access your Azure account.  
The connection string includes the authorization credentials required to connect.

---

#### Authentication methods:  
There are three authenticaton methods available:

* **Connection string**  
    * Provide a single string that includes all the options required to connect to your Azure account.  
      Learn more about Azure Storage connection strings [here](https://learn.microsoft.com/en-us/azure/storage/common/storage-configure-connection-string).
    * Note: the following connection string parameters are mandatory:  
       * `AccountName`
       * `DefaultEndpointsProtocol`
       * `QueueEndpoint` (when using http protocol)
* **Entra ID**
    * Use the Entra ID authorization method to achieve enhanced security by leveraging Microsoft Entra’s robust identity solutions.  
    * This approach minimizes the risks associated with exposed credentials commonly found in connection strings and enables
      more granular control through [Role-Based Access Controls](https://learn.microsoft.com/en-us/azure/role-based-access-control/).
* **Passwordless**
    * This authorization method requires the machine to be pre-authorized and can only be used in self-hosted mode.
    * Passwordless authorization works only when the account on the machine is assigned the Storage Account Queue Data Contributor role; the Contributor role alone is inadequate.

---

#### Example:

{CODE add_azure_connection_string@Server\OngoingTasks\ETL\Queue\AzureQueueStorageEtl.cs /}

---

#### Syntax:

{CODE queue_connection_string@Server\OngoingTasks\ETL\Queue\AzureQueueStorageEtl.cs /} 
{CODE queue_broker_type@Server\OngoingTasks\ETL\Queue\AzureQueueStorageEtl.cs /}
{CODE azure_con_str_settings@Server\OngoingTasks\ETL\Queue\AzureQueueStorageEtl.cs /}

{PANEL/}

{PANEL: Add an Azure Queue Storage ETL task}

{NOTE: }

<a id="example-basic" /> __Example__:

---

* In this example, the Azure Queue Storage ETL Task will -  
  * Extract source documents from the "Orders" collection in RavenDB.  
  * Process each "Order" document using a defined script that creates a new `orderData` object.  
  * Load the `orderData` object to the "OrdersQueue" in an Azure Queue Storage.  
* For more details about the script and the `loadTo` method, see the [transromation script](../../../../server/ongoing-tasks/etl/queue-etl/azure-queue#the-transformation-script) section below.

{CODE add_azure_etl_task@Server\OngoingTasks\ETL\Queue\AzureQueueStorageEtl.cs /}

{NOTE/}
{NOTE: }

<a id="delete-processed-documents" /> __Delete processed documents__:

---

* You have the option to delete documents from your RavenDB database once they have been processed by the Queue ETL task.

* Set the optional `Queues` property in your ETL configuration with the list of Azure queues for which processed documents should be deleted.

{CODE azure_delete_documents@Server\OngoingTasks\ETL\Queue\AzureQueueStorageEtl.cs /}

{NOTE/}

---

#### Syntax

{CODE etl_configuration@Server\OngoingTasks\ETL\Queue\AzureQueueStorageEtl.cs /}

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
   | **QueueName**  | string | The name of the Azure Queue                                                                                                      |
   | **obj**        | object | The object to transfer                                                                                                           |
   | **attributes** | object | An object with optional & required [CloudEvents attributes](../../../../server/ongoing-tasks/etl/queue-etl/overview#cloudevents) |

For example, the following two calls, which load data to "OrdersQueue", are equivalent:

* `loadToOrdersQueue(obj, {attributes})`
* `loadTo('OrdersQueue', obj, {attributes})`

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

- [Studio: Kafka ETL Task](../../../../studio/database/tasks/ongoing-tasks/kafka-etl-task)
- [Studio: RabbitMQ ETL Task](../../../../studio/database/tasks/ongoing-tasks/rabbitmq-etl-task)
- [Studio: Azure Queue Storage ETL Task](../../../../studio/database/tasks/ongoing-tasks/azure-queue-storage-etl)
