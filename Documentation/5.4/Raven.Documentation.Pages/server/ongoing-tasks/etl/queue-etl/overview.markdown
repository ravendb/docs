# Ongoing Tasks: Queue ETL Overview
---

{NOTE: }

* The message brokers currently supported by RavenDB's queue ETL are 
  high-throughput, distributed messaging services. Messages are sent 
  to these services by **producer** applications like RavenDB, and 
  hosted by them until their retrieval by **consumer** applications.  

* RavenDB takes the role of a **producer** application in this architecture.  
  Its queue ETL tasks Extract specified documents from the database, Transform 
  them to new objects by a custom script, and Load (push) them to the specified 
  queue broker.  
  
* RavenDB currently supports two queue implementations: **Kafka** and **RabbitMQ**.  

* To push documents to the queue brokers, RavenDB uses the `CloudEvents` library.  

* In this page:  
   * [Supported Brokers](../../../../server/ongoing-tasks/etl/queue-etl/overview#supported-brokers)  
   * [CloudEvents](../../../../server/ongoing-tasks/etl/queue-etl/overview#cloudevents)  
   * [Task Statistics](../../../../server/ongoing-tasks/etl/queue-etl/overview#task-statistics)  

{NOTE/}

---

{PANEL: Supported Brokers}

RavenDB can currently push messages to two types of queue brokers: Kafka and RabbitMQ.  

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

When the ETL task finishes to transform the data extracted from the database, 
the result is always a JSON document that now needs to be transferred to the target.  

However, to transfer documents to queue brokers, RavenDB formats the outgoing 
documents as CloudEvents messages first, Using the [CloudEvents Library](https://cloudevents.io).  

To do that, RavenDB adds each JSON doc 
[required attributes](https://github.com/cloudevents/spec/blob/main/cloudevents/spec.md#required-attributes)
Like [id](https://github.com/cloudevents/spec/blob/main/cloudevents/spec.md#id),
[source](https://github.com/cloudevents/spec/blob/main/cloudevents/spec.md#source-1),
[specversion](https://github.com/cloudevents/spec/blob/main/cloudevents/spec.md#specversion), 
And [type](https://github.com/cloudevents/spec/blob/main/cloudevents/spec.md#type).  

{PANEL/}

{PANEL: Task Statistics}

You can see various statistics related to the extraction, transformation and 
loading of your data from RavenDB to the target queue broker, using Studio'
s [ongoing tasks stats](../../../../studio/database/stats/ongoing-tasks-stats/overview) view.  

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
