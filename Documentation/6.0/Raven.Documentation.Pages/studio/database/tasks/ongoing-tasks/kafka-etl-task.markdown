# Kafka ETL Task
---

{NOTE: }

* RavenDB ETL tasks for **Apache Kafka** -  
   * **Extract** selected data from RavenDB documents  
   * **Transform** the data to new JSON objects and add the new objects to CloudEvents messages.  
   * **Load** the messages to **topics** of a Kafka broker.  
 
* Messages enqueued in Kafka topics are added at the queue's tail.  
  When the messages reach the queue's head, Kafka clients can access and consume them.  
 
* Kafka ETL tasks transfer **documents only**.  
  Document extensions like attachments, counters, or time series, are not transferred.  
 
* This page explains how to create a Kafka ETL task using the Studio.  
  [Learn here](../../../../server/ongoing-tasks/etl/queue-etl/kafka) how to define a Kafka ETL task using code.  

* In this page:  
  * [Open Kafka ETL Task View](../../../../studio/database/tasks/ongoing-tasks/kafka-etl-task#open-kafka-etl-task-view)  
  * [Define Kafka ETL Task](../../../../studio/database/tasks/ongoing-tasks/kafka-etl-task#define-kafka-etl-task)  
  * [Use RavenDB Certificate](../../../../studio/database/tasks/ongoing-tasks/kafka-etl-task#use-ravendb-certificate)  
  * [Options Per Topic](../../../../studio/database/tasks/ongoing-tasks/kafka-etl-task#options-per-topic)  
  * [Edit Transformation Script](../../../../studio/database/tasks/ongoing-tasks/kafka-etl-task#edit-transformation-script)  

{NOTE/}

---

{PANEL: Open Kafka ETL Task View}

![Ongoing Tasks View](images/queue/ongoing-tasks.png "Ongoing Tasks View")

1. **Ongoing Tasks**  
   Click to open the ongoing tasks view.  
2. **Add a Database Task**  
   Click to create a new ongoing task.  

---

![Define ETL Task](images/queue/kafka_task-selection.png "Define ETL Task")

* **Kafka ETL**  
  Click to define a Kafka ETL task.  
   
{PANEL/}

{PANEL: Define Kafka ETL Task}

![Define Kafka ETL Task](images/queue/kafka_etl-define-task.png "Define Kafka ETL Task")

1. **Task Name** (Optional)  
   * Enter a name for your task  
   * If no name is provided, the server will create a name based on the defined connection string,  
     e.g. *Queue ETL to conStr*  

2. **Task State**  
   Select the task state:  
   Enabled - The task runs in the background, transforming and sending documents as defined in this view.  
   Disabled - No documents are transformed and sent.  

3. **Responsible Node** (Optional)  
  * Select a node from the [Database Group](../../../../studio/database/settings/manage-database-group) to be responsible for this task.  
  * If no node is selected, the cluster will assign a responsible node (see [Members Duties](../../../../studio/database/settings/manage-database-group#database-group-topology---members-duties)).  

4. **Create new Kafka connection String**  
    * Select an existing connection string from the list or create a new one.  
    * The connection string defines the destination Kafka broker/s URL/s.  
    * **Name** - Enter a name for the connection string.  
    * **Bootstrap Servers** - Provide at least one target `URL:Port` pair.  
      To push messages to more than one server, use this format: `localhost:9092, localhost:9093`

5. **Add new Connection Option**  
   An optional Key/Value dictionary.
   This option can be used, for example, to provide the additional fields required 
   to connect a secure Kafka server.  
   
     ![Connection to Secure Server](images/queue/kafka_connection-string_connection-options.png "Connection to Secure Server")

6. **Test Connection**  
   Click after defining the connection string, to test the connection to 
   the Kafka topic.  

     ![Successful Connection](images/queue/kafka_successful-connection.png "Successful Connection")

{PANEL/}  

{PANEL: Use RavenDB Certificate}

If RavenDB has been set up **securely**, another option will show up: **Use RavenDB Certificate**

![Use RavenDB Certificate](images/queue/kafka_use-ravenDB-certificate.png "Use RavenDB Certificate")

If enabled, RavenDB will export to the target machine/s the cluster-wide 
certificate defined during setup, and secure its connection with them.  

* If you do that, you will no longer need to define your security options manually 
  (using [Add new connection option](../../../../studio/database/tasks/ongoing-tasks/kafka-etl-task#define-kafka-etl-task)).  
* **Note**, however, that to complete the process you still need to register RavenDB's 
  exported certificate in [Kafka's truststore](https://kafka.apache.org/documentation/streams/developer-guide/security.html) 
  on the target machine/s.  

{PANEL/}

{PANEL: Options Per Topic}

![Advanced](images/queue/kafka_click-for-advanced-options.png "Advanced")

Clicking the Advanced button will display per-topic options.  
In it, you'll find the option to delete documents from RavenDB 
while they were processed by the selected topic.  

![Options Per Topic - Delete Processed Documents](images/queue/kafka_options-per-topic.png "Options Per Topic - Delete Processed Documents")

1. **The Topic**  
   `loadToOrders` is the script instruction to transfer documents to the `Orders` topic.  
2. **Add Topic Options**  
   Click to add a per-topic option.  
3. **Collection/Topic Name**  
   This is the name of the Kafka topic to which the documents are pushed.  
4. **Delete Processed Documents**  
   Enabling this option will remove from the RavenDB collection documents that 
   were processed and pushed to the Kafka topic.  
   {WARNING: }
    Enabling this option will **remove processed documents** from the database.  
    The documents will be deleted after the messages are pushed.  
   {WARNING/}


{PANEL/}

{PANEL: Edit Transformation Script}

![Add or Edit Transformation Script](images/queue/add-or-edit-script.png "Add or Edit Transformation Script")

---

![Add or Edit Transform Script](images/queue/kafka_transformation-script.png "Add or Edit Transform Script")

1. **Script Name**  
   Enter a name for the script (Optional).  
   A default name will be generated if no name is entered, e.g. Script_1  

2. **Script**  
   Edit the transformation script.  
   * Define a **document object** whose contents will be extracted from 
     RavenDB documents and appended to Kafka topic/s.  
     E.g., `var orderData` in the above example.  
   * Make sure that one of the properties of the document object 
     is given the value `id(this)`. This property will contain the 
     RavenDB document ID.  
   * Use the `loadTo<TopicName>` method to pass the document object 
     to the Kafka destination.  

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

## Related Articles

### Server

- [ETL Basics](../../../../server/ongoing-tasks/etl/basics)
- [Queue ETL Overview](../../../../server/ongoing-tasks/etl/queue-etl/overview)
- [Kafka ETL](../../../../server/ongoing-tasks/etl/queue-etl/kafka)
- [RabbitMQ ETL](../../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq)

### Studio

- [Studio: RabbitMQ ETL Task](../../../../studio/database/tasks/ongoing-tasks/rabbitmq-etl-task)
