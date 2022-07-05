# Ongoing Tasks: Queue ETL Overview
---

{NOTE: }

* Message brokers are high-throughput, distributed messaging services that 
  host data they receive from **producer** applications and serve it to 
  **consumer** clients via FIFO data queue/s. 
* RavenDB takes the role of a Producer in this architecture, via ETL tasks that -  
   * Extract data from specified document collections.  
   * Transform the data to new JSON objects.  
   * Load the JSON objects to the message broker.  
* RavenDB wraps JSON objects as CloudEvents messages prioer to loading them 
  to the designated broker, using the [CloudEvents Library](https://cloudevents.io).  
* Supported message brokers currently include **Apache Kafka** and **RabbitMQ**.  

* In this page:  
   * [Supported Message Brokers](../../../../server/ongoing-tasks/etl/queue-etl/overview#supported-message-brokers)  
   * [CloudEvents](../../../../server/ongoing-tasks/etl/queue-etl/overview#cloudevents)  
   * [Task Statistics](../../../../server/ongoing-tasks/etl/queue-etl/overview#task-statistics)  

{NOTE/}

---

{PANEL: Supported Message Brokers}

Queue applications that RavenDB can currently produce data for include **Apache Kafka** and **RabbitMQ**.  

![Ongoing Tasks](images/overview_ongoing-tasks.png "Ongoing Tasks")

1. **Ongoing Tasks**  
   Click to open the ongoing tasks view.  
2. **Add a Database Task**  
   Click to create a new ongoing task.  

---

![Define ETL Task](images/overview_task-selection.png "Define ETL Task")

1. **Kafka ETL**  
   Click to define a Kafka ETL task.  
2. **RabbitMQ ETL**  
   Click to define a RabbitMQ ETL task.  

{PANEL/}

{PANEL: CloudEvents}

After preparing a JSON object that needs to be sent to a message broker, 
the ETL task wraps it as a CloudEvents message using the [CloudEvents Library](https://cloudevents.io).  

To do that, the JSON object is provided with additional 
[required attributes](https://github.com/cloudevents/spec/blob/main/cloudevents/spec.md#required-attributes) 
Like [id](https://github.com/cloudevents/spec/blob/main/cloudevents/spec.md#id),
[source](https://github.com/cloudevents/spec/blob/main/cloudevents/spec.md#source-1),
[specversion](https://github.com/cloudevents/spec/blob/main/cloudevents/spec.md#specversion), 
And [type](https://github.com/cloudevents/spec/blob/main/cloudevents/spec.md#type).  

{PANEL/}

{PANEL: Task Statistics}

Use Studio's [ongoing tasks stats](../../../../studio/database/stats/ongoing-tasks-stats/overview) view 
to see various statistics related to data extraction, transformation, and loading to the target broker.  

![Queue Brokers Stats](images/overview_stats.png "Ongoing Tasks")


{PANEL/}



## Related Articles

### ETL

- [ETL Basics](../../../../server/ongoing-tasks/etl/basics)
- [SQL ETL Task](../../../../server/ongoing-tasks/etl/sql)

### Client API

- [How to Add ETL](../../../../client-api/operations/maintenance/etl/add-etl)
- [How to Update ETL](../../../../client-api/operations/maintenance/etl/update-etl)
- [How to Reset ETL](../../../../client-api/operations/maintenance/etl/reset-etl)

### Studio

- [Define RavenDB ETL Task in Studio](../../../../studio/database/tasks/ongoing-tasks/ravendb-etl-task)
