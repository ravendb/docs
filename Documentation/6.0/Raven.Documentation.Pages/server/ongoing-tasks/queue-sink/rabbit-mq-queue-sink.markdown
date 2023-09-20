# Queue Sink: RabbitMQ
---

{NOTE: }

* **RabbitMQ** exchanges are designed to disperse data to multiple queues, 
  making for a flexible data channeling system that can easily handle complex 
  message streaming scenarios.  

* RavenDB can harness the advantages presented by RabbitMQ brokers both as 
  a producer (by [running ETL tasks](../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq)) 
  and as a **consumer** (using a sink connector to read enqueued messages).  

* To use RavenDB as a consumer, define an ongoing Queue Sink task that will 
  access selected RabbitMQ queues and read enqueued messages, use a user-defined 
  transform script to construct documents out of read messages contents, and 
  potentially store the documents in RavenDB collections.  

* In this page:  
  * [The RabbitMQ Sink Task](../../../server/ongoing-tasks/queue-sink/rabbit-mq-queue-sink#the-rabbitmq-sink-task)  
  * [Client API](../../../server/ongoing-tasks/queue-sink/rabbit-mq-queue-sink#client-api)  
     * [Add a RabbitMQ Connection String](../../../server/ongoing-tasks/queue-sink/rabbit-mq-queue-sink#add-a-rabbitmq-connection-string)  
     * [Add a RabbitMQ Sink Task](../../../server/ongoing-tasks/queue-sink/rabbit-mq-queue-sink#add-a-rabbitmq-sink-task)  
{NOTE/}

---

{PANEL: The RabbitMQ Sink Task}

In RavenDB, a RabbitMQ sink is implemented as a user-defined ongoing sink task 
that connects a RabbitMQ exchange, retrieves enqueued messages from selected queues, 
runs a user-defined transformation script to construct documents, and stores the 
documents in RavenDB collections.  

* **Connecting a RabbitMQ exchange** 
  Connecting a RabbitMQ exchange requires a connection string.  
  Read [below](../../../server/ongoing-tasks/queue-sink/rabbit-mq-queue-sink#add-a-rabbitmq-connection-string) 
  how to add a RabbitMQ connection string via API.  
  Read [here](../../../) how to add a RabbitMQ connection string using Studio.  

* **Retrieving enqueued messages** from selected RabbitMQ queues
  When a message is sent to a RabbitMQ exchange by a producer, it is pushed to 
  the tail of the queue assigned to it. The message then advances in the queue 
  as preceding messages are pulled, until it finally reaches the queue's head 
  where RavenDB's sink task and other consumers can get it.  

* **Running a user-defined transformation script**  
  A sink connector's transformation script is a JavaScript segment. Its basic 
  role is to retrieve selected RabbitMQ messages or message properties, and prepare 
  them for storage in RavenDB.  
  The script can, however, also load and delete RavenDB documents (e.g. to check 
  and handle potential duplicates) and perform various other functions.  

* **Storing documents in RavenDB collections**  
  {INFO: }
  Note that producers may enqueue 
  [multiple instances](../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq#message-duplication) 
  of the same document.  
  It is **the consumer's own responsibility** (if processing each message only 
  once is important to it) to verify the uniqueness of each consumed message.  
  {INFO/}

{PANEL/}

{PANEL: Client API}

#### Add a RabbitMQ Connection String

Prior to defining a RabbitMQ sink task, add a **RabbitMQ connection string** 
that the task will use to connect the message broker exchanges.  

To create the connection string:  

* Create a `QueueConnectionString`instance with the connection string configuration.  
  Pass it to the `PutConnectionStringOperation` store operation to add the connection string.  

    `QueueConnectionString`:  
    {CODE add_RabbitMq_connection-string@Server\OngoingTasks\QueueSink\QueueSink.cs /}

    `QueueBrokerType`:  
    {CODE QueueBrokerType@Server\OngoingTasks\QueueSink\QueueSink.cs /}

    | Property | Type | Description |
    |:-------------|:-------------|:-------------|
    | **Name** | `string` | Connection string name |
    | **BrokerType** | `QueueBrokerType` | Set to `QueueBrokerType.RabbitMq` for a RabbitMQ connection string |
    | **RabbitMqConnectionSettings** | `KafkaConnectionSettings[]` | A list of strings indicating RabbitMQ exchanges connection details |

---

#### Add a RabbitMQ Sink Task

To create the Sink task:  

* Create a `QueueSinkScript` instance with a transformation script that constructs 
  RavenDB documents from read queue messages.  
  {CODE define-rabbitmq-sink-script@Server\OngoingTasks\QueueSink\QueueSink.cs /}
  
* Prepare a `QueueSinkConfiguration`object with the sink task configuration, 
  collecting the connection string and the transformation script.  

    `QueueSinkConfiguration` properties:  
    
    | Property | Type | Description |
    |:-------------|:-------------|:-------------|
    | **Name** | `string` | The sink task name |
    | **ConnectionStringName** | `string` | The registered connection string name |
    | **BrokerType** | `QueueBrokerType` | Set to `QueueBrokerType.RabbitMq` to define a RabbitMQ sink task |
    | **Scripts** | `List<QueueSinkScript>` | A list of strings holding transformation scripts |

* Pass this object to the `AddQueueSinkOperation` store operation to add the Sink task.  

    `QueueSinkScript` properties:  
    
    | Property | Type | Description |
    |:-------------|:-------------|:-------------|
    | **Name** | `string` | Script name|
    | **Queues** | `List<string>` | list of queues |
    | **Script** | `string  ` | A transform script |

**Code Sample**:  
{CODE add_kafka_sink-task@Server\OngoingTasks\QueueSink\QueueSink.cs /}

{PANEL/}

## Related Articles

### Server

- [ETL Basics](../../../server/ongoing-tasks/etl/basics)
- [Queue ETL Overview](../../../server/ongoing-tasks/etl/queue-etl/overview)
- [Kafka ETL](../../../server/ongoing-tasks/etl/queue-etl/kafka)
- [Queue Sink Overview](../../../server/ongoing-tasks/queue-sink/overview)
- [Kafka Queue Sink](../../../server/ongoing-tasks/queue-sink/kafka-queue-sink)

### Studio

- [Studio: RabbitMQ ETL Task](../../../studio/database/tasks/ongoing-tasks/rabbitmq-etl-task)
- [Studio: Kafka ETL Task](../../../studio/database/tasks/ongoing-tasks/kafka-etl-task)
