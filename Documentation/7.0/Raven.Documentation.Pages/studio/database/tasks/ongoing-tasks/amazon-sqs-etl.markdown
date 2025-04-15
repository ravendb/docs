# Studio: Amazon SQS ETL Task
---

{NOTE: }

* The RavenDB **Amazon SQS ETL task** -  
   * **Extracts** selected data from RavenDB documents of specified collections.
   * **Transforms** the data into JSON object.
   * Wraps the JSON objects as [CloudEvents messages](https://cloudevents.io) and **Loads** them to an SQS destination.

* The Amazon SQS ETL task transfers **documents only**.  
  Document extensions like attachments, counters, time series, and revisions are not sent.  

* This page explains how to create an Amazon SQS ETL task using Studio.  
  [Learn here](../../../../server/ongoing-tasks/etl/queue-etl/amazon-sqs) how to define 
  an Amazon SQS ETL task using the Client API.  .  
 
* In this page:  
  * [Ongoing Tasks View](../../../../studio/database/tasks/ongoing-tasks/amazon-sqs-etl#ongoing-tasks-view)  
  * [Define the Amazon SQS ETL task](../../../../studio/database/tasks/ongoing-tasks/amazon-sqs-etl#define-amazon-sqs-etl-task)  
  * [Add or Edit Transformation Script](../../../../studio/database/tasks/ongoing-tasks/amazon-sqs-etl#add-or-edit-transformation-script)  
  * [Comments](../../../../studio/database/tasks/ongoing-tasks/amazon-sqs-etl#comments)  
      * [ETL message size -vs- Queue message size](../../../../studio/database/tasks/ongoing-tasks/amazon-sqs-etl#etl-message-size--vs--queue-message-size)  
{NOTE/}

---

{PANEL: Ongoing Tasks View}

![Open the 'Add a Database Task' window](images/sqs_ongoing-tasks.png "Open the 'Add a Database Task' window")

1. **Ongoing Tasks**  
   Click to open the ongoing tasks view.  
2. **Add a Database Task**  
   Click to create a new ongoing task.  

---

![Select the Task to Create](images/sqs_task-selection.png "Select the Task to Create")
   
{PANEL/}

{PANEL: Define Amazon SQS ETL task}

![Define Amazon SQS ETL Task](images/sqs_etl-define-task.png "Define Amazon SQS ETL Task")

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

4. **Connection string and Authentication**  
   The connection string contains the necessary information to connect to an SQS destination.  
    * `A.` **Create a new connection string or use an existing one**  
      Toggle ON to create a new connection string to an Amazon SQS destination, 
      or OFF to select an existing connection string.  
    * `B.` **Authentication**  
      Select if and how to authenticate with the SQS destination.  
      **Basic** will allow you to enter an Access key, a Secret key and a Region name to authenticate with.  
      **Passwordless** requires the machine to be pre-authorized and can only be used in self-hosted mode.  
    * `C.` **Advanced**  
      Click to open per-queue advanced options.  
      Use this option to determine whether to [delete documents](../../../../server/ongoing-tasks/etl/queue-etl/amazon-sqs#delete-processed-documents) 
      from the database after they are processed.  

         ![Delete processed documents](images/sqs_delete-processed-docs.png "Delete processed documents")

    * `D.` **Test Connection**  
      After defining the connection string, click to test the connection to the SQS destination.  

5. **Transform Scripts**  
   an ETL process can apply multiple transformation scripts to the data it handles.  
   Click **Add Transformation Script** to add a script.  

      ![Add or Edit Transformation Script](images/queue/add-or-edit-script.png "Add or edit transformation script")
 
      1. **Add transformation script**  
         Click to add a new transformation script that will process documents from RavenDB collection(s).
      2. **Edit transformation script**  
         Click to edit this script.  
      3. **Delete script**  
         Click to remove this script.

{PANEL/}  

{PANEL: Add or Edit Transformation Script}

![Add or Edit Transform Script](images/sqs_etl_transformation-script.png "Add or Edit Transform Script")

1. **Script Name**  
   Enter a name for the script (Optional).  
   A default name will be generated if no name is entered, e.g. Script_1  

2. **Script**  
   Edit the transformation script.  
   * Define a **document object** whose contents will be extracted from 
     RavenDB documents and sent to the destination database.  
     E.g., `var orderData` in the above example.  
   * Make sure that one of the properties of the document object 
     is given the value `id(this)`. This property will contain the 
     RavenDB document ID.  
   * Use the `loadTo<TableName>` method to pass the document object 
     to the destination table.  

3. **Syntax**  
   Click for a transformation script Syntax Sample.  

4. **Collections**  
    * **Select (or enter) a collection**  
      Type or select the names of the collections your script is using.  
    * **Collections Selected**  
      A list of collections that were already selected.  

5. **Apply script to documents from beginning of time (Reset)**  
    * When this option is **enabled**:  
      The script will be executed over **all existing documents in the 
      specified collections** the first time the task runs.  
    * When this option is **disabled**:  
      The script will be executed **only over new and modified documents**.  

6. **Add/Update**  
   Click to add a new script or update the task with changes made in an existing script.  

7. **Cancel**  
   Click to cancel your changes.  

8. **Test Script**  
   Click to **test** the transformation script.  

{PANEL/}

{PANEL: Comments}

#### ETL message size -vs- Queue message size
Please **be aware** that the maximum size of an SQS queue message is `64 KB`, while the 
maximum size of an ETL message to the queue is `256 KB`.  
The significance of this difference is that when a maximum-size ETL message arrives 
at its destination queue it may be charged for not 1 but 4 queue messages.  

{PANEL/}

## Related Articles

### Server

- [ETL Basics](../../../../server/ongoing-tasks/etl/basics)
- [Amazon SQS ETL](../../../../server/ongoing-tasks/etl/queue-etl/amazon-sqs)
- [Queue ETL Overview](../../../../server/ongoing-tasks/etl/queue-etl/overview)
- [Kafka ETL](../../../../server/ongoing-tasks/etl/queue-etl/kafka)
- [RabbitMQ ETL](../../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq)
- [Azure Queue Storage ETL](../../../../server/ongoing-tasks/etl/queue-etl/azure-queue)

### Studio

- [Studio: Kafka ETL Task](../../../../studio/database/tasks/ongoing-tasks/kafka-etl-task)
