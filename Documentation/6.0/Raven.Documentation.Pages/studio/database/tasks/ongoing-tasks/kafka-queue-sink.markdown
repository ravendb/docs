# Kafka Queue Sink Task
---

{NOTE: }

* **Apache Kafka** is a distributed, high-performance, transactional messaging 
  platform, that remains performant even as the number of messages it processes, 
  stores and streams climbs to the big-data zone.  
  
* RavenDB can collaborate with message brokers like Kafka both as a **producer**, 
  by running [ETL tasks](../../../../studio/database/tasks/ongoing-tasks/kafka-etl-task)), 
  and as a **consumer**, using a sink task to consume enqueued messages.  

* To use RavenDB as a consumer, define an ongoing **Queue Sink Task**. Sink tasks 
  can read batches of JSON formatted enqueued messages from Kafka topics, construct 
  documents using user-defined scripts, and store the documents in RavenDB collections.  

* This page explains how to create a Kafka queue sink task using the Studio.  
  Learn more about RavenDB queue sinks [here](../../../../server/ongoing-tasks/queue-sink/overview).  
  Learn how to define a Kafka queue sink using the API [here](../../../../server/ongoing-tasks/queue-sink/kafka-queue-sink).  

* In this page:  
  * [Add a Database Task](../../../../studio/database/tasks/ongoing-tasks/kafka-queue-sink#add-a-database-task)  
  * [Define a Kafka Sink Task](../../../../studio/database/tasks/ongoing-tasks/kafka-queue-sink#define-a-kafka-sink-task)  
      * [Define and Test Task Scripts](../../../../studio/database/tasks/ongoing-tasks/kafka-queue-sink#define-and-test-task-scripts)  
  * [Task Statistics](../../../../studio/database/tasks/ongoing-tasks/kafka-queue-sink#task-statistics)  

{NOTE/}

---

{PANEL: Add a Database Task}

To open the ongoing tasks view: 

![Ongoing Tasks](images/queue/sink/ongoing-tasks-view.png "Ongoing Tasks")

1. **Ongoing Tasks**  
   Click to open the ongoing tasks view.  
2. **Add a Database Task**  
   Click to create a new ongoing task.  
3. **Info Hub**  
   Click for usage and licensing assistance.  

      ![Info Hub](images/queue/sink/info-hub.png "Info Hub")

---

![Task Selection](images/queue/sink/kafka_task-selection.png "Task Selection")

* Click to create a Kafka sink task.

{PANEL/}

{PANEL: Define a Kafka Sink Task}

![New Kafka Sink](images/queue/sink/new-kafka-sink.png "New Kafka Sink")

1. **Save** to store the configuration and exit. If the task is enabled it will start running.   
   **Cancel** to revoke the creation of a new task or the changes made to an existing task.  

2. **Task Name** (Optional)  
   * Enter a name for your task  
   * If no name is provided, RavenDB will create a name based on the defined connection string,  
     e.g. *Queue Sink to KafkaServerConStr*  

3. **Task State**  
   Select the task state:  
   Enabled - The task runs in the background, reading, manipulating, and storing data as defined in this view.  
   Disabled - No documents are read or stored, and the task's script is inactive.  

4. **Responsible Node** (Optional)  
  * Select a node from the [Database Group](../../../../studio/database/settings/manage-database-group) 
    to be responsible for this task.  
  * If no node is selected, the cluster will assign a responsible node 
    (see [Members Duties](../../../../studio/database/settings/manage-database-group#database-group-topology---members-duties)).  

5. **Create a new Kafka connection String**  
   The connection string defines the source Kafka brokers URLs.  
   Enable to create a new connection string, or leave disabled to select an existing string.  
   
      ![Kafka Connection String](images/queue/sink/kafka-connection-string.png "Kafka Connection String")
   
      * A. **Name** - Enter a name for the new connection string.  
      * B. **Bootstrap Servers** - Provide at least one source server (format: `localhost:9092,localhost:9093`)
      * C. **Add new Connection Option**  
           An optional Key/Value dictionary.
           This option can be used, for example, to provide the additional fields required 
           to connect a secure Kafka server.  
           
           ![Connection to Secure Server](images/queue/kafka_connection-string_connection-options.png "Connection to Secure Server")

6. **Test Connection**  
   Click after defining the connection string, to test the connection to 
   the Kafka server.  

     ![Successful Connection](images/queue/sink/kafka_successful-connection.png "Successful Connection")

7. **Scripts**
   Click Add Script to add the task a new script.  
   Edit or Delete any existing script from the list.  

---

### Define and Test Task Scripts

![Task Script Editor](images/queue/sink/kafka-script-area.png "Task Script Editor")

1. **Name**: Name the script, or leave it for the task to name it (e.g. `Script_1`).  

2. **Syntax**: Click for assistance and sample scripts.  

3. **Script**: [Edit the script](../../../../server/ongoing-tasks/queue-sink/kafka-queue-sink#running-user-defined-scripts).  

4. **Topic**  
   Enter the name of at least one topic that resides on the Kafka server/s 
   the task connects, and click **Add Topic** to add it to the topics list.  

5. **Add** a new script to the list of scripts that this task runs, or 
   **Update** an existing script (a different button appears for existing 
   and new scripts).  

6. **Cancel** to revoke a new script or any changes made in an existing one.  

7. **Test Script**  
    Click to test your script in a test area without actually loading data 
    from topics or storing documents in RavenDB.  
    
     ![Test Script](images/queue/sink/kafka-test-area.png "Test Script")
      * A. **Script** - Edit your script here to find the version that 
        produces the documents you want.  
      * B. **Message** - write here any message you want in JSON format. 
        Your script will be applied to this message so you can check the 
        outcome.  
      * C. **Test** to run the test, or **Close Test Area** to return to 
        the task editing view.  
      * D. **Documents** - The documents created when the test is executed.  
{PANEL/}

{PANEL: Task Statistics}

Sink statistics are added to RavenDB's ongoing tasks stats view, where their 
performance can be examined from various aspects. To watch these statistics, 
enter Studio's [ongoing tasks stats](../../../../studio/database/stats/ongoing-tasks-stats/overview) 
view.  

![Queue Sink Stats](images/queue/sink/kafka-stats.png "Queue Sink Stats")

1. **Kafka sink task statistics**  
   All statistics related to the sink task.  
   Click the bars to expand or collide statistics.  
   Hover over bar sections to expose statistics.  
2. **Sink statistics**  
    * Total duration  
      The time it took to get a batch of documents (in MS) 
    * Currently allocated  
      The memory allocated for the task (in MB)  
    * Number of processed messages  
      The number of messages that were recognized and processed  
    * Number of read messages  
      The number of messages that were actually transferred to the database  
    * Successfully processed  
      Has this batch of messages been fully processed (yes/no)  
3. **Queue readings**  
   Time duration of reading from Kafka topics (in MS)  
5. **Script processing**  
   Time duration of script processing (in MS)  

{PANEL/}

## Related Articles

### Server

- [ETL Basics](../../../../server/ongoing-tasks/etl/basics)
- [Queue ETL Overview](../../../../server/ongoing-tasks/etl/queue-etl/overview)
- [Kafka ETL](../../../../server/ongoing-tasks/etl/queue-etl/kafka)
- [Kafka Sink](../../../../server/ongoing-tasks/queue-sink/kafka-queue-sink)  
- [RabbitMQ ETL](../../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq)
- [RabbitMQ Sink](../../../../server/ongoing-tasks/queue-sink/rabbit-mq-queue-sink)  

### Studio

- [Studio: RabbitMQ ETL Task](../../../../studio/database/tasks/ongoing-tasks/rabbitmq-etl-task)
- [Studio: RabbitMQ Sink Task](../../../../studio/database/tasks/ongoing-tasks/rabbitmq-queue-sink)
