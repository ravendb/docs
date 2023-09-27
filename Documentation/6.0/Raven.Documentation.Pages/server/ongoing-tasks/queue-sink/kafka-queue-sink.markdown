# Queue Sink: Apache Kafka
---

{NOTE: }

* **Apache Kafka** is a distributed, high-performance, transactional messaging 
  platform, that remains performant as the number of messages it needs to process 
  increases and the number of events it needs to stream climbs to the big-data zone.  
  
* RavenDB can harness the advantages presented by message brokers like Kafka 
  both as a producer (by [running ETL tasks](../../../server/ongoing-tasks/etl/queue-etl/kafka)) 
  and as a **consumer** (using a sink task to consume enqueued messages).  

* To use RavenDB as a consumer, define an ongoing Sink task that will read batches 
  of enqueued JSON formatted messages from Kafka topics, construct documents using 
  user-defined scripts, and store the documents in RavenDB collections.  

* In this page:  
  * [The Queue Sink Task](../../../server/ongoing-tasks/queue-sink/kafka-queue-sink#the-queue-sink-task)  
  * [Client API](../../../server/ongoing-tasks/queue-sink/kafka-queue-sink#client-api)  
     * [Add a Kafka Connection String](../../../server/ongoing-tasks/queue-sink/kafka-queue-sink#add-a-kafka-connection-string)  
     * [Add a Kafka Sink Task](../../../server/ongoing-tasks/queue-sink/kafka-queue-sink#add-a-kafka-sink-task)  
     * [Configuration Options](../../../server/ongoing-tasks/queue-sink/kafka-queue-sink#configuration-options)  
{NOTE/}

---

{PANEL: The Queue Sink Task}

Users of RavenDB 6.0 and on can create an ongoing Sink task that connects 
a Kafka broker, retrieves enqueued messages from selected Kafka topics, 
runs a user-defined script to manipulate data and construct documents, and 
potentially stores the created documents in RavenDB collections.  

---

#### Connecting a Kafka broker

In the message broker architecture, RavenDB sinks take the role of data consumers.  
A sink would connect a Kafka broker using a connection string, and retrieve messages 
from the broker's Topics.  

Read [below](../../../server/ongoing-tasks/queue-sink/kafka-queue-sink#add-a-kafka-connection-string) 
about adding a connection string via API.  
Read [here](../../../studio/database/tasks/ongoing-tasks/kafka-queue-sink#define-a-kafka-sink-task) 
about adding a connection string using Studio.  

{WARNING: }
Like all ongoing tasks, a sink task is operated by a 
[responsible node](../../../server/clustering/distribution/highly-available-tasks#responsible-node).  
When the responsibility for the task is moved from one node to another, 
e.g. from node `A` to node `B` as a result of node `A` down time:  

* The consumer task will maintain the same consumer group id it had on the original node.  
* Kafka brokers may **cease serving the sink task for some time** as the Kafka consumer 
  group rebalances (adapting to the leaving of one node and the joining of another, among 
  other changes).  

{WARNING/}

---

#### Retrieving enqueued messages from selected Kafka topics

When a message is sent to a Kafka broker by a producer, it is pushed to 
the tail of a topic. As preceding messages are pulled, the message advances 
up the queue until it reaches its head and can be consumed by RavenDB's sink.  

---

#### Running user-defined scripts

A sink task's script is a JavaScript segment. Its basic role is to retrieve 
selected Kafka messages or message properties, and construct documents that 
will then be stored in RavenDB.  

The script can simply store the whole message as a document, as in this 
segment:  
{CODE-BLOCK: javascript}
// Add the document a metadata `@collection` property to keep it in 
// this collection, or do not set it to store the document in @empty).  
this['@metadata']['@collection'] = 'Orders'; 
// Store the message as is, using its Id property as its RavenDB Id as well.  
put(this.Id.toString(), this)
{CODE-BLOCK/}

But the script can also retrieve some information from the read message 
and construct a new document that doesn't resemble the original message.  
Scripts often apply two sections: a section that creates a JSON object 
that defines the document's structure and contents, and a second section 
that stores the document.  

E.g., for Kafka messages of this format -  
{CODE-BLOCK: JSON}
{
   "Id" : 13,
   "FirstName" : "John",
   "LastName" : "Doe"
}
{CODE-BLOCK/}

We can create this script -  
{CODE-BLOCK: javascript}
var item = { 
    Id : this.Id, 
    FirstName : this.FirstName, 
    LastName : this.LastName, 
    FullName : this.FirstName + ' ' + this.LastName, 
    "@metadata" : {
        "@collection" : "Users"
    }
};

// Use .toString() to pass the Id as a string even if Kafka provides it as a number
put(this.Id.toString(), item)
{CODE-BLOCK/}

The script can also apply various other JavaScript commands, including 
`load` to load a RavenDB document (e.g. to construct a document that 
includes data from the retrieved message and complementing data from 
existing RavenDB documents), `del` to remove existing RavenDB documents, 
and [many others](../../../server/kb/javascript-engine#predefined-javascript-functions).  

---

#### Storing documents in RavenDB collections

The sink task consumes batches of queued messages and stores them in RavenDB 
in a transactional manner, processing either the entire batch or none of it.  
{NOTE: Exceptions to this rule}
Some script processing errors are allowed; when such an error occurs RavenDB 
will skip the affected message, record the event in the logs, and alert the 
user in Studio, but **continue processing the batch**.  
{NOTE/}

Once a batch is consumed, the task confirms it by calling `kafkaConsumer.Commit()`.  

Note that the number of documents included in a batch is 
[configurable](../../../server/ongoing-tasks/queue-sink/kafka-queue-sink#configuration-options).

{WARNING: Take care of duplicates}
Producers may enqueue 
[multiple instances](../../../server/ongoing-tasks/etl/queue-etl/kafka#idempotence-and-message-duplication) 
of the same document.  
if processing each message only once is important to the consumer, 
it is **the consumer's responsibility** to verify the uniqueness of 
each consumed message.  

Note that as long as the **Id** property of Kafka messages is preserved 
(so duplicate messages share an Id), the script's `put(ID, { ... })` 
command will overwrite a previous document with the same Id and only 
one copy of it will remain.  
{WARNING/}

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

* Create `QueueSinkScript` instances to define scripts with which the 
  task can process retrieved messages, apply JavaScript commands, construct 
  documents and store them in RavenDB.  
  {CODE define-kafka-sink-script@Server\OngoingTasks\QueueSink\QueueSink.cs /}

* Prepare a `QueueSinkConfiguration`object with the sink task configuration.  

    `QueueSinkConfiguration` properties:  

    | Property | Type | Description |
    |:-------------|:-------------|:-------------|
    | **Name** | `string` | The sink task name |
    | **ConnectionStringName** | `string` | The registered connection string name |
    | **BrokerType** | `QueueBrokerType` | Set to `QueueBrokerType.Kafka` to define a Kafka sink task |
    | **Scripts** | `List<QueueSinkScript>` | A list of scripts |

* Pass this object to the `AddQueueSinkOperation` store operation to add the Sink task.  

    `QueueSinkScript` properties:  

    | Property | Type | Description |
    |:-------------|:-------------|:-------------|
    | **Name** | `string` | Script name |
    | **Queues** | `List<string>` | A list of Kafka topics to consume messages from |
    | **Script** | `string  ` | The script contents |

**Code Sample**:  
{CODE add_kafka_sink-task@Server\OngoingTasks\QueueSink\QueueSink.cs /}

{PANEL/}

{PANEL: Configuration Options}

Use these configuration options to gain more control over queue sink tasks.  

* [QueueSink.MaxBatchSize](../../../server/configuration/queue-sink-configuration#queuesink.maxbatchsize)  
  The maximum number of pulled messages consumed in a single batch.
* [QueueSink.MaxFallbackTimeInSec](../../../server/configuration/queue-sink-configuration#queuesink.maxfallbacktimeinsec)  
  The maximum number of seconds the Queue Sink process will be in a fallback 
  mode (i.e. suspending the process) after a connection failure.  

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
- [Studio: Kafka Queue Sink](../../../studio/database/tasks/ongoing-tasks/kafka-queue-sink)
- [Studio: RabbitMQ Queue Sink](../../../studio/database/tasks/ongoing-tasks/rabbitmq-queue-sink)
