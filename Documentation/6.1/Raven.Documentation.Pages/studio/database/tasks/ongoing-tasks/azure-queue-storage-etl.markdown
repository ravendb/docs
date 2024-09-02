# Azure Queue Storage ETL Task
---

{NOTE: }

* The RavenDB **Azure Queue Storage ETL task** -  
   * **Extracts** selected data from RavenDB documents from specified collections.
   * **Transforms** the data into JSON object.
   * Wraps the JSON objects as [CloudEvents messages](https://cloudevents.io) and **Loads** them to an Azure Queue Storage.

* The Azure Queue Storage ETL task transfers **documents only**.  
  Document extensions like attachments, counters, time series, and revisions are not sent.  
  The maximum message size in Azure Queue Storage is 64KB, documents larger than this won’t be loaded.
  
* The Azure Queue Storage enqueues incoming messages at the tail of a queue. 
  [Azure Functions](https://learn.microsoft.com/en-us/azure/azure-functions/functions-bindings-storage-queue-trigger?tabs=python-v2%2Cisolated-process%2Cnodejs-v4%2Cextensionv5&pivots=programming-language-csharp) 
  can be triggered to access and consume messages when the enqueued messages advance to the queue head.
 
---

* This page explains how to create an Azure Queue Storage ETL task using the Studio.  
  [Learn here](../../../../server/ongoing-tasks/etl/queue-etl/azure-queue-storage) how to define an Azure Queue Storage ETL task using the Client API.  
 
* In this page:  
  * [Open Azure Queue Storage ETL task view](../../../../studio/database/tasks/ongoing-tasks/azure-queue-storage-etl#open-azure-queue-storage-etl-task-view)  
  * [Define Azure Queue Storage ETL task](../../../../studio/database/tasks/ongoing-tasks/azure-queue-storage-etl#define-azure-queue-storage-etl-task)  
  * [Authentication method](../../../../studio/database/tasks/ongoing-tasks/azure-queue-storage-etl#authentication-method)  
  * [Options per queue](../../../../studio/database/tasks/ongoing-tasks/azure-queue-storage-etl#options-per-queue)  
  * [Add transformation script](../../../../studio/database/tasks/ongoing-tasks/azure-queue-storage-etl#add-transformation-script)  

{NOTE/}

---

{PANEL: Open Azure Queue Storage ETL task view}

![Ongoing Tasks View](images/queue/add-ongoing-task.png "Add Ongoing Task")

1. **Ongoing Tasks**  
   Click to open the ongoing tasks view.  
2. **Add a Database Task**  
   Click to create a new ongoing task.  

---

![Define ETL Task](images/queue/aqs-task-selection.png "Define ETL Task")
   
{PANEL/}

{PANEL: Define Azure Queue Storage ETL task}

![Define Azure Queue Storage ETL Task](images/queue/aqs-etl-define-task.png "Define Azure Queue Storage ETL Task")

1. **Task Name** (Optional)  
   * Enter a name for your task  
   * If no name is provided, the server will create a name based on the defined connection string name,  
     e.g. *"Queue ETL to &lt;ConStrName&gt;"*  

2. **Task State**  
   Select the task state:  
   Enabled - The task runs in the background, transforming and sending documents as defined in this view.  
   Disabled - No documents are transformed and sent.  

3. **Set Responsible Node** (Optional)  
  * Select a node from the [Database Group](../../../../studio/database/settings/manage-database-group) to be responsible for this task.  
  * If no node is selected, the cluster will assign a responsible node (see [Members Duties](../../../../studio/database/settings/manage-database-group#database-group-topology---members-duties)).  

4. **Create new Azure Queue Storage connection String**  
   * The connection string contains the necessary information to connect to an Azure storage account.  
     Toggle OFF to select an existing connection string from the list, or toggle ON to create a new one.  
   * **Name** - Enter a name for the connection string.  
   * **Authentication method** - Select the authentication method by which to connect to an Azure storage account.  
     Learn more about the available authentication methods [below](../../../../studio/database/tasks/ongoing-tasks/azure-queue-storage-etl#authentication-method).  

5. **Test Connection**  
   After defining the connection string, click to test the connection to the Azure storage account.  

6. **Add Transformation Script**  
   The sent data can be filtered and modified by multiple transformation JavaScript scripts that are added to the task.  
   Click to add a transformation script.

7. **Advanced**  
   Click to open the [advanced section](../../../../studio/database/tasks/ongoing-tasks/azure-queue-storage-etl#options-per-queue) where you can configure the deletion of RavenDB documents per queue.

{PANEL/}  

{PANEL: Authentication method}

The available authentication methods to an Azure storage account are:

---

* **Connection String**  

  * A single string that includes all the options required to connect the Azure storage account.  
    Learn more about Azure Storage connection strings [here](https://learn.microsoft.com/en-us/azure/storage/common/storage-configure-connection-string).
  * The following connection string parameters are mandatory:  
        * `AccountName`
        * `AccountKey`
        * `DefaultEndpointsProtocol`
        * `QueueEndpoint` (when using http protocol)

    ![Connection string method](images/queue/aqs-etl-conn-str-method.png "Connection string method")

* **Entra ID**

    * Use the Entra ID authorization method to achieve enhanced security by leveraging Microsoft Entra’s robust identity solutions.  
    * This approach minimizes the risks associated with exposed credentials commonly found in connection strings and enables more granular control through [Role-Based Access Controls](https://learn.microsoft.com/en-us/azure/role-based-access-control/).

    ![Entra ID method](images/queue/aqs-etl-entra-id-method.png "Entra ID method")

* **Passwordless**

    * This authorization method requires the machine to be pre-authorized and can only be used in self-hosted mode.
    * Passwordless authorization works only when the account on the machine is assigned the Storage Account Queue Data Contributor role; the Contributor role alone is inadequate.

    ![Passwordless method](images/queue/aqs-etl-passwordless-method.png "passwordless method")

{PANEL/}

{PANEL: Options per queue}

You can configure the ETL process to delete documents from RavenDB that have already been sent to the queues.

![Options per queue](images/queue/aqs-etl-advanced.png "Advanced options")

1. **Add queue options**  
   Click to add a per-queue option.  
2. **Queue Name**  
   Enter the name of the Azure Queue Storage the documents are loaded to.  
   Note: The queue name defined in the transform script must follow the set of rules outlined in:  
   [Naming Queues and Metadata](https://learn.microsoft.com/en-us/rest/api/storageservices/naming-queues-and-metadata#queue-names ).
3. **Delete processed documents**  
   Enable this option to remove documents that were processed and loaded to the Azure Queue Storage from the RavenDB database.   
4. **Delete queue option**  
   Click to delete the queue option from the list.

{PANEL/}

{PANEL: Add transformation script}

![Add or Edit Transformation Script](images/queue/add-or-edit-script.png "Add or edit transformation script")

1. **Add transformation script**  
   Click to add a new transformation script that will process documents from RavenDB collection(s).
2. **Edit transformation script**  
   Click to edit this script.  
3. **Delete script**  
   Click to remove this script.
   
---

![Define Transformation Script](images/queue/aqs-etl-transformation-script.png "Define transform script")

1. **Script Name** - Enter a name for the script (Optional).  
   A default name will be generated if no name is entered, e.g. _Script_1_.  

2. **Script** - Edit the transformation script.  
   Sample script:  

       {CODE-BLOCK:javascript}
 {
     // Define a "document object" whose contents will be extracted from RavenDB documents 
     // and sent to the Azure Queue Storage. e.g. 'var orderData':
     var orderData = {
         // Verify that one of the properties of this object is given the value 'id(this)'.  
         Id: id(this), // Property with RavenDB document ID
         OrderLinesCount: this.Lines.length,
         TotalCost: 0
     };

     for (var i = 0; i < this.Lines.length; i++) {
         var line = this.Lines[i];
         var cost = (line.Quantity * line.PricePerUnit) * ( 1 - line.Discount);
         orderData.TotalCost += cost;
     }

     // Use the 'loadTo<QueueName>' method 
     // to transfer the document object to the Azure Queue destination.
     loadToOrders(orderData, { // Load to a Queue by the name of "Orders" with optional params 
         Id: id(this),
         Type: 'com.github.users',
         Source: '/registrations/direct-signup'
     });
 }
       {CODE-BLOCK/}

3. **Syntax** - Click for a transformation script syntax Sample.  

4. **Collections**  
   * **Select (or enter) a collection**  
     Type or select the names of the RavenDB collections your script is using.  
   * **Collections Selected**  
     A list of collections that were already selected.  

5. **Apply script to documents from beginning of time (Reset)**  
   * This toggle is available only when editing an existing script.  
   * When this option is **enabled**:  
     The script will be executed over all existing documents in the specified collections the first time the task runs.  
   * When this option is **disabled**:  
     The script will be executed only over new and modified documents.  

6. **Add/Update** - Click to add a new script or update an existing script.  
   **Cancel** - Click to cancel your changes.  

7. **Test Script** - Click to test the transformation script.  

{PANEL/}

## Related Articles

### Server

- [ETL Basics](../../../../server/ongoing-tasks/etl/basics)
- [Queue ETL Overview](../../../../server/ongoing-tasks/etl/queue-etl/overview)
- [Kafka ETL](../../../../server/ongoing-tasks/etl/queue-etl/kafka)
- [RabbitMQ ETL](../../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq)
- [Azure Queue Storage ETL](../../../../server/ongoing-tasks/etl/queue-etl/azure-queue)

### Studio

- [Studio: Kafka ETL Task](../../../../studio/database/tasks/ongoing-tasks/kafka-etl-task)
