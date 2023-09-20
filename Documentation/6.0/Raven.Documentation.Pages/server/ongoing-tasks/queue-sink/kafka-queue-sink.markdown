# Queue Sink: Apache Kafka
---

{NOTE: }

* **Apache Kafka** is a distributed, high-performance, transactional messaging 
  platform, that remains performant as the number of messages it needs to process 
  increases and the number of events it needs to stream climbs to the big-data zone.  
  
* RavenDB can harness the advantages presented by message brokers like Kafka 
  both as a producer (by [running ETL tasks](../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq)) 
  and as a **consumer** (using a sink connector to read enqueued messages).  

* To use RavenDB as a consumer, define an ongoing Queue Sink task that will 
  access selected Kafka topics and read enqueued messages, use a user-defined 
  transform script to construct documents out of read messages contents, and 
  potentially store the documents in RavenDB collections.  

* In this page:  
  * [The Queue Sink Task](../../../server/ongoing-tasks/queue-sink/kafka-queue-sink#the-queue-sink-task)  
  * [Client API](../../../server/ongoing-tasks/queue-sink/kafka-queue-sink#client-api)  
     * [Add a Kafka Connection String](../../../server/ongoing-tasks/queue-sink/kafka-queue-sink#add-a-kafka-connection-string)  
     * [Add a Kafka Sink Task](../../../server/ongoing-tasks/queue-sink/kafka-queue-sink#add-a-kafka-sink-task)  

{NOTE/}

---

{PANEL: The Queue Sink Task}

In RavenDB, a Kafka sink is implemented as a user-defined ongoing sink task 
that connects a Kafka server, retrieves enqueued messages from selected Kafka 
topics, runs a user-defined transformation script to construct documents, and 
stores the documents in RavenDB collections.  

* **Connecting a Kafka server** 
  Connecting a Kafka broker requires a connection string.  
  Read [below](../../../server/ongoing-tasks/queue-sink/kafka-queue-sink#add-a-kafka-connection-string) 
  how to add a Kafka connection string via API.  
  Read [here](../../../) how to add a Kafka connection string using Studio.  

* **Retrieving enqueued messages** from selected Kafka topics  
  When a message is sent to a Kafka broker by a producer, it is pushed to 
  the tail of the topic assigned to it. The message then advances in the queue 
  as preceding messages are pulled, until it finally reaches the queue's head 
  where RavenDB's sink task and other consumers can get it.  

* **Running a user-defined transformation script**  
  A sink connector's transformation script is a JavaScript segment. Its basic 
  role is to retrieve selected Kafka messages or message properties, and prepare 
  them for storage in RavenDB.  
  The script can, however, also load and delete RavenDB documents (e.g. to check 
  and handle potential duplicates) and perform various other functions.  

* **Storing documents in RavenDB collections**  
  {INFO: }
  Note that producers may enqueue 
  [multiple instances](../../../server/ongoing-tasks/etl/queue-etl/kafka#idempotence-and-message-duplication) 
  of the same document.  
  It is **the consumer's own responsibility** (if processing each message only 
  once is important to it) to verify the uniqueness of each consumed message.  
  {INFO/}

{PANEL/}

{PANEL: Client API}

#### Add a Kafka Connection String

Prior to defining a Kafka sink task, add a **Kafka connection string** 
that the task will use to connect the message broker's bootstrap servers.  

To create the connection string:  

* Create a `QueueConnectionString`instance with the connection string configuration.  
  Pass it to the `PutConnectionStringOperation` store operation to add the connection string.  

    `QueueConnectionString`:  
    {CODE add_kafka_connection-string@Server\OngoingTasks\QueueSink\QueueSink.cs /}
  
    `QueueBrokerType`:  
    {CODE QueueBrokerType@Server\OngoingTasks\QueueSink\QueueSink.cs /}

    | Property | Type | Description |
    |:-------------|:-------------|:-------------|
    | **Name** | `string` | Connection string name |
    | **BrokerType** | `QueueBrokerType` | Set to `QueueBrokerType.Kafka` for a Kafka connection string |
    | **KafkaConnectionSettings** | `KafkaConnectionSettings[]` | A list of comma-separated host:port URLs to Kafka brokers |

---

#### Add a Kafka Sink Task

To create the Sink task:  

* Create a `QueueSinkScript` instance with a transformation script that constructs 
  RavenDB documents from read queue messages.  
  {CODE define-kafka-sink-script@Server\OngoingTasks\QueueSink\QueueSink.cs /}

* Prepare a `QueueSinkConfiguration`object with the sink task configuration, 
  collecting the connection string and the transformation script.  

    `QueueSinkConfiguration` properties:  

    | Property | Type | Description |
    |:-------------|:-------------|:-------------|
    | **Name** | `string` | The sink task name |
    | **ConnectionStringName** | `string` | The registered connection string name |
    | **BrokerType** | `QueueBrokerType` | Set to `QueueBrokerType.Kafka` to define a Kafka sink task |
    | **Scripts** | `List<QueueSinkScript>` | A list of strings holding transformation scripts |

* Pass this object to the `AddQueueSinkOperation` store operation to add the Sink task.  

    `QueueSinkScript` properties:  

    | Property | Type | Description |
    |:-------------|:-------------|:-------------|
    | **Name** | `string` | Script name|
    | **Queues** | `List<string>` | list of queues |
    | **Script** | `string  ` | A transform script |

**Code Sample**:  
{CODE add_RabbitMQ_sink-task@Server\OngoingTasks\QueueSink\QueueSink.cs /}

{PANEL/}

## Related Articles

### Server

- [ETL Basics](../../../server/ongoing-tasks/etl/basics)
- [Queue ETL Overview](../../../server/ongoing-tasks/etl/queue-etl/overview)
- [RabbitMQ ETL](../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq)
- [Queue Sink Overview](../../../server/ongoing-tasks/queue-sink/overview)
- [RabbitMQ Queue Sink](../../../server/ongoing-tasks/queue-sink/rabbit-mq-queue-sink)

### Studio

- [Studio: Kafka ETL Task](../../../studio/database/tasks/ongoing-tasks/kafka-etl-task)
- [Studio: RabbitMQ ETL Task](../../../studio/database/tasks/ongoing-tasks/rabbitmq-etl-task)
