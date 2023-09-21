# Ongoing Tasks: Queue Sink Overview
---

{NOTE: }

* Message brokers are high-throughput, distributed messaging services that 
  host data they receive from **producer** applications and serve it to 
  **consumer** clients via FIFO data queue/s. 
* RavenDB 5.4 and on can function as a _Producer_ in this architecture.  
  RavenDB 6.0 and on can also function as a _Consumer_.  
  {INFO: }
  This overview and the other pages in the Queue Sink section explain 
  **only** RavenDB's role as a _Consumer_ through the implementation of 
  a sink connector.  
  To learn about RavenDB's role as a _Producer_ please refer to the 
  [Queue ETL section](../../../server/ongoing-tasks/etl/queue-etl/overview).  
  {INFO/}
* RavenDB can run an ongoing Sink task that reads messages from broker queues, 
  transforms them to documents by a user-defined script, and stores the 
  documents in RavenDB's database.  
* Supported broker queues currently include **Apache Kafka** and **RabbitMQ**.  

* In this page:  
   * [Supported Message Brokers](../../../server/ongoing-tasks/queue-sink/overview#supported-message-brokers)  
   * [Task Statistics](../../../server/ongoing-tasks/queue-sink/overview#task-statistics)  

{NOTE/}

---

{PANEL: Supported Message Brokers}

Queue brokers currently supported by RavenDB include **Apache Kafka** and **RabbitMQ**.  

![Ongoing Tasks](images/overview_ongoing-tasks.png "Ongoing Tasks")

1. **Ongoing Tasks**  
   Click to open the ongoing tasks view.  
2. **Add a Database Task**  
   Click to create a new ongoing task.  
3. **Info Hub**  
   Click for usage and licensing assistance.  

      ![Info Hub](images/info-hub.png "Info Hub")

---

![Define Queue Sink Task](images/overview_task-selection.png "Define Queue Sink Task")

1. **Kafka Sink**  
   Click to define a Kafka Queue Sink task.  
2. **RabbitMQ Sink**  
   Click to define a RabbitMQ Queue Sink task.  

{PANEL/}

{PANEL: Task Statistics}

Use Studio's [ongoing tasks stats](../../../studio/database/stats/ongoing-tasks-stats/overview) 
view to see transfer statistics.  

![Queue Brokers Stats](images/overview_stats.png "Ongoing Tasks")

1. **Kafka sink task statistics**  
   All statistics related to the sink task.  
   Click the bars to expand or collide statistics.  
   Hover over bar sections to expose statistics.  
2. **RabbitMQ sink task statistics**  
3. **Sink statistics**  
    * Total duration  
      The time it took to get a batch of documents (in MS) 
    * Currently allocated  
      Memory allocated for the task (in MB)  
    * Number of processed messages  
      The number of messages that were recognized and processed  
    * Number of read messages  
      The number of messages that were actually transferred to the database  
    * Successfully processed  
      Has this batch of messages been fully processed (yes/no)  
4. **Queue readings**  
   The duration of reading from queues (in MS)  
5. **Script processing**  
   The duration of script processing (in MS)  

{PANEL/}

## Related Articles

### Server

- [ETL Basics](../../../server/ongoing-tasks/etl/basics)
- [RabbitMQ ETL](../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq)
- [Kafka ETL](../../../server/ongoing-tasks/etl/queue-etl/kafka)
- [RabbitMQ Queue Sink](../../../server/ongoing-tasks/queue-sink/rabbit-mq-queue-sink)
- [Kafka Queue Sink](../../../server/ongoing-tasks/queue-sink/kafka-queue-sink)

### Studio

- [Studio: Kafka ETL Task](../../../studio/database/tasks/ongoing-tasks/kafka-etl-task)
- [Studio: RabbitMQ ETL Task](../../../studio/database/tasks/ongoing-tasks/rabbitmq-etl-task)
