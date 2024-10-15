# RabbitMQ Queue Sink Task
---

{NOTE: }


* **RabbitMQ** brokers are designed to disperse data to multiple queues, 
  making for a flexible data channeling system that can easily handle complex 
  message streaming scenarios.  

* RavenDB can collaborate with message brokers like RabbitMQ both as 
  a **producer**, by running [ETL tasks](../../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq)), 
  and as a **consumer**, using a sink task to consume enqueued messages.  

* To use RavenDB as a consumer, define an ongoing **Queue Sink Task**. 
  Sink tasks can read batches of JSON formatted messages from RabbitMQ 
  queues, construct documents using user-defined scripts, and store the 
  documents in RavenDB collections.  

* This page explains how to create a RabbitMQ sink task using the Studio.  
  Learn more about RavenDB queue sinks [here](../../../../server/ongoing-tasks/queue-sink/overview).  
  Learn how to define a RabbitMQ queue sink using the API [here](../../../../server/ongoing-tasks/queue-sink/rabbit-mq-queue-sink).  

* In this page:  
  * [Add a Database Task](../../../../studio/database/tasks/ongoing-tasks/rabbitmq-queue-sink#add-a-database-task)  
  * [Define a RabbitMQ Sink Task](../../../../studio/database/tasks/ongoing-tasks/rabbitmq-queue-sink#define-a-rabbitmq-sink-task)  
      * [Define and Test Task Scripts](../../../../studio/database/tasks/ongoing-tasks/rabbitmq-queue-sink#define-and-test-task-scripts)  
  * [Task Statistics](../../../../studio/database/tasks/ongoing-tasks/rabbitmq-queue-sink#task-statistics)  

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

![Task Selection](images/queue/sink/rabbitmq_task-selection.png "Task Selection")

* Click to create a RabbitMQ sink task.

{PANEL/}

{PANEL: Define a RabbitMQ Sink Task}

![New RabbitMQ Sink](images/queue/sink/new-rabbitmq-sink.png "New RabbitMQ Sink")

1. **Save** to store the configuration and exit. If the task is enabled it will start running.   
   **Cancel** to revoke the creation of a new task or the changes made to an existing task.  

2. **Task Name** (Optional)  
   * Enter a name for your task  
   * If no name is provided, RavenDB will create a name based on the defined connection string,  
     e.g. *Queue Sink to RabbitMqTaskConStr*  

3. **Task State**  
   Select the task state:  
   Enabled - The task runs in the background, reading, manipulating, and storing data as defined in this view.  
   Disabled - No documents are read or stored, and the task's script is inactive.  

4. **Responsible Node** (Optional)  
  * Select a node from the [Database Group](../../../../studio/database/settings/manage-database-group) 
    to be responsible for this task.  
  * If no node is selected, the cluster will assign a responsible node 
    (see [Members Duties](../../../../studio/database/settings/manage-database-group#database-group-topology---members-duties)).  

5. **Create a new RabbitMQ connection String**  
   The connection string defines the source RabbitMQ brokers URLs.  
   Enable to create a new connection string, or leave disabled to select an existing string.  
   
      ![RabbitMQ Connection String](images/queue/sink/rabbitmq-connection-string.png "RabbitMQ Connection String")
   
      * A. **Name** - Enter a name for the new connection string.  
      * B. **Connection string** (format: `amqp://guest:guest@localhost:5672/`)

6. **Test Connection**  
   Click after defining the connection string, to test the connection to 
   the RabbitMQ broker.  

     ![Successful Connection](images/queue/sink/rabbitmq_successful-connection.png "Successful Connection")

7. **Scripts**
   Click Add Script to add the task a new script.  
   Edit or Delete any existing script from the list.  

---

### Define and Test Task Scripts

![Task Script Editor](images/queue/sink/rabbitmq-script-area.png "Task Script Editor")

1. **Name**: Name the script, or leave it for the task to name it (e.g. `Script_1`).  

2. **Syntax**: Click for assistance and sample scripts.  

3. **Script**: [Edit the script](../../../../server/ongoing-tasks/queue-sink/kafka-queue-sink#running-user-defined-scripts).  

4. **Source Queues**  
   Enter the name of at least one RabbitMQ queue that resides on the broker/s 
   the task connects, and click **Add Queue** to add it to the queues list.  

5. **Add** a new script to the list of scripts that this task runs, or 
   **Update** an existing script (a different button appears for existing 
   and new scripts).  

6. **Cancel** to revoke a new script or any changes made in an existing one.  

7. **Test Script**  
    Click to test your script in a test area without actually loading data 
    from queues or storing documents in RavenDB.  
    
     ![Test Script](images/queue/sink/rabbitmq-test-area.png "Test Script")
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

1. **RabbitMQ sink task statistics**  
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
   Time duration of reading from RabbitMQ queues (in MS)  
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
- [Studio: Kafka Sink Task](../../../../studio/database/tasks/ongoing-tasks/kafka-queue-sink)
