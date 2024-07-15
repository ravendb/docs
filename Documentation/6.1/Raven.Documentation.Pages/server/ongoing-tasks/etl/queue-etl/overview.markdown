# Queue ETL Overview
---

{NOTE: }

* Message brokers are high-throughput, distributed messaging services that host data they receive  
  from **producer** applications and serve it to **consumer** clients via FIFO data queues.

* RavenDB can operate as a _Producer_ within this architecture to the following message brokers:
  * **Apache Kafka**
  * **RabbitMQ**
  * **Azure Queue Storage**

* This functionality is achieved by defining [Queue ETL tasks](../../../../server/ongoing-tasks/etl/queue-etl/overview#supported-message-brokers) within a RavenDB database.

* RavenDB can also function as a _Consumer_.  
  To learn about RavenDB's role as a _Consumer_ please refer to the [Queue Sink section](../../../../server/ongoing-tasks/queue-sink/overview).

* In this page:
    * [Queue ETL tasks](../../../../server/ongoing-tasks/etl/queue-etl/overview#queue-etl-tasks)
    * [CloudEvents](../../../../server/ongoing-tasks/etl/queue-etl/overview#cloudevents)
    * [Task statistics](../../../../server/ongoing-tasks/etl/queue-etl/overview#task-statistics)

{NOTE/}

---

{PANEL: Queue ETL tasks}

RavenDB produces messages to broker queues via the following Queue ETL tasks:

* **Kafka ETL Task**  
  You can define a Kafka ETL Task from the [Studio](../../../../studio/database/tasks/ongoing-tasks/kafka-etl-task)
  or using the [Client API](../../../../server/ongoing-tasks/etl/queue-etl/kafka).
* **RabbitMQ ETL Task**  
  You can define a RabbitMQ ETL Task from the [Studio](../../../../studio/database/tasks/ongoing-tasks/rabbitmq-etl-task)
  or using the [Client API](../../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq).
* **Azure Queue Storage ETL Task**  
  You can define an Azure Queue Storage ETL Task from the [Studio](../../../../studio/database/tasks/ongoing-tasks/azure-queue-storage-etl)
  or using the [Client API](../../../../server/ongoing-tasks/etl/queue-etl/azure-queue).

---

These ETL tasks:  

* **Extract** selected data from RavenDB documents from specified collections.  
* **Transform** the data to new JSON objects.  
* Wrap the JSON objects as [CloudEvents messages](https://cloudevents.io) and **Load** them to the designated message broker.  

{PANEL/}

{PANEL: CloudEvents}

* After preparing a JSON object that needs to be sent to a message broker,  
  the ETL task wraps it as a CloudEvents message using the [CloudEvents Library](https://cloudevents.io).  

* To do that, the JSON object is provided with additional [required attributes](https://github.com/cloudevents/spec/blob/main/cloudevents/spec.md#required-attributes),  
  added as headers to the message, including:   

    | Attribute       | Type     | Description                                                                                               | Default Value                                        |
    |-----------------|----------|-----------------------------------------------------------------------------------------------------------|------------------------------------------------------|
    | **id**          | `string` | [Event identifier](https://github.com/cloudevents/spec/blob/main/cloudevents/spec.md#id)                  | The document Change Vector                           |
    | **type**        | `string` | [Event type](https://github.com/cloudevents/spec/blob/main/cloudevents/spec.md#type)                      | "ravendb.etl.put"                                    |
    | **source**      | `string` | [Event context](https://github.com/cloudevents/spec/blob/main/cloudevents/spec.md#source-1)               | `<ravendb-node-url>/<database-name>/<etl-task-name>` |
    | **specversion** | `string` | [CloudEvents version used](https://github.com/cloudevents/spec/blob/main/cloudevents/spec.md#specversion) | `V1_0`                                               |

* The optional 'partitionkey' attribute can also be added.  
  Currently, it is only implemented by [Kafka ETL](../../../../server/ongoing-tasks/etl/queue-etl/kafka).  

    | Optional Attribute   | Type       | Description                                                                                                                                  | Default Value    |
    |----------------------|------------|----------------------------------------------------------------------------------------------------------------------------------------------|------------------|
    | **partitionkey**     | `string`   | [Events relationship/grouping definition](https://github.com/cloudevents/spec/blob/main/cloudevents/extensions/partitioning.md#partitionkey) | The document ID  |

{PANEL/}

{PANEL: Task statistics}

Use the Studio's [Ongoing tasks stats view](../../../../studio/database/stats/ongoing-tasks-stats/overview) to see various statistics related to data extraction, transformation,  
and loading to the target broker.  

![Queue Brokers Stats](images/overview_stats.png "Ongoing tasks stats view")

{PANEL/}


## Related Articles

### Server

- [ETL Basics](../../../../server/ongoing-tasks/etl/basics)
- [Kafka ETL](../../../../server/ongoing-tasks/etl/queue-etl/kafka)
- [RabbitMQ ETL](../../../../server/ongoing-tasks/etl/queue-etl/rabbit-mq)
- [Azure Queue Storage ETL](../../../../server/ongoing-tasks/etl/queue-etl/azure-queue)

### Studio

- [Studio: Kafka ETL Task](../../../../studio/database/tasks/ongoing-tasks/kafka-etl-task)
- [Studio: RabbitMQ ETL Task](../../../../studio/database/tasks/ongoing-tasks/rabbitmq-etl-task)
- [Studio: Azure Queue Storage ETL Task](../../../../studio/database/tasks/ongoing-tasks/azure-queue-storage-etl)
